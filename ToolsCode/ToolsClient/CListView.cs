using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MovementType = UnityEngine.UI.ScrollRect.MovementType;

internal interface IListViewHandler
{
    void Refresh(int count, int beginIndex);
    void ForceRefresh();
    void JumpToIndex(int index);
    void JumpToPos(Vector2 pos);
}

public enum ScrollAxis
{
    Horizontal,
    Vertical
}

[SelectionBase]
[AddComponentMenu("UI/CList View", 38)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public sealed class CListView :
    UIBehaviour,
    IInitializePotentialDragHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler,
    IScrollHandler,
    ICanvasElement,
    ILayoutElement,
    ILayoutGroup,
    IListViewHandler
{
    #region ItemContainer
    private abstract class ItemContainer
    {
        #region Public Property
        public int DataIndex { get { return m_DataIndex; } }
        public int ItemIndex { get { return m_ItemIndex; } }
        public Vector2 SizeDelta { get { return m_Container.sizeDelta; } }
        public Vector2 AnchoredPosition
        {
            get { return m_Container.anchoredPosition; }
            set { m_Container.anchoredPosition = value; }
        }
        public bool Active
        {
            get { return m_Container.gameObject.activeSelf; }
            set { m_Container.gameObject.SetActive(value); }
        }
        #endregion

        #region Public Methods
        public void AddRefreshAction(Action<int, int> fillDataAction)
        {
            m_FillDataAction = fillDataAction;
        }
        public GameObject Container { get { return m_Container.gameObject; } }
        public ItemContainer(GameObject item, int itemIndex, Action<int, int> fillDataAction)
        {
            m_Container = item.transform as RectTransform;
            m_ItemIndex = itemIndex;
            m_FillDataAction = fillDataAction;
        }

        public virtual void FillData(int index)
        {
            m_DataIndex = index;
        }

        public virtual void Destory()
        {
            m_FillDataAction = null;
            DestroyImmediate(m_Container.gameObject);
        }
        #endregion

        #region Internal Member
        protected RectTransform m_Container = null;
        protected int m_ItemIndex = 0;
        protected int m_DataIndex = -1;
        protected Action<int, int> m_FillDataAction = null;
        #endregion
    }
    private class SingleItemContainer : ItemContainer
    {
        public SingleItemContainer(GameObject item, int itemIndex, Action<int, int> fillDataAction)
            : base(item, itemIndex, fillDataAction) { }

        public override void FillData(int index)
        {
            base.FillData(index);
            if (m_FillDataAction != null) m_FillDataAction(m_ItemIndex, m_DataIndex);
        }
    }
    private class MultiItemContainer : ItemContainer
    {
        public MultiItemContainer(GameObject item, int itemIndex, Action<int, int> fillDataAction) :
            base(item, itemIndex, fillDataAction)
        { }

        public override void FillData(int index)
        {
            base.FillData(index);
            int realItemIndex = m_ItemIndex * m_ItemList.Count;
            int realDataIndex = m_DataIndex * m_ItemList.Count;
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                if (m_FillDataAction != null)
                    m_FillDataAction(realItemIndex + i, m_DataIndex < 0 ? realDataIndex - i : realDataIndex + i);
            }
        }
        public GameObject GetItem(int rowIndex)
        {
            if (m_ItemList.Count > rowIndex)
                return m_ItemList[rowIndex];
            else
                return null;
        }
        public void AddItem(GameObject go)
        {
            m_ItemList.Add(go);
        }

        public override void Destory()
        {
            m_ItemList.ForEach((item) => { DestroyImmediate(item); });
            m_ItemList.Clear();
            base.Destory();
        }

        private List<GameObject> m_ItemList = new List<GameObject>();
    }
    #endregion

    #region Public Property
    public float minWidth { get { return -1; } }
    public float preferredWidth { get { return -1; } }
    public float flexibleWidth { get { return -1; } }
    public float minHeight { get { return -1; } }
    public float preferredHeight { get { return -1; } }
    public float flexibleHeight { get { return -1; } }
    public int layoutPriority { get { return -1; } }
    public List<GameObject> ItemList=new List<GameObject>();
    public List<GameObject> ContainerList=new List<GameObject>();
    #endregion

    #region Interface Methods
    public void CalculateLayoutInputHorizontal()
    {

    }

    public void CalculateLayoutInputVertical()
    {

    }

    public void GraphicUpdateComplete()
    {

    }

    public void LayoutComplete()
    {

    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        m_Velocity = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!IsActive()) return;

        UpdateBounds();

        m_PointerStartLocalCursor = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ViewPort, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
        m_ContentStartPosition = m_Content.anchoredPosition;
        m_Dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (!IsActive()) return;

        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ViewPort, eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        UpdateBounds();

        var pointerDelta = localCursor - m_PointerStartLocalCursor;
        Vector2 position = m_ContentStartPosition + pointerDelta;

        // Offset to get content into place in the view.
        Vector2 offset = CalculateOffset(position - m_Content.anchoredPosition);
        position += offset;
        if (m_MovementType == MovementType.Elastic)
        {
            if (offset.x != 0)
                position.x = position.x - RubberDelta(offset.x, m_ViewBounds.size.x);
            if (offset.y != 0)
                position.y = position.y - RubberDelta(offset.y, m_ViewBounds.size.y);
        }

        SetContentAnchoredPosition(position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        m_Dragging = false;
    }

    public void OnScroll(PointerEventData eventData)
    {

    }

    public void Rebuild(CanvasUpdate executing)
    {
        if (executing == CanvasUpdate.PostLayout)
        {
            UpdateBounds();
            UpdatePrevData();
            m_HasRebuiltLayout = true;
        }
    }

    public void SetLayoutHorizontal()
    {

    }

    public void SetLayoutVertical()
    {

    }
    #endregion

    #region Public Methods
    public override bool IsActive()
    {
        return base.IsActive() && m_Content != null;
    }
    public void ClearItem()
    {
        ItemList.Clear();
        ContainerList.Clear();
        CleanAllItem();
        CleanContent();
        CleanViewPort();
    }
    public bool AddEvent(Action<int, GameObject> initAction, Action<int, int> refreshAction)
    {
        if (m_ItemTemplate == null) return false;
        CoreUtility.SetActive(m_ItemTemplate, false);
        m_ItemInitEvent = initAction;
        m_ItemRefreshEvent = refreshAction;
        AddItemEvent(m_ItemTemplate);
        isLoad = true;
        return true;
    }
    public bool OnActivation(Action<int, GameObject> initAction, Action<int, int> refreshAction)
    {
        if (m_ItemTemplate == null) return false;
        CoreUtility.SetActive(m_ItemTemplate, false);
        m_ItemInitEvent = initAction;
        m_ItemRefreshEvent = refreshAction;
        if (IsClean)
            OnPrepare();
        DoActivation();
        this.isLoad = true;
        return true;
    }
    public bool OnPreView(Action<int, GameObject> initAction, Action<int, int> refreshAction)
    {
        if (m_ItemTemplate == null) return false;
        IsPreView = true;
        CoreUtility.SetActive(m_ItemTemplate, false);
        m_ItemInitEvent = initAction;
        m_ItemRefreshEvent = refreshAction;
        OnPrepare();
        DoActivation();
        this.isLoad = true;
        return true;
    }
    public bool OnBuildItem()
    {
        if (m_ItemTemplate == null) return false;
        IsPreView = false;
        CoreUtility.SetActive(m_ItemTemplate, false);
        OnPrepare();
        CreateItem(m_ItemTemplate);
        AdjustContentAnchor();
        this.isLoad = true;
        return true;
    }

    /// <summary>
    /// 异步创建Item
    /// </summary>
    /// <param name="initAction"></param>
    /// <param name="refreshAction"></param>
    /// <param name="LoadBackAction"></param>
    /// <returns></returns>
    public bool OnAsynActivation(Action<int, GameObject> initAction, Action<int, int> refreshAction, Action LoadBackAction)
    {
        if (m_ItemTemplate == null) return false;
        CoreUtility.SetActive(m_ItemTemplate, false);
        m_ItemInitEvent = initAction;
        m_ItemRefreshEvent = refreshAction;
        m_ItemLoadBackEvent = LoadBackAction;
        if (!IsClean)
        {
            Debug.LogWarning("ClistView is not clean");
        }
        OnPrepare();
        StartCoroutine(AsynCreateAndRefresh(m_ItemTemplate));
        return true;
    }

    public void AsynRefresh(int count, int beginIndex,Action RefershFinshEvent)
    {
        m_RefreshFinishEvent = RefershFinshEvent;
        StartCoroutine(AsynRefreshContainer(count, beginIndex));
        
    }
    IEnumerator AsynRefreshContainer(int count, int beginIndex)
    {
        m_ItemCount = count;
        m_BeginIndex = 0;
        m_EndIndex = 0;

        int realBeginIndex = m_Multi ?
            Mathf.FloorToInt((float)beginIndex / (float)(m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount)) : beginIndex;

        for (int i = 0; i < m_Items.Count; i++)
        {
            int dataIndex = realBeginIndex + i - 2;
            ItemContainer container = m_Items[i];
            container.FillData(dataIndex);
            container.AnchoredPosition = GetItemPosition(dataIndex);
            AdjustContainerActive(container);

            if (dataIndex >= 0) m_EndIndex = i;
            yield return 0.033f;
        }

        AdjustContentSize(realBeginIndex);
        if (m_RefreshFinishEvent!=null)
            m_RefreshFinishEvent();
    }
    public void Refresh(int count, int beginIndex)
    {
        m_ItemCount = count;
        m_BeginIndex = 0;
        m_EndIndex = 0;

        int realBeginIndex = m_Multi ?
            Mathf.FloorToInt((float)beginIndex / (float)(m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount)) : beginIndex;
        for (int i = 0; i < m_Items.Count; i++)
        {
            int dataIndex = realBeginIndex + i - 2;
            ItemContainer container = m_Items[i];
            container.FillData(dataIndex);
            container.AnchoredPosition = GetItemPosition(dataIndex);
            AdjustContainerActive(container);

            if (dataIndex >= 0) m_EndIndex = i;
        }

        AdjustContentSize(realBeginIndex);
    }

    public void ForceRefresh()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            ItemContainer container = m_Items[i];
            container.FillData(container.DataIndex);
        }
    }

    public void JumpToIndex(int index)
    {
        int realBeginIndex = m_Multi ?
            Mathf.FloorToInt((float)index / (float)(m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount)) : index;

        Vector2 pos = CalculationContentPositionByIndex(realBeginIndex);
        JumpToPos(pos);
    }

    public void JumpToPos(Vector2 pos)
    {
        m_Velocity = Vector2.zero;
        m_JPos = pos;
        m_Jumping = true;
    }
    #endregion

    #region Unity Methods
    private bool Vector2EqualsEx(Vector2 a, Vector2 b)
    {
        int v_a = (int)(a.x * 1000);
        int v_b = (int)(b.x * 1000);
        if (Mathf.Abs(v_a - v_b) > 2000) return false;

        v_a = (int)(a.y * 1000);
        v_b = (int)(b.y * 1000);

        return Mathf.Abs(v_a - v_b) < 2000;
    }

    private void LateUpdate()
    {
        if (!isLoad)
            return;
        if (m_Jumping)
        {
            Vector2 p = Vector2.Lerp(m_Content.anchoredPosition, m_JPos, Time.deltaTime);
            SetContentAnchoredPosition(p);
            if (Vector2EqualsEx(m_Content.anchoredPosition, m_JPos))
            {
                m_Jumping = false;
                SetContentAnchoredPosition(m_JPos);
            }
            return;
        }

        if (!m_Content)
        {
            return;
        }
        EnsureLayoutHasRebuilt();
        UpdateBounds();
        float deltaTime = Time.unscaledDeltaTime;
        Vector2 offset = CalculateOffset(Vector2.zero);
        if (!m_Dragging && (offset != Vector2.zero || m_Velocity != Vector2.zero))
        {
            Vector2 position = m_Content.anchoredPosition;
            for (int axis = 0; axis < 2; axis++)
            {
                // Apply spring physics if movement is elastic and content has an offset from the view.
                if (m_MovementType == MovementType.Elastic && offset[axis] != 0)
                {
                    float speed = m_Velocity[axis];
                    position[axis] = Mathf.SmoothDamp(m_Content.anchoredPosition[axis], m_Content.anchoredPosition[axis] + offset[axis], ref speed, m_Elasticity, Mathf.Infinity, deltaTime);
                    if (Mathf.Abs(speed) < 1)
                        speed = 0;
                    m_Velocity[axis] = speed;
                }
                // Else move content according to velocity with deceleration applied.
                else if (m_Inertia)
                {
                    m_Velocity[axis] *= Mathf.Pow(m_DecelerationRate, deltaTime);
                    if (Mathf.Abs(m_Velocity[axis]) < 1)
                        m_Velocity[axis] = 0;
                    position[axis] += m_Velocity[axis] * deltaTime;
                }
                // If we have neither elaticity or friction, there shouldn't be any velocity.
                else
                {
                    m_Velocity[axis] = 0;
                }
            }

            if (m_MovementType == MovementType.Clamped)
            {
                offset = CalculateOffset(position - m_Content.anchoredPosition);
                position += offset;
            }

            SetContentAnchoredPosition(position);
        }

        if (m_Dragging && m_Inertia)
        {
            Vector3 newVelocity = (m_Content.anchoredPosition - m_PrevPosition) / deltaTime;
            m_Velocity = Vector3.Lerp(m_Velocity, newVelocity, deltaTime * 10);
        }

        if (m_ViewBounds != m_PrevViewBounds || m_ContentBounds != m_PrevContentBounds || m_Content.anchoredPosition != m_PrevPosition)
        {
            //UpdateScrollbars(offset);
            UISystemProfilerApi.AddMarker("ScrollRect.value", this);
            //m_OnValueChanged.Invoke(normalizedPosition);
            UpdatePrevData();
        }
        // UpdateScrollbarVisibility();
    }

    protected override void OnDisable()
    {
        CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
        m_HasRebuiltLayout = false;
        m_Velocity = Vector2.zero;
        m_Jumping = false;
        LayoutRebuilder.MarkLayoutForRebuild(gameObject.transform as RectTransform);
        StopAllCoroutines();
        CancelInvoke();
        base.OnDisable();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        m_ItemRefreshEvent = null;
        m_ItemInitEvent = null;
        CleanContent();
        StopAllCoroutines();
        CancelInvoke();
        CleanViewPort();
        this.ItemList.Clear();
        this.m_Items.Clear();
    }
    #endregion

    #region Internal Methods
    private void OnPrepare()
    {
        CleanAllItem();
        CleanContent();
        CleanViewPort();
        CreateViewPort();
        CreateContent();
    }
    private bool IsPreView = true;
    bool isLoad = false;
    public bool IsClean { get { return m_Content==null && m_ViewPort==null &&(m_Items==null || m_Items.Count==0); } }
    public bool IsLoad { get { return isLoad; } }
    private void DoActivation()
    {
        CreateItem(m_ItemTemplate);
        AdjustContentAnchor();
    }

    private void AdjustSize()
    {
        Vector2 itemSize = GetContainerSize();
        RectTransform rectTrans = gameObject.transform as RectTransform;

        if (m_Multi)
        {
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Axis == ScrollAxis.Horizontal ?
                m_Padding.left + m_HCount * itemSize.x + (m_HCount - 1) * m_Spacing.x + m_Padding.right :
                itemSize.x + m_Padding.left + m_Padding.right);

            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_Axis == ScrollAxis.Horizontal ?
                itemSize.y + m_Padding.top + m_Padding.bottom :
                m_Padding.top + m_VCount * itemSize.y + (m_VCount - 1) * m_Spacing.y + m_Padding.bottom);
        }
        else
        {
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            m_Padding.left + m_HCount * itemSize.x + (m_HCount - 1) * m_Spacing.x + m_Padding.right);

            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                m_Padding.top + m_VCount * itemSize.y + (m_VCount - 1) * m_Spacing.y + m_Padding.bottom);
        }
    }

    private void CreateViewPort()
    {
        GameObject particleGo;
        if (IsPreView)
        {
            particleGo = new GameObject("ParticleClipper")
            {
                hideFlags = HideFlags.DontSave
            };
        }
        else
        {
            particleGo = new GameObject("ParticleClipper")
            {
                hideFlags = HideFlags.None
            };
        }

        CImage cImage = particleGo.AddComponent<CImage>();
        cImage.sprite = m_ParticleClippersprite;
        cImage.material = m_ParticleClipperMaterial;
        if (Application.isEditor)
        {
            cImage.material.shader = Shader.Find("MOYU/Clipper");
        }
        cImage.raycastTarget = false;
        m_ParticleClipperNode = particleGo.transform as RectTransform;
        m_ParticleClipperNode.SetParent(gameObject.transform);
        m_ParticleClipperNode.localScale = Vector3.one;
        m_ParticleClipperNode.localRotation = Quaternion.identity;
        m_ParticleClipperNode.anchorMin = Vector2.zero;
        m_ParticleClipperNode.anchorMax = Vector2.one;
        m_ParticleClipperNode.pivot = new Vector2(0.5f, 0.5f);
        m_ParticleClipperNode.anchoredPosition = Vector2.zero;
        m_ParticleClipperNode.sizeDelta = Vector2.zero;
        if (m_Axis == ScrollAxis.Horizontal)
        {
            m_ParticleClipperNode.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 5000);
        }
        else
        {
            m_ParticleClipperNode.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 5000);
        }
        GameObject particleMaskGo;
        if (IsPreView)
        {
            particleMaskGo = new GameObject("ParticleMask")
            {
                hideFlags = HideFlags.DontSave
            };
        }
        else
        {
            particleMaskGo = new GameObject("ParticleMask")
            {
                hideFlags = HideFlags.None
            };
        }

        CImage cImage2 = particleMaskGo.AddComponent<CImage>();
        cImage2.sprite = m_ParticleClippersprite;

        m_ParticleMaskNode = particleMaskGo.transform as RectTransform;
        m_ParticleMaskNode.SetParent(m_ParticleClipperNode);
        m_ParticleMaskNode.localScale = Vector3.one;
        m_ParticleMaskNode.localRotation = Quaternion.identity;
        m_ParticleMaskNode.anchorMin = Vector2.zero;
        m_ParticleMaskNode.anchorMax = Vector2.one;
        m_ParticleMaskNode.pivot = new Vector2(0.5f, 0.5f);
        m_ParticleMaskNode.anchoredPosition = Vector2.zero;
        m_ParticleMaskNode.sizeDelta = Vector2.zero;

        RectTransform rectTrans = gameObject.transform as RectTransform;
        if (m_Axis == ScrollAxis.Horizontal)
        {
            m_ParticleMaskNode.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTrans.sizeDelta.x);
        }
        else
        {
            m_ParticleMaskNode.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTrans.sizeDelta.y);
        }

        Mask mask1 = particleMaskGo.AddComponent<Mask>();
        mask1.showMaskGraphic = false;
        //CImage image1 = particleMaskGo.AddComponent<CImage>();

        GameObject viewPort;
        if (IsPreView)
        {
            viewPort = new GameObject("ViewPort")
            {
                hideFlags = HideFlags.DontSave,
            };
        }
        else
        {
            viewPort = new GameObject("ViewPort")
            {
                hideFlags = HideFlags.None,
            };
        }
        m_ViewPort = viewPort.AddComponent<RectTransform>();
        m_ViewPort.SetParent(m_ParticleMaskNode);
        m_ViewPort.localScale = Vector3.one;
        m_ViewPort.localRotation = Quaternion.identity;
        m_ViewPort.anchorMin = Vector2.zero;
        m_ViewPort.anchorMax = Vector2.one;
        m_ViewPort.pivot = new Vector2(0.5f, 0.5f);
        m_ViewPort.anchoredPosition = Vector2.zero;
        m_ViewPort.sizeDelta = Vector2.zero;
        Mask mask = m_ViewPort.gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;
        CImage image = m_ViewPort.gameObject.AddComponent<CImage>();
        image.sprite = m_ParticleClippersprite;
    }

    private void CleanViewPort()
    {
        if (m_ViewPort != null)
        {
            DestroyImmediate(m_ViewPort.gameObject);
            m_ViewPort = null;
        }

        if (m_ParticleMaskNode != null)
        {
            DestroyImmediate(m_ParticleMaskNode.gameObject);
            m_ParticleMaskNode = null;
        }

        if (m_ParticleClipperNode != null)
        {
            DestroyImmediate(m_ParticleClipperNode.gameObject);
            m_ParticleClipperNode = null;
        }
    }

    private void CreateContent()
    {
        GameObject content;
        if (IsPreView)
        {
            content = new GameObject("Content")
            {
                hideFlags = HideFlags.DontSave
            };
        }
        else
        {
            content = new GameObject("Content")
            {
                hideFlags = HideFlags.None
            };
        }
        m_Content = content.AddComponent<RectTransform>();
        m_Content.SetParent(m_ViewPort);
        m_Content.localScale = Vector3.one;
        m_Content.localRotation = Quaternion.identity;
        AdjustContentAnchor();
    }

    private void CleanContent()
    {
        CleanAllItem();
        if (m_Content != null)
        {
            DestroyImmediate(m_Content.gameObject);
            m_Content = null;
        }
    }

    private void AdjustContentAnchor()
    {
        if (m_Axis == ScrollAxis.Horizontal)
        {
            m_Content.anchorMin = Vector2.zero;
            m_Content.anchorMax = Vector2.up;
            m_Content.pivot = Vector2.up;
            m_Content.anchoredPosition = Vector2.zero;
            m_Content.sizeDelta = Vector2.zero;
        }
        else
        {
            m_Content.anchorMin = Vector2.up;
            m_Content.anchorMax = Vector2.one;
            m_Content.pivot = Vector2.up;
            m_Content.anchoredPosition = Vector2.zero;
            m_Content.sizeDelta = Vector2.zero;
        }
    }

    private void AdjustContentSize(int beginIndex)
    {
        Vector2 itemSize = GetContainerSize();
        int totalCount = m_Multi ?
            Mathf.CeilToInt((float)m_ItemCount / (float)(m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount)) : m_ItemCount;

        if (m_Axis == ScrollAxis.Horizontal)
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                m_Padding.left + itemSize.x * totalCount + (totalCount - 1) * m_Spacing.x + m_Padding.right);
        }
        else
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                m_Padding.top + itemSize.y * totalCount + (totalCount - 1) * m_Spacing.y + m_Padding.bottom);
        }

        Vector2 pos = CalculationContentPositionByIndex(beginIndex);
        SetContentAnchoredPosition(pos);
    }

    private Vector2 CalculationContentPositionByIndex(int beginIndex)
    {
        if (m_Axis == ScrollAxis.Horizontal)
        {
            return new Vector2()
            {
                x = (GetItemPosition(beginIndex).x - m_Padding.left) * -1,
                y = 0
            };
        }
        else
        {
            return new Vector2()
            {
                x = 0,
                y = (GetItemPosition(beginIndex).y + m_Padding.top) * -1,
            };
        }
    }
    IEnumerator AsynCreateAndRefresh(GameObject itemTemplate)
    {
        CleanAllItem();

        if (itemTemplate == null) yield break;
        int totalCount = 0;

        if (m_Multi)
        {
            Vector2 itemSize = (itemTemplate.transform as RectTransform).sizeDelta;
            totalCount = m_Axis == ScrollAxis.Horizontal ? m_HCount + 4 : m_VCount + 4;
            int childCount = m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount;
            //StartCoroutine();
            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = new GameObject("Container");
                RectTransform rectTransform = item.AddComponent<RectTransform>();
                rectTransform.SetParent(m_Content);
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;
                AdjustItemLayout(item, Vector2.zero);
                rectTransform.sizeDelta = new Vector2()
                {
                    x = m_Axis == ScrollAxis.Horizontal ?
                        itemSize.x : childCount * itemSize.x + (childCount - 1) * m_Spacing.x,
                    y = m_Axis == ScrollAxis.Horizontal ?
                        childCount * itemSize.y + (childCount - 1) * m_Spacing.y : itemSize.y
                };

                MultiItemContainer container = new MultiItemContainer(item, i, m_ItemRefreshEvent);

                for (int j = 0; j < childCount; j++)
                {
                    GameObject child = Instantiate(m_ItemTemplate, item.transform);
                    AdjustItemLayout(child, new Vector2()
                    {
                        x = m_Axis == ScrollAxis.Horizontal ? 0 : j * itemSize.x + j * m_Spacing.x,
                        y = m_Axis == ScrollAxis.Horizontal ? (j * itemSize.y + j * m_Spacing.y) * -1 : 0
                    });
                    m_ItemInitEvent(i * childCount + j, child);
                    container.AddItem(child);

                    yield return 0.033f;
                }
                m_Items.Add(container);
            }
        }
        else
        {
            totalCount = m_Axis == ScrollAxis.Horizontal ?
                    (m_HCount + 4) * m_VCount : (m_VCount + 4) * m_HCount;
            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = Instantiate(m_ItemTemplate, m_Content);
                AdjustItemLayout(item, Vector2.zero);
                m_Items.Add(new SingleItemContainer(item, i, m_ItemRefreshEvent));
                m_ItemInitEvent?.Invoke(i, item);
                yield return 0.033f;
            }
        }
        AdjustContentAnchor();
        if (m_ItemLoadBackEvent != null)
            m_ItemLoadBackEvent();
        isLoad = true;
    }
    private void BulidItem(GameObject itemTemplate)
    {
        if (itemTemplate == null) return;
        int totalCount = 0;

        if (m_Multi)
        {
            Vector2 itemSize = (itemTemplate.transform as RectTransform).sizeDelta;
            totalCount = m_Axis == ScrollAxis.Horizontal ? m_HCount + 4 : m_VCount + 4;
            int childCount = m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount;
            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = new GameObject("Container");
                RectTransform rectTransform = item.AddComponent<RectTransform>();
                rectTransform.SetParent(m_Content);
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;
                AdjustItemLayout(item, Vector2.zero);
                rectTransform.sizeDelta = new Vector2()
                {
                    x = m_Axis == ScrollAxis.Horizontal ?
                        itemSize.x : childCount * itemSize.x + (childCount - 1) * m_Spacing.x,
                    y = m_Axis == ScrollAxis.Horizontal ?
                        childCount * itemSize.y + (childCount - 1) * m_Spacing.y : itemSize.y
                };

                MultiItemContainer container = new MultiItemContainer(item, i, m_ItemRefreshEvent);

                for (int j = 0; j < childCount; j++)
                {
                    GameObject child = Instantiate(m_ItemTemplate, item.transform);
                    AdjustItemLayout(child, new Vector2()
                    {
                        x = m_Axis == ScrollAxis.Horizontal ? 0 : j * itemSize.x + j * m_Spacing.x,
                        y = m_Axis == ScrollAxis.Horizontal ? (j * itemSize.y + j * m_Spacing.y) * -1 : 0
                    });
                   if (m_ItemInitEvent != null) m_ItemInitEvent.Invoke(i * childCount + j, child); 
                    container.AddItem(child);
                }
                m_Items.Add(container);
            }
        }
        else
        {
            totalCount = m_Axis == ScrollAxis.Horizontal ?
                    (m_HCount + 4) * m_VCount : (m_VCount + 4) * m_HCount;
            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = Instantiate(m_ItemTemplate, m_Content);
                AdjustItemLayout(item, Vector2.zero);
                m_Items.Add(new SingleItemContainer(item, i, m_ItemRefreshEvent));
                 if (m_ItemInitEvent != null) m_ItemInitEvent.Invoke(i, item); 
            }
        }
    }
    private void CreateItem(GameObject itemTemplate)
    {
        CleanAllItem();
        if (itemTemplate == null) return;
        int totalCount = 0;

        if (m_Multi)
        {
            Vector2 itemSize = (itemTemplate.transform as RectTransform).sizeDelta;
            totalCount = m_Axis == ScrollAxis.Horizontal ? m_HCount + 4 : m_VCount + 4;
            int childCount = m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount;

            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = new GameObject("Container");
                RectTransform rectTransform = item.AddComponent<RectTransform>();
                rectTransform.SetParent(m_Content);
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;
                AdjustItemLayout(item, Vector2.zero);
                rectTransform.sizeDelta = new Vector2()
                {
                    x = m_Axis == ScrollAxis.Horizontal ?
                        itemSize.x : childCount * itemSize.x + (childCount - 1) * m_Spacing.x,
                    y = m_Axis == ScrollAxis.Horizontal ?
                        childCount * itemSize.y + (childCount - 1) * m_Spacing.y : itemSize.y
                };

                MultiItemContainer container = new MultiItemContainer(item, i, m_ItemRefreshEvent);
                this.ContainerList.Add(item);
                for (int j = 0; j < childCount; j++)
                {
                    GameObject child = Instantiate(m_ItemTemplate, item.transform);
                    AdjustItemLayout(child, new Vector2()
                    {
                        x = m_Axis == ScrollAxis.Horizontal ? 0 : j * itemSize.x + j * m_Spacing.x,
                        y = m_Axis == ScrollAxis.Horizontal ? (j * itemSize.y + j * m_Spacing.y) * -1 : 0
                    });
                    m_ItemInitEvent?.Invoke(i * childCount + j, child);
                    container.AddItem(child);
                    this.ItemList.Add(child);
                }
                m_Items.Add(container);

            }


        }
        else
        {
            totalCount = m_Axis == ScrollAxis.Horizontal ?
                   (m_HCount + 4) * m_VCount : (m_VCount + 4) * m_HCount;

            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = Instantiate(m_ItemTemplate, m_Content);
                AdjustItemLayout(item, Vector2.zero);
                m_Items.Add(new SingleItemContainer(item, i, m_ItemRefreshEvent));
                this.ItemList.Add(item);
                m_ItemInitEvent?.Invoke(i, item);
            }

        }
    }
    private void AddItemEvent(GameObject itemTemplate)
    {
        if (itemTemplate == null) return;
        int totalCount = 0;

    

        if (m_Multi)
        {
            Vector2 itemSize = (itemTemplate.transform as RectTransform).sizeDelta;
            totalCount = m_Axis == ScrollAxis.Horizontal ? m_HCount + 4 : m_VCount + 4;
            int childCount = m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount;
            
            for (int i = 0; i < totalCount; i++)
            {

                MultiItemContainer container = new MultiItemContainer(this.ContainerList[i], i, m_ItemRefreshEvent);

                for (int j = 0; j < childCount; j++)
                {
                    GameObject child = this.ItemList[i * childCount + j];
                    AdjustItemLayout(child, new Vector2()
                    {
                        x = m_Axis == ScrollAxis.Horizontal ? 0 : j * itemSize.x + j * m_Spacing.x,
                        y = m_Axis == ScrollAxis.Horizontal ? (j * itemSize.y + j * m_Spacing.y) * -1 : 0
                    });
                    m_ItemInitEvent?.Invoke(i * childCount + j, child);
                    container.AddItem(child);
                }
                m_Items.Add(container);
            }
        }
        else
        {
            totalCount = m_Axis == ScrollAxis.Horizontal ?
                   (m_HCount + 4) * m_VCount : (m_VCount + 4) * m_HCount;

            for (int i = 0; i < totalCount; i++)
            {
                GameObject item = this.ItemList[i];
                AdjustItemLayout(item, Vector2.zero);
                m_Items.Add(new SingleItemContainer(item, i, m_ItemRefreshEvent));
                m_ItemInitEvent?.Invoke(i, item);
            }

        }
      
    }

    private Vector2 GetItemPosition(int idx)
    {
        Vector2 itemSize = GetContainerSize();
        return new Vector2()
        {
            x = m_Axis == ScrollAxis.Horizontal ?
                m_Padding.left + itemSize.x * idx + m_Spacing.x * idx : m_Padding.left,
            y = (m_Axis == ScrollAxis.Horizontal ?
            m_Padding.top : m_Padding.top + +itemSize.y * idx + m_Spacing.y * idx) * -1
        };
    }

    private void AdjustItemLayout(GameObject go, Vector2 anchoredPosition)
    {
        if (IsPreView)
            go.hideFlags = HideFlags.DontSave;
        else
            go.hideFlags = HideFlags.None;
        RectTransform rectTrans = go.transform as RectTransform;
        rectTrans.localScale = Vector3.one;
        rectTrans.localRotation = Quaternion.identity;
        rectTrans.anchorMin = Vector2.up;
        rectTrans.anchorMax = Vector2.up;
        rectTrans.pivot = Vector2.up;
        rectTrans.anchoredPosition = anchoredPosition;
    }

    private void AdjustContainerActive(ItemContainer container)
    {
        if (container.DataIndex < 0)
        {
            container.Active = false;
            return;
        }

        if (m_Multi)
        {
            int realDataIndex = (m_Axis == ScrollAxis.Horizontal ? m_VCount : m_HCount) * container.DataIndex;
            if (realDataIndex >= m_ItemCount)
            {
                container.Active = false;
                return;
            }
        }
        else
        {
            if (container.DataIndex >= m_ItemCount)
            {
                container.Active = false;
                return;
            }
        }

        container.Active = true;
    }

    private void CleanAllItem()
    {
        m_Items.ForEach((item) => { item.Destory(); });
        m_Items.Clear();
    }

    private void UpdatePrevData()
    {
        if (m_Content == null)
            m_PrevPosition = Vector2.zero;
        else
            m_PrevPosition = m_Content.anchoredPosition;

        m_PrevViewBounds = m_ViewBounds;
        m_PrevContentBounds = m_ContentBounds;
    }

    private void EnsureLayoutHasRebuilt()
    {
        if (!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
            Canvas.ForceUpdateCanvases();
    }

    private void UpdateBounds()
    {
        m_ViewBounds = new Bounds(m_ViewPort.rect.center, m_ViewPort.rect.size);
        m_ContentBounds = GetBounds();

        if (m_Content == null)
            return;

        Vector3 contentSize = m_ContentBounds.size;
        Vector3 contentPos = m_ContentBounds.center;
        var contentPivot = m_Content.pivot;
        AdjustBounds(ref m_ViewBounds, ref contentPivot, ref contentSize, ref contentPos);
        m_ContentBounds.size = contentSize;
        m_ContentBounds.center = contentPos;

        if (m_MovementType == MovementType.Clamped)
        {
            // Adjust content so that content bounds bottom (right side) is never higher (to the left) than the view bounds bottom (right side).
            // top (left side) is never lower (to the right) than the view bounds top (left side).
            // All this can happen if content has shrunk.
            // This works because content size is at least as big as view size (because of the call to InternalUpdateBounds above).
            Vector2 delta = Vector2.zero;
            if (m_ViewBounds.max.x > m_ContentBounds.max.x)
            {
                delta.x = Math.Min(m_ViewBounds.min.x - m_ContentBounds.min.x, m_ViewBounds.max.x - m_ContentBounds.max.x);
            }
            else if (m_ViewBounds.min.x < m_ContentBounds.min.x)
            {
                delta.x = Math.Max(m_ViewBounds.min.x - m_ContentBounds.min.x, m_ViewBounds.max.x - m_ContentBounds.max.x);
            }

            if (m_ViewBounds.min.y < m_ContentBounds.min.y)
            {
                delta.y = Math.Max(m_ViewBounds.min.y - m_ContentBounds.min.y, m_ViewBounds.max.y - m_ContentBounds.max.y);
            }
            else if (m_ViewBounds.max.y > m_ContentBounds.max.y)
            {
                delta.y = Math.Min(m_ViewBounds.min.y - m_ContentBounds.min.y, m_ViewBounds.max.y - m_ContentBounds.max.y);
            }
            if (delta.sqrMagnitude > float.Epsilon)
            {
                contentPos = m_Content.anchoredPosition + delta;
                if (m_Axis == ScrollAxis.Vertical)
                    contentPos.x = m_Content.anchoredPosition.x;
                else
                    contentPos.y = m_Content.anchoredPosition.y;
                AdjustBounds(ref m_ViewBounds, ref contentPivot, ref contentSize, ref contentPos);
            }
        }
    }

    private void SetContentAnchoredPosition(Vector2 position)
    {
        if (m_Axis == ScrollAxis.Vertical)
            position.x = m_Content.anchoredPosition.x;
        else
            position.y = m_Content.anchoredPosition.y;

        if (position != m_Content.anchoredPosition)
        {
            m_Content.anchoredPosition = position;
            UpdateBounds();
        }

        AdjustAllItemPosition();
    }

    private void AdjustAllItemPosition()
    {
        Vector2 itemSize = GetContainerSize();
        float distance = m_Axis == ScrollAxis.Horizontal ?
            Mathf.Abs(Mathf.Min(0, m_Content.anchoredPosition.x)) : Mathf.Abs(Mathf.Max(0, m_Content.anchoredPosition.y));

        int index = m_Axis == ScrollAxis.Horizontal ?
            Mathf.Max(0, Mathf.FloorToInt((distance - m_Padding.left) / (itemSize.x + m_Spacing.x))) :
            Mathf.Max(0, Mathf.FloorToInt((distance - m_Padding.top) / (itemSize.y + m_Spacing.y)));

        int offset = index - m_Items[m_BeginIndex].DataIndex;
        if (offset == 0) return;
        if (offset > 2)
        {
            int newDataIndex = m_Items[m_EndIndex].DataIndex + 1;
            ItemContainer container = m_Items[m_BeginIndex];
            container.AnchoredPosition = GetItemPosition(newDataIndex);
            container.FillData(newDataIndex);
            AdjustContainerActive(container);

            m_EndIndex = m_BeginIndex;
            m_BeginIndex = m_BeginIndex + 1 > m_Items.Count - 1 ? 0 : m_BeginIndex + 1;
        }
        else if (offset < 2)
        {
            int newDataIndex = m_Items[m_BeginIndex].DataIndex - 1;
            ItemContainer container = m_Items[m_EndIndex];
            container.AnchoredPosition = GetItemPosition(newDataIndex);
            container.FillData(newDataIndex);
            AdjustContainerActive(container);

            m_BeginIndex = m_EndIndex;
            m_EndIndex = m_EndIndex - 1 < 0 ? m_Items.Count - 1 : m_EndIndex - 1;
        }
    }

    internal static void AdjustBounds(ref Bounds viewBounds, ref Vector2 contentPivot, ref Vector3 contentSize, ref Vector3 contentPos)
    {
        Vector3 excess = viewBounds.size - contentSize;
        if (excess.x > 0)
        {
            contentPos.x -= excess.x * (contentPivot.x - 0.5f);
            contentSize.x = viewBounds.size.x;
        }
        if (excess.y > 0)
        {
            contentPos.y -= excess.y * (contentPivot.y - 0.5f);
            contentSize.y = viewBounds.size.y;
        }
    }

    private readonly Vector3[] m_Corners = new Vector3[4];

    private Vector2 GetContainerSize()
    {
        if (m_ItemTemplate == null) return Vector2.zero;
        if (m_Items.Count == 0) return Vector2.zero;
        return m_Items[0].SizeDelta;
    }

    private Bounds GetBounds()
    {
        if (m_Content == null)
            return new Bounds();
        m_Content.GetWorldCorners(m_Corners);
        var viewWorldToLocalMatrix = m_ViewPort.worldToLocalMatrix;
        return InternalGetBounds(m_Corners, ref viewWorldToLocalMatrix);
    }

    internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
    {
        var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        for (int j = 0; j < 4; j++)
        {
            Vector3 v = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[j]);
            vMin = Vector3.Min(v, vMin);
            vMax = Vector3.Max(v, vMax);
        }

        var bounds = new Bounds(vMin, Vector3.zero);
        bounds.Encapsulate(vMax);
        return bounds;
    }

    private Vector2 CalculateOffset(Vector2 delta)
    {
        Vector2 offset = Vector2.zero;
        if (m_MovementType == MovementType.Unrestricted)
            return offset;

        Vector2 min = m_ContentBounds.min;
        Vector2 max = m_ContentBounds.max;


        if (m_Axis == ScrollAxis.Horizontal)
        {
            min.x += delta.x;
            max.x += delta.x;
            if (min.x > m_ViewBounds.min.x)
                offset.x = m_ViewBounds.min.x - min.x;
            else if (max.x < m_ViewBounds.max.x)
                offset.x = m_ViewBounds.max.x - max.x;
        }
        else
        {
            min.y += delta.y;
            max.y += delta.y;
            if (max.y < m_ViewBounds.max.y)
                offset.y = m_ViewBounds.max.y - max.y;
            else if (min.y > m_ViewBounds.min.y)
                offset.y = m_ViewBounds.min.y - min.y;
        }

        return offset;
    }

    private float RubberDelta(float overStretching, float viewSize)
    {
        return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
    }

    #endregion

    #region Internal Member

    [SerializeField]
    private GameObject m_ItemTemplate = null;
    [SerializeField]
    private ScrollAxis m_Axis = ScrollAxis.Horizontal;
    [SerializeField]
    private bool m_Multi = false;

    [SerializeField]
    private int m_HCount = 1;
    [SerializeField]
    private int m_VCount = 1;

    [SerializeField]
    private RectOffset m_Padding = new RectOffset();
    [SerializeField]
    private Vector2 m_Spacing = Vector2.zero;

    [SerializeField]
    private MovementType m_MovementType = MovementType.Elastic;
    [SerializeField]
    private float m_Elasticity = 0.1f; // Only used for MovementType.Elastic
    [SerializeField]
    private bool m_Inertia = true;
    [SerializeField]
    private float m_DecelerationRate = 0.135f; // Only used when inertia is enabled

    [SerializeField]
    private Sprite m_ParticleClippersprite = null;
    [SerializeField]
    private Material m_ParticleClipperMaterial = null;
    [SerializeField]
    private RectTransform m_ParticleClipperNode = null;
    [SerializeField]
    private RectTransform m_ParticleMaskNode = null;
    [SerializeField]
    private RectTransform m_ViewPort = null;
    [SerializeField]
    private RectTransform m_Content = null;

    private Bounds m_ViewBounds;

    private Bounds m_ContentBounds;

    private Vector2 m_Velocity = Vector2.zero;

    private Vector2 m_PrevPosition = Vector2.zero;

    private Bounds m_PrevContentBounds;

    private Bounds m_PrevViewBounds;

    private Vector2 m_PointerStartLocalCursor = Vector2.zero;

    private Vector2 m_ContentStartPosition = Vector2.zero;

    private bool m_Dragging = false;

  
    private List<ItemContainer> m_Items = new List<ItemContainer>();

 
    private Action<int, GameObject> m_ItemInitEvent = null;
    private Action<int, int> m_ItemRefreshEvent = null;
    private Action m_ItemLoadBackEvent = null;
    private Action m_RefreshFinishEvent =null;
    private int m_ItemCount = 0;
    private int m_BeginIndex = 0;
    private int m_EndIndex = 0;

    private bool m_Jumping = false;
    private Vector2 m_JPos = Vector2.zero;

    [NonSerialized]
    private bool m_HasRebuiltLayout = false;
    #endregion
}
