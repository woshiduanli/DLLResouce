using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Reflection;

public class CTrigger : MonoBehaviour
{
    [System.Serializable()]
    public class AnimatorAI
    {
        public Animator Animator;
        public string AniName;
        public Animation Animation;
    }

    public enum EventType
    {
        Auto = 0,//自动开关机关
        PointTrigger = 3,
        EventID = 6,
        SafeArea = 7,//安全区
    }
    public EventType Eventtype = EventType.Auto;
    public bool Once;
    public int State;
    public List<GameObject> HideList;
    public List<GameObject> ShowList;
    public List<AnimatorAI> AnimatorList;
    public CameraShake CameraShake;
}
