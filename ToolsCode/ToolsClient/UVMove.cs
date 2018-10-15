using UnityEngine;
using System.Collections;

public class UVMove : MonoBehaviour
{
    public float scrollSpeedx = 5;
    public float scrollSpeedy = 5;
    public bool offsetX;
    public bool offsetY;
    private Vector2 uv = Vector2.zero;
    private Material material;
 
    void Start()
    {
        Renderer render = this.gameObject.GetComponent<Renderer>();
        if (render)
            material = render.sharedMaterial;
        if (!material)
            this.enabled = false;
    }

    void Update()
    {
        if (!material)
            return;

        if (offsetX)
        {
            uv.x += Time.deltaTime * scrollSpeedx;
            if (uv.x > 1)
                uv.x = 0;
        }
        if (offsetY)
        {
            uv.y += Time.deltaTime * scrollSpeedy;
            if (uv.y > 1)
                uv.y = 0;
        }
        material.SetTextureOffset("_MainTex", uv);
    }
}
