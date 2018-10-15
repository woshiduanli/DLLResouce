using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FundamentalUI : MonoBehaviour {

    public static FundamentalUI Instance;
    public Action skipTimeline;

    public Text dialogue;
    public Button skipButton;
    public GameObject dialogueNode;
    public GameObject contentNode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogue = transform.Find("GameObject/Bottom/DialogueUI/Dialogue/Text").GetComponent<Text>();
        skipButton = transform.Find("GameObject/Top/Image/Skip/Skip").GetComponent<Button>();
        dialogueNode = transform.Find("GameObject/Bottom/DialogueUI/Dialogue").gameObject;
        contentNode = transform.Find("GameObject").gameObject;
        skipButton.onClick.AddListener(() => SkipTimeline());
    }


    private void SkipTimeline()
    {
        if (skipTimeline != null)
            skipTimeline.Invoke();
    }

    public void ShowDialogue(string dialogue, string name)
    {
        contentNode.SetActive(true);
        dialogueNode.SetActive(true);
        this.dialogue.gameObject.SetActive(true);
        this.dialogue.text = string.Format(dialogue, name);
    }

    public void HideDialogueUI()
    {
        dialogueNode.gameObject.SetActive(false);
    }

    public void HideText()
    {
        dialogue.gameObject.SetActive(false);
    }

    public void HideUI()
    {
        contentNode.SetActive(false);
    }

    public void ShowUI()
    {
        contentNode.SetActive(true);
    }

}
