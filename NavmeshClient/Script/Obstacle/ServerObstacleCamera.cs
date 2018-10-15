using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

class ServerObstacleCamera : ObstacleCamera {

	private int xLineStart_ = -1;
	private int zLineStart_ = -1;	// 如果已经设置了障碍线起点，此两值不小于零

	private sbyte currentNum_ = 1;	// 当前编号

	protected override string GetDefaultFilename(string currentSceneName) {
		return Win32.SHGetFolderPath(Win32.CSIDL.CSIDL_MYDOCUMENTS) + "\\" + currentSceneName + ".svo";
	}

	private void OnDialogClose_SmartFill(State_Dialog.ResultType rt) {
		if (rt == State_Dialog.ResultType.Ok) {
			gridUtil.SmartFill_Obstacle(1);
		}
		ChangeState(new State_Normal(this));
	}

	protected override void DrawAllGrids() {
		this.gridUtil.DrawAll(currentNum_);
	}

	protected override ObstacleCamera.GridUtil.SaveType SaveType {
		get { return GridUtil.SaveType.ServerObstacle; }
	}

	protected override void SmartFill() {
		ChangeState(new State_Dialog(this, State_Dialog.ButtonStyle.OkCancel,
			"操作确认", "此操作将所有空气墙设为不可达，是否确认？",
			OnDialogClose_SmartFill));
	}

	protected override void OnKey_Control() {
		if (xLineStart_ < 0) {
			ChangeState(new State_DrawObstacleStart(this));
		} else {
			ChangeState(new State_DrawObstacleEnd(this));
		}
	}

	protected override void OnKey_Shift() {
		ChangeState(new State_Erase(this));
	}

	protected override void CheckOtherKey() {
		
	}

#if false
	protected override void CheckOtherKey() {
		int n = KeyChecker.IsNumKeyHolding();
		if (n > 0) {
			if (this.currentNum_ <= 0) {
				ChangeState(new State_SetNum(this));
			}
		}
		this.currentNum_ = (sbyte)n;
	}

	private static int ToPower2(int n) {
		int power = 2;
		while (n > power && power <= 4096) {
			power <<= 1;
		}
		return power;
	}

	private void BuildMapImage(string filename) {
		Terrain terrain = Terrain.activeTerrain;
		int cxMap, czMap;
		this.gridUtil.GetTerrainSize(out cxMap, out czMap);
		GameObject go = new GameObject("Temp Top View Camera");
		Camera camera = go.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = cxMap * 0.5f;
		camera.transform.position = new Vector3(cxMap * 0.5f, 500, czMap * 0.5f);
		camera.transform.forward = Vector3.down;
		camera.farClipPlane = 10000f;
		camera.nearClipPlane = 0.3f;
		camera.cullingMask = XYDefines.Layer.Mask.MainBuilding | XYDefines.Layer.Mask.Ignore | XYDefines.Layer.Mask.Prop | XYDefines.Layer.Mask.Ding | XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Terrain;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.black;
		camera.enabled = false;
		camera.depthTextureMode = DepthTextureMode.Depth;
		//
		RenderTexture rt = RenderTexture.GetTemporary(
			ToPower2(cxMap * 8),
			ToPower2(czMap * 8),
			0,
			RenderTextureFormat.RGB565, RenderTextureReadWrite.sRGB);
		camera.targetTexture = rt;
		RenderTexture.active = rt;
		camera.Render();

		UnityEngine.Object.Destroy(go);
		//
		Texture2D t2d = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
		t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, false);
		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(rt);
		//
		byte[] png = t2d.EncodeToPNG();
		UnityEngine.Object.Destroy(t2d);
		//
		using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
			fs.Write(png, 0, png.Length);
		}
	}
#endif

	protected override void ShowOtherButtons() {
#if false
		// 暂不提供
		if (GUI.Button(new Rect(0, Screen.height - 20, 100, 20), "生成平面图")) {
			try {
				ChangeState(new State_Dialog(this, State_Dialog.ButtonStyle.Ok, "完成", "地形PNG已成功保存",
					delegate(State_Dialog.ResultType dr) {
						ChangeState(new State_Normal(this));
					}
				));
			} catch (Exception ex) {
				ChangeState(new State_AlertBox(this, ex.Message, new State_Normal(this)));
			}
		}
#endif
		Rect rc = new Rect(0, Screen.height - 20, 120, 20);
		GUI.Label(rc, "当前工作编号");
		rc.xMin = rc.xMax;
		rc.xMax += 30;
		string s = GUI.TextField(rc, currentNum_.ToString());
		sbyte n;
		if (sbyte.TryParse(s, out n) && n > 0) {
			currentNum_ = n;
		}
	}

	/////////////////////////////////

	/// <summary>
	/// 设置障碍线的起点
	/// </summary>
	private class State_DrawObstacleStart : State_DrawOrErase {
		private readonly ServerObstacleCamera owner_;
		public State_DrawObstacleStart(ServerObstacleCamera owner) : base(owner, COLOR_MARKED_OBSTACLE) {
			this.owner_ = owner;
		}
		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButtonDown(0);
			}
		}

		protected override void Execute(int x, int z, int brushSize) {
			owner_.xLineStart_ = x;
			owner_.zLineStart_ = z;
			Owner.gridUtil.Set(x, z, PathFindDataType.MarkedObstacle, owner_.currentNum_, brushSize);
		}

		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("设置障碍线起点 ({0},{1})", Owner.xBrush, Owner.zBrush);
				} else {
					return null;
				}
			}
		}

		protected override bool IsLiving() {
			return owner_.xLineStart_ < 0 && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}
	}

	/// <summary>
	/// 设置障碍线的终点
	/// </summary>
	private class State_DrawObstacleEnd : State_DrawOrErase {
		private readonly ServerObstacleCamera owner_;
		public State_DrawObstacleEnd(ServerObstacleCamera owner)
			: base(owner, COLOR_MARKED_OBSTACLE) {
			owner_ = owner;
		}

		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("设置障碍线终点 ({0},{1})", Owner.xBrush, Owner.zBrush);
				} else {
					return null;
				}
			}
		}

		protected override void Execute(int x, int z, int brushSize) {
			Owner.gridUtil.SetLine(owner_.xLineStart_, owner_.zLineStart_, x, z, PathFindDataType.MarkedObstacle, owner_.currentNum_, brushSize);
			owner_.xLineStart_ = owner_.zLineStart_ = -1;
		}

		public override void OnPostRender() {
			base.OnPostRender();
			if (Owner.IsBrushPosValid) {
				Owner.gridUtil.SetColor(COLOR_MARKED_OBSTACLE);
				Owner.gridUtil.DrawGrid(Owner.xBrush, Owner.zBrush, Owner.BrushSize);
				GL.Begin(GL.LINES);
				GL.Vertex3(owner_.xLineStart_ + 0.5f, Owner.gridUtil.GetHigh(owner_.xLineStart_, owner_.zLineStart_), owner_.zLineStart_ + 0.5f);
				GL.Vertex3(Owner.xBrush + 0.5f, Owner.gridUtil.GetHigh(Owner.xBrush, Owner.zBrush), Owner.zBrush + 0.5f);
				GL.End();
			}
		}

		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButtonDown(0);
			}
		}

		protected override bool IsLiving() {
			return owner_.xLineStart_ >= 0 && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}

	}


	/// <summary>
	/// 删除格子
	/// </summary>
	private class State_Erase : State_DrawOrErase {

		private readonly ServerObstacleCamera owner_;

		public State_Erase(ServerObstacleCamera owner) : base(owner, COLOR_ERASE) {
			this.owner_ = owner;
		}

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
			owner_.xLineStart_ = owner_.zLineStart_ = -1;
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


	private class State_SetNum : State_DrawOrErase {
		private readonly ServerObstacleCamera owner_;
		public State_SetNum(ServerObstacleCamera owner)
			: base(owner, COLOR_PRIOR) {
			this.owner_ = owner;
		}

		protected override bool IsMouseButtonDown {
			get {
				return Input.GetMouseButtonDown(0);
			}
		}

		protected override void Execute(int x, int z, int brushSize) {
			Owner.gridUtil.Set(x, z, PathFindDataType.MarkedObstacle, owner_.currentNum_, brushSize);
		}

		public override string Desc {
			get {
				if (Owner.IsBrushPosValid) {
					return string.Format("编号 {0}", owner_.currentNum_);
				} else {
					return null;
				}
			}
		}

		protected override bool IsLiving() {
			return KeyChecker.IsNumKeyHolding() > 0;
		}
	}



	//////////////////////////////////////////

}
