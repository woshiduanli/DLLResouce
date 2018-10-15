using UnityEngine;
using UnityEditor;

/// <summary>
/// 整型选择popup控件
/// </summary>
[CustomPropertyDrawer(typeof(IntPopupAttribute))]
public class IntPopupDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IntPopupAttribute pa = this.attribute as IntPopupAttribute;
        if (property.depth <= 0)
        {
            position.xMin += 4;
        }
        int v = property.intValue;
        v = EditorGUI.IntPopup(position, pa.Lable, v, pa.DisplayedOptions, pa.OptionValues);
        if (GUI.changed)
        {
            property.intValue = v;
        }
    }
}

/// <summary>
/// 整型选择popup控件
/// </summary>
[CustomPropertyDrawer(typeof(EnumPopupAttribute))]
public class EnumPopupDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumPopupAttribute pa = this.attribute as EnumPopupAttribute;
        if (property.depth <= 0)
        {
            position.xMin += 4;
        }
        int v = property.intValue;
        v = EditorGUI.Popup(position, pa.Lable, v, pa.DisplayedOptions);
        if (GUI.changed)
            property.enumValueIndex = v;
    }
}

/// <summary>
/// 整型选择popup控件
/// </summary>
[CustomPropertyDrawer(typeof(StringPopupAttribute))]
public class StringPopupDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        StringPopupAttribute pa = this.attribute as StringPopupAttribute;
        if (property.depth <= 0)
        {
            position.xMin += 4;
        }
        int selected = System.Array.FindIndex(pa.OptionValues, value => value == property.stringValue);
        selected = selected >= 0 ? selected : 0;
        selected = EditorGUI.Popup(position, pa.Lable, selected, pa.OptionValues);
        if (GUI.changed)
        {
            property.stringValue = pa.OptionValues[selected];
        }
    }
}

[CustomPropertyDrawer(typeof(CommonAttribute))]
public class CommonDrawer : UnityEditor.PropertyDrawer
{
    private CommonAttribute Attribute { get { return this.attribute as CommonAttribute; } }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.depth <= 0)
            position.xMin += 4;
        label.text = Attribute.Lable;
        position.height = GetPropertyHeight(property, label);
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}