#define ZTK
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;


public class TLEditorWww
{
    private readonly WWW Www_;
    public AssetBundle AssetBundle { get { return Www_.assetBundle; } }
    private AssetBundle cachedAssetBundle;
    public AssetBundle CachedAssetBundle
    {
        get
        {
            if (!cachedAssetBundle)
                cachedAssetBundle = Www_.assetBundle;
            return cachedAssetBundle;
        }
        set
        {
            cachedAssetBundle = value;
        }
    }

    public TLEditorWww(string url)
    {
        Www_ = new WWW(url);
    }

    public bool Finished
    {
        get {return Www_.isDone; }
        set { Finished = value; }
    }

    public static TLEditorWww Create(string assetPath)
    {
        return new TLEditorWww(Utility.GetEditorUrl(assetPath));
    }

    public Object GetAsset()
    {
        if (Www_ == null || !Www_.assetBundle) return null;
        Object[] assets = Www_.assetBundle.LoadAllAssets();
        if (assets.Length > 0)
        {

            ReplaceShader(assets[0], string.Empty);
            return assets[0];
        }
            
        return null;
    }

    public Object GetCachedAsset()
    {
        if (!CachedAssetBundle) return null;
        Object[] assets = CachedAssetBundle.LoadAllAssets();
        if (assets.Length > 0)
        {

            ReplaceShader(assets[0], string.Empty);
            return assets[0];
        }

        return null;
    }

    public void Unload()
    {
        if (Www_ == null || !Www_.assetBundle)
            return;
#if ZTK
        Www_.assetBundle.Unload(false);
#endif
    }

    public static void ApplyParticleScale(Transform go, float scale)
    {
        scale = Mathf.Abs(scale);
        if (!go || Mathf.Approximately(scale, 1))
            return;
        for (int i = 0; i < go.childCount; i++)
            ApplyParticleScale(go.GetChild(i), scale);

        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        if (ps)
        {
            ParticleSystem.MainModule MainModule = ps.main;
            float size = MainModule.startSize.constant;
            MainModule.startSize = new ParticleSystem.MinMaxCurve(size * scale);
            go.transform.localScale = Vector3.one;
        }
    }

    #region replace shader
    public static void FindRenderers(ref List<Renderer> Renderers, Transform parent, bool includeInactive = true)
    {
        if (Renderers == null || !parent)
            return;
        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>(includeInactive);
        for (int i = 0; i < renderers.Length; i++)
            Renderers.Add(renderers[i]);

        //ParticleSystem[] ps = parent.GetComponentsInChildren<ParticleSystem>(includeInactive);
        //for (int i = 0; i < renderers.Length; i++)
        //    Renderers.Add(renderers[i]);
        //else {
        //    ParticleSystem ps = parent.GetComponent<ParticleSystem>();
        //    if (ps && ps.renderer)
        //        Renderers.Add(ps.renderer);
        //    else {
        //        ParticleEmitter pe = parent.particleEmitter;
        //        if (pe && pe.renderer)
        //            Renderers.Add(pe.renderer);
        //    }
        //}
    }

    public static List<Renderer> ReplaceShader(UnityEngine.Object obj, string url)
    {
        if (!Application.isEditor)
            return null;

        List<Renderer> render_list = new List<Renderer>();
        if (!obj)
            return render_list;
        if (obj is GameObject)
        {
            //FindRenderers(render_list, (obj as GameObject).transform);
            FindRenderers(ref render_list, (obj as GameObject).transform);
            if (render_list.Count == 0)
                return render_list;
            for (int i = 0; i < render_list.Count; i++)
                ReplaceRendererShader(render_list[i], url);
        }
        else if (obj is Material)
            ReplaceMaterialShader(obj as Material, url);

        return render_list;
    }

    static Dictionary<string, Shader> ShaderDic = new Dictionary<string, Shader>();

    public static void ReplaceMaterialShader(Material mat, string url)
    {
        if (!mat || !mat.shader)
        {
            Debug.Log("error: mat or shader of mat is null");
            return;
        }

        if (mat.shader)
        {
            int renderQueue = mat.renderQueue;
            string shadername = mat.shader.name;
            Shader shader = null;
            ShaderDic.TryGetValue(shadername, out shader);
            if (!shader)
            {
                shader = Shader.Find(shadername);
                ShaderDic[shadername] = shader;
            }
            if (shader)
            {
                mat.shader = null;
                mat.shader = shader;
                mat.renderQueue = renderQueue;
            }
            else
                Debug.Log(string.Format("error: shader {0} {1} {2} can't find in local", url, mat.name, mat.shader.name));


        }
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
    #endregion
}
