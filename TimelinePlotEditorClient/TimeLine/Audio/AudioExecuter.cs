using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UI;

public class AudioExecuter : BehaviourExecuterBase
{
    public MYAudioPlayable audioPlayable;

    private GameObject audioGameObj;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        audioPlayable = behaviour as MYAudioPlayable;
    }
    public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        Loader.Instance.CreatAudioClips(audioPlayable.audioName, clip =>
         {
             this.audioGameObj = new GameObject(clip.name);
             AudioSource source = this.audioGameObj.AddComponent(typeof(AudioSource)) as AudioSource;
             source.clip = clip;
             source.loop = false;
             source.spatialBlend = 0;
             source.dopplerLevel = 0;
             source.maxDistance = 100;
             source.rolloffMode = AudioRolloffMode.Linear;
             source.volume = 1;
             source.Play();
         });
        
    }

    public override void OnBehaviourDone(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if(!audioPlayable.isLoop)
            GameObject.DestroyImmediate(audioGameObj);
    }
}