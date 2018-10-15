using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(RoleEffectClip))]
public class RoleEffectClipEditor:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoleEffectClip clip = target as RoleEffectClip;
        if (clip == null)
            return;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("溶解效果的参数：");
        EditorGUILayout.BeginVertical();
        clip.speed = EditorGUILayout.FloatField("溶解速度：", clip.speed);
        clip.begein = EditorGUILayout.FloatField("开始值：", clip.begein);
        clip.end = EditorGUILayout.FloatField("结束值：", clip.end);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("变大变小的效果的参数：");
        EditorGUILayout.BeginVertical();
        //clip.isSetGradually = EditorGUILayout.Toggle(clip.isSetGradually);
        //clip.scale = EditorGUILayout.FloatField(clip.scale);
        clip.curve = EditorGUILayout.CurveField("大小曲线：",clip.curve);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("显示或隐藏效果的参数：");
        EditorGUILayout.BeginVertical();
        clip.activateType = (ActivateType)EditorGUILayout.EnumPopup("效果：",clip.activateType);
        clip.playbackState = (ActivationControlTrack.PostPlaybackState)EditorGUILayout.EnumPopup("结束效果：", clip.playbackState);
        EditorGUILayout.EndVertical();
    }
}

