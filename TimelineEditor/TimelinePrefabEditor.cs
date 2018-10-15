using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[CustomEditor(typeof(TimelinePrefab))]
public class TimelinePrefabEditor : Editor {

    int trackIndex;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("删除"))
        {
            GameObject targetGo = (target as TimelinePrefab).gameObject;
            if (EditorApplication.isPlaying)
                TimelineManager.Instance.DestroyTimeline(targetGo.name.Replace("(Clone)", ""));
            else
                EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
        }
        if (GUILayout.Button("保存Prefab"))
        {
            GameObject targetGo =(target as TimelinePrefab).gameObject;
            if(EditorApplication.isPlaying)
                TimelineManager.Instance.SavePrefab(targetGo.name.Replace("(Clone)", ""));
            else
                EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("TrackIndex");
        trackIndex = Convert.ToInt32(GUILayout.TextField(trackIndex.ToString(), GUILayout.Width(50)));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("生成MoveClip"))
        {
            PlayableDirector director = (target as TimelinePrefab).gameObject.GetComponent<PlayableDirector>();
            if(trackIndex-1>=0)
                TimelineManager.Instance.GenerateMoveClip(director, trackIndex-1);
        }


    }
}
