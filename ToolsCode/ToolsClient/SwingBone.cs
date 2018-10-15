using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("Physics/Swing Bone")]
public class SwingBone : MonoBehaviour
{
    public Transform Root;
    bool IsRoot = true;
    private Transform baseTransform;

    private Vector3 m_LastPos = Vector3.zero;
    private Quaternion m_LastRot = Quaternion.identity;
    private List<SwingBone> m_Bones = new List<SwingBone>();

    [Range(0.01f, 100.0f)]
    public float drag = 7;

    [Range(0.01f, 100.0f)]
    public float angelDrag = 15;

    public bool affectChild = true;
    public static bool ShowHigh = true;

    // Use this for initialization
    [ExecuteInEditMode]
    void Start()
    {
        baseTransform = this.transform;
        if (!this.Root)
            this.Root = this.transform.root;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            SwingBone bone = child.GetComponent<SwingBone>();
            if (bone == null)
            {
                bone = child.gameObject.AddComponent<SwingBone>();
                bone.drag = this.drag;
                bone.angelDrag = this.angelDrag;
                bone.Root = this.Root;
            }
            bone.IsRoot = false;
            m_Bones.Add(bone);
        }
    }


    [ExecuteInEditMode]
    void LateUpdate()
    {
        if (this.gameObject.layer == 8 || this.gameObject.layer == 9 || this.gameObject.layer == 10)
        {
            if (!ShowHigh)
                return;
        }
        if (this.IsRoot)
            this.UpdateBone();
    }

    void UpdateBone()
    {
        if (baseTransform == null)
            return;
        if (m_LastRot != Quaternion.identity)
        {
            baseTransform.rotation = Quaternion.Lerp(m_LastRot, baseTransform.rotation, Mathf.Clamp01(angelDrag * Time.deltaTime));
        }
        Vector3 RootPos = Vector3.zero;
        if (Root)
            RootPos = Root.transform.position;
        Vector3 position = baseTransform.position - RootPos;
        if (m_LastRot != baseTransform.rotation)
        {
            if (m_LastPos != Vector3.zero && m_LastPos != position)
            {
                Vector3 ParentPos = this.transform.parent.position - RootPos;
                m_LastRot = Quaternion.FromToRotation(position - ParentPos, m_LastPos - ParentPos) * baseTransform.rotation;
                baseTransform.rotation = Quaternion.Lerp(m_LastRot, baseTransform.rotation, Mathf.Clamp01(drag * Time.deltaTime));
            }
            else
                m_LastRot = baseTransform.rotation;
        }

        m_LastPos = position;

        for (int i = 0; i < this.m_Bones.Count; i++)
        {
            if (affectChild)
            {
                this.m_Bones[i].affectChild = this.affectChild;
                this.m_Bones[i].drag = this.drag;
                this.m_Bones[i].angelDrag = this.angelDrag;
                this.m_Bones[i].Root = this.Root;
            }
            this.m_Bones[i].UpdateBone();
        }
    }
}
