using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoleObject))]
class RoleObjectEditor:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("删除"))
        {
            RoleObject roleObj = target as RoleObject;
            World.Instance.RemoveRole(roleObj);
        }
    }
}

