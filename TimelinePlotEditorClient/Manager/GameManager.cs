using UnityEngine;
using System.Collections;
using Model;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance_;
    GameObject mapPlacement;

    public bool myshaderLoadDone;
    public bool PublicLoadDone;
    public bool NormalLoadDone;
    public bool TextureLoadDone;

    public const string shaderPath = "res/pc_myshader.sd";
    public const string PublicPath = "res/public.go";
    public const string NormalPath = "res/normals.go";
    public const string TexturePath = "res/textures.go";
    private static bool isSceneTexLoadComplete;//场景依赖贴图是否加载完，加载场景前，先加载依赖的贴图，
    public const string CtrlAssets = "res/controllers.ctrl";
    public static bool isCtrlLoadDone;

    private static Dictionary<string,Texture> loaedTex = new Dictionary<string, Texture>();

    public static void CreateBaseComponets()
    {
        if (!instance_)
        {
            var gameManager = new GameObject("GameManager");
            DontDestroyOnLoad(gameManager);
            instance_ = gameManager.AddComponent<GameManager>();
            instance_.Init();
        }
    }

    private void Init()
    {
        XYDirectory.Init();
        XYCoroutineEngine.Load();
        XYSingleAssetLoader.Init();
        Loader.Init();
        TimelineManager.Init();
        World.Init();
    }

    public IEnumerator LoadLevel(string levelname, MapReference mf)
    {
        if (!instance_)
        {
            yield break;
        }
        XYCoroutineEngine.Execute(LoadShader(shaderPath));
        while (!instance_.myshaderLoadDone)
        {
            yield return null;
        }

        XYCoroutineEngine.Execute(LoadPublic(PublicPath));
        while (!instance_.PublicLoadDone)
        {
            yield return null;
        }
        XYCoroutineEngine.Execute(LoadNormal(NormalPath));
        while (!instance_.NormalLoadDone)
        {
            yield return null;
        }
        XYCoroutineEngine.Execute(LoadTexture(TexturePath));
        while (!instance_.TextureLoadDone)
        {
            yield return null;
        }

        XYCoroutineEngine.Execute(LoadCtrl(CtrlAssets));
        while (!isCtrlLoadDone)
        {
            yield return null;
        }
        XYCoroutineEngine.Execute(LoadSceneTex(levelname));
        if (!isSceneTexLoadComplete)
        {
            yield return null;
        }
        XYCoroutineEngine.Execute(LoadScene(levelname));
    }

    void OnLevelWasLoaded()
    {
        GameObject[] Terrains = GameObject.FindGameObjectsWithTag("Terrain");
        if (Terrains != null)
            for (int i = 0; i < Terrains.Length; i++)
            {
                Renderer renderer = Terrains[i].GetComponent<Renderer>();
                if (!renderer)
                    continue;
                renderer.sharedMaterial.shader = Shader.Find(renderer.sharedMaterial.shader.name);
            }
        Light light= GameObject.FindObjectOfType<Light>();
        if(light!=null)
            light.cullingMask = 0;
        var ca=SetupCamera();

        var root = GameObject.FindGameObjectWithTag("root");
        ReplaceShader(root, string.Empty);
        var config = root.GetComponent<SceneConfig>();
        CloseRender(config.Mainbuilding);
        CloseRender(config.Other);
        XYCoroutineEngine.Execute(LoadSceneAsset(config.Mainbuilding));
        XYCoroutineEngine.Execute(LoadSceneAsset(config.Other));

        AddMapPlacementComponents(ca);
    }

    private void CloseRender(List<SceneConfig.AssetConfig> configs)
    {
        foreach (var config in configs)
        {
            config.Material.shader = Shader.Find("MOYU/Fogoff");
        }
    }

    private Camera SetupCamera()
    {
        var cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
        cameraGO.tag = XYDefines.Tag.MainCamera;
        var ca = cameraGO.GetComponent<Camera>();
        cameraGO.AddComponent<CameraControll>();
        cameraGO.AddComponent<AudioListener>();
        ca.nearClipPlane = 0.1f;
        ca.farClipPlane = 200;
        ca.depth = 0;
        ca.useOcclusionCulling = false;
        ca.allowHDR = false;
        ca.allowMSAA = false;
        ca.nearClipPlane = 0.3f;
        ca.clearFlags = CameraClearFlags.Skybox;
        ca.renderingPath = RenderingPath.Forward;
        ca.fieldOfView = 60f;
        ca.cullingMask = -1;
        ca.backgroundColor = RenderSettings.fogColor;
        ca.transform.localPosition = Vector3.zero;
        ca.transform.localEulerAngles = new Vector3(45.0f, 0.0f, 0.0f);
        return ca;
    }

    private void AddMapPlacementComponents(Camera ca)
    {
        mapPlacement = new GameObject("MapPlacement");
        var mp = mapPlacement.AddComponent<MapPlacementController>();
        mapPlacement.AddComponent<XYFreeCamera>();
        ca.transform.parent = mp.transform;
        mp.transform.position = new Vector3(108, 2.5f, 93);
    }

    public IEnumerator LoadCtrl(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        isCtrlLoadDone = true;
        //www.Unload();
    }

    public IEnumerator LoadShader(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        ModelLoader.sdWww = www;
        myshaderLoadDone = true;
        //www.Unload();
    }

    public static IEnumerator LoadPublic(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        instance_.PublicLoadDone = true;
    }

    public static IEnumerator LoadNormal(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        instance_.NormalLoadDone = true;
    }

    public static IEnumerator LoadTexture(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        instance_.TextureLoadDone = true;
    }

    public IEnumerator LoadScene(string name)
    {
        TLEditorWww www = TLEditorWww.Create(string.Format("res/scenes/{0}.scene", name));
        while (!www.Finished)
            yield return null;
        AssetBundle bundle= www.AssetBundle;
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(name);
        //bundle.Unload(true);
    }


    private static IEnumerator LoadSceneTex(string sceneName)
    {
        string path = Application.dataPath + string.Format("/res/scenes/txt/{0}.txt", sceneName);
        string line;
        using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                var strData = line;
                if (strData == "SceneLoad")
                    continue;
                if (!loaedTex.ContainsKey(strData))
                {
                    var request = AssetBundle.LoadFromFileAsync(Application.dataPath + string.Format("/res/scenes/texture/{0}", strData));
                    while (!request.isDone)
                    {
                        yield return null;
                    }
                    var assets = request.assetBundle.LoadAllAssets();
                    if (assets != null && assets.Length > 0)
                        loaedTex.Add(strData, assets[0] as Texture);
                    else
                        loaedTex.Add(strData, null);
                }
            }
        }
        isSceneTexLoadComplete = true;
    }

    private IEnumerator LoadSceneAsset(List<SceneConfig.AssetConfig> list)
    {
        GameObject root = GameObject.FindGameObjectWithTag("root");
        //SceneConfig congig = root.GetComponent<SceneConfig>();
        for (int i = 0; i < list.Count; i++)
        {
            SceneConfig.AssetConfig config = list[i];
            List<string> temp = new List<string>();
            for (int j = 0; j < config.Configs.Count; j++)
            {
                var tex = config.Configs[j];
                if (loaedTex.ContainsKey(tex.Texture))
                {
                    continue;
                }
                var request = AssetBundle.LoadFromFileAsync(Application.dataPath + string.Format("/res/scenes/texture/{0}", tex.Texture));
                while (!request.isDone)
                    yield return null;
                var assets = request.assetBundle.LoadAllAssets();
                if (assets != null && assets.Length > 0)
                    loaedTex.Add(tex.Texture, assets[0] as Texture);
            }
            config.Material.shader = Shader.Find(config.Shader);
            foreach (var texConfig in config.Configs)
            {
                if (loaedTex[texConfig.Texture] != null)
                    config.Material.SetTexture(texConfig.PropertyName, loaedTex[texConfig.Texture]);
            }
        }
    }



    public static void ReplaceShader(UnityEngine.Object obj, string url)
    {
        List<Renderer> render_list = new List<Renderer>();
        if (!obj)
            return;
        if (obj is GameObject)
        {
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

}
