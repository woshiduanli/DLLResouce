using UnityEngine;
using System.Collections;
using Model;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static GameManager instance_;
    GameObject mapPlacement;
    MapReference mapref;
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
    private static Dictionary<string, Texture> loaedTex = new Dictionary<string, Texture>();


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
    }


    public static IEnumerator LoadLevel(string levelname, MapReference mf)
    {
        if (!instance_)
            yield break;
        isSceneTexLoadComplete = false;
        instance_.mapref = mf;
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
        while (!isSceneTexLoadComplete)
        {
            yield return null;
        }
        XYCoroutineEngine.Execute(LoadScene(levelname));
    }

    public static IEnumerator LoadShader(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        instance_.myshaderLoadDone = true;
    }

    public static IEnumerator LoadCtrl(string path)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        www.GetAsset();
        isCtrlLoadDone = true;
        //www.Unload();
    }

    public static IEnumerator LoadScene(string name)
    {
        TLEditorWww www = TLEditorWww.Create(string.Format("res/scenes/{0}.scene", name));
        while (!www.Finished)
            yield return null;
        AssetBundle bundle = www.AssetBundle;
        yield return new WaitForSeconds(1);
        var operation = SceneManager.LoadSceneAsync(name);
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


    bool LevelWasLoaded = false;
    void OnLevelWasLoaded()
    {
        LevelWasLoaded = false;
        if (SceneManager.GetActiveScene().name != "AsyncLevelLoader" &&
            SceneManager.GetActiveScene().name != "startMenu")
        {
            LevelWasLoaded = true;
        }

        var root = GameObject.FindGameObjectWithTag("root");
        MapPlacement.MPObject.ReplaceShader(root,string.Empty);

        var config = root.GetComponent<SceneConfig>();
        CloseRender(config.Mainbuilding);
        CloseRender(config.Other);
        RenderSettings.fog = false;
        XYCoroutineEngine.Execute(LoadSceneAsset(config.Mainbuilding));
        XYCoroutineEngine.Execute(LoadSceneAsset(config.Other));
    }

    private void CloseRender(List<SceneConfig.AssetConfig> configs)
    {
        foreach (var config in configs)
        {
            config.Material.shader= Shader.Find("MOYU/Fogoff");
        }
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
                config.Material.SetTexture(texConfig.PropertyName, loaedTex[texConfig.Texture]);
            }
        }
    }

    private static IEnumerator LoadSceneTex(string sceneName)
    {
        string path =Application.dataPath+string.Format("/res/scenes/txt/{0}.txt",sceneName);
        string line;
        using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                var strData = line;
                if (strData == "SceneLoad")
                    continue;
                //TLEditorWww www = TLEditorWww.Create("res/scenes/texture/" + strData);
                if (!loaedTex.ContainsKey(strData))
                {
                    var request = AssetBundle.LoadFromFileAsync(Application.dataPath + string.Format("/res/scenes/texture/{0}", strData));
                    while (!request.isDone)
                    {
                        yield return null;
                    }
                    //Debug.Log(www.AssetBundle.name);
                    request.assetBundle.LoadAllAssets();
                    var assets = request.assetBundle.LoadAllAssets();
                    if (assets != null && assets.Length > 0)
                        loaedTex.Add(strData, assets[0] as Texture);
                }
            }
        }
        isSceneTexLoadComplete = true;
    }





    void OnGUI()
    {
        if (!LevelWasLoaded)
        {
            return;
        }
        AddMapPlacementComponents();
    }

    private void AddMapPlacementComponents()
    {
        if (!mapPlacement)
        {
            RenderSettings.fog = false;
            GameObject cameraMain= GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 cameraInitPos = Vector3.zero;
            if (cameraMain != null)
            {
                cameraInitPos = cameraMain.transform.position;
                GameObject.DestroyImmediate(cameraMain);
            }
            mapPlacement = new GameObject("MapPlacement");
            var npcPlacementCamera = new GameObject("NpcPlacementCamera");
            npcPlacementCamera.tag = XYDefines.Tag.MainCamera;
            npcPlacementCamera.transform.parent = mapPlacement.transform;
            mapPlacement.transform.position = cameraInitPos;
            npcPlacementCamera.transform.localPosition = Vector3.zero;
            npcPlacementCamera.transform.localEulerAngles = new Vector3(45.0f, 0.0f, 0.0f);

            var fc = mapPlacement.AddComponent<XYFreeCamera>();
            var mp = mapPlacement.AddComponent<MapPlacementController>();

            var camera = npcPlacementCamera.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.fieldOfView = 60;
            camera.farClipPlane = 1000;
            camera.gameObject.AddComponent<AudioListener>();
            camera.nearClipPlane = 0.1f;
            camera.depth = XYDefines.CameraDepth.Main;
            camera.cullingMask = -1;
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.backgroundColor = Color.black;
            camera.renderingPath = RenderingPath.Forward;
            camera.backgroundColor = RenderSettings.fogColor;

            var mpgl = npcPlacementCamera.AddComponent<MapPlacementGL>();
            var rectSelectComponent = npcPlacementCamera.AddComponent<RectSelectComponent>();
            rectSelectComponent.getMapObjectsEvent = mp.GetEnumerator;
            rectSelectComponent.deleteMapObjectsOp = mp.DeleteObject;
            rectSelectComponent.copyMapObjectsOp = mp.PasteObject;
            rectSelectComponent.dragMapObjectsOp = mp.DragObject;
            rectSelectComponent.dragBeginMapObjectsOp = mp.DragBeginObject;
            rectSelectComponent.dragEndMapObjectsOp = mp.DragEndObject;
        }
    }

    private void SetupCamera()
    {
        var cameraGO = new GameObject("MainCamera");
        cameraGO.tag = XYDefines.Tag.MainCamera;
        var ca = cameraGO.AddComponent<Camera>();
        cameraGO.AddComponent<CameraControll>();
        cameraGO.AddComponent<AudioListener>();
        ca.nearClipPlane = 0.1f;
        ca.farClipPlane = 40;
        ca.fieldOfView = 60f;
        ca.depth = XYDefines.CameraDepth.Main;
        ca.nearClipPlane = 0.3f;
        ca.cullingMask = -1;
        ca.clearFlags = CameraClearFlags.Skybox;
        ca.backgroundColor = Color.black;
        ca.renderingPath = RenderingPath.Forward;
        ca.backgroundColor = RenderSettings.fogColor;
    }

}
