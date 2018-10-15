using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(ActivationControlTrack))]
public class ActivationContorlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ActivationControlTrack track = target as ActivationControlTrack;
        //base.OnInspectorGUI();
        track.playbackState = (ActivationControlTrack.PostPlaybackState)EditorGUILayout.EnumPopup("PlaybackState",track.playbackState);
    }
}

