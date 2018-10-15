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
    public static TLEditorWww sdWww;

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
    public static void AddEquip(GameObject root, Equip equip,bool load = true)
    {
        EditorCoroutine.Execute(AsyncCreateEquip(root, equip, load));
    }
    private static IEnumerator AsyncCreateEquip(GameObject root, Equip equip, bool load = true)
    {
        if (load)
        {
            TLEditorWww sdWww = TLEditorWww.Create("res/myshader.sd");
            while (!sdWww.Finished)
                yield return null;
            sdWww.GetAsset();
            Shader.WarmupAllShaders();
        }
        TLEditorWww www = null;

        www = TLEditorWww.Create(equip.ModelPath);
        while (!www.Finished)
            yield return null;

        GameObject modelObject = UnityEngine.Object.Instantiate(www.GetAsset()) as GameObject;
        Renderer Renderer = modelObject.GetComponentInChildren<Renderer>();
        Renderer.enabled = true;
        Renderer.sharedMaterial.shader = Shader.Find(Renderer.sharedMaterial.shader.name);
        yield return null;
        yield return null;//延迟两帧unload，避免unload太快，instantiate还没执行完导致贴图丢失
        RoleObject ro = root.GetComponent<RoleObject>();
        www.Unload();

        foreach (EffectConfig effectInfo in equip.Effects.Where(effectInfo => !string.IsNullOrEmpty(effectInfo.EffectAssetPath)))
        {
            TLEditorWww eff = TLEditorWww.Create(effectInfo.EffectAssetPath);
            while (!eff.Finished)
                yield return null;

            GameObject effect = UnityEngine.Object.Instantiate(eff.GetAsset()) as GameObject;
            eff.Unload();
            if (effect)
            {
                Transform tf = RoleObject.FindChildTransformWithName(root, effectInfo.BindBone);
                if (!tf)
                    tf = modelObject.transform;

                effect.transform.parent = tf;
                effect.transform.localPosition = effectInfo.Position;
                effect.transform.localEulerAngles = effectInfo.Rotation;
                effect.transform.localScale = effectInfo.Scale;
                TLEditorWww.ApplyParticleScale(effect.transform, effectInfo.Scale.x);
            }

        }

        Role config = root.gameObject.GetComponent<Role>();
        if (!config)
        {
            modelObject.transform.SetParent(root.transform);
            modelObject.transform.localPosition = Vector3.zero;
            modelObject.transform.localEulerAngles = Vector3.zero;
            modelObject.transform.localScale = Vector3.one;
            yield break;
        }

        SkinnedMeshRenderer skinrender = modelObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinrender)
        {
            if (equip.Bones != null && equip.Bones.Length > 0)
            {
                Transform[] bones = new Transform[equip.Bones.Length];
                for (int i = 0; i < equip.Bones.Length; i++)
                    bones[i] = ModelLoader.FindChildByName(root.transform, equip.Bones[i]);

                skinrender.bones = bones;
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
    public static RoleObject CreateRole(Role Config)
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
        EditorCoroutine.Execute(AsyncCreateRole(ro, ro.Config));

        return ro;
    }


    private static IEnumerator AsyncCreateRole(RoleObject ro, Role config)
    {
        if (sdWww == null)
        {
            sdWww = TLEditorWww.Create("res/myshader.sd");
            while (!sdWww.Finished)
                yield return null;
            sdWww.GetAsset();
            Shader.WarmupAllShaders();
        }

        foreach (var equip in ro.Config.Equips)
        {
            TLEditorWww equipWww = TLEditorWww.Create(equip.Equip);
            while (!equipWww.Finished)
                yield return null;
            Equip equipConfig = equipWww.GetAsset() as Equip;
            if (!equipConfig)
                Debug.Log("Error, Load Equip Failed: " + equip);
            else
                AddEquip(ro.gameObject, equipConfig,false);
            equipWww.Unload();
        }

        if (!string.IsNullOrEmpty(config.Mesh))
        {
            Debug.Log("setmash" + config.Mesh);
            TLEditorWww www = TLEditorWww.Create(config.Mesh);
            while (!www.Finished)
                yield return null;

            if (config.Skin)
            {
                config.Skin.sharedMesh = www.GetAsset() as Mesh;
                config.Skin.enabled = true;
                config.Skin.sharedMaterial.shader = Shader.Find(config.Skin.sharedMaterial.shader.name);
            }
            else if (config.Filter)
                config.Filter.sharedMesh = www.GetAsset() as Mesh;
            www.Unload();
        }

        if (!string.IsNullOrEmpty(config.MainTex))
        {
            Debug.Log("setmaintext" + config.MainTex);
            TLEditorWww www = TLEditorWww.Create(config.MainTex);
            while (!www.Finished)
                yield return null;

            if (config.Skin)
                config.Skin.sharedMaterial.mainTexture = www.GetAsset() as Texture;
            else if (config.Filter)
                config.Filter.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = www.GetAsset() as Texture;
            www.Unload();
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
