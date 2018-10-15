using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectModel
{
    public bool IsDelete = false;
    public bool IsLoadOver = false;
    private BaseObject BaseObject_;

    /// <summary>
    /// 需要加载的特效GAMEOBJECT文件
    /// </summary>
    private List<GameObject> EffectObjects_;

    public ObjectModel(Equip mc, BaseObject baseObject)
    {
        ModelCfg = mc;
        BaseObject_ = baseObject;
        //GameObject = new GameObject(Path.GetFileNameWithoutExtension(mc.ModelPath));
        //GameObject.layer = XYDefines.Layer.Player;
    }

    public GameObject GameObject { get; set; }

    public Equip ModelCfg { get; private set; }

    //public MeshToBoneMapping Mesh2Bone { get; set; }

    public List<GameObject> EffectObjects
    {
        get { return EffectObjects_ ?? (EffectObjects_ = new List<GameObject>()); }
        set { EffectObjects_ = value; }
    }

    public void EnableGameObject()
    {
        if (GameObject.GetComponent<Renderer>() != null)
            GameObject.GetComponent<Renderer>().enabled = true;
    }


    public void LoadOver() //加载结束，调用LOADOVER函数进行组装
    {
        IsLoadOver = true;
        if (!BaseObject_|| BaseObject_.Disposed || !GameObject || !GameObject.GetComponent<Renderer>())
        {
            Destroy();
            return;
        }

        if (IsDelete)
        {
            return;
        }

        GameObject.transform.parent = BaseObject_.RoleObject.transform;
        GameObject.transform.localPosition = Vector3.zero;
        GameObject.transform.localEulerAngles = Vector3.zero;
        GameObject.transform.localScale = Vector3.one;

        Renderer render = GameObject.GetComponent<Renderer>();
        //if (render is SkinnedMeshRenderer && Mesh2Bone)
        //{
        //    string[] bonesPath = Mesh2Bone.BonePath;
        //    var bones = new Transform[bonesPath.Length];
        //    for (int i = 0; i < bonesPath.Length; i++)
        //    {
        //        if (string.IsNullOrEmpty(bonesPath[i]))
        //            continue;
        //        bones[i] = BaseObject_.FindBonesWithName(bonesPath[i]);
        //        if (!bones[i])
        //            Debug.Log(string.Format("Fine bone's transform failed: {0},{1}", BaseObject_.Name, bonesPath[i]));
        //    }
        //    SkinnedMeshRenderer skr = render as SkinnedMeshRenderer;
        //    skr.bones = bones;
        //    skr.rootBone = BaseObject_.FindBonesWithName(BaseObject_.BaseRoleConfig.RootBone);
        //}
        render.receiveShadows = false;
        render.castShadows = true;
        render.enabled = false;
        GameObject.SetActive(false);

        //特效
        if (EffectObjects_ != null)
        {
            for (int i = 0; i < ModelCfg.Effects.Length; i++)
            {
                if (EffectObjects_[i] == null)
                {
                    continue;
                }
                BaseObject_.EffectList.Add(EffectObjects_[i]);
                XYClientCommon.EnableRender(EffectObjects_[i], false, true);
                XYClientCommon.ChangeLayer(EffectObjects_[i], XYDefines.Layer.Effect, true);
                if (ModelCfg.Effects[i].BindBone == string.Empty) //因为没绑定到模型下，所以特效要乘以角色缩放
                {
                    EffectObjects_[i].transform.parent = GameObject.transform;
                    float scale = ModelCfg.Effects[i].Scale.x;
                    if (BaseObject_ != null && BaseObject_.BaseRoleConfig != null)
                    {
                        EffectObjects_[i].transform.localScale = Vector3.one * scale;
                    }
                    EffectObjects_[i].transform.localPosition = ModelCfg.Effects[i].Position;
                    EffectObjects_[i].transform.localEulerAngles = ModelCfg.Effects[i].Rotation;
                }
                else if (!string.IsNullOrEmpty(ModelCfg.Effects[i].BindBone))
                {
                    Transform tf = BaseObject_.FindBonesWithName(ModelCfg.Effects[i].BindBone);
                    if (tf)
                    {
                        EffectObjects_[i].transform.parent = tf;
                        float scale = ModelCfg.Effects[i].Scale.x;
                        EffectObjects_[i].transform.localScale = Vector3.one * scale;
                        EffectObjects_[i].transform.localPosition = ModelCfg.Effects[i].Position;
                        EffectObjects_[i].transform.localEulerAngles = ModelCfg.Effects[i].Position;
                    }
                    else
                    {
                        Object.Destroy(EffectObjects_[i]);
                        Debug.Log(ModelCfg.Effects[i].BindBone + "Not Find!");
                    }
                }
                else
                {
                    Object.Destroy(EffectObjects_[i]);
                }
            }
        }

        //资源处理完毕，调用BaseObject里的资源完成加载接口
    }

    public void Destroy()
    {
        BaseObject_ = null;
        //Mesh2Bone = null;
        ModelCfg = null;
        Object.Destroy(GameObject);
        GameObject = null;
        if (EffectObjects_ != null)
        {
            foreach (var t in EffectObjects_.Where(t => t))
            {
                Object.Destroy(t);
            }
            EffectObjects_.Clear();
            EffectObjects_ = null;
        }
    }

    #region 封装GameObject的各接口

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

    public static implicit operator GameObject(ObjectModel exists)
    {
        return exists == null ? null : exists.GameObject;
    }

    #endregion

    #region 封装GameObject的各接口

    private Animation Animation_ = null;

    private Transform Transform_ = null;

    public Animation Animation
    {
        get
        {
            if (Animation_ != null)
                return Animation_;
            Animation_ = GameObject.GetComponent<Animation>();
            return Animation_;
        }
    }

    public Transform Transform
    {
        get
        {
            if (Transform_ != null)
                return Transform_;
            Transform_ = GameObject.transform;
            return Transform_;
        }
    }

    public string Name
    {
        get { return GameObject.name; }
        set { GameObject.name = value; }
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

    public Renderer Renderer
    {
        get { return GameObject.GetComponent<Renderer>(); }
    }

    #endregion
}