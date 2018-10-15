using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Object = UnityEngine.Object;
using System.Linq;

public class ModelLoader
{
    public static List<RoleObject> targetList = new List<RoleObject>();
    public static GameObject PreviewingObject;

    public static RoleObject self;
    public static EditorWww sdWww;

    public static void ResetPreviewingObject()
    {
        if (PreviewingObject) Object.DestroyImmediate(PreviewingObject);
    }

    #region 功能接口
    public static Transform FindChild(Transform go, System.Predicate<GameObject> pred)
    {
        if (go)
        {
            foreach (Transform ts in go.transform)
            {
                if (pred(ts.gameObject))
                {
                    return ts.gameObject.transform;
                }
                Transform result = FindChild(ts, pred);
                if (result)
                {
                    return result;
                }
            }
        }
        return null;
    }

    public static Transform FindChildByName(Transform parent, string name)
    {
        return FindChild(parent, child => child && child.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    #endregion

    #region 创建装备
    public static void AddEquip(GameObject root, Equip equip,bool high,bool load = true)
    {
        AsyncCreateEquip(root, equip, load);
    }
    private static void AsyncCreateEquip(GameObject root, Equip equip, bool high,bool load = true)
    {
        string model = equip.ModelPath;
        string[] bones = equip.Bones;
        if (high && !string.IsNullOrEmpty(equip.HighModelPath))
        {
            model = equip.HighModelPath;
            bones = equip.HighBones;
        }

        GameObject modelObject = UnityEngine.Object.Instantiate(EditorWww.Create(model)) as GameObject;
        Renderer Renderer = modelObject.GetComponentInChildren<Renderer>();
        Renderer.enabled = true;
        Renderer.sharedMaterial.shader = Shader.Find(Renderer.sharedMaterial.shader.name);
        RoleObject ro = root.GetComponent<RoleObject>();

        foreach (EffectConfig effectInfo in equip.Effects)
        {
            string path = effectInfo.EffectAssetPath;
            if (!high && !string.IsNullOrEmpty(effectInfo.LowEffectAssetPath))
                path = effectInfo.LowEffectAssetPath;

            if (string.IsNullOrEmpty(path))
                continue;
  
             GameObject effect = UnityEngine.Object.Instantiate(EditorWww.Create(effectInfo.EffectAssetPath)) as GameObject;
            if (effect)
            {
                Transform tf = RoleObject.FindChildTransformWithName(root, effectInfo.BindBone);
                if (!tf)
                    tf = modelObject.transform;

                effect.transform.parent = tf;
                effect.transform.localPosition = effectInfo.Position;
                effect.transform.localEulerAngles = effectInfo.Rotation;
                effect.transform.localScale = effectInfo.Scale;
            }

        }

        Role config = root.gameObject.GetComponent<Role>();
        if (!config)
        {
            modelObject.transform.SetParent(root.transform);
            modelObject.transform.localPosition = Vector3.zero;
            modelObject.transform.localEulerAngles = Vector3.zero;
            modelObject.transform.localScale = Vector3.one;
            return;
        }

        SwingBone[] swingBones = config.GetComponentsInChildren<SwingBone>(true);
        for (int i = 0; i < swingBones.Length; i++)
        {
            swingBones[i].enabled = true;
            swingBones[i].gameObject.SetActive(true);
        }

        SkinnedMeshRenderer skinrender = modelObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinrender)
        {
            if (bones != null && bones.Length > 0)
            {
                Transform[] bonetfs = new Transform[bones.Length];
                for (int i = 0; i < bones.Length; i++)
                    bonetfs[i] = ModelLoader.FindChildByName(root.transform, bones[i]);

                skinrender.bones = bonetfs;
                skinrender.rootBone = FindChildByName(config.transform, equip.RootBone);
            }
            skinrender.enabled = true;
        }

        Transform bindTF = FindChildByName(config.transform, equip.ParentBone);
        if (!bindTF)
            bindTF = config.transform;

        if (ro)
            ro.Skins.Add(modelObject);

        Animation ani = modelObject.GetComponent<Animation>();
        if (ani)
            ro.Animations.Add(ani);

        modelObject.transform.SetParent(bindTF);
        modelObject.transform.localPosition = Vector3.zero;
        modelObject.transform.localEulerAngles = Vector3.zero;
        modelObject.transform.localScale = Vector3.one;
        modelObject.layer = ro.modelLayer;
    }
    #endregion


    #region 创建角色
    public static RoleObject CreateRole(Role Config, bool high)
    {
        if (!Config)
            return null;

        GameObject role = Object.Instantiate(Config.gameObject);
        role.name = Config.name;
        RoleObject ro = role.AddComponent<RoleObject>();
        ro.mActionPerformer = role.AddComponent<ActionPerformer>();
        ro.mActionPerformer.owerObject = ro;
        ro.Config = ro.GetComponent<Role>();
        role.transform.position = Vector3.zero;
        role.transform.localPosition = Vector3.zero;
        role.transform.localEulerAngles = Vector3.zero;
        role.transform.localScale = Vector3.one;
        AsyncCreateRole(ro, ro.Config, high);

        return ro;
    }


    private static void AsyncCreateRole(RoleObject ro, Role config,bool high)
    {
        foreach (var equip in ro.Config.Equips)
        {
            Equip equipConfig = EditorWww.Create(equip.Equip) as Equip;
            if (!equipConfig)
                Debug.Log("Error, Load Equip Failed: " + equip);
            else
                AddEquip(ro.gameObject, equipConfig, high, false);
        }

        if (!string.IsNullOrEmpty(config.Mesh))
        {
            if (config.Skin)
            {
                config.Skin.sharedMesh = EditorWww.Create(config.Mesh) as Mesh;
                config.Skin.enabled = true;
                config.Skin.sharedMaterial.shader = Shader.Find(config.Skin.sharedMaterial.shader.name);
            }
            else if (config.Filter)
                config.Filter.sharedMesh = EditorWww.Create(config.Mesh) as Mesh;
        }

        if (!string.IsNullOrEmpty(config.MainTex))
        {
            if (config.Skin)
                config.Skin.sharedMaterial.mainTexture = EditorWww.Create(config.MainTex) as Texture;
            else if (config.Filter)
                config.Filter.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = EditorWww.Create(config.MainTex) as Texture;
        }

        if (config.Skin)
            config.Skin.enabled = true;
    }
    #endregion

    #region 设置摄像机
    public static void SetupCamera(Transform target)
    {
        if (!Camera.main)
        {
            var cameraGO = new GameObject("MainCamera");
            var camera = cameraGO.AddComponent<Camera>();
            cameraGO.tag = "MainCamera";
        }
        Camera.main.nearClipPlane = 0.1f;
        Camera.main.farClipPlane = 30;
        Camera.main.fieldOfView = 60f;
        Camera.main.nearClipPlane = 0.3f;
        Camera.main.cullingMask = -1;
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = new Color(49 / 255f, 77 / 255f, 121 / 255f, 5 / 255f);
        Camera.main.renderingPath = RenderingPath.Forward;
        Camera.main.depth = -1;
        CameraController ca = Camera.main.gameObject.AddComponent<CameraController>();
        ca.targetTransform = target;

        ca.transform.Rotate(45, 0, 0);

        if (!Camera.main.gameObject.GetComponent<Light>())
        {
            Camera.main.gameObject.AddComponent<Light>();
            Camera.main.gameObject.GetComponent<Light>().type = LightType.Directional;
        }
        //Camera.main.gameObject.AddComponent<AudioListener>();
    }
    #endregion

}
