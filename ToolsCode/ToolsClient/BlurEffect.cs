using UnityEngine;
using System.Collections;

public class BlurEffect : MonoBehaviour
{
    public int Power = 4;
    public string shader = "MOYU/Blur";
    private int iterations = 1;
    private float blurSpread = 0.6f;
    private Shader blurShader;
    protected Material material;
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;
    void Start()
    {
        FindShaders();
        CreateMaterials();
        if (!SystemInfo.supportsImageEffects || !blurShader || !material.shader.isSupported)
        {
            enabled = false;
            return;
        }
        rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565) ? RenderTextureFormat.RGB565 : RenderTextureFormat.Default;
    }

    void FindShaders()
    {
        if (!blurShader)
            blurShader = Shader.Find(shader);
    }

    void CreateMaterials()
    {
        if (!material)
        {
            material = new Material(blurShader);
            material.hideFlags = HideFlags.DontSave;
        }
    }

    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float off = 0.5f + iteration * blurSpread;
        Graphics.BlitMultiTap(source, dest, material,
            new Vector2(-off, -off),
            new Vector2(-off, off),
            new Vector2(off, off),
            new Vector2(off, -off)
        );
    }

    private void DownSample4x(RenderTexture source, RenderTexture dest)
    {
        float off = 1.0f;
        Graphics.BlitMultiTap(source, dest, material,
            new Vector2(-off, -off),
            new Vector2(-off, off),
            new Vector2(off, off),
            new Vector2(off, -off)
        );
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtW = source.width / Power;
        int rtH = source.height / Power;
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
        DownSample4x(source, buffer);

        RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
        FourTapCone(buffer, buffer2, 0);
        RenderTexture.ReleaseTemporary(buffer);

        Graphics.Blit(buffer2, destination);
        RenderTexture.ReleaseTemporary(buffer2);
    }
}
