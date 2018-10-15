using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;

public class BaseObject : IDisposable
{

    #region IDisposable 接口
    public bool Disposed;
    // BaseObject本身的Dispose(bool)负责销毁GameObject，所以必须在主线程里

    public void Dispose()
    {
        if (Disposed) return;
        Disposed = true;
        Dispose(true);
        if (Application.isEditor) return;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        foreach (var om in GameObjectAndModelInfoList.Values)
        {
            om.Destroy();
        }
        foreach (var go in StarEffectList)
        {
            Object.Destroy(go);
        }
        foreach (var go in EffectList)
        {
            Object.Destroy(go);
        }
        BoneTrans.Clear();
        GameObjectAndModelInfoList.Clear();
        StarEffectList.Clear();
        EffectList.Clear();

        BoneTrans = null;
        GameObjectAndModelInfoList = null;
        StarEffectList = null;
        EffectList = null;
        BaseRoleConfig = null;
        LoadingEffect = null;
        RoleObject = null;

        DestroyGameObject();
    }

    #endregion

    public BaseObject() {

    }
    public BaseObject(GameObject go)
    {
        this.GameObject = go;
    }

    public BaseObject(Role roleConfig)
    {
        Create(roleConfig);
    }

    #region 各变量和角色构件
    public Dictionary<string, Transform> BoneTrans = new Dictionary<string, Transform>(); //存储骨骼的transform，用于快速查找

    public bool IsShow = true;
    protected bool IsShowWeapon = true;
    public Collider BoxCollider = null;
    public GameObject MainBone = null;
    public List<GameObject> EffectList = new List<GameObject>();

    public Dictionary<Equip, ObjectModel> GameObjectAndModelInfoList =
        new Dictionary<Equip, ObjectModel>();

    public List<GameObject> StarEffectList = new List<GameObject>();

    public Role BaseRoleConfig { get; set; }
    #endregion

    #region 封装GameObject的各接口

    protected GameObject LoadingEffect;

    public GameObject GameObject { get; private set; }

    public GameObject RoleObject { get; private set; }

    private Animation Animation_ = null;
    public Animation Animation
    {
        get
        {
            if (Animation_ != null)
            {
                return Animation_;
            }
            Animation_ = RoleObject.GetComponent<Animation>();
            return Animation_;
        }
    }

    public virtual Vector3 BoxcolliderSize { set; get; }

    public float HalfHigh { set; get; }

    private Transform Transform_ = null;
    public Transform Transform
    {
        get
        {
            if (Transform_ != null)
                return Transform_;
            if (!GameObject)
                return null;
            Transform_ = GameObject.transform;
            return Transform_;
        }
    }

    public string Name
    {
        get { return GameObject.name; }
        set
        {
            GameObject.name = value;
            //LOG.Trace("\t\tBaseObject.name = {0}", value);
        }
    }

    public bool Active
    {
        get { return GameObject.activeSelf; }
        set { GameObject.SetActive(value); }
    }

    public Collider Collider
    {
        get { return GameObject.GetComponent<Collider>(); }
    }

    public int GetInstanceId()
    {
        return GameObject.GetInstanceID();
    }
    //如果装备在下载中，显示通用模型

    public Component AddComponent(Type componentType)
    {
        return GameObject.AddComponent(componentType);
    }

    public T AddComponent<T>() where T : Component
    {
        return GameObject.AddComponent<T>();
    }

    public Component GetComponent(Type componentType)
    {
        return GameObject.GetComponent(componentType);
    }

    public T GetComponent<T>() where T : Component
    {
        return GameObject.GetComponent<T>();
    }

    public Component GetComponentInChildren(Type type)
    {
        return GameObject.GetComponentInChildren(type);
    }

    protected void SendMessage(string methodName, object value, SendMessageOptions options)
    {
        GameObject.SendMessage(methodName, value, options);
    }

    public static implicit operator bool(BaseObject exists)
    {
        return exists != null && exists.GameObject;
    }

    public static implicit operator GameObject(BaseObject exists)
    {
        return exists == null ? null : exists.GameObject;
    }

    #endregion

    #region 根据给予的名称找出子节点 FindChildWithName FindChildTransformWithName

    public static GameObject FindChildWithName(GameObject parentGO, string childName)
    {
        if (parentGO.name.ToLower() == childName.ToLower())
        {
            return parentGO;
        }
        foreach (Transform tf in parentGO.transform)
        {
            GameObject result = FindChildWithName(tf.gameObject, childName);
            if (result)
                return result;
        }
        return null;
    }

    public static Transform FindChildTransformWithName(GameObject parentGO, string childName)
    {
        if (parentGO != null)
        {
            if (parentGO.name.ToLower() == childName.ToLower())
            {
                return parentGO.transform;
            }
            foreach (Transform tf in parentGO.transform)
            {
                GameObject result = FindChildWithName(tf.gameObject, childName);
                if (result)
                    return result.transform;
            }
        }
        return null;
    }

    public Transform FindBonesWithName(string childName)
    {
        Transform trans = null;
        BoneTrans.TryGetValue(childName, out trans);
        return trans;
    }

    #endregion

    #region BUFF更新材质相关
    protected virtual void OnEndLoad()
    {
        //nothing
    }
    #endregion



    public void CreatBoneTrans(Transform parentGo)
    {
        if (!parentGo) return;
        BoneTrans[parentGo.name] = parentGo;
        foreach (Transform tf in parentGo.transform)
        {
            CreatBoneTrans(tf);
        }
    }

    private void DestroyGameObject()
    {
        if (!GameObject) return;
        Object.Destroy(GameObject);
        GameObject = null;
    }

    #region 创建一个基本的gameobject（仅包含骨骼）

    private void Create(Role roleConfig)
    {
        
    }

    #endregion

}