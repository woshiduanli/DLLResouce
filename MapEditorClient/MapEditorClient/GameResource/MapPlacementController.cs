using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using G9ZUnityGUI;
using MapPlacement;
using System.IO;
using System;
using LitJson;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEditor;
[System.Serializable]
public class MapPlacementController : MonoBehaviour, IEnumerable<MPObject> {
    public static MapPlacementController instance = null;
    public static MapReference curMap = null;
    //public static MapReference
    public static string textResourcesPath = Directory.GetCurrentDirectory() + "/resources";
    public static string mapObjectConfigsDailySavePath =Directory.GetCurrentDirectory() + "/resources/MapObjectConfigJson/Daily/{0}_{1}_MapObjectConfigs.json";
    public static string mapObjectConfigsSavePath = Directory.GetCurrentDirectory() + "/resources/MapObjectConfigJson/{0}_MapObjectConfigs.json";
    public static string mapRoleObjectResLoadPathStrFormat = "res/role/{0}.role";
    public static string mapObjectResLoadPathStrFormat = "res/role/{0}.role";

    private const int SelectableLayer = XYClientCommon.StandingLayer | XYDefines.Layer.Mask.Player;
    private Dictionary<int,MPObject> mapObjs_ = new Dictionary<int,MPObject>();
    private UGUIPropertyGrid pg_ = new UGUIPropertyGrid();
    private UGUIPropertyGrid pgRoutePoint_ = new UGUIPropertyGrid();
    private MPObject curSelectObj_ = null;

    public MPObject curSelectObj{ 
        get{ return curSelectObj_; } 
        set{
            curSelectObj_ = value;
            pg_.selectedObject = value;
            curSelectRoutePoint = null;
            objectWantToDrag_ = null;
        }
    }
    private MPRole.RoutePoint curSelectRoutePoint_ = null;
    public MPRole.RoutePoint curSelectRoutePoint{ 
        get{ return curSelectRoutePoint_; } 
        set{ 
            curSelectRoutePoint_ = value;
            pgRoutePoint_.selectedObject = value;
        }
    }
    
    private bool allRoutePointMode = false;
    public bool AllRoutePointMode{ set{ allRoutePointMode = value; } get{ return allRoutePointMode; } }
	
	void Start () {
        MapPlacementController.instance = this;
        GameObject terrain = GameObject.FindWithTag("Terrain");
        if (terrain)
        {
            Collider co = terrain.GetComponent<Collider>();
            this.gameObject.transform.position = new Vector3(co.bounds.size.x / 2, 3.5f, co.bounds.size.z / 2);
        }
        lastSaveTime_ = Time.time;
        this.AllRoutePointMode = false;
        this.sowRect_ = new Rect( Screen.width - 200, 0, 0, 0 );
        this.tlwRect_ = new Rect( this.sowRect_.xMin - 200, 0, 0, 0 );
    }

    #region IEnumerable接口
    public IEnumerator<MPObject> GetEnumerator() {
        return mapObjs_.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return mapObjs_.Values.GetEnumerator();
    }
    #endregion

    #region 清除所有数据
    private void ClearAllData(){
        curSelectObj = null;
        Debug.Log( "clear current map data" );
        foreach( MPObject mpo in mapObjs_.Values ){
            mpo.DestroyGameObject();
        }
        mapObjs_.Clear();
    }
    #endregion

    #region 加载/保存数据
    private List<MPObject> PrepareSaveObjs(){
        List<MPObject> objsToSave = new List<MPObject>();
        foreach ( MPObject mpo in mapObjs_.Values ) {
            objsToSave.Add( mpo );
        }

        objsToSave.Sort(
            delegate( MPObject m1, MPObject m2 ) {
                //return m1.sn - m2.sn;
                if ( m1 is MPRole && m2 is MPRole ) {
                    return ( (MPRole)m1 ).teamID - ( (MPRole)m2 ).teamID;
                }
                else if ( m1 is MPRole ) {
                    return -1;
                }
                else {
                    return 1;
                }
            }
            );
        return objsToSave;
    }

    private  List<TemplateMapObjectConfig> PrepareSaveJson()
    {
        List<TemplateMapObjectConfig> objs = new List<TemplateMapObjectConfig>();
        foreach(MPObject mpo in mapObjs_.Values)
        {
            TemplateMapObjectConfig tempConfig = new TemplateMapObjectConfig();
            tempConfig.objType = mpo.ObjectType;
            tempConfig.refId = mpo.mocReference_.refid;
            tempConfig.pos = MyVector3.GetMyVector3(GameUtility.StrToVector3(mpo.mocReference_.position));
            tempConfig.dir = MyVector3.GetMyVector3(GameUtility.StrToVector3(mpo.mocReference_.direction));
            tempConfig.visible = mpo.available;
            tempConfig.sn = mpo.sn;
            tempConfig.isVisibleInMiddleMap = mpo.IsShowInMiddleMap;
            if (mpo.ObjectType != "Object")
            {
                MPRole mpoRole = mpo as MPRole;
                tempConfig.patrolType = mpoRole.patrolType.ToString();
                tempConfig.patrolPath = new List<MyVector3>();
                foreach (MPRole.RoutePoint point in mpoRole.routeList)
                    tempConfig.patrolPath.Add(MyVector3.GetMyVector3(point.position));
                tempConfig.camp = (int)mpoRole.camp;
                tempConfig.aiRefId = mpoRole.aiRefid;
                tempConfig.team = mpoRole.teamID;
            }else
            {
                MPItem mpItem = mpo as MPItem;
                tempConfig.team = mpItem.teamID;
            }
            objs.Add(tempConfig);
            //if (!Physics.Raycast(new Vector3(mpo.position.x, 10, mpo.position.z), Vector3.down, 200))
            //{
            //    Debug.Log("物体坐标非法  sn：" + mpo.sn);
            //}
        }
        return objs;
    }

    private class TemplateMapObjectConfig
    {
        public string objType;
        public int refId;
        public MyVector3 pos;
        public MyVector3 dir;
        public int team;
        public int camp;
        public int sn;
        public int aiRefId;
        public bool visible;
        public bool isVisibleInMiddleMap;
        public String patrolType;
        public List<MyVector3> patrolPath;
    }

    public class MyVector3
    {
        public MyVector3() { }
        public MyVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public  Vector3 GetVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
        public static MyVector3 GetMyVector3(Vector3 vector)
        {
            return new MyVector3(vector.x, vector.y, vector.z);
        }
        public float x;
        public float y;
        public float z;
    }


    private void SaveToJson(string filePath)
    {
        List<TemplateMapObjectConfig> objectsOld=null;
        if (objectsOld == null) objectsOld = new List<TemplateMapObjectConfig>();
        List<TemplateMapObjectConfig> objsToSave = PrepareSaveJson();
        string json = JsonMapper.ToJson(objsToSave);
        var sw=File.CreateText(filePath);
        sw.Write(json);
        sw.Close();

        string name = Path.GetFileNameWithoutExtension(filePath);
        string nameForFormatedFile = string.Format("{0}_换行", name);
        string pathForFormatFile = filePath.Replace(name, nameForFormatedFile);
        using(var swForFormatFile = File.CreateText(pathForFormatFile))
        {
            swForFormatFile.Write("[\n");
            for (int i = 0; i < objsToSave.Count; i++)
            {
                string jsonSingleConfig = JsonMapper.ToJson(objsToSave[i]);
                swForFormatFile.Write(jsonSingleConfig);
                if (i < objsToSave.Count - 1)
                    swForFormatFile.Write(",");
                swForFormatFile.Write("\n");
            }
            swForFormatFile.Write("]");
        }
    }

    private IEnumerator LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
            yield break;
        StreamReader sr = File.OpenText(filePath);
        string json=sr.ReadToEnd();
        sr.Close();
        List<TemplateMapObjectConfig> objsRead = JsonMapper.ToObject<List<TemplateMapObjectConfig>>(json);
        if (objsRead == null) yield break;
        for (int i = 0; i < objsRead.Count; i++)
        {
            yield return null;//每一个都延迟一帧执行，以免加载角色出错
            MPObject mapObj = null;
            Vector3 positon = objsRead[i].pos.GetVector3();
            Vector3 direction = objsRead[i].dir.GetVector3();

            if (objsRead[i].objType != "Object")
                mapObj = new MPRole();
            else
                mapObj = new MPItem();
            mapObj.refid = objsRead[i].refId;
            mapObj.ObjectType = objsRead[i].objType;
            RoleReference reference = Global.role_mgr.GetReference(mapObj.refid);
            if (reference == null)
            {
                Debug.LogError(string.Format("无此role表Id：{0}", mapObj.refid));
            }else
                mapObj.apprId = reference.Apprid;
            mapObj.map = curMap.ID;
            mapObj.sn = objsRead[i].sn;
            mapObj.ID = curMap.ID * 10000 + mapObj.sn;
            mapObjs_[mapObj.sn] = mapObj;
            mapObj.available = objsRead[i].visible;
            mapObj.camp = (MPObject.ObjectCamp)objsRead[i].camp;
            mapObj.IsShowInMiddleMap = objsRead[i].isVisibleInMiddleMap;
            if (mapObj.ObjectType != "Object")
            {
                MPRole mpRole = mapObj as MPRole;
                mpRole.reference = Global.role_mgr.GetReference(mpRole.refid);
                mpRole.teamID = objsRead[i].team;
                mpRole.patrolType = (MPRole.PatrolType)Enum.Parse(typeof(MPRole.PatrolType), objsRead[i].patrolType);
                foreach (MyVector3 myVec in objsRead[i].patrolPath)
                    mpRole.routeList.Add(new MPRole.RoutePoint(myVec.GetVector3(), 0));
                mpRole.aiRefid = objsRead[i].aiRefId;
            }
            else
            {
                MPItem item = mapObj as MPItem;
                item.reference = Global.role_mgr.GetReference(item.refid);
                item.teamID = objsRead[i].team;
            }

            //if (!Physics.Raycast(new Vector3(positon.x, 10, positon.z), Vector3.down, 200))
            //{
            //    Debug.Log("物体坐标非法  sn：" + mapObj.sn);
            //}
            mapObj.CreateGameObject((a) =>
            {
                mapObj.position = positon;
                mapObj.direction = direction;
            });
        }
    }

    #region nouse
    private void SaveToXml( string filePath ){
        Debug.Log( filePath );
        List<MPObject> objsToSave = PrepareSaveObjs();

        XmlDocument doc = new XmlDocument();
        XmlElement ele = doc.CreateElement( "All" );
        doc.AppendChild( ele );

        for( int i = 0; i < objsToSave.Count; i++ ){
            objsToSave[i].SaveToXml( doc );
        }
        doc.Save( filePath );

        lastSaveTime_ = Time.time;
    }

    private void SaveToText() {
        Debug.Log( "Saving to text..." );
        using ( ResourceUtilWrite writer = new ResourceUtilWrite( textResourcesPath, "MapObjectConfig" ) ) {
            writer.DeleteData( "map", (int)curMap.ID );
            List<MPObject> objsToSave = PrepareSaveObjs();

            foreach ( MPObject mpo in objsToSave ) {
                writer.AddData( mpo.GetSaveToTextObject() );
            }
            
            writer.SaveToFile("./resources", writer.GetVersion());
        }
        Debug.Log( "Done" );
    }

    private void LoadFromText() {
        Global.mapobj_config_mgr.ReloadDataFromFile(textResourcesPath, EditionType.ALL, false);
        Global.mapobj_config_mgr.ForEach(delegate(MapObjectConfigReference moc) 
        {
                if( moc.map == curMap.ID ){
                    MPObject mp;
                    if ( moc.isRole ) {
                        mp = new MPRole();
                    }
                    else {
                        mp = new MPItem();
                    }
                    try {
                        mp.LoadFromText( moc );
                        //mp.CreateGameObject();
                        mapObjs_.Add( mp.sn, mp );
                    }
                    catch ( System.Exception exp ) {
                        errorCaught_ = true;
                        Debug.Log( exp.Message );
                        Debug.Log( exp.StackTrace );
                    }
                }
            }
        );
    }

    private void LoadFromXml( string filePath ){
        try {
            XmlDocument doc = new XmlDocument();
            doc.Load( filePath );
            XmlNodeList xnList = doc.DocumentElement.ChildNodes;
            foreach ( XmlNode node in xnList ) {
                XmlElement ele = node as XmlElement;
                MPObject mp;
                //Debug.Log( string.Format( "{0},{1}", ele.LocalName, ele.Name ) );
                if( ele.Name == "Role" ){
                    mp = new MPRole();
                }
                else if( ele.Name == "Item" ){
                    mp = new MPItem();
                }
                else{
                    mp = new MPObject();
                }
               
                try{
                    mp.LoadFromXml( ele );
                    //mp.CreateGameObject();
                    mapObjs_.Add( mp.sn, mp );
                }
                catch(System.Exception exp ){
                    errorCaught_ = true;
                    Debug.Log( exp.Message );
                    Debug.Log( exp.StackTrace );
                }
                
            }

            lastSaveTime_ = Time.time;
        }
        catch ( System.Exception exp ) {
            Debug.Log( exp.ToString() );
            Debug.Log( exp.StackTrace );
        }
    }
    #endregion

    #endregion

    #region 批量修正位置差
    public static bool OnFixPositionCmd( string[] fields, ref string processedCmd ){
        float fixX, fixZ;
        try{
            fixX = float.Parse( fields[1] );
            fixZ = float.Parse( fields[2] );
            Debug.Log( string.Format("fixposition,{0},{1}", fixX, fixZ ));
            MapPlacementController.instance.FixPosition( fixX, fixZ );
            return true;
        }
        catch( System.Exception exp ){
            Debug.Log( exp.Message );
            Debug.Log( exp.StackTrace );
            return false;
        }
    }

    private void FixPosition( float x, float z ){
        foreach( MPObject mpo in mapObjs_.Values ){
            mpo.FixPosition( x, z );
        }
    }
    #endregion

    #region 取得可用的无重复的sn
    int GetUseableSN() {
        int sn = 1;
        while ( sn < int.MaxValue ) {
            if ( !mapObjs_.ContainsKey( sn ) ) {
                return sn;
            }
            sn++;
        }
        return -1;
    }
    #endregion

    #region 控制面板窗口
    private Rect bswRect_ = new Rect( 0, 0, 0, 0 );
    private UGUIConfirmButton quitConfirm_ = new UGUIConfirmButton( "返回", 2.0f );
    private UGUIConfirmButton saveToJsonConfirm_ = new UGUIConfirmButton( "保存到Json", 1.5f );
    private UGUIConfirmButton loadFromXmlConfirm_ = new UGUIConfirmButton( "从Json加载", 1.5f );
    //private UGUIConfirmButton loadFromMysqlConfirm_ = new UGUIConfirmButton( "从Mysql加载", 1.5f );
    //private UGUIConfirmButton saveToMysqlConfirm_ = new UGUIConfirmButton( "保存Mysql", 1.5f );
    //private UGUIConfirmButton loadFromTextConfirm_ = new UGUIConfirmButton( "从Text加载", 1.5f );
    //private UGUIConfirmButton saveToTextConfirm_ = new UGUIConfirmButton( "保存Text", 1.5f );
    private bool showAirWall_ = true;

    void BaseSettingWindow( int wndID ){
        try{
            GUILayout.Label( string.Format( "{0} ** {1}", curMap.Name, "" ) );
            GUILayout.BeginHorizontal();
            RenderSettings.fog = GUILayout.Toggle( RenderSettings.fog, "雾" );
            if ( quitConfirm_.OnDraw() ) {
                SceneManager.LoadScene( "startMenu" );
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if ( saveToJsonConfirm_.OnDraw() ) {
                //SaveToXml( string.Format( "res/data/{0}.map.xml", curMap.ConfigFileName ) );
                SaveToJson(string.Format(mapObjectConfigsDailySavePath, curMap.ID,DateTime.Now.Day+"-"+DateTime.Now.Hour+"-"+DateTime.Now.Minute));
                SaveToJson(string.Format(mapObjectConfigsSavePath, curMap.ID));
            }
            if ( loadFromXmlConfirm_.OnDraw() ) {
                ClearAllData();
                XYCoroutineEngine.Execute(LoadFromJson(String.Format(mapObjectConfigsSavePath,curMap.ID)));
                //LoadFromXml( string.Format( "res/data/{0}.map.xml", curMap.ConfigFileName ) );
            }
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //if ( saveToTextConfirm_.OnDraw() ) {
            //    SaveToText();
            //}
            //if ( loadFromTextConfirm_.OnDraw() ) {
            //    ClearAllData();
            //    LoadFromText();
            //}
            //GUILayout.EndHorizontal();

            if ( GUILayout.Button( "空气墙" ) ) {
                showAirWall_ = !showAirWall_;
                XYClientCommon.AirWall.EnableRender( showAirWall_ );
            }
            //GUILayout.Label( string.Format( "对象统计:{0}/{1}", this.mapObjs_.Count, curMap.ObjCount ) );
			GUILayout.Label(string.Format("对象统计:{0}", this.mapObjs_.Count));
            if (GUILayout.Button("位置检查"))
            {
                CheackInvalidPos();
            }
            if (GUILayout.Button("移除非法物体"))
            {
                RemoveInvalidGo();
            }
            GUI.DragWindow( new Rect( 0, 0, 10000, 20 ) );
        }
        catch( System.Exception exc ){
            Debug.Log( exc.Message );
            Debug.Log( exc.StackTrace );
        }
        
    }

    private void RemoveInvalidGo()
    {
        List<int> keys = new List<int>();
        foreach (KeyValuePair<int, MPObject> pair in mapObjs_)
        {
            Vector3 postion = pair.Value.position;
            RaycastHit hit;
            if(!XYClientCommon.RaycastDown(postion,false,out hit))
            { 
                keys.Add(pair.Key);
            }
        }
        foreach (int key in keys)
        {
            GameObject removeGo = mapObjs_[key].mapGO;
            mapObjs_.Remove(key);
            GameObject.DestroyImmediate(removeGo);
        }
    }

    private void CheackInvalidPos()
    {
        
        foreach (KeyValuePair<int,MPObject> pair in mapObjs_)
        {
            Vector3 postion = pair.Value.position;
            RaycastHit hit;
            if (!XYClientCommon.RaycastDown(postion, false, out hit))
            {
                Debug.Log("物体坐标非法  sn：" + pair.Key);
            }
        }
    }
    #endregion

    #region 编辑对象选择窗口
    public class ReferenceSearchResult {
        public string text = string.Empty;
        public int refid = 0;
    }
    private Rect tswRect_ = new Rect( 300, 0, 0, 0 );
    private string editTargetID_ = "0";
    private string searchKeyWord_ = "";
    private int objectType_ = -1;
    private int searchSelectIndex_ = -1;
    ReferenceSearchResult[] searchResult_ = null;
    private Vector2 scrollPosition_ = Vector2.zero;

    //执行搜索
    void DoSearch(){
        return;
        List<ReferenceSearchResult> vTempResult = new List<ReferenceSearchResult>();
        searchResult_ = null;
        searchSelectIndex_ = -1;
       
        if ( objectType_ == 0 ) {
            Global.role_mgr.ForEach(
                delegate( RoleReference role ) 
                {
                    if (role.Name.Contains(searchKeyWord_))
                    {
                        ReferenceSearchResult rsr = new ReferenceSearchResult();
                        rsr.text = string.Format( "{0} , {1}", role.Name, role.ID );
                        rsr.refid = role.ID;
                        vTempResult.Add( rsr );
                    }
                }
                );
        }
        else if ( objectType_ == 1 ) {
            Global.mapobj_r_mgr.ForEach(
                delegate( MapObjectReference moRef ) {
                    if ( moRef.Name.Contains( searchKeyWord_ ) ) {
                        ReferenceSearchResult rsr = new ReferenceSearchResult();
                        rsr.text = string.Format( "{0} , {1}", moRef.Name, moRef.ID );
                        rsr.refid = moRef.ID;
                        vTempResult.Add( rsr );
                    }
                }
                );
        }
        searchResult_ = vTempResult.ToArray();
    }

    void TargetSelectWindow( int wndID ){
        GUILayout.BeginHorizontal();
        objectType_ = GUILayout.SelectionGrid( objectType_, new string[]{"Monster","NPC", "Object" }, 2 );
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label( "对象参考ID:" );
        editTargetID_ = GUILayout.TextArea( editTargetID_ );
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label( "搜索:" );
        searchKeyWord_ = GUILayout.TextArea( searchKeyWord_ );
        if ( GUILayout.Button( "GO" ) && searchKeyWord_ != string.Empty ) {
            DoSearch();
        }
        GUILayout.EndHorizontal();

        if ( searchSelectIndex_ == -1 && searchResult_ != null && searchResult_.Length > 0 ) {
            scrollPosition_ = GUILayout.BeginScrollView( scrollPosition_, GUILayout.Height( 200 ) );
            string[] items = new string[searchResult_.Length];
            for( int i = 0; i < items.Length; i++ ){
                items[i] = searchResult_[i].text;
            }
            searchSelectIndex_ = GUILayout.SelectionGrid( searchSelectIndex_, items, 1 );
            if( searchSelectIndex_ >= 0 ){
                editTargetID_ = searchResult_[searchSelectIndex_].refid.ToString();
                searchResult_ = null;
            }
            GUILayout.EndScrollView();
        }
        GUI.DragWindow( new Rect( 0, 0, 10000, 20 ) );
        
    }
    #endregion

    #region 对象定位、筛选显示窗口
    private string targetSN_ = "";
    private int filterRefid_ = 0;
    private Rect tlwRect_ = new Rect( 500, 0, 0, 0 );
    //执行定位
    void DoLocateTarget() {
        int snTo = 0;
        int.TryParse( targetSN_, out snTo );
        MPObject mo = null;
        mapObjs_.TryGetValue( snTo, out mo );
        //foreach(MPObject obj in mapObjs_.Values)
        //{
        //    if (obj.refid == snTo)
        //        mo= obj;
        //}
        if ( mo != null ) {
            //this.gameObject.transform.position  = mo.position;
            Transform thisTF = this.gameObject.transform;
            Vector3 CameraForwardXZ = ( new Vector3( thisTF.forward.x, 0, thisTF.forward.z ) ).normalized;
            float CameraToObejctY= Mathf.Abs( thisTF.position.y - mo.position.y );
            thisTF.position = mo.position - CameraForwardXZ * CameraToObejctY;
            thisTF.position = new Vector3( thisTF.position.x, thisTF.position.y + CameraToObejctY, thisTF.position.z );

            curSelectObj = mo;
            curSelectObj_.DrawInfo();

        }
    }

    void TargetLocateWindow( int wndID ){
        GUILayout.BeginHorizontal();
        GUILayout.Label( "显示参考ID:" );
        int.TryParse( GUILayout.TextField( filterRefid_.ToString() ), out filterRefid_ );
        GUILayout.Label( sameRefidSum_.ToString() );
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label( "定位SN:" );
        targetSN_ = GUILayout.TextArea( targetSN_ );
        if ( GUILayout.Button( "GO" ) ) {
            DoLocateTarget();
        }
        GUILayout.EndHorizontal();
        GUI.DragWindow( new Rect( 0, 0, 10000, 20 ) );
    }
    #endregion

    #region 选中的对象属性编辑窗口
    private Rect sowRect_ = new Rect( 800, 0, 0, 0 );
    private UGUIConfirmButton applyAISetting_ = new UGUIConfirmButton( "将AI应用到同类对象", 1.0f );
    void ApplyAIToObjectsWithSameID( MPRole role ){
        foreach( MPObject mo in mapObjs_.Values ){
            if( mo is MPRole && ((MPRole)mo).refid == role.refid ){
                ((MPRole)mo).aiRefid = role.aiRefid;
            }
        }
    }
    void SelectedObjectProperty( int wndID ){
        GUILayout.Label( "******选中的对象********" );
        if( pg_.selectedObject is MPRole ){
            if( applyAISetting_.OnDraw() ){
                ApplyAIToObjectsWithSameID( pg_.selectedObject as MPRole );
            }
        }
        pg_.OnGUI();
        if( curSelectRoutePoint_ != null ){
            GUILayout.Label( "*****选中的路径点*****" );
            pgRoutePoint_.OnGUI();
        }
        GUI.DragWindow( new Rect( 0, 0, 10000, 20 ) );
    }
    #endregion
    
    #region OnGUI()
    bool errorCaught_ = false;
    int sameRefidSum_ = 0;
    void OnGUI(){
        if( errorCaught_ ){
            GUILayout.TextArea( "读取时发生错误，成娟" );
            if( GUILayout.Button( "返回" ) ){
                SceneManager.LoadScene( "startMenu" );
            }
            return;
        }
        if (Event.current.type == EventType.MouseDrag 
            && Input.GetMouseButton(0) 
            && !Input.GetKey(KeyCode.LeftControl) 
            && !Input.GetKey(KeyCode.LeftShift) 
            && Event.current.modifiers != EventModifiers.Alt) {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if( objectWantToDrag_ != null ) {
                if( Physics.Raycast( ray, out hit, 2000.0f, XYClientCommon.StandingLayer ) ) {
                    if( objectWantToDrag_ is MPObject ){
                        curSelectObj_.position = hit.point;
                    }
                    else if( objectWantToDrag_ is MPRole.RoutePoint ){
                        curSelectRoutePoint_.position = hit.point;
                    }
                }
            }
            else {
                CameraMovementHandle();
            }
        }
        //GUI.skin = guiSkin;
        bswRect_ = GUILayout.Window( 1, bswRect_, BaseSettingWindow, "控制面板", GUILayout.Width( 200 ) );
        tswRect_ = GUILayout.Window( 2, tswRect_, TargetSelectWindow, "编辑类型", GUILayout.Width( 200 ), GUILayout.Height( 100 ) );
        sowRect_ = GUILayout.Window( 3, sowRect_, SelectedObjectProperty, "选中的对象", GUILayout.Width( 200 ), GUILayout.Height( 500 ) );
        tlwRect_ = GUILayout.Window( 4, tlwRect_, TargetLocateWindow, "定位", GUILayout.Width( 200 ), GUILayout.Height( 100 ) );

        if( curSelectObj_ != null ){
            curSelectObj_.DrawInfo();
        }
        if( curSelectRoutePoint_ != null ){
            curSelectRoutePoint_.DrawInfo();
        }

        if( Input.GetKey( KeyCode.F1 ) ){
            GUI.TextArea( new Rect( 400, 250, 300, 100 ), "F1键:显示帮助\r\nF2键:显示角色类的对象\r\nF3键:显示地图物件类对象\r\nF4键:显示“不可见”的对象\r\nF5键:只显示指定参考ID的对象\r\nF6键:显示所有路径" );
        }
        if( Input.GetKey( KeyCode.F2 ) ){
            foreach( MPObject mpo in mapObjs_.Values ){
                if( mpo is MPRole ){
                    mpo.DrawInfo();
                }
            }
        }
        if ( Input.GetKey( KeyCode.F3 ) ) {
            foreach ( MPObject mpo in mapObjs_.Values ) {
                if ( mpo is MPItem ) {
                    mpo.DrawInfo();
                }
            }
        }
        if ( Input.GetKey( KeyCode.F4 ) ) {
            foreach ( MPObject mpo in mapObjs_.Values ) {
                if ( !mpo.available ) {
                    mpo.DrawInfo();
                }
            }
        }
        if ( Input.GetKey( KeyCode.F5 ) ) {
            sameRefidSum_ = 0;
            foreach ( MPObject mpo in mapObjs_.Values ) {
                if ( mpo.refid == filterRefid_ ) {
                    sameRefidSum_++;
                    mpo.DrawInfo();
                }
            }
        }
    }
    #endregion

    #region RectSelectComponentOP
    public List<MPObject> PasteObject(List<MPObject> mapobjs) {
        List<MPObject> result = new List<MPObject>();
        if (mapobjs.Count <= 0)
            return result;

        //创建并打包
        GameObject parent = new GameObject("parent");
        parent.transform.position = mapobjs[0].position;
        for (int i = 0; i < mapobjs.Count; i++) {
            MPObject mpobj = Clone(mapobjs[i]);
            result.Add(mpobj);
            mpobj.mapGO.transform.parent = parent.transform;
        }

        //解包
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            parent.transform.position = hit.point;
        }
        for (int i = 0; i < result.Count; i++) {
            result[i].mapGO.transform.parent = null;
            ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(result[i].mapGO.transform.position));
            if (Physics.Raycast(ray, out hit)) {
                result[i].position = hit.point;
            }
        }
        Destroy(parent);

        return result;
    }

    public void DeleteObject(MPObject mapobj) {
        if (mapobj != null) {
            if (curSelectObj_ == mapobj)
                curSelectObj_ = null;
            mapobj.DestroyGameObject();
            mapObjs_.Remove(mapobj.sn);
        }
    }

    public void DragObject(List<MPObject> mapobjs) {
        if (mapobjs.Count <= 0 || dragParent == null)
            return;

        Ray ray;
        RaycastHit hit;
        RaycastHit hit2;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 2000.0f, XYClientCommon.StandingLayer)) {

                bool canDrag = true;
                for (int i = 0; i < mapobjs.Count; i++) {
                    ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(hit.point+mapobjs[i].mapGO.transform.localPosition));
                    if (!Physics.Raycast(ray, out hit2, 2000.0f, XYClientCommon.StandingLayer)) {
                        canDrag = false;
                        break;
                    }
                }

                if (canDrag)
                dragParent.position = hit.point;
            }
        
    }

    private Transform dragParent = null;
    public void DragBeginObject(List<MPObject> mapobjs) {
        if (mapobjs.Count <= 0)
            return;

        dragParent = new GameObject("dragParent").transform;
        dragParent.position = mapobjs[0].position;

        for (int i = 0; i < mapobjs.Count; i++) {
            mapobjs[i].mapGO.transform.parent = dragParent;
        }
    }

    public void DragEndObject(List<MPObject> mapobjs) {
        if (mapobjs.Count <= 0 || dragParent == null)
            return;
        for (int i = 0; i < mapobjs.Count; i++) {
            mapobjs[i].mapGO.transform.parent = null;
            mapobjs[i].position = mapobjs[i].position;
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(mapobjs[i].position));
            //if (Physics.Raycast(ray, out hit)) {
            //    mapobjs[i].position = hit.point;
            //}
        }
        Destroy(dragParent.gameObject);
    }
    #endregion

    #region 创建一个对象
    MPObject CreateObject(Vector3 pos)
    {
        int refid;
        int.TryParse(editTargetID_, out refid);
        MPObject mapobj = null;

        //暂时使用同一个reference
        RoleReference reference = Global.role_mgr.GetReference(refid);
        if (reference == null)
        {
            Debug.Log("无此ID" + refid);
            return null;
        }
        if (objectType_ == 2)
        {
            if(mapObjectResLoadPathStrFormat.Length<=0)
            {
                Debug.Log("待添加物件资源路径");
                return null;
            }
            mapobj = new MPItem();
            ((MPItem)mapobj).reference = reference;
        }
        else
        {
            mapobj = new MPRole();
            ((MPRole)mapobj).reference = reference;
        }
        if (objectType_ == 0)
            mapobj.ObjectType = "Monster";
        if (objectType_ == 1)
            mapobj.ObjectType = "Npc";
        if (objectType_ == 2)
            mapobj.ObjectType = "Object";
        mapobj.map = curMap.ID;
        mapobj.refid = refid;
        mapobj.apprId = reference.Apprid;
        mapobj.sn = GetUseableSN();
        mapobj.ID = reference.ID;
        mapobj.CreateGameObject((a) => { mapObjs_[a.sn] = a; a.position = pos; });
        return mapobj;

    }

    private void OnLoadObjectFinish(MPObject mpObject)
    {
        mapObjs_[mpObject.sn] = mpObject;
    }

    object DeepCopy(object obj) {
        System.Object targetDeepCopyObj=null;

        if (obj == null)
            return obj;

        
        Type targetType = obj.GetType();
        //值类型  
        if (targetType.IsValueType == true) {
            targetDeepCopyObj = obj;
        }
            //引用类型   
        else {
            if (obj is UnityEngine.Object)
                targetDeepCopyObj = UnityEngine.Object.Instantiate(obj as UnityEngine.Object);
            else
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象   
            System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();
            foreach (System.Reflection.MemberInfo member in memberCollection) {
                if (member.MemberType == System.Reflection.MemberTypes.Field) {
                    System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                    System.Object fieldValue = field.GetValue(obj);
                    if (fieldValue is ICloneable) {
                        field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                    } else {
                        field.SetValue(targetDeepCopyObj, DeepCopy(fieldValue));
                    }

                } else if (member.MemberType == System.Reflection.MemberTypes.Property) {
                    System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                    MethodInfo info = myProperty.GetSetMethod(false);
                    if (info != null) {
                        object propertyValue = myProperty.GetValue(obj, null);
                        if (propertyValue is BaseObject)
                            continue;
                        if (propertyValue is ICloneable) {
                            myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                        } else {
                            myProperty.SetValue(targetDeepCopyObj, DeepCopy(propertyValue), null);
                        }
                    }

                }
            }
        }
        return targetDeepCopyObj;
    }  

    MPObject Clone(MPObject obj) {
        MPObject mapobj = DeepCopy(obj) as MPObject;
        
        mapobj.sn = GetUseableSN();
        mapobj.ID = curMap.ID * 10000 + mapobj.sn;
        mapobj.mapGO.name = mapobj.sn.ToString();
        //mapobj.CreateGameObject(OnLoadObjectFinish);
        mapObjs_[mapobj.sn] = mapobj;
        return mapobj;
    }

    #endregion

    #region 摄像机位置控制
    private Vector3 lastFrameMousePosition_ = Vector3.zero;
    private void CameraMovementHandle() {
        if( lastFrameMousePosition_ == Vector3.zero ){
            lastFrameMousePosition_ = Input.mousePosition;
        }
        if( lastFrameMousePosition_ != Input.mousePosition ) {            
            Vector3 newPos = ( lastFrameMousePosition_ - Input.mousePosition ) * this.transform.position.y / 1000.0f;
            this.transform.Translate( newPos.x*XYFreeCamera.MoveSpeed, 0, newPos.y*XYFreeCamera.MoveSpeed );
            lastFrameMousePosition_ = Input.mousePosition;
        }
    }
    #endregion

    #region  编辑对象旋转摆放控制
    private void ObjectRotationObject() {
        if( curSelectObj_ != null ) {
            if( Input.GetMouseButton( 0 ) ) {
                Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hit;
                if( Physics.Raycast( ray, out hit, XYClientCommon.StandingLayer ) ) {
                    curSelectObj_.direction = hit.point - curSelectObj_.position;
                }
            }
        }
    }
    #endregion

    #region 自动存档
    float lastSaveTime_ = 0.0f;
    float autoSaveInterval_ = 60;
    void AutoSave(){
        if (Time.time - lastSaveTime_ >= autoSaveInterval_)
        {
            if (!Directory.Exists("AutoSave"))
            {
                Directory.CreateDirectory("AutoSave");
            }
            System.DateTime dt = System.DateTime.Now;
            string nowTime = string.Format("{0}-{1}-{2} {3}点{4}分", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
            //SaveToXml(string.Format("AutoSave/{0}.{1}.map.xml", curMap.ConfigFileName, nowTime));
            SaveToJson(string.Format("AutoSave/{0}.{1}.map.json", curMap.Name, nowTime));
            lastSaveTime_ = Time.time;
        }
    }
    #endregion

    #region LateUpdate()
    private System.Object objectWantToDrag_ = null;
    private Vector3 mouseButton1DownPos;
    void LateUpdate () {
        AutoSave();

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit))
            {
                mouseButton1DownPos = hit.point;
            }
        }
        if( Input.GetKey( KeyCode.LeftControl ) ) {
            ObjectRotationObject();
            if ( Input.GetKeyUp( KeyCode.Delete ) ) {
                if ( curSelectRoutePoint_ != null ) {
                    ( (MPRole)curSelectObj_ ).RemoveRoutePoint( curSelectRoutePoint_ );
                    curSelectRoutePoint = null;
                    objectWantToDrag_ = null;
                }
                else if ( curSelectObj_ != null ) {
                    curSelectObj_.DestroyGameObject();
                    mapObjs_.Remove( curSelectObj_.sn );
                    curSelectObj = null;
                }
            }
        }
        else if ( Input.GetKey( KeyCode.LeftShift ) ) {
            if( curSelectObj_ is MPRole && Input.GetMouseButtonUp( 0 ) ){
                if( Physics.Raycast( ray, out hit, 2000.0f, XYClientCommon.StandingLayer )){
                    ((MPRole)curSelectObj_).routeList.Add( new MPRole.RoutePoint( hit.point, -1 ) );
                }
                
            }
        }
        else if( Input.GetMouseButtonUp( 1 ) ) {
            if (Physics.Raycast(ray, out hit) && Vector3.Distance(hit.point, mouseButton1DownPos) <= 0.1)
            {
                CreateObject(hit.point);
            }
        }
        else if( Input.GetMouseButtonUp( 0 ) ) {
            objectWantToDrag_ = null;
            lastFrameMousePosition_ = Vector3.zero;
            if( Physics.Raycast( ray, out hit, 2000.0f, SelectableLayer ) ) {
                if( hit.collider.gameObject.layer == XYDefines.Layer.Player ) {
                    int sn;
                    if ( int.TryParse( hit.collider.gameObject.name, out sn ) || int.TryParse(hit.collider.transform.parent.name,out sn) ) {
                        MPObject mpo;
                        if ( mapObjs_.TryGetValue( sn, out mpo ) ) {
                            this.curSelectObj = mpo;
                        }
                    }
                }
                else{
                    if( this.AllRoutePointMode ){
                        foreach( MPObject mpo in this.mapObjs_.Values ){
                            if( mpo is MPRole ){
                                MPRole.RoutePoint mpRP = ( (MPRole)mpo ).GetCloseRoutePoint( hit.point, 0.3f );
                                if ( mpRP != null ) {
                                    this.curSelectObj = mpo;
                                    curSelectRoutePoint = mpRP;
                                    break;
                                }
                            }
                        }
                    }
                    else if ( curSelectObj_ is MPRole ) {
                        MPRole.RoutePoint mpRP = ((MPRole)curSelectObj_).GetCloseRoutePoint( hit.point, 0.3f );
                        if( mpRP != null ){
                            curSelectRoutePoint = mpRP;
                        }
                    }
                }
            }
        }
        else if( Input.GetMouseButtonDown( 0 ) ){
            lastFrameMousePosition_ = Input.mousePosition;
            if( curSelectObj_ != null ) {
                if( curSelectObj_.mapGO.GetComponent<Collider>().Raycast( ray, out hit, 2000.0f )){
                    objectWantToDrag_ = curSelectObj_;
                }
                else
                {
                    #region 有些物体美术在子物体中添加了包围盒并且自己添加的包围盒不合适，所以这里判断一下子物体有没有包围盒
                    bool isGet = false;
                     List<GameObject> childs = curSelectObj_.mapGO.GetAllChildren();
                    if (childs != null)
                    {
                        foreach(GameObject obj in childs)
                        {
                            Collider collider;
                            if ((collider=obj.GetComponent<Collider>()) && collider.Raycast(ray, out hit, 2000.0f))
                            {
                                objectWantToDrag_ = curSelectObj;
                                isGet = true;
                                break;
                            }
                        }
                    }
                    #endregion
                    if (!isGet && curSelectRoutePoint_ != null && Physics.Raycast(ray, out hit, 2000.0f, XYClientCommon.StandingLayer))
                    {
                        objectWantToDrag_ = curSelectRoutePoint_ == ((MPRole)curSelectObj_).GetCloseRoutePoint(hit.point, 0.3f) ? curSelectRoutePoint_ : null;
                    }
                }
            }
        }        
        else if( Input.GetKeyUp( KeyCode.Escape ) ) {
            curSelectObj = null;
        }
        else if( Input.GetKeyUp( KeyCode.F6 ) ){
            this.AllRoutePointMode = !this.AllRoutePointMode;
        }
    }
    #endregion
}

public class MapInfo
{
    public int Id;
    public string Name;
    public string FileName;
    public float Width;
    public float Height;
}
