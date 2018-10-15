using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class DuplicatorComponent : MonoBehaviour
{
    #region Public Property
    //---------------------------------------------------------------------
    public GameObject templateObject
    {
        get { return m_TemplateObject; }
    }

    //---------------------------------------------------------------------
    public Transform parentNode
    {
        get { return m_ParentNode; }
    }

    //---------------------------------------------------------------------
    public int cloneCount
    {
        get { return m_CloneCount; }
    }
    #endregion

    #region Unity Method
    //---------------------------------------------------------------------
    private void Awake()
    {
        if(m_CloneCount != 0)
            Perform();
    }

    //---------------------------------------------------------------------
    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        int validCount = 0;
        for (int i = 0; i < m_CloneObjects.Count; ++i)
        {
            if (m_CloneObjects[i] != null)
            {
                ++validCount;
            }
        }
        if (m_TemplateObject != null && validCount != m_CloneCount)
        {
            Perform();
        }
    }

    private void Destory() {
        buildEvent = null;
    }
    #endregion

    #region Internal Method
    //---------------------------------------------------------------------
    protected virtual void OnCompleted(List<GameObject> gos)
    {

    }

    //---------------------------------------------------------------------
    private void Perform()
    {
        Transform parentNode = m_ParentNode;
        if (parentNode == null)
        {
            parentNode = transform;
        }

        for (int index = 0; index < m_CloneObjects.Count; ++index)
        {
            CoreUtility.DestroyImmediate(m_CloneObjects[index]);
        }
        m_CloneObjects.Clear();

        if (!Application.isPlaying)
        {
            if (m_TemplateObject == null || m_CloneCount == 0)
            {
                return;
            }

            for (int index = 0; index < m_CloneCount; ++index)
            {
                GameObject go = Instantiate(m_TemplateObject) as GameObject;
                go.AddComponent<DontSaveComponent>();
                CoreUtility.AttachChild(parentNode, go.transform);
                CoreUtility.NormalizeTransform(go);

                go.SetActive(m_InitialActive);
                m_CloneObjects.Add(go);
            }

            OnCompleted(m_CloneObjects);
        }
        else
        {
            if (m_TemplateObject == null || m_CloneCount == 0)
            {
                CoreUtility.DestroyImmediate(this);
                return;
            }

            for (int index = 0; index < m_CloneCount; ++index)
            {
                GameObject go = Instantiate(m_TemplateObject) as GameObject;
                CoreUtility.AttachChild(parentNode, go.transform);
                CoreUtility.NormalizeTransform(go);
                go.SetActive(m_InitialActive);
                m_CloneObjects.Add(go);
            }

            OnCompleted(m_CloneObjects);

            if (m_EventReceiver != null && m_EventMethod != string.Empty)
            {
                m_EventReceiver.SendMessage(
                    m_EventMethod,
                    m_CloneObjects.ToArray(),
                    SendMessageOptions.DontRequireReceiver);
            }
            
            if (m_OwnerCallback != string.Empty)
            {
                for (int index = 0; index < m_CloneObjects.Count; ++index)
                {
                    m_CloneObjects[index].BroadcastMessage(
                        m_OwnerCallback,
                        null,
                        SendMessageOptions.DontRequireReceiver);
                }
            }

            if (buildEvent != null) {
                buildEvent(m_CloneObjects.ToArray());
            }

            if (m_DestroyTemplate && !CoreUtility.IsAsset(m_TemplateObject))
            {
                CoreUtility.DestroyImmediate(m_TemplateObject);
            }

            CoreUtility.DestroyImmediate(this);
        }
    }

    public void Clone(int cout, bool initActive = false) {
        m_CloneCount = cout;
        m_InitialActive = initActive;
        Perform();
    }
    #endregion

    #region Internal Member
    //---------------------------------------------------------------------
    [SerializeField]
    private GameObject m_TemplateObject = null;

    //---------------------------------------------------------------------
    [SerializeField]
    private Transform m_ParentNode = null;

    //---------------------------------------------------------------------
    [SerializeField]
    private int m_CloneCount = 0;

    //---------------------------------------------------------------------
    [SerializeField]
    private bool m_InitialActive = false;

    //---------------------------------------------------------------------
    [SerializeField]
    private bool m_DestroyTemplate = true;

    //---------------------------------------------------------------------
    [SerializeField]
    private string m_OwnerCallback = string.Empty;

    //---------------------------------------------------------------------
    [SerializeField]
    private GameObject m_EventReceiver = null;

    //---------------------------------------------------------------------
    [SerializeField]
    private string m_EventMethod = string.Empty;

    //---------------------------------------------------------------------
    [SerializeField, HideInInspector]
    private List<GameObject> m_CloneObjects = new List<GameObject>();

    public Action<UnityEngine.Object[]> buildEvent = null;
    #endregion
}


[ExecuteInEditMode]
public class DontSaveComponent : MonoBehaviour
{

}