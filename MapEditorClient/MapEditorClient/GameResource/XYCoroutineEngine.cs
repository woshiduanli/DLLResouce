using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class XYCoroutineEngine : MonoBehaviour
{
    private static XYCoroutineEngine instance_;

    public static void Load()
    {
        if (!instance_)
        {
            var go = new GameObject("XYCoroutineEngine");
            go.AddComponent<XYCoroutineEngine>();
            Object.DontDestroyOnLoad(go);
        }
    }

    /// <summary>
    ///     执行一段协程
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public static Coroutine Execute(IEnumerator routine)
    {
        return instance_.StartCoroutine(routine);
    }

    private void Awake()
    {
        instance_ = this;
    }
}