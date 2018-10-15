using UnityEditor;
using UnityEngine;
using System.IO;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.Reflection;
using UnityEditor.Animations;
using Action = Model.Action;
using UnityEngine.Rendering;
/// <summary>
/// 资源导出
/// 将unity资源导出成assetbundle资源
/// </summary>
public class AssetExporter
{
    public static string CreateAssetWithUniqueName(string orgPath, Object asset)
    {
        AssetDatabase.DeleteAsset(orgPath);
        AssetDatabase.Refresh();
        AssetDatabase.CreateAsset(asset, orgPath);
        AssetDatabase.Refresh();
        return orgPath;
    }

    public static void BuildAssetBundles(Object asset,bool compress=true)
    {
        if (!asset)
            return;
        string path = AssetDatabase.GetAssetPath(asset);
        BuildAssetBundles(asset, Path.GetDirectoryName(path), compress);
    }

    [MenuItem("Assets/工具/整理贴图到Resources-Public")]
    private static void MoveTextures()
    {
        EditorUtility.DisplayProgressBar("整理贴图", "", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        int count = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("整理贴图 " + asset.name, string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            count++;
            string filepath = AssetDatabase.GetAssetPath(asset);
            string[] dep = AssetDatabase.GetDependencies(filepath, true);
            foreach (var path in dep)
            {
                count++;
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer is TextureImporter)
                {
                    string filename = Path.GetFileName(path);
                    if (path.Contains("public"))
                        continue;
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
                    if (!string.IsNullOrEmpty(AssetDatabase.MoveAsset(path, "Assets/Resources/public/" + filename)))
                    {
                        string newpath = "Assets/Resources/public/" + texture.name + "_" + count.ToString() + Path.GetExtension(filename);
                        Debug.LogError(newpath);
                        AssetDatabase.MoveAsset(path, newpath);
                    }

                    AssetDatabase.Refresh();
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Assets/工具/整理role的特效贴图到Resources-Public")]
    private static void MoveRoleEffectTextures()
    {
        EditorUtility.DisplayProgressBar("整理role的特效贴图", "", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        int count = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("整理role的特效贴图 " + asset.name, string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            count++;

            if (!Utility.isRole(asset))
                continue;

            List<Texture> Textures = new List<Texture>();
            GameObject go = asset as GameObject;
            Role role = go.GetComponent<Role>();
            SkinnedMeshRenderer[] skins = role.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach(var s in skins)
            {
                Textures.AddRange(FindMatTextures(s.sharedMaterial));
            }


            string filepath = AssetDatabase.GetAssetPath(asset);
            string[] dep = AssetDatabase.GetDependencies(filepath, true);
            foreach (var path in dep)
            {
                count++;
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer is TextureImporter)
                {
                    string filename = Path.GetFileName(path);
                    if (path.Contains("public"))
                        continue;
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
                    if (Textures.Contains(texture))
                        continue;
                    if (!string.IsNullOrEmpty(AssetDatabase.MoveAsset(path, "Assets/Resources/public/" + filename)))
                    {
                        string newpath = "Assets/Resources/public/" + texture.name + "_" + count.ToString() + Path.GetExtension(filename);
                        AssetDatabase.MoveAsset(path, newpath);
                    }
                    AssetDatabase.Refresh();
                    Debug.LogError(texture.name, texture);
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Assets/工具/去除重复贴图")]
    public static void RemoveSame()
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);

        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/res_sourcefile/effect", typeof(Object));
        Object[] prefabs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var go in prefabs)
        {
            if (!(go is GameObject))
                continue;
            Renderer[] renders = (go as GameObject).GetComponentsInChildren<Renderer>(true);
            foreach (var r in renders)
            {
                if (objects.Contains(r.sharedMaterial.mainTexture))
                {
                    if (objects[0] is Texture)
                    {
                        r.sharedMaterial.mainTexture = objects[0] as Texture;
                        Debug.LogError(go.name + "   /" + r.gameObject.name, go);
                    }
                }
            }
        }
    }

    public static List<Texture> FindMatTextures(Material mat)
    {
        if (!mat)
            return null;
        List<Texture> list = new List<Texture>();
        List<string> prolist = new List<string>();
        prolist.Add("_NoiseTex");
        prolist.Add("_Alpha");
        prolist.Add("_Cutout");
        prolist.Add("_Mask");
        prolist.Add("_Ramp");
        prolist.Add("_MatCap");
        prolist.Add("_NoiseTex");
        prolist.Add("_UVMainTex");
        prolist.Add("_UVMask");
        prolist.Add("_MainTex");
        prolist.Add("_AlphaTex");
        prolist.Add("_SpecGlossMap");

        foreach (var p in prolist)
        {
            if (mat.HasProperty(p))
            {
                Texture tex = mat.GetTexture(p);
                if (tex)
                    list.Add(tex);
            }
        }
        if (mat.mainTexture)
            list.Add(mat.mainTexture);
        return list;
    }

    [MenuItem("Assets/工具/找出贴图关联的特效")]
    public static void FindTexturePrefab()
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);

        EditorUtility.DisplayProgressBar("查找中", "", 0);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/res_sourcefile/effect", typeof(Object));
        Object[] prefabs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        foreach (var go in prefabs)
        {
            EditorUtility.DisplayProgressBar("查找中"+ go.name, string.Format("{0}/{1}", i, prefabs.Length), (float)i / (float)prefabs.Length);
            i++;
            if (!(go is GameObject))
                continue;
            Renderer[] renders = (go as GameObject).GetComponentsInChildren<Renderer>(true);
            foreach (var r in renders)
            {
                List<Texture> list = FindMatTextures(r.sharedMaterial);
                foreach(var t in objects)
                {
                    if(t is Texture)
                    {
                        if (list != null && list.Contains(t as Texture))
                            Debug.LogError(t.name + " ----->  " + go.name + "   /" + r.gameObject.name, go);
                    }

                }
              
            }
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Assets/工具/材质球shader检查")]
    private static void CheckShader()
    {
        EditorUtility.DisplayProgressBar("shader检查", "", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("shader检查", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            GameObject go = asset as GameObject;
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                if (!r.sharedMaterial || !r.sharedMaterial.shader)
                    continue;
                if (!r.sharedMaterial.shader.name.Contains("MOYU"))
                {
                    string shadername = r.sharedMaterial.shader.name;
                    if (shadername.EndsWith("/Additive") || shadername == "Particles/Additive")
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/Particles/Additive");
                        Debug.Log(asset.name + "  " + r.name +"  "+ shadername+"shader 修复成功");
                        continue;
                    }
                    if (shadername.EndsWith("/AlphaBlended") || shadername == "Particles/Alpha Blended")
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/Particles/AlphaBlended");
                        Debug.Log(asset.name + "  " + r.name + "  " + shadername + "shader 修复成功");
                        continue;
                    }
                    if (shadername == "Legacy Shaders/Diffuse" || shadername == "Mobile/Diffuse" || shadername == ("Unlit/Texture"))
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/VertexLit");
                        Debug.Log(asset.name + "  " + r.name + "  " + shadername + "shader 修复成功");
                        continue;
                    }
                    if ((shadername.StartsWith("Legacy Shaders")))
                    {
                        if (shadername.Contains("Legacy Shaders/Transparent/Cutout"))
                            r.sharedMaterial.shader = Shader.Find("MOYU/AlphaTest");
                        else
                            r.sharedMaterial.shader = Shader.Find("MOYU/AlphaBlend");
                        Debug.Log(asset.name + "  " + r.name + "  " + shadername + "shader 修复成功");
                        continue;
                    }
                    Debug.LogError(asset.name + r.name + "shader错误，请使用MOYU的shader");
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    static string outAssetFolder = "Assets/AssetBundle";
    public static void BuildAssetBundles(UnityEngine.Object asset, string outputPath, bool compress = true)
    {
        if (string.IsNullOrEmpty(Utility.GetAssetExt(asset)))
            return;

        if(compress)
        {
            string[] dep = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(asset), true);
            for (int i = 0; i < dep.Length; i++)
            {
                AssetImporter depimporter = AssetImporter.GetAtPath(dep[i]);
                CompressAsset(AssetDatabase.LoadAssetAtPath(dep[i], typeof(object)));
            }
        }

        if (asset is Texture)
            CompressAsset(asset);

        if (asset is GameObject)
        {
            GameObject go = asset as GameObject;
            CheckAni(go);
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach(var r in renderers)
            {
                if(!r.sharedMaterial|| !r.sharedMaterial.shader)
                    continue;
                if (!r.sharedMaterial.shader.name.Contains("MOYU"))
                {
                    string shadername = r.sharedMaterial.shader.name;
                    if (shadername.EndsWith("/Additive") || shadername == "Particles/Additive")
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/Particles/Additive");
                        continue;
                    }
                    if (shadername.EndsWith("/AlphaBlended") || shadername == "Particles/Alpha Blended")
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/Particles/AlphaBlended");
                        continue;
                    }
                    if (shadername == "Legacy Shaders/Diffuse" || shadername == "Mobile/Diffuse"|| shadername == ("Unlit/Texture"))
                    {
                        r.sharedMaterial.shader = Shader.Find("MOYU/VertexLit");
                        continue;
                    }
                    if ((shadername.StartsWith("Legacy Shaders")))
                    {
                        if (shadername.Contains("Legacy Shaders/Transparent/Cutout"))
                            r.sharedMaterial.shader = Shader.Find("MOYU/AlphaTest");
                        else
                            r.sharedMaterial.shader = Shader.Find("MOYU/AlphaBlend");
                        continue;
                    }
                    EditorUtility.DisplayDialog("Error", asset.name + r.name + "shader错误，请使用MOYU的shader", "OK");
                    Debug.LogError(asset.name + r.name + "shader错误，请使用MOYU的shader");
                }
            }
        }
        AssetImporter importer = SetAssetBundleNameAndVariant(asset);
        if (importer == null)
            return;

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();

        if (asset is GameObject || asset is Material)
        {
            buildMap.Add(CreateAssetBundle(shaders, "myshader", "sd"));
            buildMap.Add(CreateAssetBundle(PublicAssets, "public", "go"));
        }
        if (asset&&Utility.isRole(asset))
        {
            buildMap.Add(CreateAssetBundle(Controllers, "controllers", "ctrl"));
        }

        if (asset&&Utility.isModel(asset))
        {
            buildMap.Add(CreateAssetBundle(NTextures, "normals", "go"));
            buildMap.Add(CreateAssetBundle(Textures, "textures", "go"));
        }

        AssetBundleBuild ab1 = new AssetBundleBuild();
        ab1.assetBundleName = importer.assetBundleName;
        ab1.assetBundleVariant = importer.assetBundleVariant;
        ab1.assetNames = new string[] { importer.assetPath };
        buildMap.Add(ab1);

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        if (!Directory.Exists(outAssetFolder))
            Directory.CreateDirectory(outAssetFolder);

        BuildPipeline.BuildAssetBundles(outAssetFolder, buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
        string assetname = string.Format("{0}.{1}", importer.assetBundleName, importer.assetBundleVariant);
        AssetDatabase.DeleteAsset(string.Format("{0}/{1}", outputPath, assetname));
        AssetDatabase.MoveAsset(string.Format("{0}/{1}", outAssetFolder, assetname), string.Format("{0}/{1}", outputPath, assetname));
    }

    private static void CheckAni(GameObject root)
    {
        Animation[] anis = root.GetComponentsInChildren<Animation>(true);
        for (int i = 0; i < anis.Length; i++)
        {
            anis[i].animatePhysics = false;
            if (anis[i].GetClipCount() == 0)
                UnityEngine.Object.DestroyImmediate(anis[i]);
        }

        Animator[] Animators = root.GetComponentsInChildren<Animator>(true);
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].updateMode = AnimatorUpdateMode.Normal;
            if (!Animators[i].runtimeAnimatorController)
                UnityEngine.Object.DestroyImmediate(Animators[i]);
        }
    }

    public static void CompressAsset(Object asset)
    {
        string path = AssetDatabase.GetAssetPath(asset);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is TextureImporter && asset is Texture2D)
            CompressTex(importer as TextureImporter, path, asset as Texture2D, true);
        else if (importer is ModelImporter)
            CompressMesh(path, false);
        else if (importer is AudioImporter)
            CompressAudio(asset, path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void CompressTex(TextureImporter textureImporter, string path, Texture2D texture, bool mipmap)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings("Android");
        if (PlatformSet.format == TextureImporterFormat.RGB16 || PlatformSet.format == TextureImporterFormat.RGBA16)
            return;
        if (textureImporter.textureType == TextureImporterType.Sprite)
            return;
        TextureImporterFormat texFormat = TextureImporterFormat.ETC_RGB4Crunched;
#if UNITY_IPHONE || UNITY_IOS
        if (texture.width != texture.height)
        {
            texFormat = TextureImporterFormat.RGB16;
            if (textureImporter.DoesSourceTextureHaveAlpha())
                texFormat = TextureImporterFormat.RGBA16;
        }
        else
        {
            texFormat = TextureImporterFormat.PVRTC_RGB4;
            if (textureImporter.DoesSourceTextureHaveAlpha())
                texFormat = TextureImporterFormat.PVRTC_RGBA4;
        }
#else

#if UNITY_2017_4_OR_NEWER
        texFormat = TextureImporterFormat.ETC_RGB4Crunched;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8Crunched;
#else

        texFormat = TextureImporterFormat.ETC2_RGB4;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8;
#endif
#endif
        if (textureImporter.textureType == TextureImporterType.Default ||
            textureImporter.textureType == TextureImporterType.NormalMap)
        {
            textureImporter.mipmapEnabled = mipmap;
            if (mipmap)
                textureImporter.mipmapFilter = TextureImporterMipFilter.KaiserFilter;
        }
        else
        {
            textureImporter.mipmapEnabled = false;
        }
        textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        textureImporter.isReadable = false;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.anisoLevel = 1;
        textureImporter.textureCompression = TextureImporterCompression.Compressed;

        SetPlatformTextureSettings("Android", texture, textureImporter, texFormat);
        SetPlatformTextureSettings("iPhone", texture, textureImporter, texFormat);
    }


    public static void SetPlatformTextureSettings(string platform, Texture2D texture, TextureImporter textureImporter, TextureImporterFormat format)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings(platform);
        if (PlatformSet.format == TextureImporterFormat.RGB16 || PlatformSet.format == TextureImporterFormat.RGBA16)
            return;
        //if(PlatformSet.format== format)
        //    return;
        int size = Mathf.Max(texture.width, texture.height);
        PlatformSet.overridden = true;
        PlatformSet.compressionQuality = 70;
        if (texture.name.EndsWith("_n") || texture.name.EndsWith("_s") || texture.name.EndsWith("_m") || texture.name.EndsWith("_sd") || texture.name.EndsWith("_se"))
            PlatformSet.compressionQuality = 50;

        PlatformSet.maxTextureSize = size;
        PlatformSet.textureCompression = TextureImporterCompression.Compressed;
        PlatformSet.format = format;
        textureImporter.crunchedCompression = true;
        textureImporter.SaveAndReimport();
        textureImporter.SetPlatformTextureSettings(PlatformSet);
    }


    public static void CompressMesh(string path, bool isRead)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is ModelImporter)
        {
            ModelImporter modelImporter = importer as ModelImporter;
            if (modelImporter.meshCompression == ModelImporterMeshCompression.Medium)
                return;
            modelImporter.optimizeMesh = true;
            modelImporter.isReadable = isRead;
            if (modelImporter.meshCompression == ModelImporterMeshCompression.Off)
                modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    public static void CompressAudio(Object asset, string path)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is AudioImporter)
        {
            AudioClip audio = asset as AudioClip;
            AudioImporter audioImporter = importer as AudioImporter;
            audioImporter.forceToMono = true;
            audioImporter.loadInBackground = true;
            audioImporter.preloadAudioData = false;
            audioImporter.ambisonic = false;
            SetPlatformAudioSettings("Android", audioImporter, audio.length > 5);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    public static void SetPlatformAudioSettings(string platform, AudioImporter importer, bool big)
    {
        AudioImporterSampleSettings settings = importer.GetOverrideSampleSettings(platform);
        if (big)
        {
            settings.loadType = AudioClipLoadType.Streaming;
            settings.compressionFormat = AudioCompressionFormat.MP3;
            settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            settings.quality = 0.3f;
        }
        else
        {
            settings.loadType = AudioClipLoadType.DecompressOnLoad;
            settings.compressionFormat = AudioCompressionFormat.ADPCM;
            settings.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
        }
        importer.SetOverrideSampleSettings(platform, settings);
    }

    public static AssetBundleBuild CreateAssetBundle(List<Object> assets, string BundleName, string Variant)
    {
        AssetBundleBuild publicab = new AssetBundleBuild();
        publicab.assetBundleName = BundleName;
        publicab.assetBundleVariant = Variant;
        List<string> publiclist = new List<string>();
        for (int i = 0; i < assets.Count; i++)
        {
            string path = AssetDatabase.GetAssetPath(assets[i]);
            AssetImporter sdimporter = AssetImporter.GetAtPath(path);
            sdimporter.assetBundleName = BundleName;
            sdimporter.assetBundleVariant = Variant;
            publiclist.Add(path);
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        publicab.assetNames = publiclist.ToArray();
        return publicab;
    }

    #region 预览特效
    [MenuItem("Assets/工具/Preview Effect")]
    private static void PreviewEffect()
    {
        if (!Selection.activeObject) return;
        if (!EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
            return;
        }
        string file = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.Equals(Path.GetExtension(file), ".go", StringComparison.OrdinalIgnoreCase))
        {
            PreviewEffect(file);
        }
    }

    private static void PreviewEffect(string file)
    {
        GameObject go = Object.Instantiate(EditorWww.Create(file)) as GameObject;
        Transform cameraTf = Camera.main.transform;
        go.transform.position = cameraTf.position + cameraTf.forward * 3;
    }
    #endregion

    public static List<Object> shaders = new List<Object>();
    public static List<Object> PublicAssets = new List<Object>();
    public static List<Object> Controllers = new List<Object>();
    public static List<Object> Textures = new List<Object>();
    public static List<Object> NTextures = new List<Object>();

    public static void GetShaders()
    {
        shaders.Clear();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/Resources/Shader/MOYU", typeof(Object));
        Object[] objects = Selection.GetFiltered(typeof(Shader), SelectionMode.DeepAssets);
        for (int i = 0; i < objects.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(objects[i]);
            AssetImporter sdimporter = AssetImporter.GetAtPath(path);
            string file = Path.GetFileNameWithoutExtension(path);
            shaders.Add(objects[i]);
        }
    }

    public static void GetPublicAssets()
    {
        PublicAssets.Clear();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/Resources/public", typeof(Object));
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in objects)
        {
            if (string.IsNullOrEmpty(Utility.GetAssetExt(asset)))
                continue;
            //CompressAsset(asset);
            PublicAssets.Add(asset);
        }
    }

    public static void GetTextures()
    {
        Textures.Clear();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/sourcefile/texture", typeof(Object));
        Object[] objects = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        foreach (Object asset in objects)
            Textures.Add(asset);

        NTextures.Clear();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/sourcefile/normal", typeof(Object));
        objects = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        foreach (Object asset in objects)
            NTextures.Add(asset);
    }

    public static void GetControllers()
    {
        Controllers.Clear();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath(ControllerFolder, typeof(Object));
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in objects)
        {
            if (string.IsNullOrEmpty(Utility.GetAssetExt(asset)))
                continue;
            Controllers.Add(asset);
        }
    }

    #region 资源打包到指定目录
    [MenuItem("Assets/工具/ExportAssets")]
    private static void ExportAssets()
    {
        string filePath = GetSaveFilePath(Selection.objects[0]);
        if (string.IsNullOrEmpty(filePath))
            return;

        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);
        GetShaders();
        GetPublicAssets();
        GetTextures();
        GetControllers();//控制器
        Selection.objects = objects.ToArray();
        foreach (Object asset in objects)
            BuildAssetBundles(asset, filePath);


    }

    [MenuItem("Assets/工具/资源批量打包/急速")]
    public static void BuildRes_sourcefileFast()
    {
        BuildRes_sourcefile(false);
    }

    [MenuItem("Assets/工具/资源批量打包/普通")]
    public static void BuildRes_sourcefileSlow()
    {
        BuildRes_sourcefile(true);
    }

    public static void BuildRes_sourcefile(bool compress)
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        objects.AddRange(Assets);

        GetShaders();
        GetPublicAssets();
        GetTextures();
        GetControllers();//控制器
        Selection.objects = Assets;
        foreach (Object asset in objects)
        {            if(!asset)
                continue;
            string ext = Utility.GetAssetExt(asset);
            if (string.IsNullOrEmpty(ext))
                continue;

            string assetfile = AssetDatabase.GetAssetPath(asset);
            BuildAssetBundles(asset, Path.GetDirectoryName(assetfile.Replace("Assets/res_sourcefile", outAssetFolder)), compress);
        }

    }

    [MenuItem("Assets/工具/Build Selected/快")]
    private static void BuildSelectedFast()
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);

        GetShaders();
        GetPublicAssets();
        GetTextures();
        GetControllers();//控制器
        Selection.objects = objects.ToArray();
        foreach (Object asset in objects)
            BuildAssetBundles(asset,false);


    }

    [MenuItem("Assets/工具/Build Selected/普通")]
    private static void BuildSelected()
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);

        GetShaders();
        GetPublicAssets();
        GetTextures();
        GetControllers();//控制器
        Selection.objects = objects.ToArray();

        foreach (Object asset in objects)
            BuildAssetBundles(asset,true);


    }

    #endregion

    private static string GetSaveFilePath(Object asset)
    {
        string path = EditorUtility.SaveFilePanelInProject("Build Asset", asset.name, "", "", Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset)));
        return Path.GetDirectoryName(path);
    }

    [MenuItem("Assets/工具/Get Asset From Bundles")]
    private static void GetAssetsFromBundles()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
            return;
        }
        foreach (Object asset in Selection.objects)
        {
            GetAssetFromBundles(AssetDatabase.GetAssetPath(asset));
        }
    }

    private static void GetAssetFromBundles(string bundlePath)
    {
        Object asset = EditorWww.Create(bundlePath);
        if (asset)
        {
            string assetPath = Path.ChangeExtension(bundlePath, "asset");
            if (asset is ScriptableObject)
            {
                Object scriptableObject = ScriptableObject.CreateInstance(asset.GetType());
                EditorUtility.CopySerialized(asset, scriptableObject);
                AssetDatabase.CreateAsset(scriptableObject, assetPath);
            }
            else if (asset is GameObject)
            {
                GameObject obj = Object.Instantiate(asset) as GameObject;
                string folder = Path.GetDirectoryName(bundlePath);
                string path = string.Format("{0}/{1}.prefab", folder, asset.name);
                Object tempPrefab = PrefabUtility.CreateEmptyPrefab(path);
                tempPrefab = PrefabUtility.ReplacePrefab(obj, tempPrefab);
                Object.DestroyImmediate(obj);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            UnityEngine.Debug.Log("Error: " + bundlePath);
        }
    }



    public static AssetImporter SetAssetBundleNameAndVariant(Object asset)
    {
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
        string Ext = Utility.GetAssetExt(asset);
        if(string.IsNullOrEmpty(Ext))
        {
            importer.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
            return null;
        }
        bool isrole = Utility.isRole(asset) || Ext == "role";
        string assetfile = AssetDatabase.GetAssetPath(asset);
        string buildPath = Path.ChangeExtension(assetfile, Ext);

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        string assetBundleName = Path.GetFileName(buildPath);

        if (!isrole && !assetBundleName.StartsWith(Ext + "_") && Ext != "action")
            assetBundleName = string.Format("{0}_{1}", Ext, assetBundleName);

        assetBundleName = Path.GetFileNameWithoutExtension(assetBundleName);
        if (Utility.isCtrl(asset) || Ext == "ctrl")
            assetBundleName = "controllers";
        importer.SetAssetBundleNameAndVariant(assetBundleName, Ext);
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(assetfile);
        return importer;
    }

    [MenuItem("Assets/工具/清除资源包名")]
    public static void ClearAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("setAB", "", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("setAB", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
            importer.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Assets/工具/生成移动动画")]
    public static void GenerateMoveAni()
    {
        Object[] Assets = Selection.objects;
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("生成移动动画", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            if (asset is AnimationClip)
                GenerateSigleAni(asset as AnimationClip);
            Debug.Log(Assets.Length);
            i++;
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }

    static void GenerateSigleAni(AnimationClip clip)
    {
        string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        EditorCurveBinding[] clipdatas = AnimationUtility.GetCurveBindings(clip);
        List<EditorCurveBinding> newdatas = new List<EditorCurveBinding>();
        List<AnimationCurve> curves = new List<AnimationCurve>();
       
        AnimationClip moveClip = new AnimationClip();
        moveClip.legacy = true;
        AssetDatabase.CreateAsset(moveClip, string.Format("{0}/{1}_move.anim", rootpath, clip.name));
        for (int i = 0; i < clipdatas.Length; i++)
        {
            EditorCurveBinding data = clipdatas[i];
            AnimationCurve Curve = AnimationUtility.GetEditorCurve(clip, data);
            
            List<Keyframe> kfs = new List<Keyframe>();
            List<Keyframe> mkfs = new List<Keyframe>();
            
            if (data.propertyName.Contains("m_LocalPosition.z") && data.path.Equals("Bip01"))
            {
                Debug.Log(data.propertyName);
                for (int j = 0; j < Curve.length; j++)
                {
                    Keyframe kf = Curve[j];
                    kf.value = float.Parse(string.Format("{0:f3}", kf.value));
                    kf.inTangent = float.Parse(string.Format("{0:f3}", kf.inTangent));
                    kf.outTangent = float.Parse(string.Format("{0:f3}", kf.outTangent));
                    mkfs.Add(kf);
                }
                moveClip.SetCurve("", data.type, data.propertyName, new AnimationCurve(mkfs.ToArray()));
                Curve = new AnimationCurve(null);
            }
            curves.Add(Curve);
            newdatas.Add(data);
        }
        clip.ClearCurves();
        for (int i = 0; i < newdatas.Count; i++)
            clip.SetCurve(newdatas[i].path, newdatas[i].type, newdatas[i].propertyName, curves[i]);

        AssetDatabase.Refresh();
    }

    #region CreateAsset
    public const string Boundname = "RoleBound";

    public static Object CreateRole(GameObject go)
    {
        Animator animator = go.GetComponent<Animator>();
        Avatar avatar = animator.avatar;

        foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
            Object.DestroyImmediate(smr.gameObject);

        foreach (Animation ani in go.GetComponentsInChildren<Animation>())
            Object.DestroyImmediate(ani);

        foreach (Animator Aor in go.GetComponentsInChildren<Animator>())
            Object.DestroyImmediate(Aor);

        foreach (MeshFilter mf in go.GetComponentsInChildren<MeshFilter>())
        {
            if (mf.gameObject.name == Boundname)
                continue;
            Object.DestroyImmediate(mf.gameObject);
        }

        Role role = Utility.AddComponent<Role>(go);
        if (!role.BoxCollider)
        {
            role.BoxCollider = new GameObject(Boundname);
            BoxCollider aBoxCollider = role.BoxCollider.AddComponent<BoxCollider>();
            aBoxCollider.isTrigger = true;
            role.BoxCollider.transform.parent = go.transform;
            role.BoxCollider.transform.localPosition = Vector3.zero;
            role.BoxCollider.transform.localRotation = Quaternion.identity;
            role.BoxCollider.transform.localScale = Vector3.one;
        }

        Transform[] bones = go.GetComponentsInChildren<Transform>(true);
        List<GameObject> golist = new List<GameObject>();
        for (int i = 0; i < bones.Length; i++)
            golist.Add(bones[i].gameObject);

        role.Bones = golist.ToArray();

        string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

        Selection.activeObject = AssetDatabase.LoadAssetAtPath(rootpath, typeof(Object));
        Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);

        animator = go.AddComponent<Animator>();
        animator.avatar = avatar;
        animator.runtimeAnimatorController = CreateOverrideController(string.Format("{0}.controller", go.name), rootpath, clips);

        AnimatorOverrideController Controller = animator.runtimeAnimatorController as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        Controller.GetOverrides(overrides);
        List<OverrideAnimiClip> list = new List<OverrideAnimiClip>();
        for (int i = 0; i < overrides.Count; i++)
        {
            KeyValuePair<AnimationClip, AnimationClip> kvp = overrides[i];
            if (kvp.Value == null)
                continue;
            list.Add(new OverrideAnimiClip(kvp.Key.name, kvp.Value));
        }
        role.AnimiClips = list.ToArray();

        string Prefabpath = rootpath + "/" + go.name + ".prefab";
        AssetDatabase.DeleteAsset(Prefabpath);
        AssetDatabase.Refresh();
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(Prefabpath);
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);

        Selection.activeObject = AssetDatabase.LoadAssetAtPath(Prefabpath, typeof(GameObject));
        Object.DestroyImmediate(go);

        return tempPrefab;
    }

    const string ControllerFolder = "Assets/Controller";
    const string ControllerPath = "Assets/Controller/base.controller";

    [MenuItem("Assets/工具/Create/CreateController")]
    private static void CreatePrefectController()
    {
        AnimatorController asset = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        List<AnimationClip> clips = new List<AnimationClip>();

        FieldInfo[] fields = typeof(MotionState).GetFields();
        for (int c = 0; c < fields.Length; c++)
        {
            FieldInfo fi = fields[c];
            if (fi.FieldType != typeof(MotionState))
                continue;
            asset.AddParameter(fi.Name, AnimatorControllerParameterType.Trigger);

            AnimationClip clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, string.Format("{0}/{1}.anim", ControllerFolder, fi.Name));
            clips.Add(clip);
        }

        asset.AddLayer("layer0");
        AnimatorControllerLayer layer = asset.layers[0];
        layer.stateMachine.states = new ChildAnimatorState[fields.Length];

        AnimatorState standstate = layer.stateMachine.AddState("stand");
        standstate.motion = Utility.GetClip("stand", clips.ToArray()) as Motion;
        layer.stateMachine.defaultState.AddTransition(standstate);
        int i = 0;
        int j = fields.Length / 2 - 1;
        int x = 0;
        for (int c = 0; c < fields.Length; c++)
        {
            FieldInfo fi = fields[c];
            if (fi.FieldType != typeof(MotionState) || fi.Name == "stand")
                continue;
            AddTransition(layer.stateMachine, fi.Name, clips.ToArray(), new Vector3(x, 60 * i, 0));
            j--;
            i++;
            if (j < 0)
            {
                j = fields.Length / 2;
                x += 500;
                i = 0;
            }
        }
    }


    private static void AddTransition(AnimatorStateMachine stateMachine, string statename, AnimationClip[] clips, Vector3 pos)
    {
        AnimatorState runstate = stateMachine.AddState(statename, pos);
        runstate.motion = Utility.GetClip(statename, clips) as Motion;
        AnimatorStateTransition runTrans = stateMachine.AddAnyStateTransition(runstate);
        int value = (int)Enum.Parse(typeof(MotionState), statename);
        runTrans.AddCondition(AnimatorConditionMode.If, value, statename);
        runstate.AddExitTransition().hasExitTime = true;
    }

    [MenuItem("Assets/工具/Create/动作补齐")]
    private static void AddTransitions()
    {
        AnimatorController asset = Selection.activeObject as AnimatorController;
        AnimatorControllerLayer layer = asset.layers[0];

        List<string> States = new List<string>();
        for (int i = 0; i < layer.stateMachine.states.Length; i++)
            States.Add(layer.stateMachine.states[i].state.motion.name);

        FieldInfo[] fields = typeof(MotionState).GetFields();
        for (int c = 0; c < fields.Length; c++)
        {
            FieldInfo fi = fields[c];
            if (fi.FieldType != typeof(MotionState))
                continue;
            if (States.Contains(fi.Name))
                continue;
            asset.AddParameter(fi.Name, AnimatorControllerParameterType.Trigger);
            AnimationClip clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, string.Format("{0}/{1}.anim", ControllerFolder, fi.Name));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AddTransition(layer.stateMachine, fi.Name, clip, new Vector3(300, 480, 0));
        }
    }

    private static void AddTransition(AnimatorStateMachine stateMachine, string statename, AnimationClip clip, Vector3 pos)
    {
        AnimatorState runstate = stateMachine.AddState(statename, pos);
        runstate.motion = clip as Motion;
        AnimatorStateTransition runTrans = stateMachine.AddAnyStateTransition(runstate);
        int value = (int)Enum.Parse(typeof(MotionState), statename);
        runTrans.AddCondition(AnimatorConditionMode.If, value, statename);
    }

    public static AnimatorOverrideController CreateOverrideController(string filename, string path, Object[] clips)
    {
        AnimatorOverrideController asset = new AnimatorOverrideController();
        asset.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath(ControllerPath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (asset.runtimeAnimatorController == null)
        {
            EditorUtility.DisplayDialog("Error", "沒有动画控制器，联系相关客户端", "OK");
            return null;
        }
        List<KeyValuePair<AnimationClip, AnimationClip>> Pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < asset.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = asset.runtimeAnimatorController.animationClips[i];
            KeyValuePair<AnimationClip, AnimationClip> clippair = new KeyValuePair<AnimationClip, AnimationClip>(clip, Utility.GetClip(clip.name, clips));
            Pairs.Add(clippair);
        }
        asset.ApplyOverrides(Pairs);
        CreateAssetWithUniqueName(Path.Combine(path, filename), asset);
        return asset;
    }

    private static AnimatorController CreateController(string filename, string path, Object[] clips)
    {
        if (clips == null)
        {
            EditorUtility.DisplayDialog("Error", "沒有动画信息", "OK");
            return null;
        }
        AnimationClip standclip = Utility.GetClip("stand", clips);
        if (!standclip)
        {
            EditorUtility.DisplayDialog("Error", "沒有stand动画", "OK");
            return null;
        }
        AnimatorController asset = new UnityEditor.Animations.AnimatorController();
        AnimatorControllerParameter Parameter = new AnimatorControllerParameter();
        Parameter.type = AnimatorControllerParameterType.Int;
        Parameter.name = Role.ParameterName;
        Parameter.defaultInt = 0;
        asset.AddParameter(Parameter);
        AssetDatabase.Refresh();
        FieldInfo[] fields = typeof(MotionState).GetFields();

        AnimatorControllerLayer layer0 = new AnimatorControllerLayer();
        layer0.stateMachine = new AnimatorStateMachine();
        layer0.name = "layer0";
        asset.AddLayer(layer0);
        AnimatorState runstate = layer0.stateMachine.AddState("run");
        AnimationClip runclip = Utility.GetClip("run", clips);
        runstate.motion = runclip as Motion;

        AnimatorControllerLayer layer = new AnimatorControllerLayer();
        layer.stateMachine = new AnimatorStateMachine();
        layer.stateMachine.states = new ChildAnimatorState[fields.Length];
        layer.name = "layer1";
        layer.blendingMode = AnimatorLayerBlendingMode.Override;
        layer.defaultWeight = 1;
        asset.AddLayer(layer);
        int i = 0;
        int j = fields.Length / 2;
        int x = 0;
        AnimatorState stand = layer.stateMachine.AddState("stand", new Vector3(500, 60 * (clips.Length / 2), 0));
        stand.motion = standclip as Motion;

        for (int c = 0; c < fields.Length; c++)
        {
            FieldInfo fi = fields[c];
            if (fi.FieldType != typeof(MotionState) || fi.Name == "stand")
                continue;
            AnimatorState anistate = layer.stateMachine.AddState(fi.Name, new Vector3(x, 60 * i, 0));
            AnimationClip clip = Utility.GetClip(fi.Name, clips);
            if (!clip)
            {
                layer.stateMachine.RemoveState(anistate);
                continue;
            }
            anistate.motion = clip as Motion;
            AnimatorStateTransition Transition = anistate.AddTransition(stand);
            Transition.AddCondition(AnimatorConditionMode.Equals, 0, Role.ParameterName);
            Transition.hasExitTime = true;

            int value = (int)Enum.Parse(typeof(MotionState), fi.Name);
            AnimatorStateTransition standTransition = stand.AddTransition(anistate);
            standTransition.AddCondition(AnimatorConditionMode.Equals, value, Role.ParameterName);
            standTransition.hasExitTime = false;
            j--;
            i++;
            if (j < 0)
            {
                j = fields.Length / 2;
                x += 1000;
                i = 0;
            }
        }
        string filePath = Path.Combine(path, filename);
        AssetDatabase.Refresh();
        CreateAssetWithUniqueName(filePath, asset);

        return asset;
    }

    [MenuItem("Assets/工具/控制器动画找回")]
    private static void FindAnimation()
    {
       if( !(Selection.activeObject is AnimatorOverrideController))
        {
            EditorUtility.DisplayDialog("错误", "请选择动作控制器", "OK");
            return;
        }
        AnimatorOverrideController controller = Selection.activeObject as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> Pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        controller.GetOverrides(Pairs);

        string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        Selection.activeObject = AssetDatabase.LoadAssetAtPath(rootpath, typeof(Object));
        Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);

        for (int i = 0; i < Pairs.Count; i++)
        {
            KeyValuePair<AnimationClip, AnimationClip> keyValue = Pairs[i];
            if (keyValue.Value)
                continue;
            Pairs[i] = new KeyValuePair<AnimationClip, AnimationClip>(keyValue.Key, Utility.GetClip(keyValue.Key.name, clips));
        }
        controller.ApplyOverrides(Pairs);

        Selection.activeObject = controller;
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/工具/Create/蒙皮合并")]
    private static void SkinCombine()
    {
        SkinnedMeshRenderer[] skinList = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        if (skinList.Length <= 1)
            return;

        List<CombineInstance> combineList = new List<CombineInstance>();
        List<Transform> boneList = new List<Transform>();

        GameObject combineobj = new GameObject("Combine_Model");
        combineobj.transform.parent = Selection.activeGameObject.transform;
        combineobj.transform.localPosition = Vector3.zero;
        combineobj.transform.localEulerAngles = Vector3.zero;
        combineobj.transform.localScale = Vector3.one;
        SkinnedMeshRenderer CombineSkin = combineobj.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

        CombineSkin.shadowCastingMode = ShadowCastingMode.Off;
        CombineSkin.receiveShadows = false;
        CombineSkin.skinnedMotionVectors = false;
        CombineSkin.lightProbeUsage = LightProbeUsage.Off;
        CombineSkin.reflectionProbeUsage = ReflectionProbeUsage.Off;
        CombineSkin.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

        Transform rootBone = null;
        for (int i = 0; i < skinList.Length; i++)
        {
            SkinnedMeshRenderer skinr = skinList[i];
            if (!rootBone)
                rootBone = skinr.rootBone;
            CombineInstance instance = new CombineInstance();
            instance.mesh = skinr.sharedMesh;
            combineList.Add(instance);
            boneList.AddRange(skinr.bones);
        }
        Mesh combineMesh = new Mesh();
        combineMesh.CombineMeshes(combineList.ToArray(), true, false);

        CombineSkin.rootBone = rootBone;
        CombineSkin.sharedMesh = combineMesh;
        CombineSkin.bones = boneList.ToArray();
        CombineSkin.sharedMaterial = skinList[0].sharedMaterial;

        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(skinList[0].sharedMesh));
        path = Path.Combine(path, Selection.activeGameObject.name + ".asset");

        CreateAssetWithUniqueName(path, combineMesh);

         for (int i = 0; i < skinList.Length; i++)
         {
             UnityEngine.Object.DestroyImmediate(skinList[i].gameObject, true);
         }
    }

    #endregion

    //资源打包优化
    //-------------------------------------------------------------------------------------------
    [MenuItem("Assets/工具/CreateEquip/FromModel")]
    private static void CreateEquipFromModel()
    {
        string path = EditorUtility.SaveFilePanelInProject("Build Asset", Selection.activeObject.name, "","", Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject)));
        CreateEquip(Selection.activeObject, Path.GetDirectoryName(path));
    }

    [MenuItem("Assets/工具/CreateEquip/头发")]
    private static void CreateHair()
    {
        string path = EditorUtility.SaveFilePanelInProject("Build Asset", Selection.activeObject.name, "", "", Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject)));
        CreateEquip(Selection.activeObject, Path.GetDirectoryName(path), "guadian_hair");
    }

    [MenuItem("Assets/工具/CreateEquip/Empty")]
    private static void CreateEmptyEquip()
    {
        string folder = "Assets";
        string filePath = string.Empty;
        Equip asset = ScriptableObject.CreateInstance<Equip>();
        folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        filePath = Path.Combine(folder, "equip_.asset");
        AssetExporter.CreateAssetWithUniqueName(filePath, asset);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(filePath);
    }

    private static string CreateEquip(Object go, string modelfolder = "",string parent="", bool build = false)
    {
        string folder = "Assets";
        string filePath = string.Empty;
        Equip asset = null;
        if (go is GameObject)
        {
            GameObject equip = go as GameObject;
            AssetExporter.GetShaders();
            AssetExporter.GetPublicAssets();
            Object model = CreateModel(go, modelfolder);
            Renderer render = equip.GetComponent<Renderer>();
            if (!render)
                render = equip.GetComponentInChildren<Renderer>();
            if (render)
            {
                render.shadowCastingMode = ShadowCastingMode.Off;
                render.receiveShadows = false;
                render.lightProbeUsage = LightProbeUsage.Off;
                render.reflectionProbeUsage = ReflectionProbeUsage.Off;
                asset = ScriptableObject.CreateInstance<Equip>();
                folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(model));
                asset.EquipName = string.Format("equip_{0}", equip.name);
                filePath = Path.Combine(folder, asset.EquipName + ".asset");
                List<string> bones = new List<string>();
                if (render is SkinnedMeshRenderer)
                {
                    SkinnedMeshRenderer skin = render as SkinnedMeshRenderer;
                    for (int i = 0; i < skin.bones.Length; i++)
                        bones.Add(skin.bones[i].name);
                    asset.Bones = bones.ToArray();
                    asset.RootBone = string.IsNullOrEmpty(parent) ? skin.rootBone.name : parent;
                }
                else
                {
                    asset.Bones = null;
                    asset.RootBone = string.Empty;
                    asset.ParentBone = parent;
                }
                if (asset.EquipName.Contains("wp"))
                    asset.Mask = 2;
                if (asset.EquipName.Contains("hair"))
                    asset.Mask = 3;
                if (asset.EquipName.Contains("face"))
                    asset.Mask = 4;
                asset.ModelPath = Path.ChangeExtension(AssetDatabase.GetAssetPath(model), ".model").ToLower().Remove(0, "assets/".Length);
                AssetExporter.CreateAssetWithUniqueName(filePath, asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Object equipasset = AssetDatabase.LoadAssetAtPath<Object>(filePath);
                AssetExporter.SetAssetBundleNameAndVariant(equipasset);
                if (build)
                    AssetExporter.BuildAssetBundles(equipasset);
                else
                    Selection.activeObject = equipasset;
                return filePath;
            }
        }

        EditorUtility.DisplayDialog("错误", "请选择模型", "OK");
        return string.Empty;
    }

    private static Object CreateModel(Object asset,string folder="")
    {
        GameObject characterClone = (GameObject)Object.Instantiate(asset);
        if (string.IsNullOrEmpty(folder))
            folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset));
        string path = string.Empty;
        path = string.Format("{0}/model_{1}.prefab", folder , asset.name);
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(path);
        tempPrefab = PrefabUtility.ReplacePrefab(characterClone, tempPrefab);
        Object.DestroyImmediate(characterClone);

        Renderer renderer = (tempPrefab as GameObject).GetComponent<Renderer>();
        if (!renderer)
            renderer = (tempPrefab as GameObject).GetComponentInChildren<Renderer>();

        if(renderer is SkinnedMeshRenderer)
        {
            SkinnedMeshRenderer skin = renderer as SkinnedMeshRenderer;
            Mesh skinmesh = skin.sharedMesh;
            Material skinmat = skin.sharedMaterial;

            UnityEngine.Object.DestroyImmediate(skin, true);
            skin = (tempPrefab as GameObject).AddComponent<SkinnedMeshRenderer>();
            skin.sharedMesh = skinmesh;
            skin.sharedMaterial = skinmat;
            renderer = skin;
        }

        Material mat = renderer.sharedMaterial;
        ResetPlayerMat(mat, tempPrefab.name);

        AssetExporter.SetAssetBundleNameAndVariant(tempPrefab);
        AssetExporter.BuildAssetBundles(tempPrefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return tempPrefab;
    }

    private static void ResetPlayerMat(Material mat,string prefab,string sd= "MOYU/Player")
    {
        if (mat.shader.name != sd)
        {
            Texture tex = mat.mainTexture;
            mat.shader = Shader.Find(sd);
            mat.SetTexture("_MainTex", tex);
            mat.SetTexture("_MatCap", Resources.Load("public/bianyuanguang") as Texture);
            mat.SetTexture("_Ramp", Resources.Load("public/rimT") as Texture);
            mat.SetFloat("_MatCapPower", 0.5f);
            mat.SetFloat("_Shininess", 0.092f);
            mat.SetFloat("_Gloss", 1.17f);
            mat.SetColor("_Color", Color.white);
            mat.SetColor("_SpecColor", new Color32(197, 217, 255, 255));
        }
    }

    [MenuItem("Assets/工具/创建Action")]
    private static void CreateAction()
    {
        string folder = "Assets";
        string filePath = string.Empty;
        Model.Action asset = ScriptableObject.CreateInstance<Model.Action>();
        folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        filePath = Path.Combine(folder, "act_.asset");
        AssetExporter.CreateAssetWithUniqueName(filePath, asset);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(filePath);
    }

    [MenuItem("Assets/工具/导出model")]
    private static void CreateModel()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }
        Selection.activeObject = CreateModel(Selection.activeObject);
    }


    [MenuItem("Assets/工具/一键导出/主角")]
    private static void OneKeyCreatePlayer()
    {
        if (!Selection.activeObject)
            return;
        int value;
        if (!int.TryParse(Selection.activeObject.name, out value))
        {
            EditorUtility.DisplayDialog("错误", "role的名字必须是数字", "OK");
            return;
        }
        if(!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }

        string path = EditorUtility.SaveFilePanelInProject("Build Asset", Selection.activeObject.name, "", "", Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject)));
        string folder = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(folder))
            return;
        Object oldasset = Selection.activeObject;
        GameObject go = Object.Instantiate(Selection.activeObject) as GameObject;
        string newpath = string.Format("{0}/{1}.prefab", folder, Selection.activeObject.name);
        AssetDatabase.DeleteAsset(newpath);

        go.name = Selection.activeObject.name;
        Object role = AssetExporter.CreateRole(go);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(role), newpath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        GameObject newrole = AssetDatabase.LoadAssetAtPath<GameObject>(newpath);
        Role config = newrole.GetComponent<Role>();
        List<BaseLook> equiplist = new List<BaseLook>();
        Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(oldasset));
        int i = 0;
        foreach (var obj in subAssets)
        {
            if (!(obj is GameObject))
                continue;
            GameObject model = obj as GameObject;
            if (!model.GetComponent<Renderer>())
                continue;
            String equippath = CreateEquip(obj, folder, "", true);
            equippath = Path.ChangeExtension(equippath, ".equip").ToLower().Remove(0, "assets/".Length);
            BaseLook baseLook = new BaseLook();
            baseLook.Equip = equippath;
            equiplist.Add(baseLook);
            i++;
        }
        config = newrole.GetComponent<Role>();
        config.Equips = equiplist.ToArray();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    [MenuItem("Assets/工具/一键导出/怪物，NPC.../整体")]
    private static Role OneKeyCreateMonster()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return null;
        }
        GameObject go = Object.Instantiate(Selection.activeObject) as GameObject;
        go.name = Selection.activeObject.name;

        Animator animator = go.GetComponent<Animator>();
        Avatar avatar = animator.avatar;

        Role role = Utility.AddComponent<Role>(go);
        if (!role.BoxCollider)
        {
            role.BoxCollider = new GameObject(AssetExporter.Boundname);
            BoxCollider aBoxCollider = role.BoxCollider.AddComponent<BoxCollider>();
            aBoxCollider.isTrigger = true;
            role.BoxCollider.transform.parent = go.transform;
            role.BoxCollider.transform.localPosition = Vector3.zero;
            role.BoxCollider.transform.localRotation = Quaternion.identity;
            role.BoxCollider.transform.localScale = Vector3.one;
        }

        Transform[] bones = go.GetComponentsInChildren<Transform>(true);
        List<GameObject> golist = new List<GameObject>();
        for (int i = 0; i < bones.Length; i++)
            golist.Add(bones[i].gameObject);

        role.Bones = golist.ToArray();

        string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

        Selection.activeObject = AssetDatabase.LoadAssetAtPath(rootpath, typeof(Object));
        Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);

        animator.runtimeAnimatorController = AssetExporter.CreateOverrideController(string.Format("{0}.controller", go.name), rootpath, clips);

        string Prefabpath = rootpath + "/" + go.name + ".prefab";
        AssetDatabase.DeleteAsset(Prefabpath);
        AssetDatabase.Refresh();

        SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach(SkinnedMeshRenderer smr in renderers)
            ResetPlayerMat(smr.sharedMaterial, smr.name, "MOYU/Player");

        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(Prefabpath);
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);

        Selection.activeObject = AssetDatabase.LoadAssetAtPath(Prefabpath, typeof(GameObject));
        Object.DestroyImmediate(go);

        AssetExporter.SetAssetBundleNameAndVariant(tempPrefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return (Selection.activeObject as GameObject).GetComponent<Role>();
    }

    [MenuItem("Assets/工具/一键导出/怪物，NPC.../模型拆分")]
    private static void OneKeyCreateMonsterNoMesh()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }
        Role role = OneKeyCreateMonster();
        SkinnedMeshRenderer smr = role.gameObject.GetComponent<SkinnedMeshRenderer>();
        if (!smr)
            smr = role.gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true);

        if (!smr)
            return;

        role.Skin = smr;
        smr.sharedMesh = null;
    }

    [MenuItem("Assets/工具/一键导出/怪物，NPC.../贴图拆分")]
    private static void OneKeyCreateMonsterNoTex()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }
        Role role = OneKeyCreateMonster();
        SkinnedMeshRenderer smr = role.gameObject.GetComponent<SkinnedMeshRenderer>();
        if (!smr)
            smr = role.gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true);

        if (!smr)
            return;

        role.Skin = smr;
        smr.sharedMaterial.mainTexture = null;
    }

    [MenuItem("Assets/工具/一键导出/地图物件/整体")]
    private static Role OneKeyCreateMapObj()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return null;
        }
        GameObject go = Object.Instantiate(Selection.activeObject) as GameObject;
        go.name = Selection.activeObject.name;

        Role role = Utility.AddComponent<Role>(go);
        if (!role.BoxCollider)
        {
            role.BoxCollider = new GameObject(AssetExporter.Boundname);
            BoxCollider aBoxCollider = role.BoxCollider.AddComponent<BoxCollider>();
            aBoxCollider.isTrigger = true;
            role.BoxCollider.transform.parent = go.transform;
            role.BoxCollider.transform.localPosition = Vector3.zero;
            role.BoxCollider.transform.localRotation = Quaternion.identity;
            role.BoxCollider.transform.localScale = Vector3.one;
        }

        Transform[] bones = go.GetComponentsInChildren<Transform>(true);
        List<GameObject> golist = new List<GameObject>();
        for (int i = 0; i < bones.Length; i++)
            golist.Add(bones[i].gameObject);

        role.Bones = golist.ToArray();

        string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

        string Prefabpath = rootpath + "/" + go.name + ".prefab";
        AssetDatabase.DeleteAsset(Prefabpath);
        AssetDatabase.Refresh();

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer smr in renderers)
            ResetPlayerMat(smr.sharedMaterial, smr.name, "MOYU/Player");

        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(Prefabpath);
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);

        Selection.activeObject = AssetDatabase.LoadAssetAtPath(Prefabpath, typeof(GameObject));
        Object.DestroyImmediate(go);

        AssetExporter.SetAssetBundleNameAndVariant(tempPrefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return (Selection.activeObject as GameObject).GetComponent<Role>();
    }

    [MenuItem("Assets/工具/一键导出/地图物件/模型拆分")]
    private static void OneKeyCreateMapObjNoMesh()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }
        Role role = OneKeyCreateMapObj();
        MeshFilter Filter = role.gameObject.GetComponent<MeshFilter>();
        if (!Filter)
            Filter = role.gameObject.GetComponentInChildren<MeshFilter>(true);

        if (!Filter)
            return;

        role.Filter = Filter;
        Filter.sharedMesh = null;
    }

    [MenuItem("Assets/工具/一键导出/地图物件/贴图拆分")]
    private static void OneKeyCreateMapObjNoTex()
    {
        if (!AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith("FBX"))
        {
            EditorUtility.DisplayDialog("错误", "资源必须是FBX", "OK");
            return;
        }
        Role role = OneKeyCreateMapObj();
        MeshFilter Filter = role.gameObject.GetComponent<MeshFilter>();
        if (!Filter)
            Filter = role.gameObject.GetComponentInChildren<MeshFilter>(true);

        if (!Filter)
            return;
        Filter.GetComponent<Renderer>().sharedMaterial.mainTexture = null;
        role.Filter = Filter;
    }

    private static string SaveMesh(Mesh mesh)
    {
        if (!mesh)
        {
            EditorUtility.DisplayDialog("错误", "模型丢失", "OK");
            return string.Empty;
        }
        List<CombineInstance> combineList = new List<CombineInstance>();
        CombineInstance instance = new CombineInstance();
        instance.mesh = mesh;
        combineList.Add(instance);

        Mesh combineMesh = new Mesh();
        combineMesh.CombineMeshes(combineList.ToArray(), true, false);

        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        path = Path.Combine(path, "mesh_" + mesh.name + ".asset");
        AssetExporter.CreateAssetWithUniqueName(path, combineMesh);
        AssetExporter.SetAssetBundleNameAndVariant(AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)));
        return path;
    }

    [MenuItem("Assets/工具/模型提取")]
    private static void CloneMesh()
    {
        if (!(Selection.activeObject is Mesh))
        {
            EditorUtility.DisplayDialog("错误", "请选择模型", "OK");
            return;
        }
        List<CombineInstance> combineList = new List<CombineInstance>();
        CombineInstance instance = new CombineInstance();
        instance.mesh = Selection.activeObject as Mesh;
        combineList.Add(instance);

        Mesh combineMesh = new Mesh();
        combineMesh.CombineMeshes(combineList.ToArray(), true, false);

        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        path = Path.Combine(path, Selection.activeObject.name + ".asset");
        AssetExporter.CreateAssetWithUniqueName(path, combineMesh);
        AssetExporter.SetAssetBundleNameAndVariant(AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)));
    }

    [MenuItem("Assets/工具/给Role添加位移动画")]
    private static void AddAnimationToRole()
    {
        if(!(Selection.activeObject is GameObject))
        {
            EditorUtility.DisplayDialog("错误", "请选择role", "OK");
            return;
        }
        GameObject go = Selection.activeObject as GameObject;
        Role role = go.GetComponent<Role>();
        if (!role)
        {
            EditorUtility.DisplayDialog("错误", "请选择role", "OK");
            return;
        }

        GameObject clone = Object.Instantiate(Selection.activeObject) as GameObject;
        clone.name = Selection.activeObject.name;
        role = clone.GetComponent<Role>();
        PrefabUtility.ReplacePrefab(clone, Selection.activeObject);
        Object.DestroyImmediate(clone);
    }

    [MenuItem("Assets/工具/一键导出/动画控制器")]
    static void BuildControllers()
    {
        GetControllers();

        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = "controllers";
        ab.assetBundleVariant = "ctrl";
        List<string> ctrlList = new List<string>();
        for (int i = 0; i < Controllers.Count; i++)
        {
            string path = AssetDatabase.GetAssetPath(Controllers[i]);
            AssetImporter importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = ab.assetBundleName;
            importer.assetBundleVariant = ab.assetBundleVariant;
            ctrlList.Add(path);
        }
        AssetDatabase.Refresh();
        ab.assetNames = ctrlList.ToArray();
        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        buildMap.Add(ab);
        string outputPath = "Assets/StreamingAssets/res";
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/动画/优化/高")]
    static void OptimizeAllAniH()
    {
        EditorUtility.DisplayProgressBar("查找动画", "Searching Assets", 0.01f);
        Object[] Assets = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("查找动画", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            if (asset is AnimationClip)
                OptimizeSigleAni(asset as AnimationClip, true);
            Debug.Log(Assets.Length);
            i++;
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("", "动画优化完成", "确定");
    }

    [MenuItem("Assets/动画/优化/低")]
    static void OptimizeAllAniLow()
    {
        EditorUtility.DisplayProgressBar("查找动画", "Searching Assets", 0.01f);
        Object[] Assets = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("查找动画", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            if (asset is AnimationClip)
                OptimizeSigleAni(asset as AnimationClip, false);
            Debug.Log(Assets.Length);
            i++;
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("", "动画优化完成", "确定");
    }

    static void OptimizeSigleAni(AnimationClip clip,bool scale)
    {
        EditorCurveBinding[] clipdatas = AnimationUtility.GetCurveBindings(clip);
        List<EditorCurveBinding> newdatas = new List<EditorCurveBinding>();
        List<AnimationCurve> curves = new List<AnimationCurve>();
        List<int> needRemove = new List<int>();
        bool delete = true;
        for (int i = 0; i < clipdatas.Length; i++)
        {
            delete = true;
            EditorCurveBinding data = clipdatas[i];
            AnimationCurve Curve = AnimationUtility.GetEditorCurve(clip, data);
            if (data.propertyName.Contains("m_LocalScale")&& scale)
            {
                for (int j = 0; j < Curve.length; j++)
                {
                    if (Mathf.Abs(Curve[j].value) <= 0.95f || Mathf.Abs(Curve[j].value) >= 1.05f)
                    {
                        delete = false;
                        needRemove.Clear();
                        break;
                    }
                }
                if (delete)
                    needRemove.Add(i);
            }
        }
        for (int i = 0; i < clipdatas.Length; i++)
        {
            EditorCurveBinding data = clipdatas[i];
            AnimationCurve Curve = AnimationUtility.GetEditorCurve(clip, data);
            if (data.propertyName.Contains("Nub"))
                continue;
            if (data.propertyName.Contains("m_LocalScale") && needRemove.Contains(i))
                continue;
            List<Keyframe> kfs = new List<Keyframe>();
            for (int j = 0; j < Curve.length; j++)
            {
                Keyframe kf = Curve[j];
                kf.value = float.Parse(kf.value.ToString("f3"));
                kf.inTangent = float.Parse(kf.inTangent.ToString("f3"));
                kf.outTangent = float.Parse(kf.outTangent.ToString("f3"));
                kfs.Add(kf);
            }
            Curve = new AnimationCurve(kfs.ToArray());
            curves.Add(Curve);
            newdatas.Add(data);
        }
        clip.ClearCurves();
        for (int i = 0; i < newdatas.Count; i++)
            clip.SetCurve(clipdatas[i].path, clipdatas[i].type, clipdatas[i].propertyName, curves[i]);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/动画/合并")]
    static void CombineAni()
    {
        if (LogAniDesc() <= 1)
        {
            Debug.Log("不需要合并动画");
            return;
        }

        List<EditorCurveBinding> clipdatas = new List<EditorCurveBinding>();
        List<AnimationCurve> curves = new List<AnimationCurve>();

        if (!Directory.Exists("Assets/EffectAnimation"))
            Directory.CreateDirectory("Assets/EffectAnimation");

        Animator[] animators = Selection.activeGameObject.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            Animator anim = animators[i];
            Transform anitf = anim.transform;
            List<AnimationClip> Clips = new List<AnimationClip>();
            foreach (AnimationClip c in anim.runtimeAnimatorController.animationClips)
            {
                if (!c)
                    Debug.LogError(Selection.activeGameObject.name + "/" + anim.gameObject.name + "是一个无用的空动画，将删除该动画组件");

                AnimationClip newclip = ChangeSigleAni(c, GetPath(anitf), anim.gameObject);
                if (newclip)
                    Clips.Add(newclip);
            }
            UnityEngine.Object.DestroyImmediate(anim, true);

            if (hasDelay(anitf))
                continue;
            foreach (AnimationClip c in Clips)
            {
                CombineSigleAni(c, clipdatas, curves, GetPath(anitf), anitf.gameObject);
            }

        }

        Animation[] animations = Selection.activeGameObject.GetComponentsInChildren<Animation>();
        for (int i = 0; i < animations.Length; i++)
        {
            Animation anim = animations[i];
            if (hasDelay(anim.transform))
                continue;
            if (!anim.clip)
                Debug.LogError(Selection.activeGameObject.name + "/" + anim.gameObject.name + "是一个无用的空动画，将删除该动画组件");
            else
                CombineSigleAni(anim.clip, clipdatas, curves, GetPath(anim.transform), anim.gameObject);
            UnityEngine.Object.DestroyImmediate(anim, true);
        }

        AnimationClip clip = new AnimationClip();
        for (int i = 0; i < clipdatas.Count; i++)
            clip.SetCurve(clipdatas[i].path, clipdatas[i].type, clipdatas[i].propertyName, curves[i]);
        clip.legacy = true;
        AssetDatabase.CreateAsset(clip, string.Format("Assets/EffectAnimation/{0}.anim", Selection.activeObject.name));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Animation animation = Selection.activeGameObject.GetComponent<Animation>();
        if (!animation)
            animation = Selection.activeGameObject.AddComponent<Animation>();
        animation.clip = clip;
    }

    static string GetPath(Transform transform)
    {
        string str = transform.name;
        while (transform.parent)
        {
            transform = transform.parent;
            str = transform.name + "/" + str;
        }
        return str.Replace(transform.root.name + "/", string.Empty);
    }

    static void CombineSigleAni(AnimationClip clip, List<EditorCurveBinding> clipdatas, List<AnimationCurve> curves, string name, GameObject go)
    {
        EditorCurveBinding[] datas = AnimationUtility.GetCurveBindings(clip);
        for (int i = 0; i < datas.Length; i++)
        {
            EditorCurveBinding data = datas[i];
            AnimationCurve Curve = AnimationUtility.GetEditorCurve(clip, data);
            List<Keyframe> kfs = new List<Keyframe>();
            for (int j = 0; j < Curve.length; j++)
            {
                Keyframe kf = Curve[j];
                kfs.Add(kf);
            }
            Curve = new AnimationCurve(kfs.ToArray());
            curves.Add(Curve);

        }
        for (int i = 0; i < datas.Length; i++)
        {
            if (string.IsNullOrEmpty(datas[i].path))
                datas[i].path = name;
            else if (go.transform == null || go.transform == Selection.activeGameObject.transform)
                datas[i].path = datas[i].path;
            else
                datas[i].path = name + "/" + datas[i].path;
        }
        clipdatas.AddRange(datas);
    }

    [MenuItem("Assets/优化动画/动画使用详情")]
    static int LogAniDesc()
    {
        int count = 0;
        string str = string.Empty;
        Animator[] animators = Selection.activeGameObject.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            Animator anim = animators[i];
            if (hasDelay(anim.transform))
            {
                str += string.Format("{0} 包含Delay + 动画 \n", anim.name);
            }
            else
            {
                count++;
                str += string.Format("{0} 包含动画 \n", anim.name);
            }
        }

        Animation[] animations = Selection.activeGameObject.GetComponentsInChildren<Animation>();
        for (int i = 0; i < animations.Length; i++)
        {
            Animation anim = animations[i];
            if (hasDelay(anim.transform))
            {
                str += string.Format("{0} 包含Delay + 动画 \n", anim.name);
            }
            else
            {
                count++;
                str += string.Format("{0} 包含动画 \n", anim.name);
            }
        }
        Debug.Log(string.Format("可以合并的动画数量是{0}， 具体如下：", count));
        if (!string.IsNullOrEmpty(str))
            Debug.Log(str);
        return count;
    }

    static bool hasDelay(Transform transform)
    {
        while (transform.parent)
        {
            transform = transform.parent;
            if (transform.gameObject.GetComponent<Delay>())
                return true;
        }
        return false;
    }

    [MenuItem("Assets/动画/转化成Animation")]
    static void ChangeAnimation()
    {
        if (!Directory.Exists("Assets/EffectAnimation"))
            Directory.CreateDirectory("Assets/EffectAnimation");

        Animator[] animators = Selection.activeGameObject.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            Animator anim = animators[i];
            if (hasDelay(anim.transform))
                continue;
            AnimationClip[] Clips = anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip c in Clips)
                ChangeSigleAni(c, GetPath(anim.transform), anim.gameObject);
            UnityEngine.Object.DestroyImmediate(anim, true);
        }
    }

    static AnimationClip ChangeSigleAni(AnimationClip clip, string name, GameObject go)
    {
        if (!clip)
            return null;
        List<EditorCurveBinding> clipdatas = new List<EditorCurveBinding>();
        List<AnimationCurve> curves = new List<AnimationCurve>();

        EditorCurveBinding[] datas = AnimationUtility.GetCurveBindings(clip);
        for (int i = 0; i < datas.Length; i++)
        {
            EditorCurveBinding data = datas[i];
            AnimationCurve Curve = AnimationUtility.GetEditorCurve(clip, data);
            List<Keyframe> kfs = new List<Keyframe>();
            for (int j = 0; j < Curve.length; j++)
            {
                Keyframe kf = Curve[j];
                kfs.Add(kf);
            }
            Curve = new AnimationCurve(kfs.ToArray());
            curves.Add(Curve);

        }
        for (int i = 0; i < datas.Length; i++)
        {
            if (string.IsNullOrEmpty(datas[i].path))
                datas[i].path = name;
            else
                datas[i].path = datas[i].path;
        }
        clipdatas.AddRange(datas);

        AnimationClip newclip = new AnimationClip();
        for (int i = 0; i < clipdatas.Count; i++)
            newclip.SetCurve(clipdatas[i].path, clipdatas[i].type, clipdatas[i].propertyName, curves[i]);
        newclip.legacy = true;
        AssetDatabase.CreateAsset(newclip, string.Format("Assets/EffectAnimation/{0}.anim", clip.name));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Animation animation = go.GetComponent<Animation>();
        if (!animation)
            animation = go.AddComponent<Animation>();
        animation.AddClip(newclip, clip.name);
        animation.clip = newclip;
        return newclip;
    }

    [MenuItem("Assets/工具/一键导出/Model动画")]
    public static void GetHairAnis()
    {
        Animation ani = Selection.activeGameObject.GetComponent<Animation>();
        if (!ani)
        {
            EditorUtility.DisplayDialog("", "请选择有动画的model", "OK");
            return;
        }
        string folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        Selection.activeObject = AssetDatabase.LoadAssetAtPath(folder, typeof(Object));
        Object[] fbxs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object asset in fbxs)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer is ModelImporter)
            {
                ModelImporter modelImporter = importer as ModelImporter;
                modelImporter.animationType = ModelImporterAnimationType.Legacy;
                modelImporter.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
                importer.SaveAndReimport();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                AnimationClip Asset = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                
                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(Asset, newClip);
                AssetDatabase.CreateAsset(newClip, string.Format("{0}/{1}.anim", folder, newClip.name));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                OptimizeSigleAni(newClip, true);
                ani.AddClip(newClip, newClip.name);
            }
        }
    }

    [MenuItem("Assets/工具/一键优化model")]
    public static void OptimizeModels()
    {
        Object[] models = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in models)
        {
            if ((asset is GameObject) && Utility.isModel(asset))
            {
                GameObject model = asset as GameObject;
                SkinnedMeshRenderer skin = model.GetComponent<SkinnedMeshRenderer>();
                Mesh skinmesh = skin.sharedMesh;
                Material skinmat = skin.sharedMaterial;
                UnityEngine.Object.DestroyImmediate(skin, true);
                skin = model.AddComponent<SkinnedMeshRenderer>();
                skin.sharedMesh = skinmesh;
                skin.sharedMaterial = skinmat;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                AssetExporter.BuildAssetBundles(asset);
            }
        }
    }

    [MenuItem("Assets/工具/Model和Role通道分离")]
    public static void PreFabSpliteRGBA()
    {
        Object[] models = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (GameObject go in models)
        {
            Renderer[] renders = go.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renders)
            {
                if (r.sharedMaterial.HasProperty("_AlphaTex"))
                {
                    r.sharedMaterial.SetTexture("_AlphaTex", CreateAlphaTex(r.sharedMaterial.mainTexture));
                }
            }
        }
    }


    [MenuItem("Assets/工具/贴图通道分离")]
    public static void SpliteRGBA()
    {
        Object[] models = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        foreach (Texture tex in models)
        {
           CreateAlphaTex(tex);
        }
    }

    public static Texture CreateAlphaTex(Texture obj)
    {
        Texture2D tex = (Texture2D)obj;
        if (!tex)
            return null;
        string path = AssetDatabase.GetAssetPath(obj);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (!importer.DoesSourceTextureHaveAlpha())
        {
            EditorUtility.DisplayDialog("", obj.name + "没有通道", "OK");
            return null;
        }


        importer.isReadable = true;
        importer.SaveAndReimport();
        AssetDatabase.Refresh();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        //保存alpha通道 left to right, bottom to top
        Color32[] colorArray = tex.GetPixels32();

        for (int i = 0; i < colorArray.Length; ++i)
        {
            colorArray[i].r = colorArray[i].a;
            colorArray[i].g = colorArray[i].a;
            colorArray[i].b = colorArray[i].a;
        }
        Texture2D alphaTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false, true);
        alphaTex.SetPixels32(colorArray);
        alphaTex.Apply();
        byte[] data = alphaTex.EncodeToPNG();

        string alphaTexPath = path;
        int index = alphaTexPath.LastIndexOf('.');
        alphaTexPath = alphaTexPath.Insert(index, "_alpha");
        if (File.Exists(alphaTexPath))
        {
            AssetDatabase.DeleteAsset(alphaTexPath);
            AssetDatabase.SaveAssets();
        }
        File.WriteAllBytes(alphaTexPath, data);
        data = null;

        //保存rgb
        Texture2D rgbTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
        Color32[] newColorArray = tex.GetPixels32();
        rgbTex.SetPixels32(newColorArray);
        rgbTex.Apply();
        byte[] rgbData = rgbTex.EncodeToPNG();

        string rgbTexPath = path;
        File.WriteAllBytes(rgbTexPath, rgbData);
        rgbData = null;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.ImportAsset(path);
        AssetDatabase.ImportAsset(alphaTexPath);

        CompressAsset(AssetDatabase.LoadAssetAtPath<Texture>(alphaTexPath));
        CompressAsset(AssetDatabase.LoadAssetAtPath<Texture>(rgbTexPath));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        return AssetDatabase.LoadAssetAtPath<Texture>(alphaTexPath);
    }
}