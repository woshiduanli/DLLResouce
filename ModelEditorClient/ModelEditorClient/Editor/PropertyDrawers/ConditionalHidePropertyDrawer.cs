using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : UnityEditor.PropertyDrawer
{
    public bool Draw(SerializedProperty property)
    {
        if (condHAtt.HideInInspector)
            return false;
        return GetConditionalHideAttributeResult(property);
    }

    private ConditionalHideAttribute condHAtt { get { return this.attribute as ConditionalHideAttribute; } }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Draw(property))
        {
            label.text = this.condHAtt.Lable;
            label.tooltip = this.condHAtt.Lable;
            position.height = GetPropertyHeight(property, label);
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (Draw(property))
            return EditorGUI.GetPropertyHeight(property, label);
        else
            return -EditorGUIUtility.standardVerticalSpacing;
    }

    public bool GetConditionalHideAttributeResult(SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        List<bool> list = new List<bool>();
        List<string> conditions = GetConditions();
        for (int i = 0; i < conditions.Count; i++)
        {
            string conditionPath = propertyPath.Replace(property.name, conditions[i]);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
            if (sourcePropertyValue != null)
                list.Add(CheckPropertyType(sourcePropertyValue, condHAtt.Value));
        }

        if(condHAtt.And)
            return !list.Contains(false);

        return list.Contains(true);
    }

    private List<string> GetConditions()
    {
        List<string> conditions = new List<string>();
        if (condHAtt.ConditionalSourceField.Contains("|"))
            conditions.AddRange(condHAtt.ConditionalSourceField.Split('|'));
        else
            conditions.Add(condHAtt.ConditionalSourceField);
        return conditions;
    }

    public bool CheckPropertyType(SerializedProperty sourcePropertyValue,List<int> Value)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;
            case SerializedPropertyType.String:
                return !string.IsNullOrEmpty(sourcePropertyValue.stringValue);
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            case SerializedPropertyType.Enum:
                return Value.Count == 0 || Value.Contains(sourcePropertyValue.enumValueIndex);
            default:
                return true;
        }
    }
}
