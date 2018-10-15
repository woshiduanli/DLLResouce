using Model;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 掩码选择控件
/// </summary>
[CustomPropertyDrawer(typeof(MaskFieldAttribute))]
public class MaskFieldDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MaskFieldAttribute mf = this.attribute as MaskFieldAttribute;
        if (mf == null) return;
        if (property.depth <= 0)
        {
            position.xMin += 4;
        }
        int mask = property.intValue;
        label.text = string.Format("{0}(0x{1:X})", mf.Lable, mask);
        mask = EditorGUI.MaskField(position, label, mask, mf.DisplayedOptions);
        if (GUI.changed)
        {
            property.intValue = mask;
        }
    }
}