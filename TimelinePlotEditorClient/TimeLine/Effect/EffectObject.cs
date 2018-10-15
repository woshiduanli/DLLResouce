using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject
{
    private float start_time, duration_time = 0;
    protected EffectScale effectScale;
    public GameObject gameObject;
    private Vector3 init_scale;
    public bool isGoDestroy;

    public void OnCreate(UnityEngine.Object asset)
    {
        this.gameObject = UnityEngine.Object.Instantiate(asset) as GameObject;
        DestoryByTime timeDesCompoment = gameObject.GetComponent<DestoryByTime>();
        if (timeDesCompoment != null)
        {
            start_time = Time.realtimeSinceStartup;
            duration_time = timeDesCompoment.time;
            GameObject.DestroyImmediate(timeDesCompoment);
        }
    }

    public void Destroy()
    {
        isGoDestroy = true;
        GameObject.Destroy(gameObject);
    }

    public void SetScale(Vector3 scale)
    {
        if (this.gameObject)
            this.gameObject.transform.localScale = scale;
        this.init_scale = scale;
    }

    public void SetPostion(Vector3 pos)
    {
        if (this.gameObject)
            this.gameObject.transform.position = pos;
    }

    public void OnUpdate()
    {
        if (isGoDestroy)
            return;
        if (start_time + duration_time <= Time.realtimeSinceStartup && start_time != 0)
        {
            Destroy();
        }
    }
}

