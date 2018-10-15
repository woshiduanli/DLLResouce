using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BuildingTranslucence : MonoBehaviour
{
    public List<Material> Materials = new List<Material>();
    public float Speed = 5f;
    private float Aphla = 0.35f;
    public string BlendShader = "MOYU/AlphaBlendOn";
    private Shader cBlendShader;
    private float CurrentAlpha = 1;
    private int dir = 1;
    private List<Shader> Shaders = new List<Shader>();
    private void Awake()
    {
        Renderer[] Renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < Renderers.Length; i++)
        {
            Materials.Add(Renderers[i].sharedMaterial);
            Shader shader = Renderers[i].sharedMaterial.shader;
            if (Application.isEditor && shader)
                shader = Shader.Find(shader.name);

            if (shader.name == BlendShader || string.IsNullOrEmpty(BlendShader))
                continue;
            Shaders.Add(shader);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("MainCamera"))
            return;
        dir = -1;
        CurrentAlpha = 1;
        CurrentAlpha += Time.deltaTime * Speed * dir;

        if (Shaders.Count == 0)
            return;
        if (!cBlendShader)
            cBlendShader = Shader.Find(BlendShader);
        if (!cBlendShader)
            return;
        for (int i = 0; i < Materials.Count; i++)
        {
            Material mat = Materials[i];
            if (!mat)
                continue;
            mat.shader = cBlendShader;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("MainCamera"))
            return;
        CurrentAlpha = Aphla;
        dir = 1;
        CurrentAlpha += Time.deltaTime * Speed * dir;
    }

    void ExitFinsh()
    {
        if (Shaders.Count == 0)
            return;
        for (int i = 0; i < Materials.Count; i++)
        {
            Material mat = Materials[i];
            if (!mat)
                continue;
            mat.shader = Shaders[i];
            Color color = mat.color;
            color.a = 1;
            mat.color = color;
        }
    }

    private void Update()
    {
        if (Materials == null)
        {
            this.enabled = false;
            return;
        }
        if (CurrentAlpha <= Aphla || CurrentAlpha >= 1)
        {
            if (CurrentAlpha > 1)
            {
                CurrentAlpha = 1;
                ExitFinsh();
            }
            if (CurrentAlpha <= Aphla)
            {
                CurrentAlpha = Aphla;
            }
            return;
        }
        CurrentAlpha += Time.deltaTime * Speed * dir;
        for (int i = 0; i < Materials.Count; i++)
        {
            Material mat = Materials[i];
            if (!mat)
                continue;
            Color color = mat.color;
            color.a = CurrentAlpha;
            mat.color = color;
        }
    }

    void OnApplicationQuit()
    {
        ExitFinsh();
    }
}
