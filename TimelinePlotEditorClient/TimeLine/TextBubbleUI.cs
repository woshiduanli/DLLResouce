using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBubbleUI : MonoBehaviour {

    public static TextBubbleUI Instance;
    public Canvas canvas;
    public SingleBubble bubble;

    private List<SingleBubble> bubblePools;
    public List<SingleBubble> bubbles;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        bubble = transform.Find("Bubble").gameObject.AddComponent<SingleBubble>();
        bubble.Init();
        bubble.gameObject.SetActive(false);
        bubbles = new List<SingleBubble>();
        bubblePools = new List<SingleBubble>();
    }


    public void ShowBubble(DialogueUIData data)
    {
        SingleBubble bubble;
        if (bubblePools.Count <= 0)
            bubble = Clone();
        else
        {
            bubble = bubblePools[0];
            bubblePools.RemoveAt(0);
        }
        bubbles.Add(bubble);
        bubble.Show( data);
    }

    public void Recycle(SingleBubble bubble)
    {
        if (bubbles.Contains(bubble))
            bubbles.Remove(bubble);
        if (!bubblePools.Contains(bubble))
            bubblePools.Add(bubble);
    }

    private SingleBubble Clone()
    {
        GameObject go = GameObject.Instantiate(bubble.gameObject) as GameObject;
        Transform t = go.transform;
        t.SetParent(bubble.transform.parent);
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        go.layer = bubble.gameObject.layer;
        return go.GetComponent<SingleBubble>();
    }
}


public class SingleBubble : MonoBehaviour
{
    public CText bubbleText;
    public Transform trans;

    public float offsetY = 0.1F;
    private float ShowTime;
    private float showedTime;
    private RoleObject roleObj;

    public void Init()
    {
        bubbleText = transform.Find("BubbleText").GetComponent<CText>();
        trans = gameObject.transform;
    }

    public void Show(DialogueUIData data)
    {
        bubbleText.text = data.Dialogue;
        ShowTime = data.Time;
        showedTime = 0;
        gameObject.SetActive(true);
        this.roleObj = World.Instance.GetAutoLoadRole(data.Id);
    }

    private void Update()
    {
        showedTime += Time.deltaTime;
        if (showedTime >= ShowTime)
        {
            Hide();
        }
    }

    private void LateUpdate()
    {
        if (roleObj != null)
        {
            Vector3 pos = roleObj.position;
            pos.y += roleObj.BoxCollider.y + offsetY;
            pos = Camera.main.WorldToScreenPoint(pos);
            trans.position = TextBubbleUI.Instance.canvas.worldCamera.ScreenToWorldPoint(pos);
            pos = trans.localPosition;
            pos.z = 0;
            trans.localPosition = pos;
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        TextBubbleUI.Instance.Recycle(this);
    }
}
