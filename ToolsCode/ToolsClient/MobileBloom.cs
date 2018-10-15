using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Mobile Bloom")]
public class MobileBloom : MonoBehaviour
{
    public string shader = "MOYU/MobileBloom";
    public float intensity = 0.5f;
    public Color colorMix = Color.white;
    public float colorMixBlend = 0.25f;

    private Shader bloomShader;
    private Material apply = null;
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

    void Start()
    {
        FindShaders();
        CheckSupport();
        CreateMaterials();
    }

    void FindShaders()
    {
        if (!bloomShader)
            bloomShader = Shader.Find(shader);
    }

    void CreateMaterials()
    {
        if (!apply)
        {
            apply = new Material(bloomShader);
            apply.hideFlags = HideFlags.DontSave;
        }
    }

    bool Supported()
    {
        return (SystemInfo.supportsImageEffects && bloomShader.isSupported);
    }

    bool CheckSupport()
    {
        if (!Supported())
        {
            enabled = false;
            return false;
        }
        rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565) ? RenderTextureFormat.RGB565 : RenderTextureFormat.Default;
        return true;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!bloomShader || !apply)
        {
            Graphics.Blit(source, destination);
            return;
        }
        int rtW = source.width / 6;
        int rtH = source.height / 6;

        RenderTexture tempRtLowA = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
        RenderTexture tempRtLowB = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);

        apply.SetColor("_ColorMix", colorMix);
        apply.SetVector("_Parameter", new Vector4(colorMixBlend * 0.25f, 0.0f, 0.0f, 1.0f - intensity));

        Graphics.Blit(source, tempRtLowA, apply, 1);

        tempRtLowB.DiscardContents();
        Graphics.Blit(tempRtLowA, tempRtLowB, apply, 2);

        tempRtLowA.DiscardContents();
        Graphics.Blit(tempRtLowB, tempRtLowA, apply, 3);

        apply.SetTexture("_Bloom", tempRtLowA);

        Graphics.Blit(source, destination, apply, 4);

        RenderTexture.ReleaseTemporary(tempRtLowA);
        RenderTexture.ReleaseTemporary(tempRtLowB);
    }
}