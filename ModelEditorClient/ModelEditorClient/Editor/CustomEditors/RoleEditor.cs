using UnityEditor;
using UnityEngine;
using System.IO;
using Model;
using System.Collections.Generic;
using System.Collections;

[CustomEditor(typeof(Role))]
public class RoleEditor :Editor
{
    private Role Role { get { return this.serializedObject.targetObject as Role; } }
    private static RoleObject roleObject;

    protected class SizeAndOffset {
        public Vector3 offset;
        public Vector3 size;
        public SizeAndOffset( Vector3 of, Vector3 s ) {
            offset = of;
            size = s;
        }
        public void Update( Vector3 o, Vector3 s ) {
            offset = o;
            size = s;
        }
        public bool Changed( Vector3 o, Vector3 s ) {
            if ( offset == o && size == s )
                return false;
            return true;
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(50);

        //if (!Role.Skin && Role.BaseLook.Length == 0)
        //    Role.Skin = Role.GetComponentInChildren<SkinnedMeshRenderer>();

        if (GUILayout.Button("Save"))
        {
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Build"))
        {
            AssetExporter.GetShaders();
            AssetExporter.GetPublicAssets();
            AssetExporter.BuildAssetBundles(this.Role.gameObject);
            Selection.activeObject = this.serializedObject.targetObject;
        }

        if (GUILayout.Button("生成控制器"))
        {
            if (Selection.activeObject != this.Role.gameObject)
                return;
            string rootpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(rootpath, typeof(Object));

            Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);
            Animator animator = this.Role.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = AssetExporter.CreateOverrideController(string.Format("{0}.controller", this.Role.gameObject.name), rootpath, clips);
        }

        if (GUILayout.Button("生成骨骼数据"))
        {
            if (Selection.activeObject != this.Role.gameObject)
                return;
            Transform[] bones = this.Role.gameObject.GetComponentsInChildren<Transform>(true);
            List<GameObject> golist = new List<GameObject>();
            for (int i = 0; i < bones.Length; i++)
                golist.Add(bones[i].gameObject);

            this.Role.Bones = golist.ToArray();
        }

        if (GUILayout.Button("复制动画"))
        {
            if (Role != null)
            {
                List<OverrideAnimiClip> list = new List<OverrideAnimiClip>();
                Animator roleAnimator = Role.GetComponent<Animator>();
                if (roleAnimator == null || !(roleAnimator.runtimeAnimatorController is AnimatorOverrideController) || roleAnimator.avatar == null) return;
                AnimatorOverrideController Controller = roleAnimator.runtimeAnimatorController as AnimatorOverrideController;
                List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                Controller.GetOverrides(overrides);
               
                for (int i = 0; i < overrides.Count; i++)
                {
                    KeyValuePair<AnimationClip, AnimationClip> kvp = overrides[i];
                    if (kvp.Value == null)
                        continue;
                    list.Add(new OverrideAnimiClip(kvp.Key.name, kvp.Value));
                }
                Role.AnimiClips = list.ToArray();
            }
        }

        if (GUILayout.Button("添加目标"))
        {
            if (!ModelLoader.self)
                return;
            RoleObject target = ModelLoader.CreateRole(this.Role,false);
            target.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(10, 15));
            target.transform.LookAt(ModelLoader.self.transform);
            target.tag = "target";
            target.enabled = false;
            
            ModelLoader.targetList.Add(target);
        }

        if (GUILayout.Button("预览低模"))
        {
            if ( !EditorApplication.isPlaying )
            {
                EditorUtility.DisplayDialog( "Error", "需要在运行状态下才能进行该操作", "OK" );
                return;
            }

            ModelLoader.ResetPreviewingObject();

            roleObject = ModelLoader.CreateRole(this.Role,false);

            roleObject.tag = "self";
            ModelLoader.self = roleObject;
            ModelLoader.PreviewingObject = roleObject.gameObject;
            ModelLoader.SetupCamera( ModelLoader.PreviewingObject.transform );
            TriangleArea ta = roleObject.gameObject.AddComponent<TriangleArea>();
        }

        if (GUILayout.Button("预览高模"))
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
                return;
            }

            ModelLoader.ResetPreviewingObject();

            roleObject = ModelLoader.CreateRole(this.Role, true);

            roleObject.tag = "self";
            ModelLoader.self = roleObject;
            ModelLoader.PreviewingObject = roleObject.gameObject;
            ModelLoader.SetupCamera(ModelLoader.PreviewingObject.transform);
            TriangleArea ta = roleObject.gameObject.AddComponent<TriangleArea>();
        }

        //当Inspector 面板发生变化时保存数据
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
