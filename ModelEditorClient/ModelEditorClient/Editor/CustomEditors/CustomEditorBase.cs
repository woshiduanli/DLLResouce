using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.Reflection;
using UnityEditor.Animations;
using UnityEngine.Rendering;
public class CustomEditorBase : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(50);
        if (GUILayout.Button("Save"))
        {
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Build"))
        {
            Build();
        }
    }

    protected void Build()
    {
        AssetDatabase.SaveAssets();
        if (this.serializedObject.targetObject)
        {
            AssetExporter.GetShaders();
            AssetExporter.GetPublicAssets();
            AssetExporter.BuildAssetBundles(this.serializedObject.targetObject);

            Selection.activeObject = this.serializedObject.targetObject;
        }
        AssetDatabase.Refresh();
    }
}