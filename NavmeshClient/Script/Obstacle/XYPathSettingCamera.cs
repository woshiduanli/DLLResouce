//#define GRID_USE_TEXTURE

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

static class KeyChecker {

	private static KeyCode[,] numberKeys_ = new KeyCode[10,2] {
		{ KeyCode.Alpha0, KeyCode.Keypad0 },
		{ KeyCode.Alpha1, KeyCode.Keypad1 },
		{ KeyCode.Alpha2, KeyCode.Keypad2 },
		{ KeyCode.Alpha3, KeyCode.Keypad3 },
		{ KeyCode.Alpha4, KeyCode.Keypad4 },
		{ KeyCode.Alpha5, KeyCode.Keypad5 },
		{ KeyCode.Alpha6, KeyCode.Keypad6 },
		{ KeyCode.Alpha7, KeyCode.Keypad7 },
		{ KeyCode.Alpha8, KeyCode.Keypad8 },
		{ KeyCode.Alpha9, KeyCode.Keypad9 }
	};

	public static bool IsCtrlKeyHolding() {
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	public static bool IsShiftKeyHolding() {
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	public static bool IsAltKeyHolding() {
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	public static int IsNumKeyHolding() {
		for (int i = numberKeys_.GetLowerBound(0); i <= numberKeys_.GetUpperBound(0); ++i) {
			if (Input.GetKey(numberKeys_[i,0]) || Input.GetKey(numberKeys_[i,1])) {
				return i;
			}
		}
		return -1;
	}

	public static int IsNumKeyDown() {
		for (int i = numberKeys_.GetLowerBound(0); i <= numberKeys_.GetUpperBound(0); ++i) {
			if (Input.GetKeyDown(numberKeys_[i, 0]) || Input.GetKeyDown(numberKeys_[i, 1])) {
				return i;
			}
		}
		return -1;
	}
}

/// <summary>
/// 设置客户端自动寻路的障碍图
/// </summary>
abstract class ObstacleCamera : MonoBehaviour {

	#region 常量定义
	public static readonly Color COLOR_BRUSH = new Color(1, 1, 0, 0.5f);
	public static readonly Color COLOR_DRAW = new Color(0, 1, 0, 0.5f);
	public static readonly Color COLOR_ERASE = new Color(1, 0, 0, 0.5f);
	//private static readonly Color COLOR_NORMAL = new Color(0, 0, 1, 0.5f);

	public static readonly Color COLOR_REACHABLE = new Color(0, 0, 1, 0.5f);		// 蓝色，可到达点
	public static readonly Color COLOR_MARKED_OBSTACLE = new Color(1, 0, 0, 0.5f);	// 红色，不可到达点
	public static readonly Color COLOR_PRIOR = new Color(0, 1, 1, 0.5f);			// 高优先级

	private static readonly int MIN_BRUSH_SIZE = 1;
	private static readonly int MAX_BRUSH_SIZE = 20;
	#endregion

	#region 成员变量

	private static ObstacleCamera instance_;
	private static ObstacleCamera Instance { get { return instance_; } }

	private Vector3 lookAt_;							// 虚拟的观察目标
	private float distance_ = 500;						// 摄像机距目标的距离
	private Vector3 euler_ = new Vector3(-89, 180, 0);	// 目标到摄像机的欧拉角

	private XYCameraSmooth cameraSmooth_ = new XYCameraSmooth();
	private State state_;
	private GuiUtil guiUtil_;
	private XYObstacleCamera.Effect effect_;

	protected AirWall airWall_;
	private GridUtil gridUtil_;
	public GridUtil gridUtil { get { return gridUtil_; } }

	private string filename_;

	private int brushSize_ = MIN_BRUSH_SIZE;
	public int BrushSize {
		get { return brushSize_; }
		set {
			if (brushSize_ != value) {
				brushSize_ = Mathf.Clamp(value, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
			}
		}
	}

	private int xBrush_, zBrush_;
	public int xBrush { get { return xBrush_; } }
	public int zBrush { get { return zBrush_; } }
	public bool IsBrushPosValid { get { return xBrush_ >= 0; } }

	private bool IsAirWallVisible_ {
		get { return airWall_.IsVisible; }
		set { airWall_.IsVisible = value; }
	}

	#endregion

	protected abstract string GetDefaultFilename(string currentSceneName);

	protected abstract void CheckOtherKey();

	protected abstract void ShowOtherButtons();

	void Awake() {
		if (instance_ == null) {
			instance_ = this;
		} else {
			throw new System.Exception(string.Format("{0} must be singleton", this.GetType().Name));
		}
		effect_ = new XYObstacleCamera.Effect(GetComponent<Camera>());
		filename_ = GetDefaultFilename(AsyncLevelLoader.LoadedLevelName);
		guiUtil_ = new GuiUtil(this);
		state_ = new State_Normal(this);
		//
		//XYClientCommon.AddComponent<AudioListener>(camera.gameObject);
	}

	void Start() {
        this.airWall_ = new AirWall();
        GameManager.AddAllSceneColliders();
        this.lookAt_ = new Vector3(MapPlacementController.curMap.Width * 0.5f, 0f, MapPlacementController.curMap.Height * 0.5f);
        this.lookAt_.y = XYObstacleCamera.GetTerrainHeight(this.lookAt_);
        this.effect_.ShowFog = false;
        this.effect_.ShowTrees = false;
        this.gridUtil_ = new GridUtil(1f, this.airWall_);
	}

	private void LookAt(Vector3 target) {
		this.lookAt_ = target;
		this.distance_ = Mathf.Min(this.distance_, 15);
		//Vector3 dir = Quaternion.Euler(this.euler_) * Vector3.forward;
		//camera.transform.position = this.lookAt_ + dir * this.distance_;
		//camera.transform.LookAt(this.lookAt_);	
	}

	protected abstract void DrawAllGrids();

	public void ChangeState(State newState) {
		if (newState != null && state_ != newState) {
			state_ = newState;
		}
	}

	//protected void ChangeState(System.Type t) {
	//    System.Type[] types = new System.Type[] { typeof(ObstacleCamera) };
	//    System.Reflection.ConstructorInfo ci = t.GetConstructor(types);
	//    if (ci != null) {
	//        object[] args = new object[] { this };
	//        ChangeState(ci.Invoke(args) as State);
	//    }
	//}

	private bool LoadFromFile(string filename, bool markObstacleOnly) {
		try {
			gridUtil_.LoadFromFile(filename, this.SaveType, markObstacleOnly);
			return true;
		} catch (System.Exception ex) {
			State s = state_;
			ChangeState(new State_AlertBox(this, ex.Message, s));
			return false;
		}
	}

	private bool SaveToFile(string filename) {
		try {
			gridUtil_.SaveToFile(filename, this.SaveType);
			return true;
		} catch (System.Exception ex) {
			State s = state_;
			ChangeState(new State_AlertBox(this, ex.Message, s));
			return false;
		}
	}

	protected abstract void OnKey_Control();
	protected abstract void OnKey_Shift();
	protected abstract GridUtil.SaveType SaveType { get; }

	private class GuiUtil {
		private string posOfFastGoto_ = string.Empty;
		private static readonly GUIContent guiShowTree_ = new GUIContent("显示树木");
		private static readonly GUIContent guiBrushSize_ = new GUIContent("画刷尺寸");
		private static readonly GUIContent guiAirWallVisible_ = new GUIContent("显示空气墙");
		private readonly ObstacleCamera owner_;
		public GuiUtil(ObstacleCamera owner) {
			owner_ = owner;
		}
		public void ShowLoadSaveButton() {
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("加载")) {
					owner_.ChangeState(new State_Load(owner_));
				} else if (GUILayout.Button("保存")) {
					owner_.ChangeState(new State_Save(owner_));
				} else if (GUILayout.Button("全清")) {
					owner_.ChangeState(
						new State_Dialog(
							owner_, State_Dialog.ButtonStyle.OkCancel,
							"清空所有数据",
							"此操作将清空本地图所有障碍数据，是否确定？",
							delegate(State_Dialog.ResultType rt) {
								if (rt == State_Dialog.ResultType.Ok) {
									owner_.gridUtil_.Clear();
								}
								owner_.ChangeState(new State_Normal(owner_));
							}
					));
				} else if (GUILayout.Button("智能填充(F4)")) {
					owner_.SmartFill();
				}
			}
			GUILayout.EndHorizontal();
			//
			owner_.ShowOtherButtons();
		}

		public void ShowCtrls() {
			GUI.Box(new Rect(Screen.width - 280, 0, 280, 100), string.Empty);
			//
			if (GUI.Button(new Rect(Screen.width - 200, 0, 90, 20), "返回")) {
				owner_.ChangeState(new State_Dialog(owner_,
					State_Dialog.ButtonStyle.OkCancel,
					"提示",
					"确定要返回到主菜单吗？",
					delegate(State_Dialog.ResultType rt) {
						if (rt == State_Dialog.ResultType.Ok) {
							Application.LoadLevel("startMenu");
						} else {
							owner_.ChangeState(new State_Normal(owner_));
						}
					}
				));
			}
			if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 20), "鸟瞰")) {
				Vector3 newEuler = owner_.euler_;
				newEuler.x = -89.9f;
				newEuler.y = 180;
				newEuler.z = 0;
				owner_.euler_ = newEuler;
			}
			// 显示“Brush Size”
			float x = Screen.width - 200;
			float y = 24;
			Vector2 size = GUI.skin.label.CalcSize(guiBrushSize_);
			float minX = x - size.x - 2;
			GUI.Label(new Rect(minX, y, size.x, size.y), guiBrushSize_);
			// 显示画笔尺寸水平调整条
			owner_.BrushSize = (int)GUI.HorizontalSlider(
				new Rect(x, y, 100, size.y),
				owner_.BrushSize,
				MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
			// 显示UnDo
			int historyCount = owner_.gridUtil_.HistoryCount;
			if (historyCount > 0) {
				if (GUI.Button(new Rect(x + 110, y, 100, 20), string.Format("UnDo ({0})", historyCount))) {
					owner_.gridUtil_.UnDo();
				}
			}
			// 显示"Show Trees"
			y += 24;
			bool old = owner_.effect_.ShowTrees;
			bool showTrees = GUI.Toggle(new Rect(x, y, 100, 20), old, guiShowTree_);
			if (showTrees != old) {
				owner_.effect_.ShowTrees = showTrees;
			}
			// 显示空气墙勾选框
			size = GUI.skin.toggle.CalcSize(guiAirWallVisible_);
			Rect rc = new Rect(x + 100, y, size.x, size.y);
			owner_.IsAirWallVisible_ = GUI.Toggle(rc, owner_.IsAirWallVisible_, guiAirWallVisible_);
			// 坐标定位
			y += 24;
			this.posOfFastGoto_ = GUI.TextField(new Rect(x, y, 100, 20), this.posOfFastGoto_).Trim();
			if (GUI.Button(new Rect(x + 104, y, 64, 20), "定位")) {
				if (this.posOfFastGoto_.Length > 0) {
					string[] xy = this.posOfFastGoto_.Split(',');
					if (xy.Length == 2) {
						int xx;
						if (int.TryParse(xy[0].Trim(), out xx) && xx >= 0 && xx < owner_.gridUtil_.cxTerrainSize) {
							int yy;
							if (int.TryParse(xy[1].Trim(), out yy) && yy >= 0 && yy < owner_.gridUtil_.czTerrainSize) {
								owner_.LookAt(new Vector3(xx, owner_.gridUtil_.GetHigh(xx,yy), yy));
							}
						}
					}
				}
			}

		}
	}

	/// <summary>
	/// 用户选择“智能填充F4”时被调用
	/// </summary>
	protected abstract void SmartFill();

	void OnGUI() {
		state_.OnGUI();
		string desc = state_.Desc;
		if (desc != null) {
			Vector3 pos = Input.mousePosition;
			GUI.Label(new Rect(pos.x + 32, Screen.height - pos.y + 32, 200, 200), desc);
		}
	}

	void Update() {
		GetBrushXZ();
		state_.Update(transform, cameraSmooth_, ref lookAt_, ref distance_, ref euler_);
	}

	void LateUpdate() {
		cameraSmooth_.Execute(transform, lookAt_, euler_, distance_);
	}

	void OnPostRender() {
		effect_.BeforeUseGL();
		state_.OnPostRender();
	}

	private bool GetBrushXZ() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (gridUtil_.WorldPosToGridListIndex(hit.point, out this.xBrush_, out this.zBrush_)) {
				return true;
			}
		}
		this.xBrush_ = this.zBrush_ = -1;
		return false;
	}

	public class State {

		protected readonly ObstacleCamera Owner;

		protected State(ObstacleCamera owner) {
			this.Owner = owner;
		}

		//protected void ChangeToOther() {
		//    System.Type t;
		//    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) {
		//        t = typeof(State_AdjustCamera);
		//    } else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
		//        Owner.OnHoldKey_Control();
		//        return;
		//    } else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
		//        Owner.OnHoldKey_Shift();
		//        return;
		//    } else if (Input.GetKey(KeyCode.V)) {
		//        t = typeof(State_DrawPrior);
		//    } else {
		//        t = typeof(State_Normal);
		//    }
		//    //
		//    if (!t.Equals(this.GetType())) {
		//        Owner.ChangeState(t);
		//    }
		//}

		public virtual void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {
			XYCameraController.Update(cameraTransform, cameraSmooth, ref lookAt, ref distance);
			bool toSmall = Input.GetKeyDown(KeyCode.LeftBracket);
			bool toLarge = Input.GetKeyDown(KeyCode.RightBracket);
			if (toSmall) {
				if (!toLarge) {
					Owner.BrushSize = Owner.BrushSize - 1;
				}
			} else if (toLarge) {
				Owner.BrushSize = Owner.BrushSize + 1;
			}
		}

		public virtual void OnPostRender() {
			Owner.DrawAllGrids();
		}

		protected void DrawBrush(Color color) {
			if (Owner.IsBrushPosValid) {
				Owner.gridUtil.SetColor(color);
				Owner.gridUtil_.DrawGrid(Owner.xBrush_, Owner.zBrush_, Owner.brushSize_);
			}
		}

		public virtual void OnGUI() {
			Owner.guiUtil_.ShowLoadSaveButton();
			Owner.guiUtil_.ShowCtrls();
		}

		public virtual string Desc {
			get {
				if (Owner.xBrush_ >= 0) {
					return string.Format("({0},{1})", Owner.xBrush_, Owner.zBrush_);
				} else {
					return null;
				}
			}
		}

	}

	/// <summary>
	/// 常规状态
	///		单击右键快速定位摄像机视点
	/// </summary>
	protected class State_Normal : State {
		public State_Normal(ObstacleCamera owner) : base(owner) { }

		public override void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {
			base.Update(cameraTransform, cameraSmooth, ref lookAt, ref distance, ref euler);
			if (Input.GetMouseButtonUp(1)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					Owner.LookAt(hit.point);
					lookAt = Owner.lookAt_;
					distance = Owner.distance_;
				}
			}
			//
			if (Input.GetKeyDown(KeyCode.F2)) {
				Owner.ChangeState(new State_Save(Owner));
			} else if (Input.GetKeyDown(KeyCode.F4)) {
				Owner.SmartFill();
			} if (KeyChecker.IsAltKeyHolding()) {
		        Owner.ChangeState(new State_AdjustCamera(Owner));
			} else if (KeyChecker.IsCtrlKeyHolding()) {
		        Owner.OnKey_Control();
		    } else if (KeyChecker.IsShiftKeyHolding()) {
		        Owner.OnKey_Shift();
			} else {
				Owner.CheckOtherKey();
			}
		}

		public override void OnPostRender() {
			base.OnPostRender();
			base.DrawBrush(COLOR_BRUSH);
		}
	}

	/// <summary>
	/// 当用户按下ALT键时，进入“调整摄像机”状态
	/// </summary>
	private class State_AdjustCamera : State {
		public State_AdjustCamera(ObstacleCamera owner) : base(owner) { }
		public override void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {
			base.Update(cameraTransform, cameraSmooth, ref lookAt, ref distance, ref euler);
			if (Input.GetMouseButton(0)) {
				float turnX = Input.GetAxisRaw("Mouse X");	// 偏角偏移
				float turnY = Input.GetAxisRaw("Mouse Y");	// 仰角偏移
				if (turnX != 0 || turnY != 0) {
					euler.x = Mathf.Clamp(euler.x + turnY * XYObstacleCamera.TurnSpeed, -89.99f, -1);
					euler.y = Mathf.Repeat(euler.y + turnX * XYObstacleCamera.TurnSpeed, 360);
					cameraSmooth.Active = false;
				}
			}
			if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) {
				Owner.ChangeState(new State_Normal(Owner));
			}
		}
		public override string Desc {
			get {
				return "调整视角";
			}
		}
	}

	protected abstract class State_DrawOrErase : State {
		private readonly Color brushColor_;
		protected State_DrawOrErase(ObstacleCamera owner, Color brushColor)
			: base(owner)
		{
			this.brushColor_ = brushColor;
		}
		public override void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {
			base.Update(cameraTransform, cameraSmooth, ref lookAt, ref distance, ref euler);
			if (IsMouseButtonDown) {
				if (Owner.IsBrushPosValid) {
					Execute(Owner.xBrush_, Owner.zBrush_, Owner.BrushSize);
				}
			}
			if (!IsLiving()) {
				Owner.ChangeState(new State_Normal(Owner));
			}
		}
		public override void OnPostRender() {
			base.OnPostRender();
			base.DrawBrush(brushColor_);
		}
		protected abstract void Execute(int x, int z, int brushSize);
		protected abstract bool IsMouseButtonDown { get; }
		protected abstract bool IsLiving();
	}

	protected class State_DrawReachable : State_DrawOrErase {

		public State_DrawReachable(ObstacleCamera owner) : base(owner, COLOR_DRAW) { }

		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButton(0);
			}
		}

		protected override void Execute(int x, int z, int brushSize) {
			Owner.gridUtil.Set(x, z, PathFindDataType.Reachable, 0, brushSize);
		}

		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("设置可达 ({0},{1})", Owner.xBrush_, Owner.zBrush_);
				} else {
					return null;
				}
			}
		}

		protected override bool IsLiving() {
			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}
	}

	/// <summary>
	/// Load或Save对话状态
	/// </summary>
	protected abstract class State_LoadOrSave : State {
		private readonly Rect rectLabel_;
		private readonly Rect rectFilenameBox_;
		private readonly Rect rectBtnExecute_;
		protected readonly Rect rectBtnCancel_;
		private readonly GUIContent guiLabel_;
		private readonly GUIContent guiBtnExecute_;
		private readonly GUIContent guiBtnCancel_;

		protected State_LoadOrSave(ObstacleCamera owner, string execute)
			: base(owner)
		{
			guiLabel_ = new GUIContent("请输入文件名：");
			Vector2 size = GUI.skin.label.CalcSize(guiLabel_);
			rectLabel_ = new Rect(16, 16, size.x, size.y);
			rectFilenameBox_ = new Rect(rectLabel_.xMin, rectLabel_.yMax + 4,
				320,
				GUI.skin.textField.CalcSize(new GUIContent("Test")).y);
			//
			guiBtnExecute_ = new GUIContent(execute);
			size = GUI.skin.button.CalcSize(guiBtnExecute_);
			rectBtnExecute_ = new Rect(rectFilenameBox_.xMin, rectFilenameBox_.yMax + 4, size.x, size.y);
			//
			guiBtnCancel_ = new GUIContent("取消");
			size = GUI.skin.button.CalcSize(guiBtnCancel_);
			rectBtnCancel_ = new Rect(rectBtnExecute_.xMax + 24, rectBtnExecute_.yMin, size.x, size.y);
		}

		protected virtual string Filename {
			get { return Owner.filename_; }
			set { Owner.filename_ = value; }
		}

		public override void OnGUI() {
			GUI.Label(rectLabel_, guiLabel_);
			this.Filename = GUI.TextField(rectFilenameBox_, this.Filename, 256);
			if (GUI.Button(rectBtnExecute_, guiBtnExecute_)) {
				if (Execute(this.Filename)) {
					Owner.ChangeState(new State_Normal(Owner));
				}
				return;
			}
			if (GUI.Button(rectBtnCancel_, guiBtnCancel_)) {
				Owner.ChangeState(new State_Normal(Owner));
			}
		}
		protected abstract bool Execute(string filename);
		public override string Desc {
			get {
				return null;
			}
		}
	}

	private class State_Load : State_LoadOrSave {
		private readonly GUIContent toggle_;
		private readonly Rect rectToggle_;
		private bool importMarkObstacleOnly_;
		public State_Load(ObstacleCamera owner) : base(owner, "加载") {
			if (Owner.SaveType == GridUtil.SaveType.ClientAutoMove) {
				toggle_ = new GUIContent("只导入人工标注障碍");
				Vector2 size = GUI.skin.toggle.CalcSize(toggle_);
				rectToggle_ = new Rect(
					rectBtnCancel_.xMax + 8, rectBtnCancel_.yMin,
					size.x, rectBtnCancel_.height);
			}
		}

		protected override bool Execute(string filename) {
			return Owner.LoadFromFile(filename, this.importMarkObstacleOnly_);
		}

		public override void OnGUI() {
			base.OnGUI();
			if (Owner.SaveType == GridUtil.SaveType.ClientAutoMove) {
				importMarkObstacleOnly_ = GUI.Toggle(rectToggle_, importMarkObstacleOnly_, toggle_);
			}
		}
	}

	private class State_Save : State_LoadOrSave {
		public State_Save(ObstacleCamera owner) : base(owner, "保存") { }
		protected override bool Execute(string filename) {
			return Owner.SaveToFile(filename);
		}
	}

	/// <summary>
	/// 显示一个供用户选择的对话框
	/// </summary>
	protected class State_Dialog : State {
		public enum ResultType {
			Ok,
			Cancel,
			Yes,
			No
		};
		public enum ButtonStyle {
			Ok, OkCancel, YesNo, YesNoCancel
		}
		private const int WIDTH = 480;
		private const int HEIGHT = 320;
		private Rect wndRect_;
		private readonly ButtonStyle buttonStyle_;
		private readonly Action<ResultType> onClose_;
		private readonly string title_;
		private readonly string content_;
		public State_Dialog(
			ObstacleCamera owner,
			ButtonStyle buttonStyle,
			string title,
			string content,
			System.Action<ResultType> onClose)
			: base(owner)
		{
			this.title_ = title;
			this.content_ = content;
			this.buttonStyle_ = buttonStyle;
			this.onClose_ = onClose;
			this.wndRect_ = new Rect(
				(Screen.width - WIDTH) * 0.5f,
				(Screen.height - HEIGHT) * 0.5f,
				WIDTH, HEIGHT);
		}
		public override void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {}
		public override void OnGUI() {
			GUILayout.Window(103, wndRect_, WndFunc, title_);
		}
		private void ShowBtnOk() {
			if (GUILayout.Button("确定")) {
				onClose_(ResultType.Ok);
			}
		}
		private void ShowBtnCancel() {
			if (GUILayout.Button("取消")) {
				onClose_(ResultType.Cancel);
			}
		}
		private void ShowBtnYes() {
			if (GUILayout.Button("是")) {
				onClose_(ResultType.Yes);
			}
		}
		private void ShowBtnNo() {
			if (GUILayout.Button("否")) {
				onClose_(ResultType.No);
			}
		}
		private void WndFunc(int id) {
			GUILayout.Space(20);
			GUILayout.Label(content_);
			GUILayout.Space(20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				switch (this.buttonStyle_) {
				case ButtonStyle.OkCancel:
					ShowBtnOk();
					ShowBtnCancel();
					break;
				case ButtonStyle.YesNo:
					ShowBtnYes();
					ShowBtnNo();
					break;
				case ButtonStyle.YesNoCancel:
					ShowBtnYes();
					ShowBtnNo();
					ShowBtnCancel();
					break;
				default:
					ShowBtnOk();
					break;
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();
		}
		public override string Desc {
			get {
				return null;
			}
		}
	}

	/// <summary>
	/// 显示错误信息状态
	/// </summary>
	protected class State_AlertBox : State {
		private const int CX_WINDOW = 480;
		private const int CY_WINDOW = 240;
		private readonly GUIContent message_;
		private readonly State nextState_;
		private Rect rectWindow_;
		public State_AlertBox(ObstacleCamera owner, string message, State nextState)
			: base(owner)
		{
			nextState_ = nextState;
			message_ = new GUIContent(message);
			rectWindow_ = new Rect(
				(Screen.width - CX_WINDOW) * 0.5f,
				48,
				CX_WINDOW, CY_WINDOW);
		}
		public override void OnGUI() {
			rectWindow_ = GUI.Window(1234, rectWindow_, WndProc, "Message");
		}
		private void WndProc(int id) {
			GUILayout.Space(24);
			GUILayout.Label(message_);
			GUILayout.Space(24);
			if (GUILayout.Button("确定")) {
				Owner.ChangeState(nextState_);
			}
		}
		public override string Desc {
			get {
				return null;
			}
		}
	}

	/// <summary>
	/// 为自动创建设置起始点的状态
	/// </summary>
	protected class State_ChooseStartPoint : State {
		private float nextColorTime_ = Time.time + 0.5f;
		private Color color_ = new Color(1, 1, 1, 0.5f);
		public State_ChooseStartPoint(ObstacleCamera owner) : base(owner) { }
		public override void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance, ref Vector3 euler) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Owner.ChangeState(new State_Normal(Owner));
				return;
			}
			if (Owner.xBrush_ >= 0) {
				if (Input.GetMouseButtonDown(0)) {
					if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
						Owner.gridUtil_.SmartFill_Reachable(Owner.xBrush_, Owner.zBrush_);
						Owner.ChangeState(new State_Normal(Owner));
					}
				}
			}
		}
		public override void OnPostRender() {
			base.OnPostRender();
			if (Owner.xBrush_ >= 0) {
				if (Time.time >= nextColorTime_) {
					nextColorTime_ = Time.time + 0.5f;
					if (color_.b > 0.5f) {
						color_ = Color.red;
					} else {
						color_ = new Color(1, 1, 1, 0.5f);
					}
				}
				Owner.gridUtil.SetColor(color_);
				Owner.gridUtil_.DrawGrid(Owner.xBrush_, Owner.zBrush_, 1);
			}
		}

		public override void OnGUI() {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height),
				string.Format(
					"\nCtrl+鼠标左击选择一个可到达的起始点，ESC返回……\n",
					Owner.xBrush_, Owner.zBrush_)
				);
		}
	}

	/// <summary>
	/// 摄像机控制
	/// </summary>
	static class XYCameraController {
		private static float calcSpeedAndNormalize(ref Vector3 movement) {
			float speed = Mathf.Min(movement.magnitude, 1);
			movement.Normalize();
			return speed;
		}

		public static void Update(Transform cameraTransform, XYCameraSmooth cameraSmooth, ref Vector3 lookAt, ref float distance) {
			float speedAdjust = XYObstacleCamera.GetSpeedFromInput() * Time.deltaTime;
			// 横移
			float moveX = Input.GetAxisRaw("Horizontal");
			if (0 != moveX) {
				cameraSmooth.Active = false;
				Vector3 movement = moveX * cameraTransform.right;
				float speed = calcSpeedAndNormalize(ref movement) * speedAdjust;
				lookAt += movement * speed;
			}
			// 纵移
			float moveZ = Input.GetAxisRaw("Vertical");
			if (0 != moveZ) {
				cameraSmooth.Active = false;
				Vector3 movement = Quaternion.Euler(0, -90, 0) * cameraTransform.right;
				movement *= moveZ;
				float speed = calcSpeedAndNormalize(ref movement) * speedAdjust;
				lookAt += movement * speed;
			}
			// 鼠标滚动调整距离
			float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
			if (Input.GetKey(KeyCode.Q)) {
				cameraSmooth.Active = false;
				wheel += 0.1f;
			}
			if (Input.GetKey(KeyCode.E)) {
				cameraSmooth.Active = false;
				wheel -= 0.1f;
			}
			if (0 != wheel) {
				cameraSmooth.Active = false;
				distance -= wheel * distance * 0.5f;
				if (distance <= 1) {
					Vector3 movement = cameraTransform.forward * wheel;
					float speed = calcSpeedAndNormalize(ref movement) * speedAdjust;
					distance = 1;
					lookAt += movement * speed;
				}
			}
		}
	}

	public class GridUtil {

		private class HoldStack {

			public struct Entry {
				public readonly PathFindDataType[,] Grids;
				public readonly sbyte[,] Numbers;
				public Entry(PathFindDataType[,] grids, sbyte[,] numbers) {
					this.Grids = grids;
					this.Numbers = numbers;
				}
			}

			private readonly int maxCount_;
			private readonly LinkedList<Entry> stack_;

			public HoldStack(int maxCount) {
				this.maxCount_ = maxCount;
				stack_ = new LinkedList<Entry>();
			}
			public void Push(Entry entry) {
				if (stack_.Count >= maxCount_) {
					stack_.RemoveFirst();
				}
				stack_.AddLast(entry);
			}
			public void Push(PathFindDataType[,] grids, sbyte[,] numbers) {
				Push(new Entry(grids, numbers));
			}

			public bool Pop(out PathFindDataType[,] grids, out sbyte[,] numbers) {
				if (stack_.Count > 0) {
					grids = stack_.Last.Value.Grids;
					numbers = stack_.Last.Value.Numbers;
					stack_.RemoveLast();
					return true;
				} else {
					grids = null;
					numbers = null;
					return false;
				}
			}
			public int Count { get { return stack_.Count; } }
			public void Clear() {
				this.stack_.Clear();
			}
		}

		private bool changed_;
		public readonly int cxTerrainSize;
		public readonly int czTerrainSize;
		private readonly float gridSize_;

		private PathFindDataType[,] gridList_;
		private sbyte[,] numberList_;	// 编号
		private readonly float[,] high_;
		private readonly float[,] crossList_;	// 交叉点的高度
		private readonly bool[,] isAirWall_;

		private readonly HoldStack holdStack_ = new HoldStack(16);

#if GRID_USE_TEXTURE
		private readonly Material material_;
		private readonly Texture2D texture_;
#endif

		public GridUtil(float gridSize, AirWall aw) {
            this.gridSize_ = gridSize;
            this.gridList_ = new PathFindDataType[this.cxTerrainSize, this.czTerrainSize];
            this.numberList_ = new sbyte[this.cxTerrainSize, this.czTerrainSize];
            this.high_ = new float[this.cxTerrainSize, this.czTerrainSize];
            this.crossList_ = new float[this.cxTerrainSize + 1, this.czTerrainSize + 1];
            this.isAirWall_ = new bool[this.cxTerrainSize + 1, this.czTerrainSize + 1];
            this.GetCrossHeightList(aw);
			//
#if GRID_USE_TEXTURE
			this.material_ = new Material(
				"Shader \"Game9Z/Ha Ha Ha\" {\n" +
				"	Properties {\n" +
				"		_MainTex (\"Base (RGB)\", 2D) = \"white\" {}\n" +
				"		_Color (\"MainColor (RGB)\", Color) = (1,1,1,1)\n" +
				"	}\n" +
				"	SubShader {\n" +
				"		Pass {\n" +
				"			Blend SrcAlpha OneMinusSrcAlpha\n" +
				"			ZWrite Off Cull Off Fog { Mode Off }\n" +
				"			BindChannels { Bind \"vertex\", vertex Bind \"color\", color }\n" +
				"			SetTexture [_MainTex] {\n" +
				"				constantColor [_Color]\n" +
				"				combine constant lerp(texture) previous\n" +
				"			}\n" +
				"			SetTexture [_MainTex] { combine previous * texture }\n" +
				"		}\n" +
				"	}\n" +
				"}"
			);
			if (!material_) {
				material_.hideFlags = HideFlags.HideAndDontSave;
				material_.shader.hideFlags = HideFlags.HideAndDontSave;
			}
			//
			this.texture_ = Resource9Z.Loader.LoadTexture(@"res\misc\attackUi.png", Resource9Z.Loader.TextureCompress.HighQuality, true);
#endif
		}

#if GRID_USE_TEXTURE
		private Rect GetNumberRectInTexture(int n) {
			XYGuiMisc.Rect rc;
			XYHurtTexture.GetNumElementRect(n, out rc);
			float w = 1f / texture_.width;
			float h = 1f / texture_.height;
			return Rect.MinMaxRect(
				rc.Left * w,
				1f - rc.Top * h,
				rc.Right * w,
				1f - rc.Bottom * h);
		}
#endif
		public void GetTerrainSize(out int cx, out int cz) {
			cx = this.gridList_.GetLength(0);
			cz = this.gridList_.GetLength(1);
		}

		public float GetHigh(int x, int z) {
			return this.high_[x, z];
		}

		public float GetHigh(int x, int z, out bool isSlope) {
			float[] h = new float[4];
			h[0] = this.crossList_[x, z];
			h[1] = this.crossList_[x+1, z];
			h[2] = this.crossList_[x+1, z+1];
			h[3] = this.crossList_[x, z+1];
			Array.Sort(h);
			isSlope = (h[3] - h[0]) > 0.6f;
			return this.high_[x, z];
		}

		/// <summary>
		/// 判断给定点是不是空气墙（四角都是空气墙才返回True，用于服务端阻挡）
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		private bool IsAirWall(int x, int z) {
			return this.isAirWall_[x, z] && this.isAirWall_[x + 1, z] && this.isAirWall_[x + 1, z + 1] && this.isAirWall_[x, z + 1];
		}

		private void ForEach(System.Action<PathFindDataType> action) {
			for (int x = 0; x < gridList_.GetLength(0); ++x) {
				for (int z = 0; z < gridList_.GetLength(1); ++z) {
					action(gridList_[x, z]);
				}
			}
		}

		private void GetCrossHeightList(AirWall aw) {
			using (FindPath.WaterCollider wc = new FindPath.WaterCollider()) {
				// 交叉点的高度
				for (int x = 0; x < this.crossList_.GetLength(0); ++x) {
					for (int z = 0; z < this.crossList_.GetLength(1); ++z) {
						isAirWall_[x, z] = false;
						Vector3 pos = new Vector3(
							x * this.gridSize_,
							1000,
							z * this.gridSize_);
						RaycastHit hit;
						if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, -1)) {
							crossList_[x, z] = hit.point.y;
							Collider c = hit.collider;
							if (c && aw.Contains(c)) {
								isAirWall_[x, z] = true;
							}
						} else {
                            crossList_[x, z] = XYObstacleCamera.GetTerrainHeight(pos);
						}
					}
				}
				// 格子中点的高度
				float half = gridSize_ * 0.5f;
				for (int x = 0; x < this.high_.GetLength(0); ++x) {
					for (int z = 0; z < this.high_.GetLength(1); ++z) {
						Vector3 pos = new Vector3(
							x * this.gridSize_ + half,
							1000,
							z * this.gridSize_ + half);
						RaycastHit hit;
						if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, -1)) {
							high_[x, z] = hit.point.y;
						} else {
                            high_[x, z] = XYObstacleCamera.GetTerrainHeight(pos);
						}
					}
				}
			}
		}

		public PathFindDataType[,] CloneGridList() {
			return gridList_.Clone() as PathFindDataType[,];
		}

		public bool WasChanged { get { return changed_; } }

		public void UnDo() {
			if (holdStack_.Count > 0) {
				holdStack_.Pop(out this.gridList_, out this.numberList_);
			}
		}

		public int HistoryCount {
			get { return this.holdStack_.Count; }
		}

		public void Clear() {
			for (int x = 0; x < gridList_.GetLength(0); ++x) {
				for (int z = 0; z < gridList_.GetLength(1); ++z) {
					gridList_[x, z] = 0;
				}
			}
		}

		private HoldStack.Entry HoldCurrent() {
			return new HoldStack.Entry(
				this.gridList_.Clone() as PathFindDataType[,],
				this.numberList_.Clone() as sbyte[,]);
		}

		private static bool IsArrayEqual<T>(T[,] t1, T[,] t2) {
			if (t1 == t2) { return true; }
			int xEnd = t1.GetLength(0);
			if (xEnd != t2.GetLength(0)) { return false; }
			int zEnd = t1.GetLength(1);
			if (zEnd != t2.GetLength(1)) { return false; }
			for (int x = 0; x < xEnd; ++x) {
				for (int z = 0; z < zEnd; ++z) {
					if (!t1[x, z].Equals(t2[x, z])) {
						return false;
					}
				}
			}
			return true;
		}

		private void PushToHistoryIfChanged(HoldStack.Entry hse) {
			if (!IsArrayEqual(this.gridList_, hse.Grids) || !IsArrayEqual(this.numberList_, hse.Numbers)) {
				this.changed_ = true;
				this.holdStack_.Push(hse);
			}
		}


		// 
		/// <summary>
		/// 根据地形自动创建可达点
		/// </summary>
		/// <param name="xStart">第一个可到达点的格子坐标X</param>
		/// <param name="zStart">第一个可到达点的格子坐标Z</param>
		public void SmartFill_Reachable(int xStart, int zStart) {
			HoldStack.Entry hse = HoldCurrent();
			this.gridList_ = PassableDataBuilder.Execute(this, xStart, zStart);
			this.PushToHistoryIfChanged(hse);
		}

		/// <summary>
		/// 将所有空气墙所在格，设为绝对阻挡
		/// </summary>
		public void SmartFill_Obstacle(sbyte num) {
			HoldStack.Entry hse = HoldCurrent();
			for (int x = 0; x < this.cxTerrainSize; ++x) {
				for (int z = 0; z < this.czTerrainSize; ++z) {
					if (IsAirWall(x, z)) {
						this.gridList_[x, z] = PathFindDataType.MarkedObstacle;
						this.numberList_[x, z] = num;
					}
				}
			}
			this.PushToHistoryIfChanged(hse);
		}

		public PathFindDataType Get(int x, int z) {
			return this.gridList_[x, z];
		}

		public PathFindDataType Get(Vector3 worldPos) {
			int x, z;
			if (WorldPosToGridListIndex(worldPos, out x, out z)) {
				return this.gridList_[x, z];
			} else {
				return PathFindDataType.MarkedObstacle;
			}
		}

		private void DoSet(int x, int z, PathFindDataType pfdt, sbyte num) {
			PathFindDataType old = gridList_[x, z];
			sbyte oldNum = numberList_[x, z];
			switch (pfdt) {
			case PathFindDataType.UnSet:
			case PathFindDataType.Reachable:
			case PathFindDataType.Prior:
				num = 0;
				break;
			default:
				System.Diagnostics.Debug.Assert(num > 0);
				if (num <= 0) {
					num = 1;
				}
				break;
			}
			//
			if (old != pfdt || oldNum != num) {
				changed_ = true;
				gridList_[x, z] = pfdt;
				numberList_[x, z] = num;
			}
		}

		private void SetWithNoHistory(int x, int z, PathFindDataType pfdt, sbyte num, int brushSize) {
			if (brushSize <= 1) {
				DoSet(x, z, pfdt, num);
			} else {
				BrushExecute(x, z, brushSize,
					delegate(int xx, int zz) {
						DoSet(xx, zz, pfdt, num);
					}
				);
			}
		}

		public void Set(int x, int z, PathFindDataType pfdt, sbyte num, int brushSize) {
			HoldStack.Entry hse = HoldCurrent();
			SetWithNoHistory(x, z, pfdt, num, brushSize);
			PushToHistoryIfChanged(hse);
		}

		//public void Set(Vector3 worldPos, PathFindDataType value, int brushSize) {
		//    int x, z;
		//    if (WorldPosToGridListIndex(worldPos, out x, out z)) {
		//        Set(x, z, value, brushSize);
		//    }
		//}

		public bool WorldPosToGridListIndex(Vector3 worldPos, out int x, out int z) {
			x = Mathf.RoundToInt(worldPos.x / gridSize_);
			z = Mathf.RoundToInt(worldPos.z / gridSize_);
			return x >= 0 && x < gridList_.GetLength(0)
				&& z >= 0 && z < gridList_.GetLength(1);
		}

		//public void AdjustToGridCenter(ref Vector3 point) {
		//    point.x = Mathf.RoundToInt(point.x / gridSize_) * gridSize_ + halfGridSize_;
		//    point.z = Mathf.RoundToInt(point.z / gridSize_) * gridSize_ + halfGridSize_;
		//}

		private delegate void BrushExecuteAction(int x, int z);
		private void BrushExecute(int x, int z, int brushSize, BrushExecuteAction bea) {
			int halfBrushSize = brushSize / 2;
			int minX = Mathf.Max(x - halfBrushSize, 0);
			int minZ = Mathf.Max(z - halfBrushSize, 0);
			int maxX = Mathf.Min(minX + brushSize, gridList_.GetUpperBound(0));
			int maxZ = Mathf.Min(minZ + brushSize, gridList_.GetUpperBound(1));
			for (int xx = minX; xx <= maxX; ++xx) {
				for (int zz = minZ; zz <= maxZ; ++zz) {
					bea(xx, zz);
				}
			}
		}

		public void SetColor(Color color) {
#if GRID_USE_TEXTURE
			material_.color = color;
#else
			GL.Color(color);
#endif
		}

		public void DrawGrid(int x, int z, int brushSize) {
			if (brushSize <= 1) {
				DrawGrid(x, z);
			} else {
				BrushExecute(x, z, brushSize, DrawGrid);
			}
		}

		private void DrawGrid(int x, int z) {
			Vector3 pos_0 = new Vector3(x * gridSize_, crossList_[x, z], z * gridSize_);
			Vector3 pos_1 = new Vector3(pos_0.x + gridSize_, crossList_[x + 1, z], pos_0.z);
			Vector3 pos_2 = new Vector3(pos_1.x, crossList_[x + 1, z + 1], pos_1.z + gridSize_);
			Vector3 pos_3 = new Vector3(pos_0.x, crossList_[x, z + 1], pos_2.z);
			//
#if GRID_USE_TEXTURE
			material_.mainTexture = null;
			material_.SetPass(0);
#endif
			GL.Begin(GL.QUADS);
			GL.Vertex(pos_0);
			GL.Vertex(pos_1);
			GL.Vertex(pos_2);
			GL.Vertex(pos_3);
			GL.End();
			//
#if GRID_USE_TEXTURE
			sbyte num = numberList_[x, z];
			if (num > 0) {
				Rect rc = this.GetNumberRectInTexture(num);
				material_.mainTexture = texture_;
				Color oldColor = material_.color;
				SetColor(Color.white);
				material_.SetPass(0);
				GL.Begin(GL.QUADS);
				GL.TexCoord2(rc.xMin, rc.yMin);
				GL.Vertex(pos_0);
				GL.TexCoord2(rc.xMin, rc.yMax);
				GL.Vertex(pos_1);
				GL.TexCoord2(rc.xMax, rc.yMax);
				GL.Vertex(pos_2);
				GL.TexCoord2(rc.xMax, rc.yMin);
				GL.Vertex(pos_3);
				GL.End();
				SetColor(oldColor);
			}

#endif
			//GL.TexCoord2(0, 0);
			//GL.Vertex(pos);
			//pos.x += gridSize_;
			//pos.y = crossList_[x + 1, z];
			//GL.TexCoord2(0, 1);
			//GL.Vertex(pos);
			//pos.z += gridSize_;
			//pos.y = crossList_[x + 1, z + 1];
			//GL.TexCoord2(1, 1);
			//GL.Vertex(pos);
			//pos.x -= gridSize_;
			//pos.y = crossList_[x, z + 1];
			//GL.TexCoord2(1, 0);
			//GL.Vertex(pos);
			//GL.End();
			//
		}

		//private void DrawGrid(Vector3 worldPos) {
		//    int x, z;
		//    if (WorldPosToGridListIndex(worldPos, out x, out z)) {
		//        DrawGrid(x, z);
		//    }
		//}

		/// <summary>
		/// 用Bresenham算法画一条线
		/// </summary>
		/// <param name="x0"></param>
		/// <param name="y0"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="brushSize"></param>
		public void SetLine(int x0, int y0, int x1, int y1, PathFindDataType value, sbyte num, int brushSize) {
			HoldStack.Entry hse = HoldCurrent();
			//
			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep) {
				XYMisc.Swap(ref x0, ref y0);
				XYMisc.Swap(ref x1, ref y1);
			}
			if (x0 > x1) {
				XYMisc.Swap(ref x0, ref x1);
				XYMisc.Swap(ref y0, ref y1);
			}
			int deltax = x1 - x0;
			int deltay = Math.Abs(y1 - y0);
			int error = deltax / 2;
			int y = y0;
			int ystep = (y0 < y1) ? 1 : -1;
			for (int x = x0; x <= x1; ++x) {
				if (steep) {
					this.SetWithNoHistory(y, x, value, num, brushSize);
				} else {
					this.SetWithNoHistory(x, y, value, num, brushSize);
				}
				error -= deltay;
				if (error < 0) {
					y += ystep;
					error += deltax;
				}
			}
			//
			PushToHistoryIfChanged(hse);
		}

		public void DrawAll(sbyte num) {
			Camera camera = Camera.main;
			Matrix4x4 mx = camera.projectionMatrix * camera.worldToCameraMatrix;
			for (int x = 0; x < gridList_.GetLength(0); ++x) {
				for (int z = 0; z < gridList_.GetLength(1); ++z) {
					PathFindDataType pfdt = gridList_[x, z];
					if (pfdt == PathFindDataType.UnSet) {
						continue;
					}
					if (num > 0 && num != this.numberList_[x, z]) {
						continue;
					}
					//
					Vector4 v4 = new Vector4(x * gridSize_, this.high_[x, z], z * gridSize_, 1f);
					v4 = mx * v4;
					float w = 1f / v4.w;
					float xx = v4.x * w;
					if (xx > -1.5f && xx < 1.5f) {
						float yy = v4.y * w;
						if (yy > -1.5f && yy < 1.5f) {
							float zz = v4.z * w;
							if (zz > 0 && zz < 1) {
								switch (pfdt) {
								case PathFindDataType.MarkedObstacle:
									if (num <= 1) {
										SetColor(COLOR_MARKED_OBSTACLE);
									} else {
										SetColor(COLOR_PRIOR);
									}
									break;
								case PathFindDataType.Prior:
									SetColor(COLOR_PRIOR);
									break;
								case PathFindDataType.Reachable:
									SetColor(COLOR_REACHABLE);
									break;
								}
								DrawGrid(x, z);
							}
						}
					}
				}
			}
		}

		//public void ToggleGrid(Vector3 worldPos) {
		//    int x, z;
		//    if (WorldPosToGridListIndex(worldPos, out x, out z)) {
		//        this.gridList_[x, z] =
		//            (this.gridList_[x, z] == 0) ? (sbyte)1 : (sbyte)0;
		//			changed_ = true;
		//    }
		//}

		//[System.Diagnostics.Conditional("DEBUG")]
		public bool Compare(GridUtil other) {
			if (other != null) {
				int cx = this.gridList_.GetLength(0);
				int cz = this.gridList_.GetLength(1);
				if (cx == other.gridList_.GetLength(0) && cz == other.gridList_.GetLength(1)) {
					for (int x = 0; x < cx; ++x) {
						for (int z = 0; z < cz; ++z) {
							if (gridList_[x, z] != other.gridList_[x, z]) {
								return false;
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		//public void LoadFromStream(Stream input) {
		//    using (BinaryReader br = new BinaryReader(input)) {
		//        int cx = br.ReadInt16();
		//        int cz = br.ReadInt16();
		//        if (cx != this.gridList_.GetLength(0) || cz != this.gridList_.GetLength(1)) {
		//            throw new System.Exception("The data file is not built by this terrain");
		//        }
		//        long len = input.Seek(0, SeekOrigin.End) - 4;
		//        if (len != (cx * cz * 2 + 7) / 8) {
		//            throw new System.Exception("Invalid data file");
		//        }
		//        input.Seek(4, SeekOrigin.Begin);
		//        //
		//        XYBitReader r = new XYBitReader(input);
		//        for (int x = 0; x < gridList_.GetLength(0); ++x) {
		//            for (int z = 0; z < gridList_.GetLength(1); ++z) {
		//                gridList_[x, z] = PathFindDataIO.ReadFromBitStream(r);
		//                //if (r.Read()) {
		//                //    gridList_[x, z] = PathFindDataType.Reachable;
		//                //} else {
		//                //    gridList_[x, z] = PathFindDataType.UnSet;
		//                //}
		//            }
		//        }
		//        changed_ = false;
		//        this.holdStack_.Clear();
		//    }
		//}

		public enum SaveType {
			ClientAutoMove,
			ServerObstacle
		}

		private void SaveNumToStream(Stream output) {
			using (BinaryWriter writer = new BinaryWriter(output)) {
				int cx = numberList_.GetLength(0);
				int cz = numberList_.GetLength(1);
				writer.Write((short)cx);
				writer.Write((short)cz);
				for (int x = 0; x < cx; ++x) {
					for (int z = 0; z < cz; ++z) {
						writer.Write(this.numberList_[x, z]);
					}
				}
			}
		}

		private void SaveNumToFile(string filename) {
			using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
				SaveNumToStream(fs);
			}
		}

		private sbyte[,] LoadNumFromStream(Stream input) {
			using (BinaryReader reader = new BinaryReader(input)) {
				int cx = reader.ReadInt16();
				int cz = reader.ReadInt16();
				Debug.Log(string.Format("cx={0}, cz={1}", cx, cz));
				if (cx <= 0 || cz <= 0) {
					throw new System.Exception("无效数据");
				}
				sbyte[,] result = new sbyte[cx, cz];
				for (int x = 0; x < cx; ++x) {
					for (int z = 0; z < cz; ++z) {
						result[x, z] = reader.ReadSByte();
					}
				}
				return result;
			}
		}

		private sbyte[,] LoadNumFromFile(string filename) {
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				return LoadNumFromStream(fs);
			}
		}

		public void SaveToFile(string filename, SaveType st) {
			if (st == SaveType.ClientAutoMove) {
				PathFindDataIO.WriteToFile(filename, this.gridList_);
			} else {
				SaveNumToFile(filename);
			}
			this.changed_ = false;
			this.holdStack_.Clear();
		}

		public void LoadFromFile(string filename, SaveType st, bool markObstacleOnly) {
			if (st == SaveType.ClientAutoMove) {
				PathFindDataType[,] list = PathFindDataIO.ReadFromFile(filename);
				if (list == null) {
					throw new System.Exception("无效的数据文件");
				}
				if (list.GetLength(0) != this.gridList_.GetLength(0)
				|| list.GetLength(1) != this.gridList_.GetLength(1)) {
					throw new System.Exception("指定的数据文件不符合本地图尺寸");
				}
				if (markObstacleOnly) {
					changed_ = true;
					this.holdStack_.Push(new HoldStack.Entry(this.gridList_, this.numberList_));
					for (int x = 0; x < list.GetLength(0); ++x) {
						for (int z = 0; z < list.GetLength(1); ++z) {
							if (list[x, z] == PathFindDataType.MarkedObstacle) {
								this.gridList_[x, z] = PathFindDataType.MarkedObstacle;
							}
						}
					}
				} else {
					this.gridList_ = list;
					changed_ = false;
				}
			} else {
				sbyte[,] nums = LoadNumFromFile(filename);
				if (nums.GetLength(0) != this.numberList_.GetLength(0)
					|| nums.GetLength(1) != this.numberList_.GetLength(1)
				) {
					throw new System.Exception("指定的文件不符合本地图尺寸");
				}
				this.numberList_ = nums;
				for (int x = 0; x < nums.GetLength(0); ++x) {
					for (int z = 0; z < nums.GetLength(1); ++z) {
						if (nums[x, z] <= 0) {
							gridList_[x, z] = PathFindDataType.UnSet;
						} else {
							gridList_[x, z] = PathFindDataType.MarkedObstacle;
						}
					}
				}
				changed_ = false;
			}
			//
			if (!changed_) {
				this.holdStack_.Clear();
			}

		}

	}


	/// <summary>
	/// 空气墙
	/// </summary>
	public class AirWall {
		private bool isVisible_;
		private readonly HashSet<Collider> colliders_ = new HashSet<Collider>();
		public AirWall() {
			GameObject[] airWalls = XYClientCommon.AirWall.RemoveRenderAndMesh(false);
			if (airWalls != null) {
				foreach (GameObject aw in airWalls) {
					XYClientCommon.ForEachChildren(
						aw,
						delegate(GameObject go) {
							if (go.GetComponent<Collider>()) {
								colliders_.Add(go.GetComponent<Collider>());
							}
						},
						true
					);
				}
			}
		}
		public bool Contains(Collider c) {
			return colliders_.Contains(c);
		}
		public bool IsVisible {
			get { return isVisible_; }
			set {
				if (value != isVisible_) {
					isVisible_ = value;
					XYClientCommon.AirWall.EnableRender(value);
				}
			}
		}
	}


	private class PassableDataBuilder {
		
		#region Open表
		private class OpenList {

			private readonly List<uint> container_;

			public OpenList(int capacity) {
				this.container_ = new List<uint>(capacity);
			}

			public bool Add(uint gridId) {
				int idx = container_.BinarySearch(gridId);
				if (idx < 0) {
					container_.Insert(~idx, gridId);
				} else if (idx >= container_.Count) {
					container_.Insert(idx, gridId);
				} else {
					return false;
				}
				return true;
			}

			public uint Take() {
				int last = container_.Count - 1;
				if (last >= 0) {
					uint r = container_[last];
					container_.RemoveAt(last);
					return r;
				} else {
					return INVALID_GRID_ID;
				}
			}
		}
		#endregion

		private static readonly int[,] DIR_DELTA = new int[8, 2] {
			{0, 1}, {1, 1}, {1, 0}, {1, -1},
			{0, -1}, {-1, -1}, {-1, 0}, {-1, 1}
		};

		private const uint INVALID_GRID_ID = uint.MaxValue;

		private static uint GridIdFromXZ(int x, int z) { return (uint)x | ((uint)z << 16); }

		public static void XZFromGridId(uint gridId, out int x, out int z) {
			x = (int)(gridId & 0xffff);
			z = (int)(gridId >> 16);
		}

		public static PathFindDataType[,] Execute(GridUtil gridUtil, int xStart, int zStart) {
			if (xStart < 0 || zStart < 0 || xStart > gridUtil.cxTerrainSize || zStart > gridUtil.czTerrainSize) {
				return null;
			}
			OpenList openList = new OpenList(1000);
			//
			PathFindDataType[,] result = gridUtil.CloneGridList();
			//
			openList.Add(GridIdFromXZ(xStart, zStart));
			do {
				uint gridId = openList.Take();
				if (gridId == INVALID_GRID_ID) { break; }
				int x, z;
				XZFromGridId(gridId, out x, out z);
				result[x, z] = PathFindDataType.Reachable;
				float h1 = gridUtil.GetHigh(x, z);
				for (int i = 0; i < 8; ++i) {
					int x2 = x + DIR_DELTA[i, 0];
					if (x2 < 0 || x2 >= gridUtil.cxTerrainSize) { continue; }
					int z2 = z + DIR_DELTA[i, 1];
					if (z2 < 0 || z2 >= gridUtil.czTerrainSize) { continue; }
					if (result[x2, z2] == PathFindDataType.UnSet) {
						bool isSlope;
						float h2 = gridUtil.GetHigh(x2, z2, out isSlope);
						if (isSlope) {
							result[x2, z2] = PathFindDataType.TmpObstacle;
						} else if (Mathf.Abs(h2 - h1) < XYDefines.OBSTACLE_HIGH_DELTA) {
							openList.Add(GridIdFromXZ(x2, z2));
						}
					}
				}
			} while (true);
			//
			for (int x = 0; x < result.GetLength(0); ++x) {
				for (int z = 0; z < result.GetLength(1); ++z) {
					if (result[x, z] == PathFindDataType.TmpObstacle) {
						result[x, z] = PathFindDataType.UnSet;
					}
				}
			}
			return result;
		}
	}


}

public enum PathFindDataType : sbyte {
	TmpObstacle = -2,			// 自动填充工具用的，不会出现在最终数据文件里
	MarkedObstacle = -1,		// 手工标记的绝对不可达地点
	UnSet,						// 未设置的
	Reachable,					// 可到达的
	Prior 						// 高优先级可达点
}

/// <summary>
/// 格子数据的IO
/// </summary>
static class PathFindDataIO {

	#region 输出

	public static void WriteToFile(string filename, PathFindDataType[,] dtGrid) {
		using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
			WriteToStream(fs, dtGrid);
		}
	}

	public static void WriteToStream(Stream output, PathFindDataType[,] dtGrid) {
		int cx = dtGrid.GetLength(0);
		int cy = dtGrid.GetLength(1);
		using (BinaryWriter bw = new BinaryWriter(output)) {
			bw.Write((short)cx);
			bw.Write((short)cy);
			using (XYBitWriter w = new XYBitWriter(output)) {
				for (int x = 0; x < cx; ++x) {
					for (int y = 0; y < cy; ++y) {
						WriteToBitStream(w, dtGrid[x, y]);
					}
				}
			}
		}
	}

	public static void WriteToBitStream(XYBitWriter bw, PathFindDataType dt) {
		switch (dt) {
		case PathFindDataType.MarkedObstacle:	// 11
			bw.Write(true);
			bw.Write(true);
			break;
		case PathFindDataType.Reachable:		// 01（低位在前）
			bw.Write(true);
			bw.Write(false);
			break;
		case PathFindDataType.Prior:			// 10（低位在前）
			bw.Write(false);
			bw.Write(true);
			break;
		default:								// 00
			bw.Write(false);
			bw.Write(false);
			break;
		}
	}
	#endregion

	#region 输入

	public static PathFindDataType[,] ReadFromFile(string filename) {
		using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			return ReadFromStream(fs);
		}
	}

	public static byte[] ReadFromFile(string filename, out int cx, out int cz) {
		using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			return ReadFromStream(fs, out cx, out cz);
		}
	}

	private static bool CheckInput(BinaryReader br, out int cx, out int cz) {
		cx = br.ReadInt16();
		cz = br.ReadInt16();
		if (cx <= 0 || cz <= 0) { return false; }
		long oldPos = br.BaseStream.Position;
		long validLen = (cx * cz * 2 + 7) / 8;
		try {
			long newPos = br.BaseStream.Seek(validLen, SeekOrigin.Current);
			return (newPos - oldPos) == validLen;
		} finally {
			br.BaseStream.Position = oldPos;
		}
	}

	public static byte[] ReadFromStream(Stream input, out int cx, out int cz) {
		using (BinaryReader br = new BinaryReader(input)) {
			if (!CheckInput(br, out cx, out cz)) { return null; }
			int bytes = cx * cz;
			byte[] result = new byte[bytes];
			BitReader9Z r = new BitReader9Z(input);
			for (int i = 0; i < bytes; ++i) {
				byte b;
				switch (ReadFromBitStream(r)) {
				case PathFindDataType.Reachable:
					b = 1;
					break;
				case PathFindDataType.Prior:
					b = 2;
					break;
				case PathFindDataType.MarkedObstacle:
					b = 3;
					break;
				default:
					b = 0;
					break;
				}
				result[i] = b;
			}
			return result;
		}
	}

	private static PathFindDataType[,] ReadFromStream(Stream input) {
		using (BinaryReader br = new BinaryReader(input)) {
			int cx, cz;
			if (!CheckInput(br, out cx, out cz)) { return null; }
			PathFindDataType[,] result = new PathFindDataType[cx, cz];
			BitReader9Z r = new BitReader9Z(input);
			for (int x = 0; x < cx; ++x) {
				for (int z = 0; z < cz; ++z) {
					result[x, z] = ReadFromBitStream(r);
				}
			}
			return result;
		}
	}

	private static PathFindDataType ReadFromBitStream(BitReader9Z br) {
		bool b1 = br.Read();
		bool b2 = br.Read();
		if (b1) {
			if (b2) {
				return PathFindDataType.MarkedObstacle;	// 11
			} else {
				return PathFindDataType.Reachable;		// 01
			}
		} else {
			if (b2) {
				return PathFindDataType.Prior;			// 10
			} else {
				return PathFindDataType.UnSet;			// 00
			}
		}
	}

	#endregion

}

public static class HighDataForServer_Util {

	public static class Adjuster {

		// 8个邻居的坐标偏移
		private static readonly int[,] DELTA_XY = new int[8, 2] {
			{-1, -1}, {0, -1}, {1, -1},
			{-1, 0}, {1, 0},
			{-1, 1}, {0, 1}, {1, 1}
		};

		// 判断指定格子的周围8个格子，如果有一个比当前格
		// 低1.5，则返回较低格子的高度。如果没有这样的邻居格
		// 则返回当前格的原始高度
		private static float AdjustHighWithNeighbor(float[,] rawHigh, int i, int j) {
			int maxX = rawHigh.GetUpperBound(0);
			int maxY = rawHigh.GetUpperBound(1);
			float current = rawHigh[i, j];
			for (int n = 0; n < 8; ++n) {
				int x = i + DELTA_XY[n, 0];
				if (x < 0 || x > maxX) {
					continue;
				}
				int y = j + DELTA_XY[n, 1];
				if (y < 0 || y > maxY) {
					continue;
				}
				float high = rawHigh[x, y];
				if (current - high >= 1.5f) {
					return high;
				}
			}
			return current;
		}

		public static float[,] Execute(float[,] rawHighData) {
			float[,] result = new float[rawHighData.GetLength(0), rawHighData.GetLength(1)];
			for (int x = 0; x < rawHighData.GetLength(0); ++x) {
				for (int z = 0; z < rawHighData.GetLength(1); ++z) {
					result[x, z] = AdjustHighWithNeighbor(rawHighData, x, z);
				}
			}
			return result;
		}
	}

	public static void SaveToFile(string filename, float[,] highData, bool adjusted) {
		float[,] data = adjusted ? highData : Adjuster.Execute(highData);
		using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
			using (BinaryWriter bw = new BinaryWriter(fs)) {
				int cx = data.GetLength(0);
				int cz = data.GetLength(1);
				bw.Write((short)cx);
				bw.Write((short)cz);
				for (int x = 0; x < cx; ++x) {
					for (int z = 0; z < cz; ++z) {
						ushort value = (ushort)(Mathf.RoundToInt(data[x, z] * 10));
						bw.Write(value);
					}
				}
			}
		}
	}

	public static float[,] GetRawHigh(float gridSize) {
		FindPath.AirWall airWall = new FindPath.AirWall();
		if (!airWall.Exists) {
			Debug.LogError("WARNING: No AirWall Found!");
		}
		using (FindPath.WaterCollider wc = new FindPath.WaterCollider()) {
			return FindPath.High.GetGridHigh(gridSize, airWall);
		}
	}

	public class Config {
		private readonly string cfgFilename_;

		private string lastSaveDirecctory_;
		public string LastSaveDirectory {
			get {
				return lastSaveDirecctory_;
			}
			set {
				if (string.Compare(value, lastSaveDirecctory_, true) != 0) {
					lastSaveDirecctory_ = value;
					SaveToCfgFile();
				}
			}
		}

		public Config() {
			cfgFilename_ = GetConfigFilename();
			LoadFromCfgFile();
		}

		private bool SaveToCfgFile() {
			if (cfgFilename_ != null) {
				try {
					using (FileStream fs = new FileStream(cfgFilename_, FileMode.Create, FileAccess.Write, FileShare.Read)) {
						using (TextWriter tw = new StreamWriter(fs)) {
							tw.WriteLine(this.lastSaveDirecctory_ ?? string.Empty);
						}
					}
					return true;
				} catch { }
			}
			return false;
		}

		private bool LoadFromCfgFile() {
			if (cfgFilename_ != null) {
				try {
					using (FileStream fs = new FileStream(cfgFilename_, FileMode.Open, FileAccess.Read, FileShare.Read)) {
						using (TextReader tr = new StreamReader(fs)) {
							this.lastSaveDirecctory_ = tr.ReadLine().Trim();
						}
					}
					return true;
				} catch { }
			}
			return false;
		}

		private static string GetConfigFilename() {
			string dir = XYMisc.AppendDirectorySeparatorChar(
				XYMisc.ReplaceDirectorySeparatorChar(
					Win32.SHGetFolderPath(Win32.CSIDL.CSIDL_APPDATA)
				)
			);
			return (dir ?? string.Empty) + "game9z_highdatabuilder.cfg";
		}

	}


}

class XYPathSettingCamera : ObstacleCamera {

	protected override string GetDefaultFilename(string currentSceneName) {
		return XYDirectory.Res + "res\\data\\" + currentSceneName + ".cn3";
	}
	protected override void DrawAllGrids() {
		 this.gridUtil.DrawAll(-1);
	}

	protected override void SmartFill() {
		this.ChangeState(new State_ChooseStartPoint(this));
	}
	protected override void OnKey_Control() {
		ChangeState(new State_DrawReachable(this));
	}
	protected override void OnKey_Shift() {
		ChangeState(new State_Erase(this));
	}

	protected override void CheckOtherKey() {
		if (Input.GetKey(KeyCode.V)) {
			ChangeState(new State_DrawPrior(this));
		}
	}

	protected override ObstacleCamera.GridUtil.SaveType SaveType {
		get { return GridUtil.SaveType.ClientAutoMove; }
	}

	protected override void ShowOtherButtons() {
		if (GUI.Button(new Rect(0, Screen.height - 20, 100, 20), "服务端高度图")) {
			ChangeState(new State_SaveHighDataForServer(this));
		}
	}

	private class State_Erase : State_DrawOrErase {
		public State_Erase(ObstacleCamera owner) : base(owner, COLOR_ERASE) { }
		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButtonDown(0);
			}
		}
		protected override void Execute(int x, int z, int brushSize) {
			PathFindDataType newValue =
				(Owner.gridUtil.Get(x, z) == PathFindDataType.MarkedObstacle)
				? PathFindDataType.UnSet
				: PathFindDataType.MarkedObstacle;
			Owner.gridUtil.Set(x, z, newValue, 0, brushSize);
		}
		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("擦除 ({0},{1})", Owner.xBrush, Owner.zBrush);
				} else {
					return null;
				}
			}
		}
		protected override bool IsLiving() {
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}
	}

	private class State_DrawPrior : State_DrawOrErase {
		public State_DrawPrior(ObstacleCamera owner) : base(owner, COLOR_PRIOR) { }
		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButton(0);
			}
		}
		protected override void Execute(int x, int z, int brushSize) {
			Owner.gridUtil.Set(x, z, PathFindDataType.Prior, 0, brushSize);
		}
		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("设置高优先级 ({0},{1})", Owner.xBrush, Owner.zBrush);
				} else {
					return null;
				}
			}
		}
		protected override bool IsLiving() {
			return Input.GetKey(KeyCode.V);
		}
	}

	// 保存服务端所用的高度图
	private class State_SaveHighDataForServer : State_LoadOrSave {
		private HighDataForServer_Util.Config cfg_ = new HighDataForServer_Util.Config();
		private string filename_;
		public State_SaveHighDataForServer(ObstacleCamera owner)
			: base(owner, "保存高度图") {
			filename_ = XYMisc.ReplaceDirectorySeparatorChar(cfg_.LastSaveDirectory);
			filename_ = XYMisc.AppendDirectorySeparatorChar(filename_);
			filename_ = (filename_ ?? string.Empty) + AsyncLevelLoader.LoadedLevelName + ".high";
		}
		protected override string Filename {
			get {
				return filename_;
			}
			set {
				this.filename_ = value;
			}
		}
		protected override bool Execute(string filename) {
			try {
				float[,] rawHigh = HighDataForServer_Util.GetRawHigh(0.5f);
				HighDataForServer_Util.SaveToFile(this.filename_, rawHigh, false);
				cfg_.LastSaveDirectory = System.IO.Path.GetDirectoryName(this.filename_);
				return true;
			} catch (System.Exception ex) {
				Owner.ChangeState(new State_AlertBox(Owner, ex.Message, new State_Normal(Owner)));
				return false;
			}
		}
	}
}