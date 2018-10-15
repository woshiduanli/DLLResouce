using UnityEngine;
using UnityEditor;
using System.IO;

public class BundlePreviewEditor : Editor
{
    private Texture Texture_ = null;
    private GameObject Mesh_ = null;
    private RenderTexture RT_ = new RenderTexture(512, 512, 0);
    private GameObject Camera_;

    void OnEnable()
    {
        GL.wireframe = true;
        PrepareCamera();
        Resources.UnloadUnusedAssets();
        if (!this.serializedObject.targetObject) return;
        var assetPath = AssetDatabase.GetAssetPath(this.serializedObject.targetObject);
        var ext = Path.GetExtension(assetPath);
        switch (ext)
        {
            case ".tex":
                LoadTexture(assetPath);
                break;
            case ".mesh":
                LoadMesh(assetPath);
                break;
        }
    }

    void OnDisable()
    {
        if (Texture_)
        {
            Object.DestroyImmediate(this.Texture_);
        }
        if (Mesh_)
        {
            Object.DestroyImmediate(this.Mesh_);
        }

        Object.DestroyImmediate(Camera_);
        Object.DestroyImmediate(Mesh_);
        GL.wireframe = false;
    }



    void PrepareCamera()
    {
        Camera_ = EditorUtility.CreateGameObjectWithHideFlags("Camera", HideFlags.DontSave, typeof(Camera));
        Camera_.GetComponent<Camera>().targetTexture = RT_;
        Camera_.transform.position = Vector3.zero;
        Camera_.transform.rotation = Quaternion.identity;
        Camera_.GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
        Camera_.GetComponent<Camera>().backgroundColor = Color.black;
    }

    void LoadTexture(string assetPath)
    {
        var www = new WWW("file://" + System.IO.Path.GetFullPath(assetPath));
        if (!string.IsNullOrEmpty(www.error)) return;
        Texture_ = www.assetBundle.mainAsset as Texture;
        www.assetBundle.Unload(false);
    }

    void LoadMesh(string assetPath)
    {
        if (Mesh_)
        {
            Object.DestroyImmediate(Mesh_);
        }
        var www = new WWW("file://" + System.IO.Path.GetFullPath(assetPath));
        if (!string.IsNullOrEmpty(www.error)) return;
        var mesh = www.assetBundle.mainAsset as Mesh;
        www.assetBundle.Unload(false);

        Mesh_ = EditorUtility.CreateGameObjectWithHideFlags("mesh", HideFlags.DontSave,
                                                                         typeof(MeshFilter),
                                                                         typeof(MeshRenderer));
        var mf = Mesh_.GetComponent<MeshFilter>();
        mf.mesh = mesh;
        Mesh_.transform.position = new Vector3(0, 0, 1);
    }

    public override void OnInspectorGUI()
    {
        if (this.serializedObject.targetObject)
        {
            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(this.serializedObject.targetObject));
            string text = string.Format("{0},{1},{2}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetMouseButton(1));
            EditorGUILayout.LabelField(text);
        }
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if (RT_)
        {
            EditorGUI.DrawPreviewTexture(r, this.RT_, null, ScaleMode.ScaleToFit);
        }
        else if (Texture_)
        {
            EditorGUI.DrawPreviewTexture(r, this.Texture_, null, ScaleMode.ScaleToFit);
        }
        else if (Mesh_)
        {

        }

        r.width = 100;
        r.height = 50;

    }

    public override bool HasPreviewGUI()
    {
        return true;
    }
}