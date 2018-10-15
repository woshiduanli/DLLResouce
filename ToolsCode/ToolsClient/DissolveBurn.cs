using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DissolveBurn : MonoBehaviour
{
    public string shader = "MOYU/DissolveBurn";
    public float m_Speed = 0.1f;
    public float From = 0;
    public float To = 1.5f;
    public float value;
    public List<Material> m_Mats = new List<Material>();
    private float factor = 1;
    public System.Action Finish;
    private bool Destroyfinish;
    private Dictionary<Material, Shader> Oldshaders = new Dictionary<Material, Shader>();

    public void SetFinish(System.Action cb)
    {
        Finish = cb;
    }

    public void SetMats(Material mat,Texture tex,Color DissColor)
    {
        if (mat && tex)
        {
            Shader s = mat.shader;
            mat.shader = Shader.Find(shader);
            mat.SetTexture("_DissolveSrc", tex);
            mat.SetColor("_DissColor", DissColor);
            m_Mats.Add(mat);
            Oldshaders[mat] = s;
            mat.SetFloat("_Amount", From);
        }
    }

    public static DissolveBurn Begin(GameObject go, float speed, float start, float end, bool destroyfinish = false)
    {
        DissolveBurn db = go.GetComponent<DissolveBurn>();
        if (!db)
            db = go.AddComponent<DissolveBurn>();
        else
            db.enabled = true;
        db.m_Speed = speed;
        db.From = start;
        db.To = end;
        db.factor = 1;
        db.Destroyfinish = destroyfinish;
        db.m_Mats.Clear();

        return db;
    }

    void Update()
    {
        if (m_Mats.Count == 0)
            return;
        factor -= Time.deltaTime * m_Speed;
        value = To * (1f - factor) + From * factor;
        for (int i = 0; i < m_Mats.Count; i++)
        {
            if (!m_Mats[i])
                continue;
            m_Mats[i].SetFloat("_Amount", value);
        }
        if (factor <= 0)
        {
            if (Finish != null)
                Finish();
            
            if (Destroyfinish)
            {
                foreach (var kvp in Oldshaders)
                {
                    if (kvp.Key)
                        kvp.Key.shader = kvp.Value;
                }
                Destroy(this);
                return;
            }
            this.enabled = false;
        }
    }
}