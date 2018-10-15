using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.IO;
using SimpleSpritePacker;
using System;
using Object = UnityEngine.Object;
using UnityEngine.UI;
using Cinemachine.Timeline;
using UnityEngine.Timeline;
using Cinemachine;

public static class AssetBundleBuilder {


    [MenuItem("Assets/Timeline/BuildTimeline")]
    public static void BuildOneAsset()
    {
        List<Object> willBuild = GetWillBuildTimelines();
        BuildAssetBundles(willBuild.ToArray());
        Debug.Log("build success");
    }

    public static List<Object> GetWillBuildTimelines()
    {
        Object[] selectObj = Selection.objects;
        List<Object> willBuild = new List<Object>();
        if (selectObj == null)
            return new List<Object>();
        foreach (Object obj in selectObj)
        {
            GameObject gameObj = obj as GameObject;
            if (gameObj != null)
            {
                PlayableDirector director = gameObj.GetComponent<PlayableDirector>();
                if (director != null)
                    willBuild.Add(gameObj);
            }
        }
        return willBuild;
    }

    [MenuItem("Assets/Timeline/BuildAllTimeline")]
    public static void BuildAllTimeline()
    {
        List<Object> willBuild = new List<Object>();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/Resources/Timeline/Timeline", typeof(Object));
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var obj in objs)
        {
            GameObject gameObj = obj as GameObject;
            if (gameObj != null)
            {
                PlayableDirector director = gameObj.GetComponent<PlayableDirector>();
                if (director != null)
                    willBuild.Add(gameObj);
            }
        }
        BuildAssetBundles(willBuild.ToArray());
    }

    static string outputPath = "Assets/res/timeline";
    public static void BuildAssetBundles(UnityEngine.Object[] assets)
    {
        EditorUtility.DisplayProgressBar("批量打包timeline中", 0 + "/" + assets.Length, 0 / assets.Length);
        int i = 0;
        foreach (UnityEngine.Object asset in assets)
        {
            RemoveUIConjunction(asset);
            SetVirtualCamera(asset);
            string assetfile = AssetDatabase.GetAssetPath(asset);
            string buildPath = Path.ChangeExtension(assetfile, "timeline");
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = Path.GetFileName(buildPath);
            buildMap[0].assetNames = new string[] { assetfile };
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            AddUIConjunction(asset);
            i++;
            EditorUtility.DisplayProgressBar("批量打包timeline中", i + "/" + assets.Length,i/ assets.Length);
        }
        EditorUtility.ClearProgressBar();
        return;
    }

    private static void SetVirtualCamera(Object asset)
    {
        //GameObject timelineObject = asset as GameObject;
        //PlayableDirector director = timelineObject.gameObject.GetComponent<PlayableDirector>();
        //foreach (PlayableBinding binding in director.playableAsset.outputs)
        //{
        //    if (binding.sourceObject is CinemachineTrack)
        //    {
        //        foreach (TimelineClip clip in (binding.sourceObject as TrackAsset).GetClips())
        //        {
        //            var shot = clip.asset as CinemachineShot;
        //            if (shot.isMainVitualCamera)
        //            {
        //                shot.VirtualCamera.exposedName = UnityEditor.GUID.Generate().ToString();
        //                director.ClearReferenceValue(shot.VirtualCamera.exposedName);
        //            }
        //        }
        //    }
        //}
    }




    public static Font font, titlefont;
    static void LoadFont()
    {
        if (!font)
            font = Resources.Load("UI/Font/uifont", typeof(Font)) as Font;
        if (!titlefont)
            titlefont = Resources.Load("UI/Font/uifont_title", typeof(Font)) as Font;
        AssetDatabase.Refresh();
    }

    private static void AddUIConjunction(Object asset)
    {
        LoadFont();
        if (asset is GameObject)
        {
            GameObject UI = asset as GameObject;
            Dictionary<string, SPInstance> spdic = new Dictionary<string, SPInstance>();
            CImage[] sprites = UI.GetComponentsInChildren<CImage>(true);
            for (int j = 0; j < sprites.Length; j++)
            {
                CImage sprite = sprites[j];
                SPInstance sp;
                if (!spdic.TryGetValue(sprite.AtlasName, out sp))
                {
                    sp = Resources.Load(string.Format("Timeline/UI/Atlas/timeline"), typeof(SPInstance)) as SPInstance;
                    if (!sp)
                        continue;
                    spdic[sprite.AtlasName] = sp;
                }
                sprite.sprite = sp.GetSprite(sprite.SpriteName);
            }

            //CRawImage[] RawImages = UI.GetComponentsInChildren<CRawImage>(true);
            //for (int j = 0; j < RawImages.Length; j++)
            //{
            //    CRawImage rawImage = RawImages[j];
            //    if (rawImage.IsLoad)
            //        continue;
            //    rawImage.texture = null;
            //}

            CText[] texts = UI.GetComponentsInChildren<CText>(true);
            for (int j = 0; j < texts.Length; j++)
            {
                //texts[j].material = null;
                if(texts[j].FontName=="uifont")
                    texts[j].font = font;
                else
                    texts[j].font = titlefont;

            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }

    private static void RemoveUIConjunction(Object asset)
    {
        if (asset is GameObject)
        {
            GameObject UI = asset as GameObject;
            Image[] images = UI.GetComponentsInChildren<Image>(true);

            if (images != null && images.Length > 0)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] is CImage)
                        continue;
                    EditorUtility.DisplayDialog("Error", asset.name + "包含Image，请执行>> UI脚本更换 <<", "OK");
                    return;
                }
            }

            Text[] textarray = UI.GetComponentsInChildren<Text>(true);
            if (textarray != null && textarray.Length > 0)
            {
                for (int i = 0; i < textarray.Length; i++)
                {
                    if (textarray[i] is CText)
                        continue;
                    EditorUtility.DisplayDialog("Error", asset.name + "包含Text，请执行>> UI脚本更换 <<", "OK");
                    return;
                }
            }

            CImage[] sprites = UI.GetComponentsInChildren<CImage>(true);
            for (int j = 0; j < sprites.Length; j++)
            {
                CImage sprite = sprites[j];
                sprite.sprite = null;
            }

            CRawImage[] RawImages = UI.GetComponentsInChildren<CRawImage>(true);
            for (int j = 0; j < RawImages.Length; j++)
            {
                CRawImage rawImage = RawImages[j];
                if (rawImage.IsLoad)
                    continue;
                rawImage.texture = null;
            }

            CText[] texts = UI.GetComponentsInChildren<CText>(true);
            for (int j = 0; j < texts.Length; j++)
            {
                texts[j].material = null;
                texts[j].font = null;
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }

    [MenuItem("Assets/Timeline/RepairUI")]
    private static void RepaireUI()
    {
        if (Selection.activeGameObject == null)
            return;
        AddUIConjunction(Selection.activeGameObject);
    }

    [MenuItem("Assets/Timeline/BuildSprite")]
    private static void BuildSprite()
    {
        foreach (UnityEngine.Object asset in Selection.objects)
        {
            BuildSprite(asset);
        }
        AssetDatabase.Refresh();
    }

    static void BuildSprite(UnityEngine.Object asset)
    {
        AssetDatabase.Refresh();
        if (asset is SPInstance)
            BuildAsset(asset, "sp", "Assets/res/ui/sprite");
    }

    //[MenuItem("Assets/Timeline/BuildTex")]
    private static void BuildUITex()
    {
        foreach (UnityEngine.Object asset in Selection.objects)
        {
            AssetDatabase.Refresh();
            if (asset is Texture)
                BuildAsset(asset, "tex", "Assets/res/ui/tex");
        }
    }

    static void BuildAsset(UnityEngine.Object asset, string Ext, string outputPath)
    {
        string assetfile = AssetDatabase.GetAssetPath(asset);
        string buildPath = Path.ChangeExtension(assetfile, Ext);
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = Path.GetFileName(buildPath);
        buildMap[0].assetNames = new string[] { assetfile };
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }


    [MenuItem("Assets/Timeline/BuilUIRoot")]
    private static void BuildUIRoot()
    {
        GameObject uiRoot = Selection.activeGameObject;
        if (uiRoot==null)
            return;
        if (uiRoot.GetComponent<TimelineUI>() == null)
            return;
        RemoveUIConjunction(uiRoot);
        BuildAsset(uiRoot, "ui", "Assets/res/ui/uiprefab");
        AddUIConjunction(uiRoot);
    }

}
