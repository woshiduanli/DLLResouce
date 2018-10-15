using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Delay : MonoBehaviour
{
	public float delayTime = 1.0f;
    private bool isReset = false;
    void Start()
    {
        if (isReset)
            return;
        ResetDelay();
    }
	
    public void ResetDelay()
    {
        isReset = true;
        gameObject.SetActive(false);
        Invoke("DelayFunc", delayTime);
    }

	void DelayFunc()
	{
        gameObject.SetActive(true);
    }

}

public class DelayChildren : MonoBehaviour
{
    public List<Delay> Children = new List<Delay>();

    void OnEnable()
    {
        if(Children != null && Children.Count > 0)
        {
            for (int i =0;i<Children.Count;i++)
            {
                Children[i].CancelInvoke();
                Children[i].ResetDelay();
            }
        }
    }
}

public class EffectReplay : MonoBehaviour
{
    public GameObject Effect;
    private GameObject Clone;

    void OnEnable()
    {
        Clone = Object.Instantiate<GameObject>(Effect, Effect.transform.position, Effect.transform.rotation, Effect.transform.parent);
        Effect.SetActive(false);
        Clone.SetActive(true);
    }
}

public class EffectScale : MonoBehaviour
{
    public List<ParticleSize> Particles = new List<ParticleSize>();
    private float Size;
    private float Scale = 1;
    private Transform TF;
    void Awake()
    {
        this.TF = this.transform;
        ParticleSystem[] particles = this.gameObject.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSize item = new ParticleSize();
            ParticleSystem ps = particles[i];
            item.ps = ps;
            if (ps.main.startSize3D)
                item.Size = new Vector3(ps.main.startSizeX.constant, ps.main.startSizeY.constant, ps.main.startSizeZ.constant);
            else
                item.Size = new Vector3(ps.main.startSize.constant, 0, 0);

            if(ps.shape.enabled)
            {
                item.shapeRadius = ps.shape.radius;
                item.shapeScale = ps.shape.scale;
            }
            Particles.Add(item);
        }
    }

    public void SetScale(float s)
    {
        this.Scale = s;
    }

    void Update()
    {
        if (this.TF.localScale != Vector3.one)
            this.TF.localScale = Vector3.one;
        if (Size == this.TF.lossyScale.x * this.Scale)
            return;
        Size = this.TF.lossyScale.x * this.Scale;
        for (int i = 0; i < Particles.Count; i++)
            ApplyParticleScale(Particles[i]);
    }

    private void ApplyParticleScale(ParticleSize item)
    {
        ParticleSystem ps = item.ps;
        if (!ps)
            return;
        ParticleSystem.MainModule MainModule = ps.main;
        MainModule.startSizeX = new ParticleSystem.MinMaxCurve(item.Size.x * this.Size);
        if (ps.main.startSize3D)
        {
            MainModule.startSizeY = new ParticleSystem.MinMaxCurve(item.Size.y * this.Size);
            MainModule.startSizeZ = new ParticleSystem.MinMaxCurve(item.Size.z * this.Size);
        }
        if (ps.shape.enabled)
        {
            ParticleSystem.ShapeModule shapeModule = ps.shape;
            //shapeModule.radius = item.shapeRadius * this.Size;
            shapeModule.scale = item.shapeScale * this.Size;
        }
        ps.transform.localScale = Vector3.one;
    }

    public class ParticleSize
    {
        public ParticleSystem ps;
        public Vector3 Size;
        public float shapeRadius;
        public Vector3 shapeScale = Vector3.zero;
    }
}