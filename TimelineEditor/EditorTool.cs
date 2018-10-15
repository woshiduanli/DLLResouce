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
using System.Linq;

public static class EditorTool {


    //[MenuItem("Assets/Timeline/AddSkipPartToAll")]
    public static void AddSkipPart()
    {
        Selection.activeObject= AssetDatabase.LoadAssetAtPath("Assets/Resources/Timeline/Timeline", typeof(Object));
        GameObject[] objs = Selection.GetFiltered(typeof(GameObject),SelectionMode.DeepAssets) as GameObject[];
        AddSkipPartToTimeline(objs);
    }

    //[MenuItem("Assets/Timeline/AddSkipPartToSelect")]
    public static void AddSkipPartToSelect()
    {
        GameObject[] objs = Selection.gameObjects;
        AddSkipPartToTimeline(objs);
    }

    public static void AddSkipPartToTimeline(GameObject[] objs)
    {
        if (objs == null || objs.Length <= 0)
            return;
        GameObject skipPartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Timeline/UI/Prefab/TopLeft.Prefab");
        EditorUtility.DisplayProgressBar("批量设置跳过ui", "设置", objs.Length);
        int i = 0;
        foreach (var obj in objs)
        {
            GameObject objInstance = PrefabUtility.InstantiatePrefab(obj) as GameObject;
            PrefabUtility.DisconnectPrefabInstance(objInstance);
            var uiNode = objInstance.transform.Find("UI");
            if (uiNode == null)
            {
                continue;
            }
            if (uiNode.Find("TopLeft"))
            {
                GameObject.DestroyImmediate(objInstance);
                continue;
            }
            GameObject skipPartInstance = PrefabUtility.InstantiatePrefab(skipPartPrefab) as GameObject;
            PrefabUtility.DisconnectPrefabInstance(skipPartInstance);
            var skipInstanceTrans = skipPartInstance.transform;
            skipInstanceTrans.SetParent(uiNode);
            (skipInstanceTrans as RectTransform).anchoredPosition = Vector3.zero;
            skipInstanceTrans.localScale = Vector3.one;
            PrefabUtility.ReplacePrefab(objInstance, obj);
            GameObject.DestroyImmediate(objInstance);
            i++;
            EditorUtility.DisplayProgressBar("批量设置跳过ui", i + "/" + objs.Length, i / objs.Length);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }



}
