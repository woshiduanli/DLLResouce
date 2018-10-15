using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoleData))]
public class RoleDataEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("刷新位置朝向"))
        {
            RoleData roleData = target as RoleData;
            RoleObject roleObj = World.Instance.GetRoleObj(roleData);
            if (roleObj == null)
            {
                roleObj = World.Instance.GetRoleObj(roleData.Id);
            }
            if (roleObj == null)
                return;
            roleData.transform.position = roleObj.transform.position;
            roleData.transform.eulerAngles = roleObj.transform.eulerAngles;
            roleData.InitialPos = roleObj.transform.position;
            roleData.InitialRotation = roleObj.transform.eulerAngles;
        }
    }
}
