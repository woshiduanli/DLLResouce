using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Text;
public class XYObstacleCamera : MonoBehaviour {
    public const int FLY_LAYER = 8;
    public const int SAFE_ZONE_LAYER = 10;

    // Fields
    private static GameObject airWall_;
    private XYCameraSmooth cameraSmooth_ = new XYCameraSmooth();
    private CmdStack cmdStack_ = new CmdStack(50);
    public Color ColorAddPoint = new Color(0f, 0f, 1f, 0.5f);
    public Color ColorRemoveLine = new Color(1f, 0f, 0f, 0.5f);
    private Status currentStatus_ = StatusNormal.Instance;
    private float distance_;
    private Transform ori_transform;
    private Vector3 ori_lookAt;
    private Vector3 ori_euler;
    private Effect effect_;
    public bool enabletopview = false;
    private Vector3 euler_ = new Vector3(-89f, 0f, 0f);
    public const float FastSpeed = 200f;
    public string obstacle_filename;
    public string zone_filename;
    private GridPosToWorldPos gridPosToWorldPos_ = new GridPosToWorldPos();
    private bool hasPosOfLineBegin_;
    private Vector3 lookAt_;
    public const float NormalSpeed = 100f;
    private XYObstacleLineList obstacleLineList_ = new XYObstacleLineList();
    private bool isDrawQuads_ = false;
    private XYGridPos posOfLineBegin_;
    private static bool showAirWall_ = true;
    private static bool showCollider_ = true;
    private static bool showNavLine_ = false;
    private static bool showNavPoint_ = true;
    private static bool freeMode_ = false;
    private static bool safeZone_ = false;
    public const float SlowSpeed = 1f;
    private Camera topviewcamera;
    private Matrix4x4 topviewcamera_vp;
    private GameObject topviewgameobject;
    private RenderTexture topviewrendertexture;
    public const float TurnSpeed = 3f;
    private short xMapSize_;
    private short zMapSize_;

    private UnityEngine.AI.NavMeshTriangulation navTriangles_;
    private bool bCalcNavTriangles_;
    //private List<NavGridPosLineInfo> navLines_ = new List<NavGridPosLineInfo>();
    //private List<XYGridPos> LayerPos_ = new List<XYGridPos>();
    private List<Vector3> FilterNavPos_ = new List<Vector3>();
    private List<NavLineInfo> FilterNavline_ = new List<NavLineInfo>();
    private List<int> navLayers_ = new List<int>();
    private int curLayer_ = 0;
    private bool bFallToPlane_;
    private GameObject empty_plane_;
    //安全区域相关定义
    private byte[,] safeZoneByteArray_;
    private List<XYGridPos> safeZonePolyPos_ = new List<XYGridPos>();
    private List<List<XYGridPos>> safeZoneLoopPolys_ = new List<List<XYGridPos>>();
    private static int polyPosIndex;
    private static int polyLineIndex;
    //private List<XYGridPos> fillPolySafeZonePos_ = new List<XYGridPos>();
    private Dictionary<XYGridPos, bool> fillPolySafeZonePos_ = new Dictionary<XYGridPos, bool>();
    private XYObstacleLineList polySafeZoneLine_ = new XYObstacleLineList();
    private static bool isSelectedPoly_ = false;
    private static bool isLoopPoly = false;
    // private List<XYGridPos> safeZoneRectPos_ = new List<XYGridPos>();
    // private List<XYGridPos> safeZoneCriclePos_ = new List<XYGridPos>();
    //  private List<XYGridPos> fillRectSafeZonePos_ = new List<XYGridPos>();
    //  private List<XYGridPos> fillCircleSafeZonePos_ = new List<XYGridPos>();
    //  private List<XYGridPos> LoadSafeZonePos_ = new List<XYGridPos>();
    // private static bool isSelectedRec_ = false;
    // private static bool isSelectedCircle_ = false;
    public class NavGridPosLineInfo {
        public XYGridPos bPos_;
        public XYGridPos ePos_;

        public NavGridPosLineInfo(XYGridPos b_pos, XYGridPos e_pos) {
            bPos_ = b_pos;
            ePos_ = e_pos;
        }
    }

    public class NavLineInfo {
        public int bIndex_;
        public int eIndex_;

        public NavLineInfo(int b_index, int e_index) {
            bIndex_ = b_index;
            eIndex_ = e_index;
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            if (this.GetType() != obj.GetType()) {
                return false;
            }
            NavLineInfo line = (NavLineInfo)obj;
            return line == this;
        }
        public static bool operator ==(NavLineInfo l1, NavLineInfo l2) {
            return (l1.bIndex_ == l2.bIndex_ && l1.eIndex_ == l2.eIndex_) || (l1.bIndex_ == l2.eIndex_ && l1.eIndex_ == l2.bIndex_);
        }
        public static bool operator !=(NavLineInfo l1, NavLineInfo l2) {
            return !(l1 == l2);
        }
    };

    // Methods
    private void AddVertexObstacleLine() {
        XYGridPos[] posArray = new XYGridPos[] { new XYGridPos(0, 0), new XYGridPos(0, (short)(this.zMapSize_ - 1)), new XYGridPos((short)(this.xMapSize_ - 1), (short)(this.zMapSize_ - 1)), new XYGridPos((short)(this.xMapSize_ - 1), 0) };
        for (int i = 0; i < posArray.Length; i++) {
            this.obstacleLineList_.Add(new XYObstacleLine(posArray[i], posArray[(i + 1) % posArray.Length]));
        }
    }

    private static float AlignToGrid(float v) {
        return (Mathf.Floor(v * 2f) * 0.5f);
    }

    private void AlignToGrid(ref Vector3 pos, bool useTerrainHeight) {
        this.AlignToGridCenter(ref pos, useTerrainHeight);
        pos.x -= 0.25f;
        pos.z -= 0.25f;
    }

    private static float AlignToGridCenter(float v) {
        return ((Mathf.Floor(v * 2f) * 0.5f) + 0.25f);
    }

    private void AlignToGridCenter(ref Vector3 pos, bool useTerrainHeight) {
        RaycastHit hit;
        pos.x = AlignToGrid(pos.x) + 0.25f;
        pos.z = AlignToGrid(pos.z) + 0.25f;
        if (Physics.Raycast(pos, Vector3.down, out hit)) {
            pos.y = hit.point.y;
        } else {
            pos.y = 0f;
        }
    }

    private void Awake() {
        this.effect_ = new Effect(base.GetComponent<Camera>());
        this.obstacle_filename = XYDirectory.ProjectRoot + AsyncLevelLoader.LoadedLevelName + ".obstacle";
        this.zone_filename = XYDirectory.ProjectRoot + AsyncLevelLoader.LoadedLevelName + ".zonepoint.txt";
        this.xMapSize_ = (short)Mathf.FloorToInt((float)(MapPlacementController.curMap.Width * 2));
        this.zMapSize_ = (short)Mathf.FloorToInt((float)(MapPlacementController.curMap.Height * 2));
        this.lookAt_ = new Vector3(MapPlacementController.curMap.Width * 0.5f, 0f, MapPlacementController.curMap.Height * 0.5f);
        this.lookAt_.y = GetTerrainHeight(this.lookAt_);
        this.distance_ = 500f;
        ori_transform = base.transform;
        ori_lookAt = this.lookAt_;
        ori_euler = this.euler_;
        this.cameraSmooth_.Active = false;
        this.AddVertexObstacleLine();
        XYClientCommon.AddComponent<AudioListener>(base.GetComponent<Camera>().gameObject);
        this.CreateTopViewCamera();
        airWall_ = XYClientCommon.AirWall.FindRoot();
        showAirWall_ = true;
        if (airWall_ != null) {
            airWall_.SetActive(true);
        }
        MakePlane();
    }
    private void CameraReset() {
       // base.transform = ori_transform;
        this.lookAt_ = ori_lookAt;
        this.euler_ = ori_euler;
        this.distance_ = 500f;
        this.cameraSmooth_.Execute(base.transform, this.lookAt_, this.euler_, this.distance_);
    }
    Mesh CreateMesh(float width, float height) {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-5, 0, -5),
         new Vector3(width+5, 0, -5),
         new Vector3(width+5, 0, height+5),
         new Vector3(-5, 0, height+5)
        };
        m.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2(1, 1),
         new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }

    private void MakePlane() {
        empty_plane_ = new GameObject("New_Empty_Plane");
        MeshFilter meshFilter = (MeshFilter)empty_plane_.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(xMapSize_ * 0.5f, zMapSize_ * 0.5f);
        MeshRenderer renderer = empty_plane_.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(39 / 255.0f, 61 / 255.0f, 71 / 255.0f));
        tex.Apply();
        renderer.material.mainTexture = tex;
        empty_plane_.layer = XYDefines.Layer.Terrain;
        BoxCollider collider = (BoxCollider)empty_plane_.AddComponent(typeof(BoxCollider));
        collider.size = new Vector3(xMapSize_ * 0.5f + 10, 0, zMapSize_ * 0.5f + 10);
        collider.center = new Vector3(xMapSize_ * 0.25f + 5, 0, zMapSize_ * 0.25f + 5);
        empty_plane_.SetActive(false);
    }

    private void CreateTopViewCamera() {
        this.topviewgameobject = new GameObject("topviewcamera");
        this.topviewgameobject.transform.rotation = (Quaternion.Euler(90f, 0f, 0f));
        this.topviewcamera = this.topviewgameobject.AddComponent<Camera>();
        this.topviewrendertexture = RenderTexture.GetTemporary(0x200, 0x200, 0x10);
        this.topviewcamera.clearFlags = CameraClearFlags.SolidColor;
        this.topviewcamera.backgroundColor = (Color.gray);
        this.topviewcamera.enabled = (false);
        this.topviewcamera.orthographic = (true);
        this.topviewcamera.orthographicSize = (50f);
        this.topviewcamera.nearClipPlane = (0.3f);
        this.topviewcamera.farClipPlane = (5000f);
        this.topviewcamera.cullingMask = (0x38005);
        this.topviewcamera.targetTexture = (this.topviewrendertexture);
    }

    private static void DrawGrid(Vector3 pos, Color color) {
        GL.Begin(7);
        GL.Color(color);
        Vector3 p = pos;
        p.x += 0.25f;
        p.z += 0.25f;
        GL.Vertex(p);

        p = pos;
        p.x -= 0.25f;
        p.z += 0.25f;
        GL.Vertex(p);

        p = pos;
        p.x -= 0.25f;
        p.z -= 0.25f;
        GL.Vertex(p);

        p = pos;
        p.x += 0.25f;
        p.z -= 0.25f;
        GL.Vertex(p);
        GL.End();
        GL.Clear(false, false, Color.clear);
    }

    private static void ForEachCollider(Action<GameObject> action) {
        GameObject[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj2 in objArray) {
            if (obj2.name.Contains("collider")) {
                action(obj2);
            }
        }
    }

    private bool GetGridPosFromMouse(out XYGridPos pos, out Vector3 worldpos, XYObstacleCamera owner) {
        RaycastHit hit;
        if (Physics.Raycast(owner.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit)) {
            short x = Convert.ToInt16(Mathf.FloorToInt(hit.point.x * 2f));
            pos = new XYGridPos(x, Convert.ToInt16(Mathf.FloorToInt(hit.point.z * 2f)));
            worldpos = hit.point;
            return true;
        }
        pos = XYGridPos.Zero;
        worldpos = Vector3.zero;
        return false;
    }
    //安全区域 ,用于计算两个对角线点可以组成的矩形的点
    private void SetSafeZonePos(RaycastHit hit, XYObstacleCamera owner) {

        if (Physics.Raycast(owner.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit)) {
            short x = Convert.ToInt16(Mathf.FloorToInt(hit.point.x * 2f));
            XYGridPos pos = new XYGridPos(x, Convert.ToInt16(Mathf.FloorToInt(hit.point.z * 2f)));
            //if (XYObstacleCamera.isSelectedRec_) {
            //    safeZoneRectPos_.Add(pos);
            //    SetRectSafeZonArray();
            //}
            //if (XYObstacleCamera.isSelectedCircle_) {
            //    safeZoneCriclePos_.Add(pos);
            //    SetCircleSafeZonArray();
            //}
            if (XYObstacleCamera.isSelectedPoly_)
                SetSafeZonePoly(pos);
        }
    }
    private void SetSafeZonePoly(XYGridPos pos) {
        if (safeZonePolyPos_.Count == 0 || isLoopPoly) {
            isLoopPoly = false;
            safeZonePolyPos_.Add(pos);
            return;
        }
        safeZonePolyPos_.Add(pos);
        if (safeZonePolyPos_[polyPosIndex] == pos) {
            List<XYGridPos> list = new List<XYGridPos>();
            for (int i = polyPosIndex; i < safeZonePolyPos_.Count; i++)
                list.Add(safeZonePolyPos_[i]);
            safeZoneLoopPolys_.Add(list);
            SetPolySafeZonArray();
            PolyLine(pos, true);
            isLoopPoly = true;
            polyPosIndex = safeZonePolyPos_.Count;
        } else
            PolyLine(pos, false);


    }
    private int CalculateLoopPosNum(XYGridPos pos) {
        int loop_num = 0;
        foreach (XYGridPos item in safeZonePolyPos_) {
            if (item == pos)
                loop_num++;
        }
        return loop_num;
    }
    private int ReturnLastLoopPosIndex() {
        int index = -1;
        bool last_loop = false;
        for (int i = safeZonePolyPos_.Count - 1; i >= 0; i--) {
            for (int j = i - 1; j >= 0; j--) {
                if (safeZonePolyPos_[j] == safeZonePolyPos_[i]) {
                    index = i;
                    last_loop = true;
                    break;
                }
                if (last_loop)
                    break;
            }
        }
        return (index +1);
    }
    //撤销安全区域点线
    private void RemovePolyLine() {
        XYGridPos pos = safeZonePolyPos_[safeZonePolyPos_.Count - 1];
        if (CalculateLoopPosNum(pos) == 2 && safeZoneLoopPolys_.Count > 0) {
            safeZoneLoopPolys_.RemoveAt(safeZoneLoopPolys_.Count - 1);
            isLoopPoly = false;
        }
        safeZonePolyPos_.RemoveAt(safeZonePolyPos_.Count - 1);
        if (safeZoneLoopPolys_.Count > 0 && CalculateLoopPosNum(safeZonePolyPos_[safeZonePolyPos_.Count - 1]) == 2) {
            isLoopPoly = true;
            polyLineIndex = safeZonePolyPos_.Count > 0 ? safeZonePolyPos_.Count : 0;
            polyPosIndex = safeZonePolyPos_.Count > 0 ? safeZonePolyPos_.Count : 0;
        } else {
            polyLineIndex = safeZonePolyPos_.Count > 0 ? safeZonePolyPos_.Count - 1 : 0;
            polyPosIndex = ReturnLastLoopPosIndex();
        }
        for (int i = polySafeZoneLine_.Count - 1; i >= 0; i--) {
            if (polySafeZoneLine_.PointList.ContainsKey(pos))
                polySafeZoneLine_.PointList.Remove(pos);
            if (polySafeZoneLine_.list_[i].End == pos)
                polySafeZoneLine_.list_.Remove(polySafeZoneLine_.list_[i]);
        }
    }
    //安全区域多边形线段添加
    private void PolyLine(XYGridPos pos, bool is_loop) {
        for (int i = polyLineIndex; i < safeZonePolyPos_.Count - 1; i++) {
            bool add = AddLine(safeZonePolyPos_[i], safeZonePolyPos_[(i + 1) % safeZonePolyPos_.Count]);
            if (!add && !is_loop) {
                safeZonePolyPos_.Remove(pos);
                return;
            }
            polySafeZoneLine_.Add(new XYObstacleLine(safeZonePolyPos_[i], safeZonePolyPos_[(i + 1) % safeZonePolyPos_.Count]));
        }
        if (is_loop)
            polyLineIndex = safeZonePolyPos_.Count;
        else
            polyLineIndex = safeZonePolyPos_.Count - 1;
    }
    private short CalculateMaxYX(List<XYGridPos> pos_list, bool max_x) {
        short max = short.MinValue;
        for (int i = 0; i < pos_list.Count; i++) {
            if (max_x) {
                if (max < pos_list[i].x)
                    max = pos_list[i].x;
            } else {
                if (max < pos_list[i].y)
                    max = pos_list[i].y;
            }
        }
        return max;
    }
    private short CalculateMinYX(List<XYGridPos> pos_list, bool min_x) {
        short min = short.MaxValue;
        for (int i = 0; i < pos_list.Count; i++) {
            if (min_x) {
                if (min > pos_list[i].x)
                    min = pos_list[i].x;
            } else {
                if (min > pos_list[i].y)
                    min = pos_list[i].y;
            }
        }
        return min;
    }
    private bool PointInPolygon(XYGridPos pos, List<XYGridPos> polygon) {
        int i;
        int j = polygon.Count - 1;
        bool oddNodes = false;
        for (i = 0; i < polygon.Count; i++) {
            if ((polygon[i].y < pos.y && polygon[j].y >= pos.y || polygon[j].y <pos.y && polygon[i].y >= pos.y)
                && (polygon[i].x <= pos.x || polygon[j].x <= pos.x)) {
                if ((float)polygon[i].x +(float)(pos.y - polygon[i].y) /(float) (polygon[j].y - polygon[i].y) * (float)(polygon[j].x - polygon[i].x) < (float)pos.x) {
                    oddNodes = !oddNodes;
                }
            }
            j = i;
        }
        return oddNodes;
    }
    //计算两点之间的距离
    private float CalculateDistance(XYGridPos pos1, XYGridPos pos2) {
        float distance = Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
        return distance;
    }
    //计算多边形的安全区域
    private void SetPolySafeZonArray() {
        if (safeZonePolyPos_.Count == 0)
            return;
        fillPolySafeZonePos_.Clear();
        for (int i = 0; i < safeZoneLoopPolys_.Count; i++) {
            int minX = CalculateMinYX(safeZoneLoopPolys_[i], true);
            int minY = CalculateMinYX(safeZoneLoopPolys_[i], false);
            int maxX = CalculateMaxYX(safeZoneLoopPolys_[i], true);
            int maxY = CalculateMaxYX(safeZoneLoopPolys_[i], false);
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++) {
                    XYGridPos pos = new XYGridPos((short)x, (short)y);
                    if (PointInPolygon(pos, safeZoneLoopPolys_[i])) {
                        if (!fillPolySafeZonePos_.ContainsKey(pos))
                            fillPolySafeZonePos_.Add(pos, true);
                    }
                }
        }
    }
    #region 安全区域舍弃代码
    //计算圆形安全区域
    //private void SetCircleSafeZonArray() {
    //    if (safeZoneCriclePos_.Count == 0)
    //        return;
    //    for (int i = 0; i < safeZoneCriclePos_.Count; i += 2) {
    //        if (i + 1 < safeZoneCriclePos_.Count) {
    //            float radius = CalculateDistance(safeZoneCriclePos_[i], safeZoneCriclePos_[i + 1]);
    //            int beginX = safeZoneCriclePos_[i].x - (int)radius;
    //            int beginY = safeZoneCriclePos_[i].y - (int)radius;
    //            int endX = safeZoneCriclePos_[i].x + (int)radius;
    //            int endY = safeZoneCriclePos_[i].y + (int)radius;
    //            for (int j = beginX-1; j < endX+1; j++) {
    //                for (int k = beginY-1; k < endY+1; k++) {
    //                    XYGridPos pos = new XYGridPos((short)j, (short)k);
    //                    float distacne = CalculateDistance(pos, safeZoneCriclePos_[i]);
    //                    if (distacne <= radius) {
    //                        safeZoneByteArray_[j, k] = (byte)1;
    //                        if (!fillCircleSafeZonePos_.Contains(pos))
    //                            fillCircleSafeZonePos_.Add(pos);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    ////计算矩形安全区域
    //private void SetRectSafeZonArray() {
    //    if (safeZoneRectPos_.Count == 0)
    //        return;
    //    for (int i = 0; i < safeZoneRectPos_.Count; i += 2) {
    //        if (i + 1 < safeZoneRectPos_.Count) {
    //            List<XYGridPos> rect_pos = new List<XYGridPos>();
    //            rect_pos.Add(safeZoneRectPos_[i]);
    //            rect_pos.Add(safeZoneRectPos_[i + 1]);
    //            int beginX, beginY, endX, endY;
    //            beginX = CalculateMinYX(rect_pos, true);
    //            beginY = CalculateMinYX(rect_pos, false);
    //            endX = CalculateMaxYX(rect_pos, true);
    //            endY = CalculateMaxYX(rect_pos, false);
    //            for (int j = beginX; j <= endX; j++) {
    //                for (int k = beginY; k <= endY; k++) {
    //                    XYGridPos pos = new XYGridPos((short)j, (short)k);
    //                    safeZoneByteArray_[j, k] = (byte)1;
    //                    if (!fillRectSafeZonePos_.Contains(pos))
    //                        fillRectSafeZonePos_.Add(pos);
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion
    //保存安全区域填充点
    private void SaveSafeZoneArray() {
        foreach (XYGridPos pos in fillPolySafeZonePos_.Keys) {
            safeZoneByteArray_[pos.x, pos.y] = (byte)1;
        }
    }

    public static float GetSpeedFromInput() {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) {
            return FastSpeed;
        } else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            return SlowSpeed;
        } else {
            return NormalSpeed;
        }
    }

    public static float GetTerrainHeight(Vector3 pos) {
        RaycastHit hit;
        pos.y += 1000f;
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, XYDefines.Layer.Mask.Terrain)) {
            return hit.point.y;
        }
        return 0f;
    }

    private bool GetWorldPosFromMouse(out Vector3 pos, out int layer, bool alignToGrid) {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            pos = hit.point;
            layer = hit.collider.gameObject.layer;
            if (alignToGrid) {
                AlignToGrid(ref pos, false);
            }
            return true;
        } else {
            pos = Vector3.zero;
            layer = -1;
            return false;
        }
    }

    private bool IsPointInTopView(XYGridPos point) {
        Vector3 vector = new Vector3((point.x * 0.5f) + 0.25f, 0f, (point.y * 0.5f) + 0.25f);
        Vector3 vector2 = this.topviewcamera.projectionMatrix.MultiplyPoint(this.topviewcamera.worldToCameraMatrix.MultiplyPoint(vector));
        if ((((vector2.x < -1f) || (vector2.x > 1f)) || (vector2.y < -1f)) || (vector2.y > 1f)) {
            return false;
        }
        return true;
    }

    private void LateUpdate() {
        this.cameraSmooth_.Execute(base.transform, this.lookAt_, this.euler_, this.distance_);
        this.topviewgameobject.transform.position = (new Vector3(this.lookAt_.x, 3000f, this.lookAt_.z));
        this.topviewgameobject.transform.rotation = (Quaternion.Euler(90f, Camera.main.transform.rotation.eulerAngles.y, 0f));
        this.topviewcamera_vp = this.topviewcamera.projectionMatrix * this.topviewcamera.worldToCameraMatrix;
    }

    private void LoadObstacleFromFile() {
        if (this.obstacle_filename != string.Empty || this.zone_filename !=string.Empty) {
            try {
                if (!File.Exists(this.obstacle_filename)) {
                    throw new Exception(string.Format("文件不存在：{0}", this.obstacle_filename));
                }
                if(!File.Exists(this.zone_filename)) {
                    throw new Exception(string.Format("文件不存在：{0}", this.zone_filename));
                }
                //选择安全区域复选框才会导入安全区域的文件，否则不导入。
                if (XYObstacleCamera.safeZone_) {
                    if (!File.Exists(AsyncLevelLoader.LoadedLevelName + ".zonepoint.txt"))
                        throw new Exception(string.Format("文件不存在：{0}", AsyncLevelLoader.LoadedLevelName + ".zonepoint.txt"));
                    using (StreamReader reader = new StreamReader(AsyncLevelLoader.LoadedLevelName + ".zonepoint.txt")) {
                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            string[] array = line.Split(' ');
                            XYGridPos pos = new XYGridPos(short.Parse(array[0]), short.Parse(array[1]));
                            SetSafeZonePoly(pos);
                        }
                    }
                } else {
                    using (FileStream stream = new FileStream(this.obstacle_filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        using (BinaryReader reader = new BinaryReader(stream)) {
                            this.obstacleLineList_ = XYObstacleSerializer.ReadFromBinary(reader, out this.xMapSize_, out this.zMapSize_);
                            this.AddVertexObstacleLine();
                        }
                    }

                    this.hasPosOfLineBegin_ = false;
                    this.gridPosToWorldPos_.Clear();
                    this.CurrentStatus = StatusModalDialog.Show("成功", "从文件中加载障碍数据成功！");
                }
            } catch (Exception exception) {
                this.CurrentStatus = StatusModalDialog.Show("错误", exception.Message);
                Debug.Log(exception.StackTrace);
            }

        }
    }


    private void OnGUI() {
        switch (Event.current.type) {
            case EventType.Ignore:
            case EventType.Used:
                return;
            default:
                break;
        }
        GUIContent content = new GUIContent(string.Format("线段总数：{0}", this.obstacleLineList_.Count.ToString()));
        Vector2 vector = GUI.skin.label.CalcSize(content);
        GUI.Label(new Rect(0f, Screen.height - vector.y, vector.x, vector.y), content);
        content = new GUIContent(string.Format("当前功能：{0}", this.CurrentStatus.Desc));
        vector = GUI.skin.label.CalcSize(content);
        GUI.Label(new Rect((Screen.width - vector.x) / 2f, Screen.height - vector.y, vector.x, vector.y), content);
        content = new GUIContent(string.Format("摄像机：高度={0}, 距离={1}", base.transform.position.y, this.distance_));
        vector = GUI.skin.label.CalcSize(content);
        GUI.Label(new Rect(Screen.width - vector.x, Screen.height - vector.y, vector.x, vector.y), content);
        string str = string.Empty;
        int num = 0;
        foreach (KeyValuePair<XYGridPos, BegeinEndNum> pair in this.obstacleLineList_.PointList) {
            if (pair.Value.begin_num != pair.Value.end_num) {
                str = str + pair.Key.ToString() + "\n";
                num++;
            }
        }
        content = new GUIContent("奇点个数：" + num.ToString() + "\n" + str);
        vector = GUI.skin.label.CalcSize(content);
        GUI.Label(new Rect(Screen.width - vector.x, (Screen.height - vector.y) / 2f, vector.x, vector.y), content);

        if (this.enabletopview) {
            this.topviewcamera.Render();
            RenderTexture texture = RenderTexture.active;
            RenderTexture.active = null;
            Graphics.DrawTexture(new Rect(0f, 0f, 512f, 512f), this.topviewrendertexture);
            RenderTexture.active = texture;

            GL.PushMatrix();
            this.effect_.BeforeUseGL();
            GL.LoadPixelMatrix(0f, (float)Screen.width, 0f, (float)Screen.height);
            GL.Begin(1);
            GL.Color(c3);
            foreach (XYObstacleLine line in this.obstacleLineList_) {
                if (this.IsPointInTopView(line.Begin) || this.IsPointInTopView(line.End)) {
                    Vector3 vector2 = ((Vector3)(this.WordPositionToTopView(this.gridPosToWorldPos_.Execute(line.Begin)) * 0.5f)) + new Vector3(0.5f, 0.5f, 0f);
                    Vector3 vector3 = ((Vector3)(this.WordPositionToTopView(this.gridPosToWorldPos_.Execute(line.End)) * 0.5f)) + new Vector3(0.5f, 0.5f, 0f);
                    vector2.y = 1f - vector2.y;
                    vector3.y = 1f - vector3.y;
                    vector2.z = 0f;
                    vector3.z = 0f;
                    vector2 = (Vector3)(vector2 * 512f);
                    vector3 = (Vector3)(vector3 * 512f);
                    vector2.y = Screen.height - vector2.y;
                    vector3.y = Screen.height - vector3.y;
                    GL.Vertex(vector2);
                    GL.Vertex(vector3);
                }
            }
            GL.End();
            GL.PopMatrix();
        }
        this.CurrentStatus.OnGUI(this);

        this.enabletopview = GUI.Toggle(new Rect((float)(Screen.width - 500), 30f, 100f, 20f), enabletopview, "enabletopview");
    }

    Color32 c1 = new Color32(0, 0, 255, 125);
    Color32 c2 = new Color32(0, 255, 255, 125);
    Color32 c3 = new Color32(255, 0, 0, 255);
    Color32 c4 = new Color32(0, 255, 0, 125);
    private void OnPostRender() {
        effect_.BeforeUseGL();
        if (bCalcNavTriangles_) {
            if (showNavLine_) {   //辅助线
                GL.Begin(GL.LINES);
                GL.Color(Color.yellow);
                //foreach ( NavGridPosLineInfo line in navLines_ ) {
                //    GL.Vertex( gridPosToWorldPos_.Execute( line.bPos_ ) );
                //    GL.Vertex( gridPosToWorldPos_.Execute( line.ePos_ ) );
                //}

                foreach (NavLineInfo line in FilterNavline_) {
                    GL.Vertex(FilterNavPos_[line.bIndex_]);
                    GL.Vertex(FilterNavPos_[line.eIndex_]);
                }
                GL.End();
            }

            if (showNavPoint_) {   //辅助点
                GL.Begin(GL.QUADS);
                GL.Color(Color.red);
                //foreach ( XYGridPos vertice in LayerPos_ ) {
                //    //XYGridPos bPos = new XYGridPos( Convert.ToInt16( Mathf.FloorToInt( vertice.x * 2f ) ),
                //    //Convert.ToInt16( Mathf.FloorToInt( vertice .z* 2f ) ) );

                //    SubmitQuadVertex( vertice );
                //}

                foreach (Vector3 vertice in FilterNavPos_) {
                    Vector3 vector = vertice;
                    vector.x -= 0.25f;
                    GL.Vertex(vector);
                    vector.z += 0.25f;
                    GL.Vertex(vector);
                    vector.x += 0.25f;
                    GL.Vertex(vector);
                    vector.z -= 0.25f;
                    GL.Vertex(vector);
                }
                GL.End();
            }

            //GL.Begin( GL.LINES );
            //GL.Color( Color.yellow );
            //for ( int i = 0; i < navTriangles_.indices.Length; ) {
            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i]] );
            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i + 1]] );

            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i + 1]] );
            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i + 2]] );

            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i + 2]] );
            //    GL.Vertex( navTriangles_.vertices[navTriangles_.indices[i]] );
            //    i += 3;
            //}
            //GL.End();

            //GL.Begin( GL.QUADS );
            //GL.Color( Color.red );
            //foreach ( Vector3 vertices in navTriangles_.vertices ) {
            //    Vector3 vector = vertices;
            //    vector.x -= 0.25f;
            //    GL.Vertex( vector );
            //    vector.z += 0.25f;
            //    GL.Vertex( vector );
            //    vector.x += 0.25f;
            //    GL.Vertex( vector );
            //    vector.z -= 0.25f;
            //    GL.Vertex( vector );
            //}
            //GL.End();
        }

        //// 画线段
        GL.Begin(GL.LINES);
        {
            GL.Color(Color.red);
            foreach (XYObstacleLine line in this.obstacleLineList_) {
                Vector3 begin = gridPosToWorldPos_.Execute(line.Begin);
                Vector3 end = gridPosToWorldPos_.Execute(line.End);
                if (bFallToPlane_) {
                    begin.y = 0;
                    end.y = 0;
                }
                GL.Vertex(begin);
                GL.Vertex(end);
            }
        }
        GL.End();
        //画安全区域多边形线段

        GL.Begin(GL.LINES);
        {
            GL.Color(Color.green);
            foreach (XYObstacleLine line in this.polySafeZoneLine_) {
                Vector3 begin = gridPosToWorldPos_.Execute(line.Begin);
                Vector3 end = gridPosToWorldPos_.Execute(line.End);
                if (bFallToPlane_) {
                    begin.y = 0;
                    end.y = 0;
                }
                GL.Vertex(begin);
                GL.Vertex(end);
            }
        }
        GL.End();
       // 画线段的端点

        GL.Begin(GL.QUADS);
        {
            GL.Color(c1);
            foreach (XYObstacleLine line in this.obstacleLineList_) {
                SubmitQuadVertex(line.Begin);
                SubmitQuadVertex(line.End);
            }
            //画安全区域的端点
            //foreach (XYGridPos gridPos in safeZoneRectPos_) {
            //    SubmitQuadVertex(gridPos);
            //}
            //foreach (XYGridPos gridPos in safeZoneCriclePos_) {
            //    SubmitQuadVertex(gridPos);
            //}
            //foreach (XYGridPos gridPos in LoadSafeZonePos_) {
            //    SubmitQuadVertex(gridPos);
            //
        }
        GL.End();
        //画安全区域闭合的点
        GL.Begin(GL.QUADS);
        {
            GL.Color(c1);
            for (int i = 0; i < safeZoneLoopPolys_.Count; i++) {
                for (int j = 0; j < safeZoneLoopPolys_[i].Count; j++) {
                    SubmitQuadVertex(safeZoneLoopPolys_[i][j]);
                }
            }
        }
        GL.End();
        //画安全区域独立点
        GL.Begin(GL.QUADS);
        {
            GL.Color(Color.white);
            int last_loop = 0;
            for (int i = 0; i < safeZoneLoopPolys_.Count; i++) {
                last_loop += safeZoneLoopPolys_[i].Count;
            }
            for (int j = last_loop; j < safeZonePolyPos_.Count; j++) {
                SubmitQuadVertex(safeZonePolyPos_[j]);
            }
        }
        GL.End();
        //画所有奇数点
        GL.Begin(GL.QUADS);
        {
            GL.Color(Color.white);
            foreach (KeyValuePair<XYGridPos, BegeinEndNum> kvp in obstacleLineList_.PointList) {
                if (kvp.Value.begin_num != kvp.Value.end_num) {
                    SubmitQuadVertex(kvp.Key);
                }
            }
        }
        GL.End();

        // 如果有起始点，画出来
        if (hasPosOfLineBegin_) {
            GL.Color(c2);
            GL.Begin(GL.QUADS);
            SubmitQuadVertex(posOfLineBegin_);
            GL.End();
        }

        //填充安全区域

        //if (fillRectSafeZonePos_.Count > 0) {
        //    GL.Begin(GL.QUADS);
        //    {
        //        GL.Color(c4);
        //        foreach (XYGridPos gridPos in fillRectSafeZonePos_) {
        //            SubmitQuadVertex(gridPos);
        //        }
        //    }
        //    GL.End();
        //}
        //if (fillCircleSafeZonePos_.Count > 0) {
        //    GL.Begin(GL.QUADS);
        //    {
        //        GL.Color(c4);
        //        foreach (XYGridPos gridPos in fillCircleSafeZonePos_) {
        //            SubmitQuadVertex(gridPos);
        //        }
        //    }
        //    GL.End();
        //}
        //安全区域填充
        if (fillPolySafeZonePos_.Count > 0) {
            GL.Begin(GL.QUADS);
            {
                GL.Color(c4);
                foreach (XYGridPos gridPos in fillPolySafeZonePos_.Keys) {
                    SubmitQuadVertex(gridPos);
                }
            }
            GL.End();
        }
        CurrentStatus.OnPostRender(this);

        GL.Clear(false, false, Color.clear);
    }

    public void SaveObstacleToFile() {
        if (this.obstacle_filename != string.Empty && this.zone_filename != string.Empty) {
            try {
                FileStream stream;
                string directoryName = Path.GetDirectoryName(this.obstacle_filename);
                if (!Directory.Exists(directoryName)) {
                    throw new Exception(string.Format("路径不存在：{0}", directoryName));
                }
                string directoryName1 = Path.GetDirectoryName(this.zone_filename);
                if (!Directory.Exists(directoryName1)) {
                    throw new Exception(string.Format("路径不存在：{0}", this.zone_filename));
                }
                //输出安全区域文件
                using (stream = new FileStream(AsyncLevelLoader.LoadedLevelName + ".zone", FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    for (int i = 0; i < xMapSize_; i++) {
                        for (int j = 0; j < zMapSize_; j++) {
                            stream.WriteByte(safeZoneByteArray_[i, j]);
                        }
                    }
                }
                using (stream = new FileStream(this.zone_filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    using (TextWriter writer2 = new StreamWriter(stream, Encoding.ASCII)) {
                       foreach(XYGridPos pos in safeZonePolyPos_) {
                            writer2.WriteLine("{0} {1}", pos.x.ToString(), pos.y.ToString());
                        }
                    }
                }
                using (stream = new FileStream(this.obstacle_filename, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    using (BinaryWriter writer = new BinaryWriter(stream)) {
                        XYObstacleSerializer.WriteToBinary(writer, this.obstacleLineList_, this.xMapSize_, this.zMapSize_);
                    }
                }

                using (stream = new FileStream(string.Format("{0}.{1}.{2}.txt", this.obstacle_filename, this.xMapSize_.ToString(), this.zMapSize_.ToString()), FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    using (TextWriter writer2 = new StreamWriter(stream, Encoding.ASCII)) {
                        XYObstacleSerializer.WriteToText(writer2, this.obstacleLineList_);
                    }
                }
                this.hasPosOfLineBegin_ = false;
                this.CurrentStatus = StatusModalDialog.Show("成功", "障碍数据已成功保存到文件中！");
            } catch (Exception exception) {
                this.CurrentStatus = StatusModalDialog.Show("错误", exception.Message);
                Debug.Log(exception.StackTrace);
            }
        }
    }

    private void Start() {
        safeZoneByteArray_ = new byte[xMapSize_, zMapSize_];
        for (int i = 0; i < xMapSize_; i++)
            for (int j = 0; j < zMapSize_; j++)
                safeZoneByteArray_[i, j] = (byte)0;
        safeZonePolyPos_.Clear();
        polyPosIndex = 0;
        polyLineIndex = 0;
        ForEachCollider(delegate (GameObject go) {
            MeshFilter component = go.GetComponent<MeshFilter>();
            if ((component != null) && (component.sharedMesh)) {
                go.SetActive(true);
                if (go.GetComponent<Renderer>()) {
                    go.GetComponent<Renderer>().enabled = (true);
                }
                Collider collider = go.GetComponent<Collider>();
                if (collider == null) {
                    go.AddComponent<MeshCollider>().sharedMesh = (component.sharedMesh);
                } else {
                    MeshCollider collider2 = collider as MeshCollider;
                    if (collider2) {
                        collider2.sharedMesh = (component.sharedMesh);
                    }
                }
            }
        });
        navTriangles_ = UnityEngine.AI.NavMesh.CalculateTriangulation();
        AnalyseNavLayers();
        UpdateLayer(0);
        //AnalyseLayerPos();
        //AnalyseNavLines();
        bCalcNavTriangles_ = true;
    }

    public void UpdateLayer(int layer) {
        curLayer_ = layer;

        FilterNavPos_.Clear();
        FilterNavline_.Clear();
        int layer_index = 0;
        if (navTriangles_.indices.Length == 0)
        {
            Debug.LogError("没有 NavMesh 数据 联系美术场景");
        }
        for (int i = 0; i < navTriangles_.indices.Length;) {
            if (navTriangles_.areas[layer_index] != FLY_LAYER && (curLayer_ == -1 || navTriangles_.areas[layer_index] == curLayer_))
            {
                AddFilterNavPos(i);
                AddFilterNavLine(i);
            }
            ++layer_index;
            i += 3;
        }
        FilterNavLine();
        AddobstacleLine();
    }

    private void AddFilterNavPos(int index) {
        if (!FilterNavPos_.Contains(navTriangles_.vertices[navTriangles_.indices[index]]))
            FilterNavPos_.Add(navTriangles_.vertices[navTriangles_.indices[index]]);

        if (!FilterNavPos_.Contains(navTriangles_.vertices[navTriangles_.indices[index + 1]]))
            FilterNavPos_.Add(navTriangles_.vertices[navTriangles_.indices[index + 1]]);

        if (!FilterNavPos_.Contains(navTriangles_.vertices[navTriangles_.indices[index + 2]]))
            FilterNavPos_.Add(navTriangles_.vertices[navTriangles_.indices[index + 2]]);
    }

    private void AddFilterNavLine(int index) {

        Vector3 vecNavPos_1 = navTriangles_.vertices[navTriangles_.indices[index]];
        Vector3 vecNavPos_2 = navTriangles_.vertices[navTriangles_.indices[index + 1]];
        int index_1 = FilterNavPos_.IndexOf(vecNavPos_1);
        int index_2 = FilterNavPos_.IndexOf(vecNavPos_2);
        if (index_1 != -1 && index_2 != -1)
            FilterNavline_.Add(new NavLineInfo(index_1, index_2));
        else {
            Debug.Log("Analyse Nav Line Error!!!");
            //continue;
        }

        vecNavPos_1 = navTriangles_.vertices[navTriangles_.indices[index + 1]];
        vecNavPos_2 = navTriangles_.vertices[navTriangles_.indices[index + 2]];
        index_1 = FilterNavPos_.IndexOf(vecNavPos_1);
        index_2 = FilterNavPos_.IndexOf(vecNavPos_2);
        if (index_1 != -1 && index_2 != -1)
            FilterNavline_.Add(new NavLineInfo(index_1, index_2));
        else {
            Debug.Log("Analyse Nav Line Error!!!");
            //continue;
        }

        vecNavPos_1 = navTriangles_.vertices[navTriangles_.indices[index + 2]];
        vecNavPos_2 = navTriangles_.vertices[navTriangles_.indices[index]];
        index_1 = FilterNavPos_.IndexOf(vecNavPos_1);
        index_2 = FilterNavPos_.IndexOf(vecNavPos_2);
        if (index_1 != -1 && index_2 != -1)
            FilterNavline_.Add(new NavLineInfo(index_1, index_2));
        else {
            Debug.Log("Analyse Nav Line Error!!!");
            //continue;
        }
    }

    private void FilterNavLine() {
        List<NavLineInfo> remove_lines = new List<NavLineInfo>();
        for (int i = 0; i < FilterNavline_.Count; ++i) {

            if (remove_lines.Contains(FilterNavline_[i]))
                continue;

            bool bRemove = false;
            for (int j = i + 1; j < FilterNavline_.Count; ++j) {
                if (FilterNavline_[j] == FilterNavline_[i]) {
                    remove_lines.Add(FilterNavline_[j]);
                    bRemove = true;
                }
            }

            if (bRemove)
                remove_lines.Add(FilterNavline_[i]);
        }
        foreach (NavLineInfo line in remove_lines)
            FilterNavline_.Remove(line);

    }

    private void AddobstacleLine() {
        this.obstacleLineList_.Clear();
        AddVertexObstacleLine();
        foreach (NavLineInfo line in FilterNavline_) {
            XYGridPos beginGridPos = new XYGridPos(Convert.ToInt16(Mathf.FloorToInt(FilterNavPos_[line.bIndex_].x * 2f)), Convert.ToInt16(Mathf.FloorToInt(FilterNavPos_[line.bIndex_].z * 2f)));
            XYGridPos endGridPos = new XYGridPos(Convert.ToInt16(Mathf.FloorToInt(FilterNavPos_[line.eIndex_].x * 2f)), Convert.ToInt16(Mathf.FloorToInt(FilterNavPos_[line.eIndex_].z * 2f)));
            if (beginGridPos == endGridPos)
                continue;
            TryToAddLine(beginGridPos, endGridPos);
        }
    }

    private void FallFilterPos() {
        for (int i = 0; i < FilterNavPos_.Count; ++i) {
            FilterNavPos_[i] = new Vector3(FilterNavPos_[i].x, 0, FilterNavPos_[i].z);
        }

        if (bFallToPlane_) {

        } else {
            FilterNavPos_.Clear();
            int layer_index = 0;
            for (int i = 0; i < navTriangles_.indices.Length;) {
                if (navTriangles_.areas[layer_index] != FLY_LAYER && (curLayer_ == -1 || navTriangles_.areas[layer_index] == curLayer_))
                {
                    AddFilterNavPos(i);
                }
                ++layer_index;
                i += 3;
            }
        }
    }

    private void AnalyseNavLayers() {
        navLayers_.Clear();
        for (int i = 0; i < navTriangles_.areas.Length; ++i)
        {
            if (navTriangles_.areas[i] == FLY_LAYER)
                continue;
            if (!navLayers_.Contains(navTriangles_.areas[i]))
                navLayers_.Add(navTriangles_.areas[i]);
        }
    }
    private bool AddLine(XYGridPos begin_pos,XYGridPos end_pos) {
        if (begin_pos == end_pos)
            return false;
        if (polySafeZoneLine_.IsPosInLineAndNotAtEndPoint(end_pos))
            return false;
        XYObstacleLine new_line = new XYObstacleLine(begin_pos, end_pos);
        foreach(XYObstacleLine existsLine in polySafeZoneLine_) {
            if (new_line == existsLine)
                return false;
            if (XYObstacleLine.LinesIntersect(existsLine, new_line)) {
                if (new_line.Begin != existsLine.Begin && new_line.Begin != existsLine.End
                    && new_line.End != existsLine.Begin && new_line.End != existsLine.End) {
                    Debug.LogWarning("添加失败，线段相交");
                    return false;
                }
            }
        }
        return true;
    }
    private void TryToAddLine(XYGridPos beginGridPos, XYGridPos endGridPos) {
        XYObstacleLine new_line = new XYObstacleLine(beginGridPos, endGridPos);
        bool result = this.obstacleLineList_.Add(new_line);
        if (result == false) {
            foreach (XYObstacleLine existsLine in obstacleLineList_) {
                if (new_line == existsLine) {
                    return;
                }
                if (XYObstacleLine.LinesIntersect(existsLine, new_line)) {
                    if (new_line.Begin != existsLine.Begin && new_line.Begin != existsLine.End
                    && new_line.End != existsLine.Begin && new_line.End != existsLine.End) {
                        Debug.LogWarning("添加失败，线段相交");
                        return;
                    }
                    if (existsLine.IsSlopeEqual(new_line)) {
                        XYObstacleLine line;
                        if (new_line.Begin == existsLine.Begin) {
                            obstacleLineList_.RemoveWithPos(beginGridPos, out line);
                            TryToAddLine(endGridPos, existsLine.End);
                        } else if (new_line.Begin == existsLine.End) {
                            obstacleLineList_.RemoveWithPos(beginGridPos, out line);
                            TryToAddLine(existsLine.Begin, endGridPos);
                        } else if (new_line.End == existsLine.Begin) {
                            obstacleLineList_.RemoveWithPos(endGridPos, out line);
                            TryToAddLine(beginGridPos, existsLine.End);
                        } else if (new_line.End == existsLine.End) {
                            obstacleLineList_.RemoveWithPos(endGridPos, out line);
                            TryToAddLine(existsLine.Begin, beginGridPos);
                        }
                        break;
                    }
                }
            }
        }
    }

    private void SubmitQuadVertex(XYGridPos gp) {
        Vector3 vector = this.gridPosToWorldPos_.Execute(gp);
        if (bFallToPlane_)
            vector.y = 0.0f;
        vector.x -= 0.25f;
        vector.z -= 0.25f;
        GL.Vertex(vector);
        vector.z += 0.5f;
        GL.Vertex(vector);
        vector.x += 0.5f;
        GL.Vertex(vector);
        vector.z -= 0.5f;
        GL.Vertex(vector);
    }

    private void Undo() {
        Cmd cmd = this.cmdStack_.Pop();
        if (cmd != null) {
            cmd.Undo();
        }
    }

    private void Update() {
        this.CurrentStatus.Update(this);
    }

    private Vector3 WordPositionToTopView(Vector3 pos) {
        return this.topviewcamera.projectionMatrix.MultiplyPoint(this.topviewcamera.worldToCameraMatrix.MultiplyPoint(pos));
    }

    // Properties
    private Status CurrentStatus {
        get {
            return this.currentStatus_;
        }
        set {
            this.currentStatus_ = value;
        }
    }

    // Nested Types
    private abstract class Cmd {
        // Fields
        private readonly float CameraDistance;
        private readonly Vector3 CameraEuler;
        private readonly Vector3 CameraLookAt;
        protected readonly XYObstacleCamera Owner;

        // Methods
        public Cmd(XYObstacleCamera owner) {
            this.Owner = owner;
            this.CameraLookAt = owner.lookAt_;
            this.CameraEuler = owner.euler_;
            this.CameraDistance = owner.distance_;
        }

        public void Undo() {
            this.UndoImpl();
            this.Owner.lookAt_ = this.CameraLookAt;
            this.Owner.distance_ = this.CameraDistance;
            this.Owner.euler_ = this.CameraEuler;

        }

        protected abstract void UndoImpl();
    }

    private class CmdAddBeginPos : XYObstacleCamera.Cmd {
        // Methods
        public CmdAddBeginPos(XYObstacleCamera owner)
            : base(owner) {
        }

        protected override void UndoImpl() {
            base.Owner.hasPosOfLineBegin_ = false;

        }
    }

    private class CmdAddLine : XYObstacleCamera.Cmd {
        // Fields
        private readonly XYGridPos beginPos_;

        // Methods
        public CmdAddLine(XYObstacleCamera owner, XYGridPos beginPos)
            : base(owner) {
            this.beginPos_ = beginPos;
        }

        protected override void UndoImpl() {
            base.Owner.obstacleLineList_.RemoveLast();
            base.Owner.hasPosOfLineBegin_ = true;
            base.Owner.posOfLineBegin_ = this.beginPos_;

        }
    }

    private class CmdRemoveBeginPos : XYObstacleCamera.Cmd {
        // Fields
        private readonly XYGridPos pos_;

        // Methods
        public CmdRemoveBeginPos(XYObstacleCamera owner, XYGridPos pos)
            : base(owner) {
            this.pos_ = pos;
        }

        protected override void UndoImpl() {
            base.Owner.posOfLineBegin_ = this.pos_;
            base.Owner.hasPosOfLineBegin_ = true;
        }
    }

    private class CmdRemoveLine : XYObstacleCamera.Cmd {
        // Fields
        private readonly XYObstacleLine line_;

        // Methods
        public CmdRemoveLine(XYObstacleCamera owner, XYObstacleLine line)
            : base(owner) {
            this.line_ = line;
        }

        protected override void UndoImpl() {
            base.Owner.obstacleLineList_.Add(this.line_);
        }
    }

    private class CmdStack {
        // Fields
        private readonly int maxUndo_;
        private readonly List<XYObstacleCamera.Cmd> stack_;

        // Methods
        public CmdStack(int maxUndo) {
            this.maxUndo_ = (maxUndo < 20) ? 20 : maxUndo;
            this.stack_ = new List<XYObstacleCamera.Cmd>(this.maxUndo_);
        }

        public void Clear() {
            this.stack_.Clear();
        }

        public XYObstacleCamera.Cmd Pop() {
            if (this.stack_.Count == 0) {
                return null;
            }
            int index = this.stack_.Count - 1;
            XYObstacleCamera.Cmd cmd = this.stack_[index];
            this.stack_.RemoveAt(index);
            return cmd;
        }

        public void Push(XYObstacleCamera.Cmd cmd) {
            if (this.stack_.Count == this.maxUndo_) {
                this.stack_.RemoveAt(0);
            }
            this.stack_.Add(cmd);
        }

        // Properties
        public int Count {
            get {
                return this.stack_.Count;
            }
        }
    }

    public class Effect {
        // Fields
        private static Material material_;
        private bool showShdow_;
        public bool ShowTrees = true;
        public bool ShowNavLine = false;
        private GameObject trees_;

        // Methods
        public Effect(Camera camera) {
            camera.farClipPlane = (10000f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            this.CreateMaterial();
            CreateLight(camera);
            DisableOtherCameras(camera);
            this.FindTrees();
            this.ShowFog = false;
            this.ShowTrees = false;
            this.ShowShadow = false;
        }

        public void BeforeUseGL() 
        {
            material_.SetPass(0);
        }

        private static void CreateLight(Camera camera) {
            GameObject go = new GameObject("camera_light");
            go.transform.parent = camera.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            Light lt = go.AddComponent<Light>();
            lt.type = LightType.Directional;
            lt.renderMode = LightRenderMode.ForceVertex;
            lt.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            lt.intensity = 0.2f;

            //寻找斜阻挡的另外一个光源
            GameObject go2 = new GameObject("camera_light2");
            go2.transform.forward = Vector3.down;
            Light lt2 = go2.AddComponent<Light>();
            lt2.type = LightType.Directional;
            lt2.renderMode = LightRenderMode.ForcePixel;
            lt2.color = Color.white;
            lt2.intensity = 0.01f;
            lt2.shadowStrength = 1.0f;
            lt2.shadows = LightShadows.Hard;
            lt2.cullingMask = XYDefines.Layer.Mask.MainBuilding | XYDefines.Layer.Mask.Ignore | XYDefines.Layer.Mask.Terrain | XYDefines.Layer.Mask.Prop;
            QualitySettings.shadowDistance = 1000.0f;
            QualitySettings.pixelLightCount = 4;
        }

        private void CreateMaterial(/*GameObject go*/) {
            if (!material_) {
                material_ = new Material(Shader.Find("LinesColored Blended"));
                material_.hideFlags = HideFlags.HideAndDontSave;
                material_.shader.hideFlags = HideFlags.HideAndDontSave;
                //UnityEngine.Object.DontDestroyOnLoad(material_);
                //Renderer r = XYClientCommon.AddComponent<MeshRenderer>(go);
                //r.castShadows = r.receiveShadows = false;
                //r.material = material_;
                //r.enabled = false;
            }
        }

        private static void DisableOtherCameras(Camera me) {
            foreach (Camera camera in Camera.allCameras) {
                if (camera != me) {
                    camera.gameObject.SetActive(false);
                }
            }
        }

        private void FindTrees() {
            if (this.trees_ == null) {
                string[] strArray = new string[] { "/root/zhibei", "/GameObject/zhibei" };
                for (int i = 0; i < strArray.Length; i++) {
                    this.trees_ = GameObject.Find(strArray[i]);
                    if (this.trees_) {
                        break;
                    }
                }
            }
        }

        // Properties
        public bool ShowFog {
            get {
                return RenderSettings.fog;
            }
            set {
                RenderSettings.fog = (value);
            }
        }

        public bool ShowShadow {
            get {
                return this.showShdow_;
            }
            set {
                GameObject obj2 = GameObject.Find("touyingzhezhao");
                if (obj2) {
                    obj2.SetActive(value);
                    this.showShdow_ = value;
                } else {
                    this.showShdow_ = false;
                }
            }
        }
    }

    private class GridPosToWorldPos {
        // Fields
        private Dictionary<XYGridPos, Vector3> dict_ = new Dictionary<XYGridPos, Vector3>();

        // Methods
        public void Clear() {
            this.dict_.Clear();
        }

        public Vector3 Execute(XYGridPos gp) {
            Vector3 vector;
            if (!this.dict_.TryGetValue(gp, out vector)) {
                vector = new Vector3((gp.x * 0.5f) + 0.25f, 600f, (gp.y * 0.5f) + 0.25f);
                vector.y = XYObstacleCamera.GetTerrainHeight(vector);
                this.dict_.Add(gp, vector);
            }
            return vector;
        }
    }

    private abstract class Status {
        private Rect NpcPlacementWindowRect_ = new Rect(0, 0, 0, 0);
        private Vector2 scrollPosition_;

        // Methods
        protected Status() {
        }

        private static float calcSpeedAndNormalize(ref Vector3 movement) {
            float num = Mathf.Min(movement.magnitude, 1f);
            movement.Normalize();
            return num;
        }

        private void CheckCameraMove(XYObstacleCamera owner) {
            Vector3 vector;
            float num3;
            float num = XYObstacleCamera.GetSpeedFromInput() * Time.deltaTime;
            float axisRaw = Input.GetAxisRaw("Horizontal");
            if (0f != axisRaw) {
                owner.cameraSmooth_.Active = false;
                vector = (Vector3)(axisRaw * owner.transform.right);
                num3 = calcSpeedAndNormalize(ref vector) * num;
                owner.lookAt_ += (Vector3)(vector * num3);
            }
            float num4 = Input.GetAxisRaw("Vertical");
            if (0f != num4) {
                owner.cameraSmooth_.Active = false;
                vector = Quaternion.Euler(0f, -90f, 0f) * owner.transform.right;
                vector = (Vector3)(vector * num4);
                num3 = calcSpeedAndNormalize(ref vector) * num;
                owner.lookAt_ += (Vector3)(vector * num3);
            }
            float num5 = Input.GetAxisRaw("Mouse ScrollWheel");
            if (Input.GetKey(KeyCode.Q)) {
                owner.cameraSmooth_.Active = false;
                num5 += 0.1f;
            }
            if (Input.GetKey(KeyCode.E)) {
                owner.cameraSmooth_.Active = false;
                num5 -= 0.1f;
            }
            if (0f != num5) {
                owner.cameraSmooth_.Active = false;
                owner.distance_ -= (num5 * owner.distance_) * 0.5f;
                if (owner.distance_ <= 1f) {
                    vector = (Vector3)(owner.transform.forward * num5);
                    num3 = calcSpeedAndNormalize(ref vector) * num;
                    owner.distance_ = 1f;
                    owner.lookAt_ += (Vector3)(vector * num3);
                }
            }
        }

        public virtual void OnGUI(XYObstacleCamera owner) {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("加载", new GUILayoutOption[0])) {
                owner.CurrentStatus = XYObstacleCamera.StatusLoadDialog.Instance;
            } else if ((owner.obstacleLineList_.Count > 0) && GUILayout.Button("保存", new GUILayoutOption[0])) {
                owner.CurrentStatus = XYObstacleCamera.StatusSaveDialog.Instance;
            } else if ((owner.obstacleLineList_.Count > 0) && GUILayout.Button("下降到0平面", new GUILayoutOption[0])) {
                owner.bFallToPlane_ = !owner.bFallToPlane_;
                owner.empty_plane_.SetActive(owner.bFallToPlane_);
                owner.FallFilterPos();
            } else {
                XYGridPos pos;
                Vector3 worldpos;
                if (owner.GetGridPosFromMouse(out pos, out worldpos, owner)) {
                    GUILayout.Label(string.Format("({0})", pos), new GUILayoutOption[0]);
                }
            }
            GUILayout.EndHorizontal();
            int count;
                count = owner.cmdStack_.Count + owner.safeZonePolyPos_.Count;
            if ((owner.cmdStack_.Count > 0  || owner.safeZonePolyPos_.Count > 0) && GUI.Button(new Rect((float)(Screen.width - 480), 0f, 90f, 20f), string.Format("撤销({0})", count))) {
                if (XYObstacleCamera.safeZone_) {
                    for (int i = 0; i < owner.xMapSize_; i++)
                        for (int j = 0; j < owner.zMapSize_; j++)
                            owner.safeZoneByteArray_[i, j] = (byte)0;
                    if (XYObstacleCamera.isSelectedPoly_ && owner.safeZonePolyPos_.Count > 0) {
                        owner.fillPolySafeZonePos_.Clear();
                        owner.RemovePolyLine();
                        owner.SetPolySafeZonArray();
                    }

                } else
                    owner.Undo();

            }


            if (GUI.Button(new Rect((float)(Screen.width - 390), 0f, 100f, 20f), "选择辅助层")) {
                owner.CurrentStatus = XYObstacleCamera.StatusSelectLayer.Instance;
            }
            if (GUI.Button(new Rect((float)(Screen.width - 290), 0f, 90f, 20f), "返回")) {
                owner.CurrentStatus = XYObstacleCamera.StatusReturnDialog.Instance;
            }
            if (GUI.Button(new Rect((float)(Screen.width - 200), 0f, 100f, 20f), "鸟瞰")) {
                owner.euler_.x = -89.9f;
            }
            if (GUI.Button(new Rect((float)(Screen.width - 100), 0f, 100f, 20f), "摄像机还原")) {
                owner.CameraReset();
            }
            //设置安全区域的复选框,勾选了这个框，就不能勾选自由模式。
            bool flag7 = GUI.Toggle(new Rect((float)(Screen.width - 600), 30f, 100f, 20f), XYObstacleCamera.safeZone_, "安全区域");
            if (flag7 != XYObstacleCamera.safeZone_) {
                XYObstacleCamera.safeZone_ = flag7;
                XYObstacleCamera.freeMode_ = false;

            }

            if (XYObstacleCamera.safeZone_) {
                Rect wndRect_ = new Rect((float)(Screen.width - 200), 80f, 200f, 60f);
                GUILayout.Window(104, wndRect_, FillStyleSelectedFunc, "选择填充区域样式");
            }
            //   GUILayout.Window(104, new Rect((float)(Screen.width - 100), 50f, 200f, 40f), FillStyleSelectedFunc, "选择填充安全区域样式");
            //}
            bool flag6 = GUI.Toggle(new Rect((float)(Screen.width - 400), 30f, 100f, 20f), XYObstacleCamera.freeMode_, "自由模式");
            if (flag6 != XYObstacleCamera.freeMode_) {
                XYObstacleCamera.freeMode_ = flag6;
                XYObstacleCamera.safeZone_ = false;
            }
            bool flag = GUI.Toggle(new Rect((float)(Screen.width - 300), 30f, 50f, 20f), owner.effect_.ShowTrees, "树");
            if (flag != owner.effect_.ShowTrees) {
                owner.effect_.ShowTrees = flag;
            }
            bool flag2 = GUI.Toggle(new Rect((float)(Screen.width - 250), 30f, 70f, 20f), XYObstacleCamera.showAirWall_, "空气墙");
            if (XYObstacleCamera.showAirWall_ != flag2) {
                XYObstacleCamera.showAirWall_ = flag2;
                if (XYObstacleCamera.airWall_) {
                    XYObstacleCamera.airWall_.SetActive(flag2);
                }
            }
            bool flag3 = GUI.Toggle(new Rect((float)(Screen.width - 180), 30f, 60f, 20f), XYObstacleCamera.showCollider_, "阻挡");
            if (flag3 != XYObstacleCamera.showCollider_) {
                XYObstacleCamera.showCollider_ = flag3;
                XYObstacleCamera.ForEachCollider(delegate (GameObject go) {
                    Renderer component = go.GetComponent<Renderer>();
                    if (component) {
                        component.enabled = (XYObstacleCamera.showCollider_);
                    }
                });
            }
            bool flag4 = GUI.Toggle(new Rect((float)(Screen.width - 120), 30f, 70f, 20f), XYObstacleCamera.showNavLine_, "辅助线");
            if (flag4 != XYObstacleCamera.showNavLine_) {
                XYObstacleCamera.showNavLine_ = flag4;
            }
            bool flag5 = GUI.Toggle(new Rect((float)(Screen.width - 50), 30f, 70f, 20f), XYObstacleCamera.showNavPoint_, "辅助点");
            if (flag5 != XYObstacleCamera.showNavPoint_) {
                XYObstacleCamera.showNavPoint_ = flag5;
            }
            string cursorDesc = this.CursorDesc;
            if ((cursorDesc != null) && (cursorDesc.Length > 0)) {
                Vector3 vector = Input.mousePosition;
                GUI.Label(new Rect(vector.x + 16f, (Screen.height - vector.y) + 16f, 100f, 20f), cursorDesc);
            }
        }
        //安全区域填充样式选择
        private void FillStyleSelectedFunc(int id) {
            GUILayout.BeginHorizontal();
            bool flag1 = GUILayout.Toggle(isSelectedPoly_, "多边形", new GUILayoutOption[0]);
            if (flag1 != XYObstacleCamera.isSelectedPoly_) {
                XYObstacleCamera.isSelectedPoly_ = flag1;
            }
            GUILayout.EndHorizontal();
        }
        public virtual void OnPostRender(XYObstacleCamera owner) {
        }

        public virtual void Update(XYObstacleCamera owner) {
            CheckCameraMove(owner);
            bool altKeyDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            bool ctrlKeyDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            if (altKeyDown) {
                owner.CurrentStatus = StatusAdjustCamera.Instance;
            } else if (ctrlKeyDown) {
                owner.CurrentStatus = StatusAddPoint.Instance;
            } else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                owner.CurrentStatus = StatusRemoveLine.Instance;
            } else {
                owner.CurrentStatus = StatusNormal.Instance;
            }
            //显示隐藏顶视图
            if (Input.GetKeyUp(KeyCode.F12)) {
                owner.enabletopview = (!owner.enabletopview);
            }

        }

        // Properties
        public virtual string CursorDesc {
            get {
                return this.Desc;
            }
        }

        public abstract string Desc { get; }
    }

    private class StatusAddPoint : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.Status instance_ = new XYObstacleCamera.StatusAddPoint();

        // Methods
        private StatusAddPoint() {
        }

        public override void OnPostRender(XYObstacleCamera owner) {
            Vector3 vector;
            int layer;
            if (owner.GetWorldPosFromMouse(out vector, out layer, false)) {
                XYObstacleCamera.DrawGrid(vector, layer != XYDefines.Layer.Terrain ? Color.red : owner.ColorAddPoint);
            }
        }

        public override void Update(XYObstacleCamera owner) {

            RaycastHit hit;
            base.Update(owner);

            if (Input.GetMouseButtonUp(0)) {
                Ray ray = owner.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.gameObject.layer != XYDefines.Layer.Terrain) {
                        if (hit.collider.transform.parent)
                            Debug.Log(string.Format("傻吊，点到{0}/{1}了", hit.collider.transform.parent.name, hit.collider.name));
                        else
                            Debug.Log(string.Format("傻吊，点到{0}了", hit.collider.name));
                        return;
                    }
                    //存储安全模式下点击的点
                    if (isSelectedPoly_ && XYObstacleCamera.safeZone_)
                        owner.SetSafeZonePos(hit, owner);
                }
                XYGridPos gridPos;
                Vector3 worldpos;
                if (!XYObstacleCamera.safeZone_) {
                    if (owner.GetGridPosFromMouse(out gridPos, out worldpos, owner)) {

                        if (!XYObstacleCamera.freeMode_) {
                            Vector3 samePos = Vector3.zero;
                            foreach (Vector3 vec in owner.FilterNavPos_) {
                                Vector3 vec_cur = owner.GetComponent<Camera>().WorldToScreenPoint(vec);
                                Vector3 vec_sel = owner.GetComponent<Camera>().WorldToScreenPoint(worldpos);
                                if ((vec_cur - vec_sel).sqrMagnitude < 300) {
                                    Debug.Log((vec_cur - vec_sel).sqrMagnitude);
                                    samePos = vec;
                                    break;
                                }
                            }
                            if (samePos == Vector3.zero) {
                                Debug.Log(string.Format("选择坐标错误！"));
                                return;
                            }
                            gridPos = new XYGridPos(Convert.ToInt16(Mathf.FloorToInt(samePos.x * 2f)), Convert.ToInt16(Mathf.FloorToInt(samePos.z * 2f)));
                        }


                        if (owner.hasPosOfLineBegin_ && owner.posOfLineBegin_ == gridPos) {
                            owner.hasPosOfLineBegin_ = false;
                            owner.cmdStack_.Push(new CmdRemoveBeginPos(owner, gridPos));
                            return;
                        }
                        if (owner.obstacleLineList_.IsPosInLineAndNotAtEndPoint(gridPos)) {
                            return;
                        }
                        // 如果已有新线段起始点，画线
                        if (owner.hasPosOfLineBegin_) {
                            XYObstacleLine line = new XYObstacleLine(owner.posOfLineBegin_, gridPos);
                            if (owner.obstacleLineList_.Add(line)) {
                                owner.hasPosOfLineBegin_ = false;
                                owner.cmdStack_.Push(new CmdAddLine(owner, owner.posOfLineBegin_));

                            }
                        } else {
                            owner.hasPosOfLineBegin_ = true;
                            owner.posOfLineBegin_ = gridPos;
                            owner.cmdStack_.Push(new CmdAddBeginPos(owner));
                        }

                    }
                }
            }
        }
        // Properties
        public override string Desc {
            get {
                return "加点";
            }
        }

        public static XYObstacleCamera.Status Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusAdjustCamera : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.Status instance_ = new XYObstacleCamera.StatusAdjustCamera();

        // Methods
        private StatusAdjustCamera() {
        }

        public override void Update(XYObstacleCamera owner) {
            base.Update(owner);
            if (Input.GetMouseButton(0)) {
                float axisRaw = Input.GetAxisRaw("Mouse X");
                float num2 = Input.GetAxisRaw("Mouse Y");
                if ((axisRaw != 0f) || (num2 != 0f)) {
                    owner.euler_.x = Mathf.Clamp(owner.euler_.x + (num2 * 3f), -89.9f, -1f);
                    owner.euler_.y = Mathf.Repeat(owner.euler_.y + (axisRaw * 3f), 360f);
                    owner.cameraSmooth_.Active = false;
                }
            }
        }

        // Properties
        public override string Desc {
            get {
                return "调整摄像机";
            }
        }

        public static XYObstacleCamera.Status Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusLoadDialog : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.StatusLoadDialog instance_ = new XYObstacleCamera.StatusLoadDialog();
        private XYObstacleCamera owner_;
        private Rect wndRect_ = new Rect(0f, 0f, 2f, 2f);

        // Methods
        private StatusLoadDialog() {
        }

        public override void OnGUI(XYObstacleCamera owner) {
            this.owner_ = owner;
            wndRect_ = GUILayout.Window(101, wndRect_, WndFunc, "加载障碍数据");
        }

        public override void Update(XYObstacleCamera owner) {
        }

        private void WndFunc(int id) {
            GUILayout.Label("文件名：", new GUILayoutOption[0]);
            if (safeZone_)
                this.owner_.zone_filename = GUILayout.TextField(this.owner_.zone_filename, new GUILayoutOption[] { GUILayout.MinWidth(300f) });
            else
                this.owner_.obstacle_filename = GUILayout.TextField(this.owner_.obstacle_filename, new GUILayoutOption[] { GUILayout.MinWidth(300f) });
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("确定", new GUILayoutOption[0])) {
                this.owner_.LoadObstacleFromFile();
            }
            if (GUILayout.Button("取消", new GUILayoutOption[0])) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
            }
            GUILayout.EndHorizontal();
        }

        // Properties
        public override string Desc {
            get {
                return "加载……";
            }
        }

        public static XYObstacleCamera.StatusLoadDialog Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusModalDialog : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.StatusModalDialog instance_ = new XYObstacleCamera.StatusModalDialog();
        private string message_;
        private XYObstacleCamera owner_;
        private string title_;
        private Rect wndRect_ = new Rect(0f, 0f, 2f, 2f);

        // Methods
        private StatusModalDialog() {
        }

        public override void OnGUI(XYObstacleCamera owner) {
            this.owner_ = owner;
            wndRect_ = GUILayout.Window(100, wndRect_, WndFunc, title_);
        }

        public static XYObstacleCamera.StatusModalDialog Show(string title, string message) {
            instance_.title_ = title;
            instance_.message_ = message;
            return instance_;
        }

        public override void Update(XYObstacleCamera owner) {
        }

        private void WndFunc(int id) {
            GUILayout.Label(this.message_, new GUILayoutOption[] { GUILayout.MinWidth(200f) });
            GUILayout.Space(20f);
            if (GUILayout.Button("close", new GUILayoutOption[0])) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
            }
        }

        // Properties
        public override string Desc {
            get {
                return "模态对话框";
            }
        }
    }

    private class StatusNormal : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.Status instance_ = new XYObstacleCamera.StatusNormal();

        // Methods
        private StatusNormal() {
        }

        public override void Update(XYObstacleCamera owner) {
            RaycastHit hit;
            base.Update(owner);
            if (Input.GetMouseButtonUp(1) && Physics.Raycast(owner.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit)) {
                owner.lookAt_ = hit.point;
                owner.distance_ = Mathf.Min(owner.distance_, 15f);
                owner.cameraSmooth_.Active = true;
            }
        }

        // Properties
        public override string CursorDesc {
            get {
                return null;
            }
        }

        public override string Desc {
            get {
                return "normal";
            }
        }

        public static XYObstacleCamera.Status Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusRemoveLine : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.Status instance_ = new XYObstacleCamera.StatusRemoveLine();

        // Methods
        private StatusRemoveLine() {
        }

        public override void OnPostRender(XYObstacleCamera owner) {
            Vector3 vector;
            int layer;
            if (owner.GetWorldPosFromMouse(out vector, out layer, true)) {
                XYObstacleCamera.DrawGrid(vector, owner.ColorRemoveLine);
            }
        }

        public override void Update(XYObstacleCamera owner) {
            XYGridPos pos;
            Vector3 worldpos;
            base.Update(owner);
            if (Input.GetMouseButtonUp(0) && owner.GetGridPosFromMouse(out pos, out worldpos, owner)) {
                if (owner.hasPosOfLineBegin_ && (owner.posOfLineBegin_ == pos)) {
                    owner.hasPosOfLineBegin_ = false;
                    owner.cmdStack_.Push(new XYObstacleCamera.CmdRemoveBeginPos(owner, pos));
                } else {
                    XYObstacleLine line;
                    if (owner.obstacleLineList_.RemoveWithPos(pos, out line)) {
                        owner.cmdStack_.Push(new XYObstacleCamera.CmdRemoveLine(owner, line));
                    }
                }
            }

        }

        // Properties
        public override string Desc {
            get {
                return "删线";
            }
        }

        public static XYObstacleCamera.Status Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusReturnDialog : XYObstacleCamera.Status {
        // Fields
        public static readonly XYObstacleCamera.StatusReturnDialog Instance = new XYObstacleCamera.StatusReturnDialog();
        private XYObstacleCamera owner_;

        // Methods
        private StatusReturnDialog() {
        }

        public override void OnGUI(XYObstacleCamera owner) {
            this.owner_ = owner;
            GUILayout.Window(103,
                 new Rect(
                     (Screen.width - 320) * 0.5f,
                     (Screen.height - 240) * 0.5f,
                     320, 240),
                     WndFunc, "确认");
        }

        public override void Update(XYObstacleCamera owner) {
        }

        private void WndFunc(int id) {
            GUILayout.Label("您是否确定要返回？", new GUILayoutOption[0]);
            GUILayout.Label("所有未保存的数据将丢失！！", new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Space(20f);
            if (GUILayout.Button("返回", new GUILayoutOption[0])) {
                Application.LoadLevel("startMenu");
            }
            GUILayout.Space(20f);
            if (GUILayout.Button("取消", new GUILayoutOption[0])) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
            }
            GUILayout.Space(20f);
            GUILayout.EndHorizontal();
        }

        // Properties
        public override string Desc {
            get {
                return "确定要返回到主菜单吗？";
            }
        }
    }

    private class StatusSaveDialog : XYObstacleCamera.Status {
        // Fields
        private static XYObstacleCamera.StatusSaveDialog instance_ = new XYObstacleCamera.StatusSaveDialog();
        private XYObstacleCamera owner_;
        private Rect wndRect_ = new Rect(0f, 0f, 2f, 2f);

        // Methods
        private StatusSaveDialog() {
        }

        public override void OnGUI(XYObstacleCamera owner) {
            this.owner_ = owner;
            wndRect_ = GUILayout.Window(102, wndRect_, WndFunc, "保存障碍数据");
        }

        public override void Update(XYObstacleCamera owner) {
        }

        private void WndFunc(int id) {
            GUILayout.Label("文件名：", new GUILayoutOption[0]);
            if (safeZone_)
                this.owner_.zone_filename = GUILayout.TextField(this.owner_.zone_filename, new GUILayoutOption[] { GUILayout.MinWidth(300f) });
            else
                this.owner_.obstacle_filename = GUILayout.TextField(this.owner_.obstacle_filename, new GUILayoutOption[] { GUILayout.MinWidth(300f) });
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("确定", new GUILayoutOption[0])) {
                this.owner_.SaveSafeZoneArray();
                this.owner_.SaveObstacleToFile();
            } else if (GUILayout.Button("取消", new GUILayoutOption[0])) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
            }
            GUILayout.EndHorizontal();
        }

        // Properties
        public override string Desc {
            get {
                return "保存……";
            }
        }

        public static XYObstacleCamera.StatusSaveDialog Instance {
            get {
                return instance_;
            }
        }
    }

    private class StatusSelectLayer : XYObstacleCamera.Status {
        // Fields
        public static readonly XYObstacleCamera.StatusSelectLayer Instance = new XYObstacleCamera.StatusSelectLayer();
        private XYObstacleCamera owner_;

        // Methods
        private StatusSelectLayer() {
        }

        public override void OnGUI(XYObstacleCamera owner) {
            this.owner_ = owner;
            GUILayout.Window(200,
                 new Rect(
                     Screen.width - 300,
                     20,
                     200, 360),
                     WndFunc, "选择辅助层");
        }

        public override void Update(XYObstacleCamera owner) {
        }

        private void WndFunc(int id) {
            if (GUILayout.Button("返回")) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
            }

            if (GUILayout.Button("All")) {
                this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
                this.owner_.UpdateLayer(-1);
            }

            foreach (int layer in owner_.navLayers_) {
                if (GUILayout.Button(layer.ToString())) {
                    this.owner_.CurrentStatus = XYObstacleCamera.StatusNormal.Instance;
                    this.owner_.UpdateLayer(layer);
                }
            }
        }

        public override string Desc {
            get {
                return "选择辅助层";
            }
        }
    }

}

class XYCameraSmooth {
    private Vector3 speed_;

    private bool active_ = true;
    public bool Active {
        get { return active_; }
        set {
            if (active_ != value) {
                active_ = value;
                speed_ = Vector3.zero;
            }
        }
    }
    public void Execute(Transform ts, Vector3 lookAt, Vector3 euler, float distance) {
        Vector3 dir = Quaternion.Euler(euler) * Vector3.forward;
        Vector3 target = lookAt + dir * distance;
        if (active_) {
            Vector3 current = ts.position;
            if (current != target) {
                current.x = Mathf.SmoothDamp(current.x, target.x, ref speed_.x, 0.3f);
                current.y = Mathf.SmoothDamp(current.y, target.y, ref speed_.y, 0.3f);
                current.z = Mathf.SmoothDamp(current.z, target.z, ref speed_.z, 0.3f);
                ts.position = current;
            } else {
                speed_ = Vector3.zero;
            }
        } else {
            ts.position = target;
        }
        ts.LookAt(lookAt);
    }
}
