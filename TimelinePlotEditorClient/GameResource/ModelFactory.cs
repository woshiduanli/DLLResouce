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
