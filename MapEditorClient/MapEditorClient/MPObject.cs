using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Reflection;
using Model;
using System;
using Object = UnityEngine.Object;

namespace MapPlacement
{
    #region 地图摆放基础对象
    public class MPObject
    {

        /// <summary>
        /// 版本类型表示，为了方便编辑，将server定义的const int方式换成枚举方式
        /// </summary>
        public enum ObjectEditionType
        {
            Common = EditionType.COMMON,
            Game9z = EditionType.CHINA,
            Safe360 = EditionType.ET360,
        }

        /// <summary>
        ///怪物阵营，为了方便编辑，将server定义的const int方式换成枚举方式
        /// </summary>
        public enum ObjectCamp
        {
            玩家 = CampType.A,
            怪物物件 = CampType.B,
            全部友好 = CampType.C,
            玩家友好 = CampType.D,
            //CampE = CampType.E,
            //CampF = CampType.F,
            //CampAllFriendly = CampType.FRIEND,
        }

        public MapObjectConfigReference mocReference_;

        public MapObjectConfigReference GetSaveToTextObject()
        {
            OnSaveToText();
            return mocReference_;
        }

        public MPObject()
        {
            mocReference_ = new MapObjectConfigReference();
            mocReference_.available = true;
            mocReference_.direction = GameUtility.VectorToStr(Vector3.zero);
            mocReference_.aiRefid = 0;
            mocReference_.camp = CampType.D;
            mocReference_.isRole = true;
            mocReference_.map = 0;
            mocReference_.Name = "";
            mocReference_.patrolType = "fix";
            mocReference_.position = GameUtility.VectorToStr(Vector3.zero);
            mocReference_.refid = 0;
            mocReference_.route = "";
            mocReference_.sn = 0;
            mocReference_.teamID = -1;
            mocReference_.EditionType = EditionType.COMMON;
        }

        #region 是否显示在中地图
        private bool _isShowInMiddleMap;
        [Category("中地图显示")]
        public bool IsShowInMiddleMap
        {
            get { return _isShowInMiddleMap; }
            set
            {
                _isShowInMiddleMap = value;
            }
        }
        #endregion

        #region 版本类型
        private ObjectEditionType editionType_ = ObjectEditionType.Common;
        [CategoryAttribute("地区版本")]
        public ObjectEditionType editionType
        {
            get { return editionType_; }
            set
            {
                editionType_ = value;
                mocReference_.EditionType = (short)editionType_;
            }
        }
        #endregion

        #region 阵营
        private ObjectCamp camp_ = ObjectCamp.怪物物件;
        [CategoryAttribute("阵营")]
        public ObjectCamp camp
        {
            get { return camp_; }
            set
            {
                camp_ = value;
                mocReference_.camp = (int)camp_;
            }
        }
        #endregion

        #region ID
        public int ID
        {
            get { return this.mocReference_.ID; }
            set { this.mocReference_.ID = value; }
        }
        #endregion

        #region 是否出生时就可用 available
        private bool available_ = true;
        [CategoryAttribute("是否可见")]
        public bool available
        {
            get { return available_; }
            set
            {
                available_ = value;
                mocReference_.available = value;
            }
        }
        #endregion

        #region 地图
        public int map
        {
            set { mocReference_.map = value; }
            get { return mocReference_.map; }
        }
        #endregion

        #region 位置 position
        private Vector3 position_ = Vector3.zero;
        [CategoryAttribute("位置")]
        public Vector3 position
        {
            get
            {
                if (mapGO_)
                {
                    return mapGO_.transform.position;
                }
                else
                {
                    return position_;
                }
            }
            set
            {
                if (mapGO_)
                {
                    Vector3 newPos = value;
                    XYClientCommon.AdjustToGround(ref newPos, false);
                    mapGO_.transform.position = newPos;
                    position_ = newPos;
                }
                else
                {
                    position_ = value;
                }
                mocReference_.position = GameUtility.VectorToStr(position_);
            }
        }
        #endregion

        #region 朝向 direction
        private Vector3 direction_ = Vector3.forward;
        [CategoryAttribute("方向")]
        public Vector3 direction
        {
            get
            {
                if (mapGO_)
                {
                    return mapGO_.transform.forward;
                }
                else
                {
                    return direction_;
                }
            }
            set
            {
                if (mapGO_)
                {
                    Vector3 newDir = value;
                    newDir.y = 0.0f;
                    mapGO_.transform.forward = newDir;
                    direction_ = newDir;
                }
                else
                {
                    direction_ = value;
                }
                mocReference_.direction = GameUtility.VectorToStr(direction_);
            }
        }
        #endregion

        #region 唯一辨识sn
        private int sn_ = 0;
        [BrowsableAttribute(false)]
        public int sn
        {
            get { return sn_; }
            set
            {
                sn_ = value;
                mocReference_.sn = value;
            }
        }
        #endregion

        #region 参考对象的refid
        private int refid_ = 0;
        [CategoryAttribute("参考ID")]
        public virtual int refid
        {
            get { return refid_; }
            set
            {
                refid_ = value;
                mocReference_.refid = (int)value;
            }
        }
        #endregion

        #region 本对象类型ObjectType
        [CategoryAttribute("对象类型")]
        public virtual string ObjectType
        {
            get;
            set;
        }
        #endregion

        #region 外观Id apprid
        private int apprId_;
        [CategoryAttribute("外观ID")]
        public virtual int apprId
        {
            get { return apprId_; }
            set { apprId_ = value; }
        }
        #endregion


        #region 外观配置对象 roleCfg
        private Role roleCfg_;
        [BrowsableAttribute(false)]
        public Role roleCfg
        {
            get { return roleCfg_; }
            set { roleCfg_ = value; }
        }
        #endregion

        #region 显示对象 BaseConfigObject mapGO
        private GameObject mapGO_;
        [BrowsableAttribute(false)]
        public GameObject mapGO
        {
            set
            {
                mapGO_ = value;
                if (mapGO_)
                {
                    this.position = position_;
                    this.direction = direction_;
                }
            }
            get { return mapGO_; }
        }
        #endregion

        #region 保存/加载
        public void SaveToXml(XmlDocument vDoc)
        {
            XmlElement node = vDoc.CreateElement(ObjectType);
            node.SetAttribute("available", mocReference_.available.ToString());
            node.SetAttribute("sn", mocReference_.sn.ToString());
            node.SetAttribute("map", mocReference_.map.ToString());
            node.SetAttribute("refid", mocReference_.refid.ToString());
            node.SetAttribute("position", mocReference_.position);
            node.SetAttribute("direction", mocReference_.direction);
            node.SetAttribute("editiontype", mocReference_.EditionType.ToString());
            node.SetAttribute("camp", mocReference_.camp.ToString());
            OnSaveToXml(vDoc, node);
            vDoc.DocumentElement.AppendChild(node);
        }

        public void LoadFromXml(XmlElement vNode)
        {
            this.available = bool.Parse(vNode.GetAttribute("available"));
            this.sn = int.Parse(vNode.GetAttribute("sn"));
            this.map = int.Parse(vNode.GetAttribute("map"));
            this.refid = int.Parse(vNode.GetAttribute("refid"));
            this.position = GameUtility.StrToVector3(vNode.GetAttribute("position"));
            this.direction = GameUtility.StrToVector3(vNode.GetAttribute("direction"));
            this.editionType = (ObjectEditionType)System.Enum.Parse(typeof(ObjectEditionType), vNode.GetAttribute("editiontype"));
            this.camp = (ObjectCamp)System.Enum.Parse(typeof(ObjectCamp), vNode.GetAttribute("camp"));
            OnLoadFromXml(vNode);
        }


        public void LoadFromText(MapObjectConfigReference reference)
        {
            this.available = reference.available;
            this.sn = reference.sn;
            this.map = reference.map;
            this.refid = reference.refid;
            this.position = GameUtility.StrToVector3(reference.position);
            this.direction = GameUtility.StrToVector3(reference.direction);
            this.editionType = (ObjectEditionType)((int)reference.EditionType);
            this.camp = (ObjectCamp)(reference.camp);
            //这里做一次ID的检测，是否跟约定一致
            mocReference_.ID = reference.ID;
            if (mocReference_.ID != this.map * 10000 + this.sn)
            {
                Debug.Log("Error, ID != map *10000 + sn");
                throw new System.Exception("Error, ID != map *10000 + sn");
            }
            OnLoadFromText(reference);
        }
        #endregion

        #region 位置偏移的修正
        public void FixPosition(float fixX, float fixZ)
        {
            this.position = new Vector3(this.position.x + fixX, this.position.y, this.position.z + fixZ);
            OnFixPosition(fixX, fixZ);
        }
        #endregion

        public virtual void CreateGameObject(System.Action<MPObject> loadFinish)
        {
            XYCoroutineEngine.Execute(LoadRoleObject(string.Format(MapPlacementController.mapRoleObjectResLoadPathStrFormat, apprId), loadFinish));
        }

        public IEnumerator LoadRoleObject(string path, System.Action<MPObject> loadFinish)
        {
            TLEditorWww roleWww = TLEditorWww.Create(path);
            while (!roleWww.Finished)
                yield return null;
            GameObject role = null;
            role = roleWww.GetAsset() as GameObject;
            ReplaceShader(role, string.Empty);
            if (role == null)
            {
                Debug.LogError("Error, Load role Failed: " + path);
                roleWww.Unload();
            }
            else
            {
                Role roleConfig = role.GetComponent<Role>();
                if (roleConfig == null)
                    Debug.Log("Error,can't find role component");
                GameObject roleGo = ModelLoader.CreateRole(roleConfig).gameObject;
                roleGo.gameObject.GetComponent<ActionPerformer>().enabled = false;
                roleGo.gameObject.GetComponent<RoleObject>().enabled = false;
                roleWww.Unload();
                this.mapGO = roleGo;
                this.mapGO.name = sn.ToString();
                BoxCollider bc = this.mapGO.AddComponent<BoxCollider>();
                bc.size = new Vector3(1, 2, 1);
                bc.center = new Vector3(0, 1, 0);
                XYClientCommon.ChangeLayer(this.mapGO, XYDefines.Layer.Player, true);
                loadFinish(this);
            }
        }

        public static void ReplaceShader(UnityEngine.Object obj,string url)
        {
            List<Renderer> render_list = new List<Renderer>();
            if (!obj)
                return;
            if (obj is GameObject)
            {
                //FindRenderers(render_list, (obj as GameObject).transform);
                FindRenderers(ref render_list, (obj as GameObject).transform);
                if (render_list.Count == 0)
                    return;
                for (int i = 0; i < render_list.Count; i++)
                    ReplaceRendererShader(render_list[i], url);
            }
            else if (obj is Material)
                ReplaceMaterialShader(obj as Material, url);

            return;
        }



        private static void FindRenderers(ref List<Renderer> Renderers, Transform parent, bool includeInactive = true)
        {
            if (Renderers == null || !parent)
                return;
            Renderer[] renderers = parent.GetComponentsInChildren<Renderer>(includeInactive);
            for (int i = 0; i < renderers.Length; i++)
                Renderers.Add(renderers[i]);
        }


        public static void ReplaceRendererShader(Renderer renderer, string url)
        {
            //替换SHADER
            if (renderer)
            {
                Material[] sharedMaterials = renderer.sharedMaterials;
                for (int i = 0; i < sharedMaterials.Length; i++)
                {
                    Material mat = sharedMaterials[i];
                    if (mat)
                    {
                        ReplaceMaterialShader(mat, url);
                        //renderer.lightProbeUsage = LightProbeUsage.Off;
                        //renderer.shadowCastingMode = ShadowCastingMode.Off;
                        //renderer.receiveShadows = false;
                    }
                }
            }
        }

        public static void ReplaceMaterialShader(Material mat, string url)
        {
            if (!mat || !mat.shader)
            {
                return;
            }

            if (mat.shader)
            {
                string shadername = mat.shader.name;
                Shader shader = null;
                shader = Shader.Find(shadername);
                if (shader)
                {
                    mat.shader = null;
                    mat.shader = shader;
                }
            }
        }


        public virtual void DestroyGameObject()
        {
            Object.Destroy((GameObject)this.mapGO);
            this.mapGO = null;
        }

        protected virtual void OnSaveToXml(XmlDocument vDoc, XmlElement vNode) { }
        protected virtual void OnSaveToText() { }
        protected virtual void OnLoadFromXml(XmlElement vNode) { }
        protected virtual void OnLoadFromText(MapObjectConfigReference reference) { }
        protected virtual void OnFixPosition(float fixX, float fixZ) { }

        public virtual void DrawInfo()
        {
            if (this.mapGO_)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(this.mapGO_.transform.position);
                GUILayout.Button(screenPos.ToString());
                string text = string.Format("{0},{1}", sn, refid_);
                GUIContent content = new GUIContent(text);
                Vector2 size = GUI.skin.label.CalcSize(content);
                GUI.Label(new Rect(screenPos.x, (Screen.height - screenPos.y) + 10, size.x, size.y), content);
            }
        }
    }
    #endregion

    #region 地图摆放角色类对象
    public class MPRole : MPObject
    {

        public MPRole()
            : base()
        {
            mocReference_.isRole = true;
        }


        #region 路径点对象定义RoutePoint
        public class RoutePoint
        {
            private Vector3 position_;
            public Vector3 position
            {
                get { return position_; }
                set { position_ = value; }
            }

            private float stopTime_;
            [CategoryAttribute("停留时间")]
            public float stopTime
            {
                get { return stopTime_; }
                set { stopTime_ = value; }
            }

            public RoutePoint(Vector3 vPos, float vStopTime)
            {
                position = vPos;
                stopTime = vStopTime;
            }

            public void DrawInfo()
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(position_);
                string text = string.Format("{0}, 停留时间: {1} 秒", position_, stopTime_);
                GUIContent content = new GUIContent(text);
                Vector2 size = GUI.skin.label.CalcSize(content);
                GUI.Label(new Rect(screenPos.x, (Screen.height - screenPos.y) + 10, size.x, size.y), content);
            }
        }
        #endregion

        #region 巡逻方式定义和接口
        public enum PatrolType
        {
            none,
            circle,
            back,
            fix,
            random,
            once,
        }
        #endregion

        public override string ObjectType { get; set; }

        #region 参考对象RoleReference reference_
        private RoleReference reference_;
        [BrowsableAttribute(false)]
        public RoleReference reference
        {
            get { return reference_; }
            set { reference_ = value; }
        }
        [CategoryAttribute("参考名称")]
        public string refName
        {
            get
            {
                if (reference_ != null)
                {
                    return reference_.Name;
                }
                else
                {
                    return "???";
                }
            }
        }
        public override int refid
        {
            get { return base.refid; }
            set
            {

                if (value != base.refid)
                {
                    base.refid = value;
                    //this.reference_ = null;
                    //this.roleCfg = null;

                    //this.reference_ = Global.role_mgr.GetReference(this.refid);
                    //if (this.reference_ != null)
                    //{
                    //    this.roleCfg = ResourceManager.GetRoleConfig(this.reference_.apprid);
                    //    if (this.mapGO)
                    //    {
                    //        this.DestroyGameObject();
                    //        this.CreateGameObject();
                    //    }
                    //}
                    //else
                    //{
                    //    throw new System.Exception(string.Format("role reference not found,ID:{0}", this.refid));
                    //}
                }
            }
        }
        #endregion

        #region 巡逻方式
        private PatrolType patrolType_ = PatrolType.back;
        [CategoryAttribute("巡逻方式")]
        public PatrolType patrolType
        {
            get { return patrolType_; }
            set
            {
                patrolType_ = value;
                mocReference_.patrolType = patrolType_.ToString();
            }
        }
        #endregion

        #region 路径点相关定义和接口
        private List<RoutePoint> routeList_ = new List<RoutePoint>();
        [BrowsableAttribute(false)]
        public List<RoutePoint> routeList { get { return routeList_; } }
        [CategoryAttribute("路径点数")]
        public int routePointCount { get { return routeList_.Count; } }
        //取得与参数点靠近的路径
        public RoutePoint GetCloseRoutePoint(Vector3 vHitPoint, float vOffset)
        {
            for (int i = 0; i < routeList_.Count; i++)
            {
                if ((routeList_[i].position - vHitPoint).magnitude <= vOffset)
                {
                    return routeList_[i];
                }
            }
            return null;
        }
        //删除一个路径点
        public void RemoveRoutePoint(RoutePoint vRP)
        {
            for (int i = 0; i < routeList_.Count; i++)
            {
                if (routeList_[i] == vRP)
                {
                    routeList_.RemoveAt(i);
                    return;
                }
            }
        }
        #endregion

        #region AI参考ID
        private int aiRefid_ = 0;
        [CategoryAttribute("AI参考ID")]
        public int aiRefid
        {
            get { return aiRefid_; }
            set
            {
                mocReference_.aiRefid = (short)value;
                aiRefid_ = value;
            }
        }
        #endregion

        #region 队伍ID
        private int teamID_ = -1;
        [CategoryAttribute("队伍编号")]
        public int teamID
        {
            get { return teamID_; }
            set
            {
                teamID_ = value;
                mocReference_.teamID = value;
            }
        }
        #endregion

        #region 保存 加载
        protected override void OnSaveToXml(System.Xml.XmlDocument vDoc, System.Xml.XmlElement vNode)
        {
            base.OnSaveToXml(vDoc, vNode);
            vNode.SetAttribute("aiRefid", mocReference_.aiRefid.ToString());
            vNode.SetAttribute("patrolType", mocReference_.patrolType);
            vNode.SetAttribute("teamID", mocReference_.teamID.ToString());
            for (int i = 0; i < routeList_.Count; i++)
            {
                XmlElement routeEle = vDoc.CreateElement("Route");
                routeEle.SetAttribute("position", GameUtility.VectorToStr(routeList_[i].position));
                routeEle.SetAttribute("stoptime", routeList_[i].stopTime.ToString());
                vNode.AppendChild(routeEle);
            }
        }

        protected override void OnLoadFromXml(XmlElement vNode)
        {
            base.OnLoadFromXml(vNode);
            aiRefid = int.Parse(vNode.GetAttribute("aiRefid"));
            patrolType = (PatrolType)System.Enum.Parse(typeof(PatrolType), vNode.GetAttribute("patrolType"));//PatrolTypeParse( vNode.GetAttribute( "patrolType") );
            teamID = int.Parse(vNode.GetAttribute("teamID"));
            LoadRoutes(vNode);

            OnLoadComplete();
        }

        protected override void OnSaveToText()
        {
            mocReference_.route = GetRouteXML();
        }

        protected override void OnLoadFromText(MapObjectConfigReference reference)
        {
            base.OnLoadFromText(reference);
            aiRefid = reference.aiRefid;
            patrolType = (PatrolType)System.Enum.Parse(typeof(PatrolType), reference.patrolType);//PatrolTypeParse( vNode.GetAttribute( "patrolType") );
            teamID = reference.teamID;
            LoadRoutes(reference.route);
            OnLoadComplete();
        }


        private void LoadRoutes(string xml)
        {//读取路径数据
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                LoadRoutes(doc.DocumentElement);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

        }

        private void LoadRoutes(XmlElement vNode)
        {//读取路径数据
            XmlNodeList xnList = vNode.SelectNodes("Route");
            for (int i = 0; i < xnList.Count; i++)
            {
                float stopTime = float.Parse(xnList[i].Attributes["stoptime"].Value);
                RoutePoint rp = new RoutePoint(
                    XYClientCommon.AdjustToGround(GameUtility.StrToVector3(xnList[i].Attributes["position"].Value), false),
                    stopTime);
                this.routeList_.Add(rp);
            }
        }

        private void OnLoadComplete()
        {
            this.reference = Global.role_mgr.GetReference(this.refid);
            this.roleCfg = ResourceManager.GetRoleConfig(this.reference.Apprid);
        }
        #endregion

        #region 取得数据库保存参数
        private string GetRouteXML()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<data>");
            foreach (RoutePoint rp in this.routeList)
            {
                sb.Append(string.Format("<Route position=\"{0}\" stoptime=\"{1}\" />", GameUtility.VectorToStr(rp.position), rp.stopTime));
            }
            sb.Append("</data>");
            return sb.ToString();
        }
        #endregion

        #region 修正位置偏移
        protected override void OnFixPosition(float fixX, float fixZ)
        {
            base.OnFixPosition(fixX, fixZ);
            foreach (RoutePoint rp in this.routeList_)
            {
                Vector3 newPos = rp.position;
                newPos.x += fixX;
                newPos.z += fixZ;
                XYClientCommon.AdjustToGround(ref newPos, false);
                rp.position = newPos;
            }
        }
        #endregion

        public override void CreateGameObject(System.Action<MPObject> loadFinish)
        {
            XYCoroutineEngine.Execute(LoadRoleObject(string.Format(MapPlacementController.mapRoleObjectResLoadPathStrFormat,apprId), loadFinish));
            return;
            //this.mapGO = ModuleFactory.CreateBaseRole( sn.ToString(), this.roleCfg.roleBone );
            if (this.reference.Apprid <= 0)
            {
                base.CreateGameObject(loadFinish);
                return;
            }
            
            BaseObject bco = new BaseObject(ResourceManager.GetRoleConfig(this.reference.Apprid));
            //bco.UpdateEquip();
            this.mapGO = bco;
            if (this.mapGO.name != "BaseObject_NoRoleConfig")
                this.mapGO.name = sn.ToString();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.mapGO.transform;
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localEulerAngles = Vector3.zero;
            cube.transform.localScale = new Vector3(1, 2, 1);

            BoxCollider bc = this.mapGO.AddComponent(typeof(BoxCollider)) as BoxCollider;
            bc.size = new Vector3(1, 2, 1);
            bc.center = new Vector3(0, 1, 0);

            XYClientCommon.ChangeLayer(this.mapGO, XYDefines.Layer.Player, true);
            //return this.mapGO;
        }

        public override void DrawInfo()
        {
            if (this.mapGO)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(this.mapGO.transform.position);
                if (screenPos.x < 0 || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.y > Screen.height)
                {
                    return;
                }
                string text;
                if (this.reference_ != null)
                {
                    text = string.Format("[ {0} ], sn : {1}, 参考 id : {2} , 队伍 : {3}", this.reference_.Name, this.sn, this.refid, this.teamID_);
                }
                else
                {
                    text = string.Format("{0}, {1}", this.sn, this.refid);
                }
                GUIContent content = new GUIContent(text);
                Vector2 size = GUI.skin.label.CalcSize(content);
                GUI.Label(new Rect(screenPos.x, (Screen.height - screenPos.y) + 10, size.x, size.y), content);
            }
        }
    }
    #endregion

    #region 地图摆放场景物件可采集类对象
    public class MPItem : MPObject
    {

        public MPItem()
            : base()
        {
            mocReference_.isRole = false;
        }

        public override string ObjectType
        {
            get
            {
                return "Object";
            }
        }

        #region 队伍ID
        private int teamID_ = -1;
        [CategoryAttribute("队伍编号")]
        public int teamID
        {
            get { return teamID_; }
            set
            {
                teamID_ = value;
                mocReference_.teamID = value;
            }
        }
        #endregion

        #region 参考对象
        private RoleReference reference_;  //暂时用这个reference
        [BrowsableAttribute(false)]
        public RoleReference reference
        {
            get { return reference_; }
            set { reference_ = value; }
        }
        [CategoryAttribute("参考名称")]
        public string refName
        {
            get
            {
                if (reference_ != null)
                {
                    return reference_.Name;
                }
                else
                {
                    return "???";
                }
            }
        }

        public override int refid
        {
            get { return base.refid; }
            set
            {
                if (value != base.refid)
                {
                    base.refid = value;
                    //this.reference_ = null;
                    //this.roleCfg = null;

                    //this.reference_ = Global.mapobj_r_mgr.GetReference(this.refid);
                    //if (this.reference_ != null)
                    //{
                    //    this.radius_ = (float)this.reference_.radius;
                    //    this.roleCfg = ResourceManager.GetRoleConfig(this.reference_.Apprid);
                    //    if (this.mapGO)
                    //    {
                    //        this.DestroyGameObject();
                    //        this.CreateGameObject();
                    //    }
                    //}
                    //else
                    //{
                    //    throw new System.Exception(string.Format("map object reference not found,ID:{0}", this.refid));
                    //}
                }
            }
        }
        #endregion

        #region 作用半径
        private float radius_ = 0.0f;
        [CategoryAttribute("作用半径")]
        public float radius
        {
            get { return radius_; }
            set
            {
                radius_ = value;
            }
        }
        #endregion

        #region 保存/加载
        protected override void OnSaveToXml(System.Xml.XmlDocument vDoc, System.Xml.XmlElement vNode)
        {
            base.OnSaveToXml(vDoc, vNode);
        }

        protected override void OnLoadFromXml(XmlElement vNode)
        {
            base.OnLoadFromXml(vNode);
            OnLoadComplete();
        }

        protected override void OnSaveToText()
        {
            base.OnSaveToText();

        }
        protected override void OnLoadFromText(MapObjectConfigReference reference)
        {
            base.OnLoadFromText(reference);
            OnLoadComplete();
        }

        private void OnLoadComplete()
        {
            this.reference = Global.role_mgr.GetReference(this.refid);
            if (this.reference == null)
            {
                Debug.Log(string.Format("error,{0}", this.refid));
                this.roleCfg = ResourceManager.GetRoleConfig(10000);
            }
            else
            {
                this.roleCfg = ResourceManager.GetRoleConfig(this.reference.Apprid);
            }
        }
        #endregion

        public override void CreateGameObject(System.Action<MPObject> loadFinish)
        {
            if (MapPlacementController.mapObjectResLoadPathStrFormat.Length == 0)
            {
                Debug.Log("待添加资源路径");
                return;
            }
            XYCoroutineEngine.Execute(LoadRoleObject(string.Format(MapPlacementController.mapObjectResLoadPathStrFormat, apprId), loadFinish));
            return;
            //this.mapGO = ModuleFactory.CreateBaseRole( sn.ToString(), this.roleCfg.roleBone );
            if (this.reference.Apprid <= 0)
            {
                 base.CreateGameObject(loadFinish);
                return;
            }
            BaseObject bco = new BaseObject(ResourceManager.GetRoleConfig(this.reference.Apprid));
            //bco.UpdateEquip();
            this.mapGO = bco;
            this.mapGO.name = sn.ToString();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.mapGO.transform;
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localEulerAngles = Vector3.zero;
            cube.transform.localScale = new Vector3(1, 2, 1);
            BoxCollider bc = this.mapGO.AddComponent(typeof(BoxCollider)) as BoxCollider;
            bc.size = new Vector3(1, 2, 1);
            bc.center = new Vector3(0, 1, 0);

            XYClientCommon.ChangeLayer(this.mapGO, XYDefines.Layer.Player, true);

            //return this.mapGO;
        }

        public override void DrawInfo()
        {
            if (this.mapGO)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(this.mapGO.transform.position);
                if (screenPos.x < 0 || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.y > Screen.height)
                {
                    return;
                }
                string text;
                if (this.reference_ != null)
                {
                    text = string.Format("[ {0} ], sn : {1}, 参考 id : {2},队伍id：{3}", this.reference_.Name, this.sn, this.refid,this.teamID);
                }
                else
                {
                    text = string.Format("{0}, {1}", this.sn, this.refid);
                }
                GUIContent content = new GUIContent(text);
                Vector2 size = GUI.skin.label.CalcSize(content);
                GUI.Label(new Rect(screenPos.x, (Screen.height - screenPos.y) + 10, size.x, size.y), content);
            }
        }
    }
    #endregion
}