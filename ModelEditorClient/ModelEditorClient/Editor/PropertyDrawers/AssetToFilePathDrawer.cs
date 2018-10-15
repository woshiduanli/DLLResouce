using System.IO;
using System.Reflection;
using Model;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源对象和路径的属性编辑器
/// 拖拽资源到控件，自动保存该资源的路径到对应属性
/// 可以处理数组
/// </summary>
[CustomPropertyDrawer(typeof(AssetToFilePathAttribute))]
public class AssetToFilePathDrawer : ConditionalHidePropertyDrawer
{
    private AssetToFilePathAttribute CustomAttribute { get { return this.attribute as AssetToFilePathAttribute; } }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Draw(property))
        {
            if (property.depth <= 0)
                position.xMin += 4;
            position.height = GetPropertyHeight(property, label);
            label.text = this.CustomAttribute.Lable;
            label.tooltip = this.CustomAttribute.Lable;
            DrawDropableField(position, property, label);
        }
    }

    private void DrawDropableField(Rect position, SerializedProperty property, GUIContent label)
    {
        UnityEngine.Object asssetObj = null;
        if (!string.IsNullOrEmpty(property.stringValue))
        {
            asssetObj = AssetDatabase.LoadAssetAtPath("assets/" + property.stringValue, typeof(UnityEngine.Object));
            if (!asssetObj)
            {
                Debug.Log("File not found: " + "assets/" + property.stringValue);
                property.stringValue = string.Empty;
            }
        }

        label.tooltip = string.Format("{0}\n({1})", label.text, property.stringValue);
        asssetObj = EditorGUI.ObjectField(position, label, asssetObj, typeof(UnityEngine.Object), false);
        if (GUI.changed)
        {
            if (!asssetObj)
            {
                property.stringValue = string.Empty;
                return;
            }
            string filePath = AssetDatabase.GetAssetPath(asssetObj).ToLower();
            //资源必须是assets/开头的路径
            if (!filePath.StartsWith("assets/"))
            {
                EditorUtility.DisplayDialog("错误", string.Format("资源路径错误，{0}", filePath), "OK");
                return;
            }

            filePath = filePath.Remove(0, "assets/".Length);
            if (string.IsNullOrEmpty(this.CustomAttribute.AssetExt))
                property.stringValue = filePath;
            else if (Path.GetExtension(filePath).Equals(this.CustomAttribute.AssetExt, System.StringComparison.CurrentCultureIgnoreCase))
                property.stringValue = filePath;
            else
            {
                asssetObj = null;
                EditorUtility.DisplayDialog("错误", string.Format("资源格式不正确，该字段需要后缀为'{0}'的资源", this.CustomAttribute.AssetExt), "OK");
            }
        }

    }
}