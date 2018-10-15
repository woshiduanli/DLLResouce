using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectClip))]
public class EffectClipAssetEditor:Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EffectClip effectClip = target as EffectClip;

        GameObject obj = null;
        obj=EditorGUILayout.ObjectField("位置物体", obj, typeof(GameObject),true) as GameObject;
        if (obj != null)
        {
            effectClip.pos = obj.transform.position;
            effectClip.rotation = obj.transform.eulerAngles;
            effectClip.scale = obj.transform.localScale;
        }
        
    }
}

