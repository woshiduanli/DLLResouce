using UnityEngine;
public class RadialBlur : MonoBehaviour
{
    public string shader = "MOYU/radialBlur";
    public Shader SCShader;
    private float TimeX = 1.0f;
    [Range(-0.5f, 0.5f)]
    public float Intensity = 0.125f;
    [Range(-2f, 2f)]
    public float MovX = 0.5f;
    [Range(-2f, 2f)]
    public float MovY = 0.5f;
    [Range(0f, 10f)]
    private float blurWidth = 1f;
    private Material material;
    void Start()
    {
        if (!SCShader)
            SCShader = Shader.Find(shader);
        if (!SystemInfo.supportsImageEffects || !SCShader.isSupported)
        {
            enabled = false;
            return;
        }
        if (!material)
        {
            material = new Material(SCShader);
            material.hideFlags = HideFlags.HideAndDontSave;
        }
    }


    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (SCShader != null)
        {
            TimeX += Time.deltaTime;
            if (TimeX > 100) TimeX = 0;
            material.SetFloat("_TimeX", TimeX);
            material.SetFloat("_Value", Intensity);
            material.SetFloat("_Value2", MovX);
            material.SetFloat("_Value3", MovY);
            material.SetFloat("_Value4", blurWidth);
            material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width * 0.2f, sourceTexture.height * 0.2f, 0.0f, 0.0f));
            Graphics.Blit(sourceTexture, destTexture, material);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
}
