using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//屏幕ui效果节点，黑屏或白屏
public class ScreenEffectUI : MonoBehaviour {

    public static ScreenEffectUI Instance;

    public Image blockImage;

    public Image whiteImage;


    private void Awake()
    {
        Instance = this;
    }

}
