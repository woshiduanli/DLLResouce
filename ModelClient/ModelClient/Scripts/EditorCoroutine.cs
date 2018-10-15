using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class EditorCoroutine : MonoBehaviour
{
    public static EditorCoroutine Instance { get; private set; }

    public static Coroutine Execute(IEnumerator routine)
    {
        if (!Application.isPlaying)
            return null;
        if (!Instance)
        {
            GameObject go = new GameObject("EditorCoroutine");
            Instance = go.AddComponent<EditorCoroutine>();
        }

        return Instance.StartCoroutine(routine);
    }

    void Awake()
    {
        Instance = this;
    }
}
