using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PlotDialogueUI : MonoBehaviour
{
    public static PlotDialogueUI Instance;

    public Action<bool> SetTimelinePlay;

    public RectTransform dialoguePanel;


    private Text txtName;
    private Text txtDialogue;
    private Button mask;
    private Button skip;
    private Button next;
    private GameObject modelParent;
    private GameObject contentRoot;

    private List<DialogueUIData> dialogues;
    private int curIndex;
    private float curDialogueWaitTime;
    public bool isPlayingPlot;
    private bool isPauseTimeline;
    private RoleObject model;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        txtName = transform.Find("Mask/Bottom/Bg/NpcName/Name").GetComponent<Text>();
        txtDialogue = transform.Find("Mask/Bottom/Bg/Dialogue").GetComponent<Text>();
        mask = transform.Find("Mask").GetComponent<Button>();
        skip = transform.Find("Mask/Bottom/Bg/Skip").GetComponent<Button>();
        next = transform.Find("Mask/Bottom/Bg").GetComponent<Button>();
        modelParent = transform.Find("Mask/Bottom/Bg/ModelParent").gameObject;
        contentRoot = mask.gameObject;

        mask.onClick.AddListener(() => ShowSingleDialogue());
        next.onClick.AddListener(() => ShowSingleDialogue());
        skip.onClick.AddListener(() => DoClose());

    }

    public void ShowDialogue(bool pauseTimeline, List<DialogueUIData> dialogues)
    {
        if (dialogues == null || dialogues.Count <= 0)
        {
            Debug.LogWarning("对话clip中没有对话数据");
            return;
        }
        this.dialogues = dialogues;
        this.isPauseTimeline = pauseTimeline;
        Begin();
    }

    private void Begin()
    {
        if (isPauseTimeline && SetTimelinePlay != null)
        {
            SetTimelinePlay(false);
        }

        if (FundamentalUI.Instance)
        {
            FundamentalUI.Instance.HideUI();
        }

        curIndex = -1;
        curDialogueWaitTime = 0;
        isPlayingPlot = true;
        ShowSingleDialogue();

        contentRoot.SetActive(true);
    }


    private void ShowSingleDialogue()
    {
        curIndex++;
        if (curIndex >= dialogues.Count)
        {
            DoClose();
            return;
        }
        txtDialogue.text = dialogues[curIndex].Dialogue;
        txtName.text = dialogues[curIndex].Name;
        curDialogueWaitTime = 0;
        Loader.Instance.CreateRoleObject(dialogues[curIndex].Id,OnRoleLoadComplete);
    }

    private void OnRoleLoadComplete(RoleObject obj)
    {
        if (model != null)
            GameObject.Destroy(model.gameObject);
        obj.gameObject.transform.SetParent(modelParent.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        obj.modelLayer = XYDefines.Layer.TimelineUI;
        TimelineManager.Instance.SetLayerRecursively(obj.gameObject, XYDefines.Layer.TimelineUI);
        model = obj;
    }


    public void Update()
    {
        if (!isPlayingPlot)
        {
            return;
        }

        if (curIndex < 0 || curIndex >= dialogues.Count)
        {
            DoClose();
            return;
        }

        curDialogueWaitTime += Time.deltaTime;
        if (dialogues[curIndex].Time <= curDialogueWaitTime)
        {
            ShowSingleDialogue();
            return;
        }
    }

    private void DoClose()
    {
        isPlayingPlot = false;
        contentRoot.SetActive(false);
        if (isPauseTimeline && SetTimelinePlay != null)
        {
            SetTimelinePlay(true);
        }
        if (FundamentalUI.Instance)
        {
            FundamentalUI.Instance.ShowUI();
        }

    }
}


