using UnityEngine;
using UnityEditor;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(Equip))]
public class EquipEditor : CustomEditorBase
{
    private Equip Equip { get { return this.serializedObject.targetObject as Equip; } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (string.IsNullOrEmpty( Equip.EquipName))
        {
            Equip.EquipName =Path.GetFileNameWithoutExtension( Equip.ModelPath);
        }
        if (GUILayout.Button("生成低模蒙皮信息"))
        {
            if (Selection.activeObject is GameObject)
            {
                GameObject model = Selection.activeObject as GameObject;
                SkinnedMeshRenderer skin = model.GetComponent<SkinnedMeshRenderer>();
                if (!skin)
                {
                    EditorUtility.DisplayDialog("Error", "没有发现蒙皮信息", "OK");
                    return;
                }
                List<string> bones = new List<string>();
                for (int i = 0; i < skin.bones.Length; i++)
                    bones.Add(skin.bones[i].name);
                Equip.Bones = bones.ToArray();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "需要蒙皮model", "OK");
                return;
            }
        }
        if (GUILayout.Button("生成高模蒙皮信息"))
        {
            if (Selection.activeObject is GameObject)
            {
                GameObject model = Selection.activeObject as GameObject;
                SkinnedMeshRenderer skin = model.GetComponent<SkinnedMeshRenderer>();
                if (!skin)
                {
                    EditorUtility.DisplayDialog("Error", "没有发现蒙皮信息", "OK");
                    return;
                }
                List<string> bones = new List<string>();
                for (int i = 0; i < skin.bones.Length; i++)
                    bones.Add(skin.bones[i].name);
                Equip.HighBones = bones.ToArray();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "需要蒙皮model", "OK");
                return;
            }
        }

        if (GUILayout.Button("Preview"))
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
                return;
            }
            ModelLoader.ResetPreviewingObject();
            ModelLoader.PreviewingObject = new GameObject(this.Equip.EquipName);
            ModelLoader.PreviewingObject.transform.rotation = Quaternion.Euler(270, 0, 0);
            ModelLoader.PreviewingObject.transform.position = Vector3.zero;
            ModelLoader.AddEquip(ModelLoader.PreviewingObject, this.Equip,false);
            ModelLoader.SetupCamera(ModelLoader.PreviewingObject.transform);
        }
    }
}