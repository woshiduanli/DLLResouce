using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortImageEffect : MonoBehaviour
{
    public static float Factor = 0.6f;
    private RenderTexture source;

    void Start()
    {
        var width = (int)(Screen.width * Factor);
        var height = (int)(Screen.height * Factor);
        if (!source)
        {
            source = new RenderTexture(width, height, 16, RenderTextureFormat.DefaultHDR);
            source.hideFlags = HideFlags.DontSave;
        }
    }

    void OnDestroy()
    {
        if (source)
        {
            source.Release();
            DestroyImmediate(source);
            source = null;
        }
    }

    private void LateUpdate()
    {
        Shader.EnableKeyword("DISTORT_OFF");
        if (Camera.main)
        {
            var hdr = Camera.main.allowHDR;
            source.DiscardContents();
            Camera.main.allowHDR = true;
            Camera.main.targetTexture = source;
            Camera.main.Render();
            Camera.main.allowHDR = hdr;
            Camera.main.targetTexture = null;
        }
        Shader.SetGlobalTexture("_GrabTextureMobile", source);
        Shader.SetGlobalFloat("_GrabTextureMobileScale", Factor);
        Shader.DisableKeyword("DISTORT_OFF");
    }
}