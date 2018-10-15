using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Model;
using Object = UnityEngine.Object;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
public partial class Utility
{
    public static string GetEditorUrl(string assetPath)
    {
        if (!assetPath.StartsWith("Assets", System.StringComparison.OrdinalIgnoreCase))
            assetPath = Path.Combine("Assets/", assetPath);
        assetPath = Path.GetFullPath(assetPath);
        return assetPath.Replace('\\', '/');
    }

    public static T AddComponent<T>(GameObject target) where T : UnityEngine.Component
    {
        if (target)
        {
            T result = target.GetComponent(typeof(T)) as T;
            if (result == null)
                result = target.AddComponent(typeof(T)) as T;
            return result;
        }
        return null;
    }

    public static string GetAssetExt(Object asset)
    {
        if (isRole(asset))
            return "role";
        if(isModel(asset))
            return "model";
        return GetAssetExt(asset.GetType());
    }

    /// <summary>
    /// 后缀
    /// </summary>
    /// <param name="assetType"></param>
    /// <returns></returns>
    public static string GetAssetExt(System.Type assetType)
    {
        if (assetType == typeof(Model.Action))
            return "action";
        if (assetType == typeof(AudioClip))
            return "audio";
        if (assetType == typeof(Equip))
            return "equip";
        if (assetType == typeof(Texture) || assetType == typeof(Texture2D))
            return "tex";
        if (assetType == typeof(GameObject))
            return "go";
        if (assetType == typeof(Material))
            return "mat";
        if (assetType == typeof(Mesh))
            return "mesh";
        if (assetType == typeof(AnimatorController))
            return "ctrl";
        if (assetType == typeof(AnimationClip))
            return "clip";
        return string.Empty;
    }

    public static bool isRole(Object asset)
    {
        if (!asset)
            return false;
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
        if (importer.assetBundleVariant == "role")
            return true;
        if (asset is GameObject)
        {
            GameObject go = asset as GameObject;
            return go && go.GetComponent<Role>();
        }
        return false;
    }

    public static bool isModel(Object asset)
    {
        if (!asset)
            return false;
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
        if (importer.assetBundleVariant == "model")
            return true;
        if (asset is GameObject)
        {
            GameObject go = asset as GameObject;
            return go.name.StartsWith("model_");
        }
        return false;
    }

    public static bool isCtrl(Object asset)
    {
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
        if (importer.assetBundleVariant == "ctrl")
            return true;
        if (asset is AnimatorController)
            return true;
        if (asset.name.EndsWith(".controller"))
            return true;

        return false;
    }

    public static AnimationClip GetClip(string name, Object[] clips)
    {
        if (clips == null)
            return null;
        for (int i = 0; i < clips.Length; i++)
        {
            AnimationClip clip = clips[i] as AnimationClip;
            if (clip.name == name)
                return clip;
        }
        return null;
    }
   
}

public class PListNode
{
    public PListNode next;
    public PListNode prev;
}

public class PList : PListNode
{
    public int Count = 0;

    public PList()
    {
        Init();
    }

    public bool Empty()
    {
        return this.prev == this;
    }

    public void Init()
    {
        this.next = this;
        this.prev = this;
        this.Count = 0;
    }

    public void Add(PListNode node)
    {
        this.next.prev = node;
        node.next = this.next;
        node.prev = this;
        this.next = node;
        this.Count++;
    }

    public void AddTail(PListNode node)
    {
        PListNode p = prev;
        this.prev = node;
        node.next = this;
        node.prev = p;
        p.next = node;
        Count++;
    }

    public void Remove(PListNode node)
    {
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
        Count--;
    }

    public PListNode Pop()
    {
        if (prev == this)
            return null;

        PListNode node = this.next;
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
        Count--;

        return node;
    }

    public void AddListTail(PList list)
    {
        if (list.next == list)
            return;

        PListNode first = list.next;
        PListNode last = list.prev;
        PListNode at = this.prev;

        at.next = first;
        first.prev = at;

        this.prev = last;
        last.next = this;

        this.Count += list.Count;
        list.Init();
    }
}

public class GameTimer
{
    //当前时间，统一接口用
    public static float time
    {
        get { return Time.realtimeSinceStartup; }
    }

    /// <summary>
    ///     检测给予时间和当前时间是否在一定范围内
    /// </summary>
    /// <param name="time"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static bool Within(float time, float amount)
    {
        return GameTimer.time - time < amount;
    }
}


