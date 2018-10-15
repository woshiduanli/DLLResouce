using System.Reflection;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 通过反射方式调用Unity一些Internal方法
/// </summary>
public class EditorGui
{
    private static MethodInfo GetSinglePropertyHeightMethod = null;
    private static MethodInfo HasVisibleChildFieldsMethod = null;
    private static PropertyInfo IndentProperty = null;

    public static bool PropertyField(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
    {
        if (!includeChildren)
            return EditorGUI.PropertyField(position, property, label);
        Vector2 iconSize = UnityEditor.EditorGUIUtility.GetIconSize();
       // if (EditorGuiUtility.LookLikeInspector)
            //UnityEditor.EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
        bool enabled = GUI.enabled;
        int indentLevel = EditorGUI.indentLevel;
        int num = indentLevel - property.depth;
        position.height = 16f;
        SerializedProperty serializedProperty = property.Copy();
        SerializedProperty endProperty = serializedProperty.GetEndProperty();
        EditorGUI.indentLevel = serializedProperty.depth + num;
        bool enterChildren = EditorGUI.PropertyField(position, serializedProperty, label) && HasVisibleChildFields(serializedProperty);
        position.y += EditorGUI.GetPropertyHeight(serializedProperty, null, false);


        while (serializedProperty.NextVisible(enterChildren) && !SerializedProperty.EqualContents(serializedProperty, endProperty))
        {

            EditorGUI.indentLevel = serializedProperty.depth + num;
            EditorGUI.BeginChangeCheck();
            //enterChildren = EditorInvoker.EditorGui.SinglePropertyField( position, serializedProperty, null ) && EditorInvoker.EditorGui.HasVisibleChildFields( serializedProperty );
            enterChildren = PropertyField(position, serializedProperty, null, true) && HasVisibleChildFields(serializedProperty);
            if (!EditorGUI.EndChangeCheck())
            {
                position.y += EditorGUI.GetPropertyHeight(serializedProperty, null, true);
            }
            else
                break;
        }

        GUI.enabled = enabled;
        UnityEditor.EditorGUIUtility.SetIconSize(iconSize);
        EditorGUI.indentLevel = indentLevel;
        return false;
    }

    public static float GetSinglePropertyHeight(SerializedProperty property)
    {
        if (GetSinglePropertyHeightMethod == null)
        {
            GetSinglePropertyHeightMethod = typeof(EditorGUI).GetMethod("GetSinglePropertyHeight",
                                                                           BindingFlags.Static |
                                                                           BindingFlags.NonPublic, null,
                                                                           new System.Type[] { typeof(SerializedProperty) }, null);
        }
        return (float)GetSinglePropertyHeightMethod.Invoke(null, new object[] { property });
    }

    public static bool HasVisibleChildFields(SerializedProperty property)
    {
        if (HasVisibleChildFieldsMethod == null)
        {
            HasVisibleChildFieldsMethod = typeof(EditorGUI).GetMethod("HasVisibleChildFields",
                                                                           BindingFlags.Static |
                                                                           BindingFlags.NonPublic, null,
                                                                           new System.Type[] { typeof(SerializedProperty) }, null);
        }
        return (bool)HasVisibleChildFieldsMethod.Invoke(null, new object[] { property });
    }

    public static float Indent
    {
        get
        {
            if (IndentProperty == null)
            {
                IndentProperty = typeof(EditorGUI).GetProperty("indent",
                                                                              BindingFlags.Static |
                                                                              BindingFlags.NonPublic);
            }
            return (float)IndentProperty.GetValue(null, null);
        }
    }
}