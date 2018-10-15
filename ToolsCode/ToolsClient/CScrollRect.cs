using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CScrollRect : ScrollRect
{
    public Action<int> SelceteChanged;

    [SerializeField]
    private GridLayoutGroup group;
    [SerializeField]
    private int nextId = 2;
    [SerializeField]
    private int seleted = 2;
    [SerializeField]
    private int itemCount = 0;
    private bool draging = false; // 是否在拖动中
    private bool isReset = true;  // 是否已经重置过content的位置
    private float minY;// 第0项位置
    private float maxY;// 最后一项位置
    protected override void Start()
    {
        base.Start();
        if (!group)
        {
            Debug.Log("Content GridLayoutGroup is null");
            return;
        }
        #region [计算底部和顶部位子]
        minY = (0 - seleted) * group.cellSize.y;
        maxY = (itemCount - seleted) * group.cellSize.y;
        #endregion
        SetContentPostion();
    }

    public void SetCount(int count)
    {
        itemCount = count;
        maxY = (itemCount - seleted) * group.cellSize.y;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        draging = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        // 限定在第一个选项
        if (minY > content.anchoredPosition.y)
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, minY);
        // 限定在最后一个选项
        if (maxY < content.anchoredPosition.y)
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxY);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        draging = false;
        isReset = false;

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (!draging && !isReset)
        {
            // 限定在第一个选项
            if (minY > content.anchoredPosition.y)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, minY);
                velocity = Vector2.zero;
            }
            // 限定在最后一个选项
            if (maxY < content.anchoredPosition.y)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxY);
                velocity = Vector2.zero;
            }
            if (velocity.y == 0)
            {
                SetContentPostion();
            }
        }
    }

    public void OnNextItem(int idx)
    {
        nextId = idx;
    }

    public void OnOldExit()
    {
        SetContentPostion();
    }

    public void SetContentPostion()
    {
        int start = nextId - seleted;
        float offest = start * group.cellSize.y;
        Vector2 end = new Vector2(content.anchoredPosition.x, offest);
        content.anchoredPosition = end;
        isReset = true;
        if (SelceteChanged != null)
            SelceteChanged(nextId);
    }
}