using UnityEditor;
using UnityEngine;
using Cinemachine.Editor;

namespace Cinemachine.Timeline
{
    [CustomEditor(typeof(MYCinemachineShot))]
    internal sealed class MYCinemachineShotEditor : UnityEditor.Editor
    {
        private static readonly string[] m_excludeFields = new string[] { "m_Script" };
        private SerializedProperty mVirtualCameraProperty = null;
        private MYCinemachineShot mShot;
        private SerializedProperty mVMPath = null;
        //private static readonly GUIContent kVirtualCameraLabel
            //= new GUIContent("Virtual Camera", "The virtual camera to use for this shot");

        private void OnEnable()
        {
            if (serializedObject != null)
            {
                mShot = serializedObject.targetObject as MYCinemachineShot;
                mVirtualCameraProperty = serializedObject.FindProperty("VirtualCamera");
                mVMPath = serializedObject.FindProperty("VmPath");
            }
        }

        private void OnDisable()
        {
            DestroyComponentEditors();
        }

        private void OnDestroy()
        {
            DestroyComponentEditors();
        }

        public override void OnInspectorGUI()
        {
            if(!mShot)
                base.OnInspectorGUI();
            if (mShot.VirtualCamera == null)
            {
                //serializedObject.Update();
                EditorGUILayout.BeginHorizontal();
                mShot.VirtualCamera = EditorGUILayout.ObjectField("Virtual Camera", mShot.VirtualCamera, typeof(CinemachineVirtualCameraBase), true, GUILayout.ExpandWidth(true)) as CinemachineVirtualCameraBase;
                if ((mShot.VirtualCamera == null) && GUILayout.Button(new GUIContent("Create"), GUILayout.ExpandWidth(false)))
                {
                    CinemachineVirtualCameraBase vcam = CinemachineMenu.CreateDefaultVirtualCamera();
                    mShot.VirtualCamera = vcam;
                }
                if(mShot.VirtualCamera != null)
                {
                    mShot.VmPath = mShot.VirtualCamera.name;
                }
                EditorGUILayout.EndHorizontal();
                //serializedObject.ApplyModifiedProperties();
            }
            else
            {
                serializedObject.Update();
                DrawPropertiesExcluding(serializedObject, m_excludeFields);

                // Create an editor for each of the cinemachine virtual cam and its components
                UpdateComponentEditors(mShot.VirtualCamera);
                if (m_editors != null)
                {
                    foreach (UnityEditor.Editor e in m_editors)
                    {
                        EditorGUILayout.Separator();
                        if (e.target.GetType() != typeof(Transform))
                        {
                            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                            EditorGUILayout.LabelField(e.target.GetType().Name, EditorStyles.boldLabel);
                        }
                        e.OnInspectorGUI();
                    }
                }
                serializedObject.ApplyModifiedProperties();
            }
        }

        CinemachineVirtualCameraBase m_cachedReferenceObject;
        UnityEditor.Editor[] m_editors = null;
        void UpdateComponentEditors(CinemachineVirtualCameraBase obj)
        {
            MonoBehaviour[] components = null;
            if (obj != null)
                components = obj.gameObject.GetComponents<MonoBehaviour>();
            int numComponents = (components == null) ? 0 : components.Length;
            int numEditors = (m_editors == null) ? 0 : m_editors.Length;
            if (m_cachedReferenceObject != obj || (numComponents + 1) != numEditors)
            {
                DestroyComponentEditors();
                m_cachedReferenceObject = obj;
                if (obj != null)
                {
                    m_editors = new UnityEditor.Editor[components.Length + 1];
                    CreateCachedEditor(obj.gameObject.GetComponent<Transform>(), null, ref m_editors[0]);
                    for (int i = 0; i < components.Length; ++i)
                        CreateCachedEditor(components[i], null, ref m_editors[i + 1]);
                }
            }
        }

        void DestroyComponentEditors()
        {
            m_cachedReferenceObject = null;
            if (m_editors != null)
            {
                for (int i = 0; i < m_editors.Length; ++i)
                {
                    if (m_editors[i] != null)
                        UnityEngine.Object.DestroyImmediate(m_editors[i]);
                    m_editors[i] = null;
                }
                m_editors = null;
            }
        }
    }
}
