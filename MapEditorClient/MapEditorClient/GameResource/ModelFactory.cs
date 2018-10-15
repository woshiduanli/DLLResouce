using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Model;
using UnityEngine;
using Action = System.Action;
using Object = UnityEngine.Object;

/// <summary>
///     异步操作代理对象
///     通过开启独立的线程来完成传入的delegate
/// </summary>
internal class AsyncDelegate
{
    private readonly Thread Thread_;
    private readonly Action Work_;

    public AsyncDelegate(Action work)
    {
        Work_ = work;
        Thread_ = new Thread(ThreadProc);
    }

    public bool IsDone { get; private set; }

    public void Start()
    {
        Thread_.Start();
    }

    private void ThreadProc()
    {
        if (Work_ != null)
        {
            Work_();
        }
        IsDone = true;
    }

    /// <summary>
    ///     提供一个静态的方法
    ///     需要用MonoBehaviour.StartCoroutine来执行
    /// </summary>
    /// <param name="work"></param>
    /// <returns></returns>
    public static IEnumerator Execute(Action work)
    {
        var ad = new AsyncDelegate(work);
        ad.Start();
        while (!ad.IsDone)
        {
            yield return null;
        }
    }
}

//模型工厂
public class ModelFactory
{
    public static GameObject CreateBaseRole(string roleName //角色名称
                                            , string boneFile) //骨骼信息文件路径   
    {
        GameObject role;
        if (!string.IsNullOrEmpty(boneFile))
        {
            role = Object.Instantiate(ResourceManager.Get(boneFile) as Object) as GameObject;
            role.name = roleName;
        }
        else
        {
            role = new GameObject(roleName);
            var boneGo = new GameObject("mainbone");
            boneGo.transform.parent = role.transform;
            boneGo.transform.localPosition = Vector3.zero;
        }
        if (role.GetComponent<Animation>())
            role.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
        return role;
    }

    public static void LoadModel(ObjectModel model)
    {
        XYCoroutineEngine.Execute(AsyncLoadModel(model));
    }

    private static IEnumerator AsyncLoadModel(ObjectModel model)
    {
        yield return null;
        AsyncLoadRequest request;
        

        if (!string.IsNullOrEmpty(model.ModelCfg.ModelPath))
        {
            request = XYSingleAssetLoader.AsyncLoad(model.ModelCfg.ModelPath, null, null);
            while (!request.isDone)
            {
                yield return null;
            }
            model.GameObject = request.asset as GameObject;
            model.GameObject.name = model.ModelCfg.EquipName;
        }

        //特效
        foreach (EffectConfig effectInfo in model.ModelCfg.Effects.Where(effectInfo => !string.IsNullOrEmpty(effectInfo.EffectAssetPath)))
        {
            request = XYSingleAssetLoader.AsyncLoad(effectInfo.EffectAssetPath, null, null);
            while (!request.isDone)
            {
                yield return null;
            }
            model.EffectObjects.Add(request.asset as GameObject);
        }
        model.LoadOver();
    }
}