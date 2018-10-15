using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class Mirror : MonoBehaviour
{
    public static class Layer
    {
        public const int Reflection = 17;

        public static class Mask
        {
            public const int Reflection = 1 << Layer.Reflection;
        }
    }

    private void Clear()
    {
        Mirrors.Remove(this);
        if (mTex)
        {
            mTex.Release();
            DestroyImmediate(mTex);
            mTex = null;
        }
        if (reflectionCamera)
            DestroyImmediate(reflectionCamera.gameObject);
    }

    public static List<Mirror> Mirrors = new List<Mirror>();
    public Camera reflectionCamera;
    public string reflect = "_ReflectionTex";
    public float m_ClipPlaneOffset = 0.07f;
    private Transform mTrans;
    private RenderTexture mTex = null;
    private Renderer mRen;
    public LayerMask ReflectionMask = -1;
    /// <summary>
    /// Caching is always a good idea.
    /// </summary>

    void Awake()
    {
        ReflectionMask = Layer.Mask.Reflection;
        mTrans = transform;
        mRen = GetComponent<Renderer>();

        Material mat = mRen.sharedMaterial;
        if (!mat)
        {
            this.enabled = false;
            return;
        }

        if (!Camera.main)
        {
            this.enabled = false;
            return;
        }

        if(!mTex)
        {
            mTex = new RenderTexture(256, 256, 0);
            mTex.name = "__MirrorReflection" + GetInstanceID();
            mTex.isPowerOfTwo = true;
            mTex.hideFlags = HideFlags.DontSave;
        }

        GetReflectionCamera(mat);
        CopyCamera(reflectionCamera);

        if (!Mirrors.Contains(this))
            Mirrors.Add(this);
    }

    void OnDisable()
    {
        Clear();
    }

    void OnDestroy()
    {
        Clear();
    }

    public static void Destroy()
    {
        for (int i = 0; i < Mirrors.Count; i++)
        {
            Mirror mirror = Mirrors[i];
            Object.Destroy(mirror);
        }
        Mirrors.Clear();
    }

    /// <summary>
    /// Copy camera settings from source to destination.
    /// </summary>

    void CopyCamera(Camera dest)
    {
        if (!Camera.main)
            return;
        dest.clearFlags = CameraClearFlags.SolidColor;
        dest.backgroundColor = Color.white;
        dest.farClipPlane = Camera.main.farClipPlane;
        dest.nearClipPlane = Camera.main.nearClipPlane;
        dest.orthographic = Camera.main.orthographic;
        dest.fieldOfView = Camera.main.fieldOfView;
        dest.aspect = Camera.main.aspect;
        dest.orthographicSize = Camera.main.orthographicSize;
        dest.depthTextureMode = DepthTextureMode.None;
        dest.renderingPath = RenderingPath.Forward;
    }

    /// <summary>
    /// Get or create the camera used for reflection.
    /// </summary>

    void GetReflectionCamera(Material mat)
    {
        if (!Camera.main)
            return;
        if (!reflectionCamera)
        {
            GameObject go = new GameObject("Mirror Refl Camera id" + GetInstanceID() + " for " + Camera.main.GetInstanceID(), typeof(Camera));

            reflectionCamera = go.GetComponent<Camera>();
            reflectionCamera.enabled = false;

            Transform t = reflectionCamera.transform;
            t.position = mTrans.position;
            t.rotation = mTrans.rotation;
        }

        if (mat.HasProperty(reflect))
            mat.SetTexture(reflect, mTex);
    }

    static float SignExt(float a)
    {
        if (a > 0.0f) return 1.0f;
        if (a < 0.0f) return -1.0f;
        return 0.0f;
    }


    static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
    {
        Vector4 q = projection.inverse * new Vector4(SignExt(clipPlane.x), SignExt(clipPlane.y), 1.0f, 1.0f);
        Vector4 c = clipPlane * (2.0F / (Vector4.Dot(clipPlane, q)));

        // third row = clip plane - fourth row
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
    }

    /// <summary>
    /// Calculates reflection matrix around the given plane.
    /// </summary>

    static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }

    /// <summary>
    /// Given position/normal of the plane, calculates plane in camera space.
    /// </summary>

    Vector4 CameraSpacePlane(Vector3 pos, Vector3 normal, float sideSign)
    {
        Matrix4x4 m = reflectionCamera.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint(pos);
        Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
    }

    /// <summary>
    /// Called when the object is being renderered.
    /// </summary>

    void OnWillRenderObject()
    {
        if (!Camera.main)
            return;

        if (!enabled || !mRen || !mRen.enabled)
            return;

        Vector3 pos = mTrans.position;
        Vector3 normal = mTrans.up;

        float d = -Vector3.Dot(normal, pos) - m_ClipPlaneOffset;
        Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);
        Matrix4x4 reflection = Matrix4x4.zero;

        CalculateReflectionMatrix(ref reflection, reflectionPlane);

        Vector3 oldpos = Camera.main.transform.position;
        Vector3 newpos = reflection.MultiplyPoint(oldpos);
        reflectionCamera.worldToCameraMatrix = Camera.main.worldToCameraMatrix * reflection;

        Vector4 clipPlane = CameraSpacePlane(pos, normal, 1.0f);
        Matrix4x4 projection = Camera.main.projectionMatrix;

        CalculateObliqueMatrix(ref projection, clipPlane);

        reflectionCamera.projectionMatrix = projection;
        reflectionCamera.cullingMask = ~(1 << 4) & ReflectionMask.value;
        reflectionCamera.targetTexture = mTex;

        GL.invertCulling = true;
        reflectionCamera.transform.position = newpos;
        Vector3 euler = Camera.main.transform.eulerAngles;
        reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
        reflectionCamera.Render();
        reflectionCamera.transform.position = oldpos;
        GL.invertCulling = false;
    }
}