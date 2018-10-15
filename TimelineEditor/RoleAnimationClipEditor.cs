using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(RoleAnimationClip))]
public class RoleAnimationClipEditor : Editor {

    RoleAnimationClip tlAnimationClip;
    AnimationClip[] clips;
    int selectClipIndex;
    string selectClipName;
    RoleObject roleObj;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        return;
        if (tlAnimationClip == null)
            tlAnimationClip = target as RoleAnimationClip;
        if (tlAnimationClip == null)
            return;
        if (tlAnimationClip.Role==null)
            return;
        if (roleObj == null)
            roleObj = World.Instance.GetRoleObj(tlAnimationClip.Role);
        if (roleObj == null)
            return;
        if (clips == null || clips.Length <= 0)
            clips = roleObj.Animator.runtimeAnimatorController.animationClips;
        if (clips == null || clips.Length <= 0)
            return;

        if (string.IsNullOrEmpty(selectClipName))
            for (int i = 0; i < clips.Length; i++)
                if (clips[i].name == tlAnimationClip.AnimationName)
                    selectClipIndex = i;

        selectClipIndex = EditorGUILayout.Popup(selectClipIndex, clips.Select(clip => clip.name).ToArray());
        tlAnimationClip.AnimationName =selectClipName = clips[selectClipIndex].name;
    }
}
