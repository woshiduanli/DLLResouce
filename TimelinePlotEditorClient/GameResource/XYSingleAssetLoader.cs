using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

#region 异步加载数据对象

public class AsyncLoadRequest
{
    public delegate void AsyncLoadCallBack(object context, Object res);

    public AsyncLoadRequest(AsyncLoadCallBack vCB, System.Object vContex, bool preload)
    {
        isPreload = preload;
        callback = vCB;
        contex = vContex;
    }

    public AsyncLoadRequest(AsyncLoadCallBack vCB, System.Object vContex)
    {
        isPreload = false;
        callback = vCB;
        contex = vContex;
    }

    public AsyncLoadCallBack callback { get; private set; }
    public System.Object contex { get; private set; }
    public bool isPreload { get; private set; }

    public bool isDone { get; private set; }

    public Object asset { get; private set; }

    public void LoadFinish(Object asset)
    {
        isDone = true;
        this.asset = asset;
        if (callback != null)
        {
            callback(contex, this.asset);
        }
    }
}

#endregion

public class XYSingleAssetLoader : MonoBehaviour
{
    private static XYSingleAssetLoader Instance_;

    public static XYSingleAssetLoader Instance
    {
        get { return Instance_; }
    }

    #region 数据定义

    private class SameRequestCache
    {
        public readonly List<AsyncLoadRequest> srcList = new List<AsyncLoadRequest>();
    }

    private readonly Dictionary<string, SameRequestCache> sameRequestCache_ = new Dictionary<string, SameRequestCache>();
    //private static string cacheLock_ = "nothing";

    private class LoadQueueData
    {
        public readonly string resPath;
        public bool isDone = false;
        public LoadQueueData(string vResPath)
        {
            resPath = vResPath;
        }
    }

    private readonly List<LoadQueueData> requestQueue_ = new List<LoadQueueData>();
    private int working_;
    //static List<LoadQueueData> loadedQueue_ = new List<LoadQueueData>();

    #endregion

    /// <summary>
    ///     初始化加载器
    ///     只有一个实例
    /// </summary>
    public static void Init()
    {
        if (!Instance_)
        {
            var go = new GameObject("AssetLoader");
            DontDestroyOnLoad(go);
            go.AddComponent<XYSingleAssetLoader>();
        }
    }

    void Destroy()
    {
        UnityEngine.Object.Destroy(this.gameObject);
        Instance_ = null;
    }

    #region Awake

    private void Awake()
    {
        Instance_ = this;
        /*Thread t = new Thread( LoadingThread );
        t.Priority = System.Threading.ThreadPriority.BelowNormal;
        t.Start();
        this.StartCoroutine( CreateAssetFromMemory() );*/
    }

    #endregion

    #region update

    /// <summary>
    ///     这里针对微端做了特殊处理
    ///     因为下载中的资源可能会妨碍已在本地的资源的加载
    ///     所以针对这种情况做了特殊处理，陈俊，2012/5/30
    /// </summary>
    private void HandleLoadQueue()
    {
        if (requestQueue_.Count > 0)
        {
            if (working_ < 3)
            {
                StartCoroutine(CreateFromWWW(requestQueue_[0]));
                requestQueue_.RemoveAt(0);
            }
        }
    }

    private void Update()
    {
        HandleLoadQueue();
    }

    public int GetLoadingQueueCount()
    {
        return requestQueue_.Count + working_;
    }


    #endregion

    #region WWW的方式加载

    private IEnumerator CreateFromWWW(LoadQueueData queueData)
    {
        working_++;
        yield return null; 
        string path =string.Empty;
        using (var www = new WWW(XYMisc.EncodeLocalFileURL(XYDirectory.Res + queueData.resPath)))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
            }

            AssetBundle ab = www.assetBundle;
            UnityEngine.Object mainAssetAsset = ab.mainAsset;
            if (ab && mainAssetAsset)
			{
                OnAssetLoaded(queueData.resPath, ab, mainAssetAsset);
                ab.Unload(false);
            }
            else
            {
                OnAssetLoaded(queueData.resPath, null, mainAssetAsset);
            }
        }
        working_--;
    }

    #endregion

    #region 资源加载成功处理

    private void OnAssetLoaded(string lowerResPath, AssetBundle ab, UnityEngine.Object asset)
    {
        SameRequestCache src;
        bool isPreload = false;
        if (sameRequestCache_.TryGetValue(lowerResPath, out src))
        {
            try
            {
                foreach (AsyncLoadRequest request in src.srcList)
                {
                    isPreload = isPreload | request.isPreload;
                    Callback(lowerResPath, request, asset);
                }
                src.srcList.Clear();
                sameRequestCache_.Remove(lowerResPath);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.StackTrace);
                Debug.Log("Execption in OnAssetLoaded*********");
                Debug.Log(ex.ToString());
            }
        }
        if (ab && asset)
        {
            ResourceManager.Add(lowerResPath, ab, asset, isPreload);
        }
    }

    #endregion

    #region 遍历子节点的MATERIAL ,并且替换成本地的shader

    private static void OnEachGameObject(GameObject go)
    {
        var pr = go.GetComponent<ParticleRenderer>();
        var tr = go.GetComponent<TrailRenderer>();
        try
        {
            go.layer = XYDefines.Layer.Effect;
            if (tr)
            {
                for (int i = 0; i < tr.sharedMaterials.Length; i++)
                {
                    if (tr.sharedMaterials[i])
                    {
                        ReplaceMaterialShader(tr.sharedMaterials[i]);
                        tr.castShadows = false;
                        tr.receiveShadows = false;
                    }
                }
            }
            if (pr)
            {
                if (pr.sharedMaterials.Length <= 0)
                {
                    Debug.Log("sharedMaterials <= 0");
                }
                for (int i = 0; i < pr.sharedMaterials.Length; i++)
                {
                    if (pr.sharedMaterials[i])
                    {
                        ReplaceMaterialShader(pr.sharedMaterials[i]);
                        pr.castShadows = false;
                        pr.receiveShadows = false;
                    }
                }
            }
            if (go.GetComponent<Renderer>()) //替换SHADER
            {
                Renderer renderer = go.GetComponent<Renderer>();
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (renderer.sharedMaterials[i])
                    {
                        ReplaceMaterialShader(renderer.sharedMaterials[i]);
                        renderer.castShadows = false;
                        renderer.receiveShadows = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
    }

    private static void ReplaceMaterialShader(Material mat)
    {
        if (mat.shader)
        {
            Shader shader = Shader.Find(mat.shader.name);
            if (shader)
            {
                mat.shader = shader;
            }
            else
            {
                Debug.Log(string.Format("errrrr: shader {0} 无法从本地找到", mat.shader.name));
            }
        }
        else
        {
            Debug.Log("errrrrr: 材质shader为空");
        }
    }

    private static void ReplaceShader(Object obj)
    {
        if (obj is GameObject)
        {
//加载GAMEOBJECT时
            XYClientCommon.ForEachChildren(
                obj as GameObject
                , OnEachGameObject
                , true
                );
        }
        else if (obj is Material)
        {
//加载MATERIAL时
            ReplaceMaterialShader(obj as Material);
        }
    }

    #endregion

    #region 异步加载Unity资源

    //回调
    private static void Callback(string url, AsyncLoadRequest request, Object resObj)
    {
        Object result = resObj;
        if (!result)
        {
            Debug.Log(string.Format("xxx错误加载失败{1},{2},地址：{0}", url, resObj, result));
        }
        else if (resObj is Material || resObj is GameObject)
        {
            result = Instantiate(resObj);
            ReplaceShader(result);
        }
        request.LoadFinish(result);
    }

    //预加载
    public static AsyncLoadRequest AsyncPreload(string resPath)
    {
        return AsyncLoad(resPath, null, null, true);
    }

    public static AsyncLoadRequest AsyncLoad(string resPath, AsyncLoadRequest.AsyncLoadCallBack onAsyncLoadComplete,
                                             object context)
    {
        return AsyncLoad(resPath, onAsyncLoadComplete, context, false);
    }

    public static AsyncLoadRequest AsyncLoad(string resPath, AsyncLoadRequest.AsyncLoadCallBack onAsyncLoadComplete,
                                             object context, bool preload)
    {
        var request = new AsyncLoadRequest(onAsyncLoadComplete, context, preload);
        System.Object resObject = ResourceManager.Get(resPath);
        if (resObject != null)
        {
            Callback(resPath, request, resObject as Object);
        }
        else
        {
            SameRequestCache src;
            if (Instance_.sameRequestCache_.TryGetValue(resPath, out src))
            {
                src.srcList.Add(request);
            }
            else
            {
                src = new SameRequestCache();
                src.srcList.Add(request);
                Instance_.sameRequestCache_[resPath] = src;
                Instance_.requestQueue_.Add(new LoadQueueData(resPath));
            }
        }
        return request;
    }

    #endregion
}