using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//timelineui根节点
public class TimelineUI : MonoBehaviour
{
    public static TimelineUI Instance;
    public GameObject PlotDialogueUINode;
    public GameObject DialogueUINode;
    public GameObject TextBubbleNode;

    private void Awake()
    {
        Instance = this;
    }
}
