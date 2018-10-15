using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FaQiuControl : MonoBehaviour 
{
    private Transform Tf = null;
    public Vector3 offset = new Vector3(0, 1, 0);
    public float Upspeed = 2.7f;
    public float roundspeed = 100;
    public float RangeH = 0.2f;
    public float radius = 1;
    private float angle = 0;
    private Vector3 eulerAngles;
    public void Awake()
    {
        Tf = this.transform;
        eulerAngles = Vector3.zero;
    }

    void LateUpdate()
    {
        angle += Time.deltaTime * Upspeed;
        if (angle >= 6.28f)
            angle = 0;
        Vector3 Temppos = Tf.localPosition;
        float sin = Mathf.Sin(angle);
        Temppos.y = sin * RangeH;
        Temppos.x = sin * radius;
        Temppos.z = Mathf.Cos(angle) * radius;
        Tf.localPosition = Temppos + offset;

        eulerAngles.y += Time.deltaTime * roundspeed;
        Tf.eulerAngles = eulerAngles;
    }
}
