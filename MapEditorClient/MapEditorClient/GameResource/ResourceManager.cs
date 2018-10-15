using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Model;
using UnityEngine;
using Object = System.Object;

//资源管理器
public static class ResourceManager
{
    #region 数据缓冲对象CacheValue

    public class CacheValue
    {
        private readonly AssetBundle ab_;
        private readonly Object obj_;
        public bool dontRelease = false;

        public CacheValue(AssetBundle ab)
        {
            ab_ = ab;
        }

        public CacheValue(AssetBundle ab, Object value)
        {
            ab_ = ab;
            obj_ = value;
        }

        public AssetBundle AB
        {
            get { return ab_; }
        }

        public Object OBJ
        {
            get { return obj_; }
        }

    }

    #endregion

    #region 数据定义

    public const float CacheTimeOut = 60.0f;
    private static readonly Dictionary<string, CacheValue> cache_ = new Dictionary<string, CacheValue>();

    private static Dictionary<int, Role> roleCfgs_ = new Dictionary<int, Role>();

    #endregion
    static float AsyncPreloadTotal = 0;
    #region 初始化

    /// <summary>
    ///     预加载一些资源和设置
    /// </summary>
    public static void Initialize()
    {
        XYCoroutineEngine.Execute(AsyncPreloadRoleConfig(0.25f));
        XYCoroutineEngine.Execute(AsyncPreloadEquipConfig(0.25f));
        XYCoroutineEngine.Execute(AsyncPreloadRoleBone(0.5f));
    }
    #endregion

    #region System.Object类资源存取

    public static void Add(string lowerResPath, AssetBundle ab, Object value, bool preload)
    {
        lowerResPath = lowerResPath.ToLower();
        lowerResPath = lowerResPath.Replace('\\', '/');

        if (value != null)
        {
            if (value is Role)
            {
                Role role = value as Role;
                //roleCfgs_[role.ApprId] = role;
            }
            else
            {
                var cv = new CacheValue(ab, value);
                cv.dontRelease = preload;
                cache_[lowerResPath] = cv;
            }
        }
    }

    public static bool CheckResourceExist(string lowerResPath)
    {
        return cache_.ContainsKey(lowerResPath);
    }

    public static Object Get(string lowerResPath)
    {
        CacheValue cv;
        lowerResPath = lowerResPath.Replace('\\', '/');
        if (cache_.TryGetValue(lowerResPath.ToLower(), out cv))
        {
            if (cv.OBJ != null)
            {
                return cv.OBJ;
            }
            else
            {
                cache_.Remove(lowerResPath);
            }
        }
        return null;
    }

    public static AssetBundle GetAB(string lowerResPath)
    {
        CacheValue cv;
        lowerResPath = lowerResPath.Replace('\\', '/');
        if (cache_.TryGetValue(lowerResPath, out cv))
        {
            if (cv.AB)
            {
                return cv.AB;
            }
            else
            {
                cache_.Remove(lowerResPath);
            }
        }
        return null;
    }

    #endregion

    #region 缓冲数据的清除

    //过图时的缓存清理
    public static void Clear_ChangeMap()
    {
        var list = new List<string>();
        foreach (var kvp in cache_)
        {
            if (kvp.Value.OBJ is UnityEngine.Object)
            {
                if (kvp.Value.dontRelease)
                {
                    //设置了不释放标记的资源
                    continue;
                }
                else
                {
                    list.Add(kvp.Key);
                }
            }
        }
        foreach (string s in list)
        {
            ClearByPath(s);
        }
    }

    private static void ClearByPath(string resPath)
    {
        string lowerResPath = resPath.ToLower();
        CacheValue cv;
        if (cache_.TryGetValue(lowerResPath, out cv))
        {
            if (cv.AB)
            {
                cv.AB.Unload(false);
                UnityEngine.Object.Destroy(cv.AB);
            }
            cache_.Remove(lowerResPath);
        }
    }

    public static void UnloadUnusedAssets()
    {
        XYCoroutineEngine.Execute(AsyncUnloadUnusedAssets());
    }

    private static IEnumerator AsyncUnloadUnusedAssets()
    {
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        while (!ao.isDone)
        {
            yield return null;
        }
    }

    public static void ClearTimeOut()
    {
        XYCoroutineEngine.Execute(DoClearTimeout(false));
    }

    /// <summary>
    ///     通过协成方式来做，避免顿卡的现象
    /// </summary>
    /// <param name="avoidTime">忽略时间间隔，调试用</param>
    /// <returns></returns>
    private static IEnumerator DoClearTimeout(bool avoidTime)
    {
        var cacheKeys = new string[cache_.Keys.Count];

        cache_.Keys.CopyTo(cacheKeys, 0);
        int totalClear = 0;
        foreach (string key in cacheKeys)
        {
            CacheValue cv;
            if (cache_.TryGetValue(key, out cv))
            {
                if (cv.OBJ is UnityEngine.Object)
                {
                    if (cv.dontRelease)
                    {
                        //设置了不释放标记的资源
                        continue;
                    }
                    else if (cv.OBJ is GameObject
                             || cv.OBJ is AnimationClip
                             || cv.OBJ is Texture
                             || cv.OBJ is Mesh)
                    {
                        if (avoidTime)
                        {
                            totalClear++;
                            ClearByPath(key);
                        }
                    }
                }
            }
            yield return null;
        }
 
        Resources.UnloadUnusedAssets();
    }

    public static void PrintCacheListInfo()
    {
        var ht = new Hashtable();

        foreach (var kvp in cache_)
        {
            string objType = kvp.Value.OBJ.GetType().ToString();
            int count = ht[objType] == null ? 0 : (int)ht[objType];
            ht[objType] = count + 1;
        }
        foreach (DictionaryEntry kv in ht)
        {

        }
    }

    #endregion

    #region 主角XYAction类资源的预加载和存取处理

    private static void PreloadActions()
    {
    }

    #endregion

    #region 外观设置RoleConfig对象的加载和存取

    private static IEnumerator AsyncPreloadRoleConfig(float Preloadpercent)
    {

        string[] files = Directory.GetFiles(@"res/model/role/", "*.role", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            AsyncLoadRequest ar = XYSingleAssetLoader.AsyncPreload(file);
            while (!ar.isDone)
            {
                yield return null;
            }
        }
        
        yield return null;
        AsyncPreloadTotal += Preloadpercent;
    }

    private static Role GetRoleConfig(string filePath)
    {
        return Get(filePath) as Role;
    }

    public static Role GetRoleConfig(int appearID)
    {
        if (roleCfgs_.ContainsKey(appearID))
        {
            Role role = roleCfgs_[appearID];
            if (role != null)
                return role;
            else
            {
                roleCfgs_.Remove(appearID);
            }
        }
        return roleCfgs_[10000];
    }

    #endregion

    #region 主角Equip的预加载

    private static IEnumerator AsyncPreloadEquipConfig(float Preloadpercent)
    {
        string[] files = Directory.GetFiles(@"res/model", "*.item", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            AsyncLoadRequest ar = XYSingleAssetLoader.AsyncPreload(file);
            while (!ar.isDone)
            {
                yield return null;
            }
        }
        yield return null;
        AsyncPreloadTotal += Preloadpercent;
    }

    #endregion

    #region 主角RoleBone(角色骨骼数据)的预加载

    private static IEnumerator AsyncPreloadRoleBone(float Preloadpercent)
    {
        string[] files = Directory.GetFiles(@"res/model", "*.bone", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            AsyncLoadRequest ar = XYSingleAssetLoader.AsyncPreload(file);
            while (!ar.isDone)
            {
                yield return null;
            }
        }
        yield return null;
        AsyncPreloadTotal += Preloadpercent;
    }
    #endregion

    public static float GetAsyncPreloadTotal()
    {
        return AsyncPreloadTotal;
    }

    public static Equip GetEquipConfig(string filePath)
    {
        return Get(filePath) as Equip;
    }
}