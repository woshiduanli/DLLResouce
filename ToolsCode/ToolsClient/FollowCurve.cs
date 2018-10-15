using UnityEngine;
using System.Collections;
public class FollowCurve : MonoBehaviour
{
    public WrapMode wrapMode = WrapMode.Once;
    public Curve curve;
    public float position = 0;
    public Transform target;
    [HideInInspector]
    public Vector3 debugLookPos;
    public float targetFocusStrength = 2.0f;
    public float LifeTime = 5;
    public AnimationCurve animCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    private Transform tf;
    public float usetime = 0;
    private int dir = 1;
    public bool Changforward = true;
    void Awake()
    {
        tf = this.transform;
        animCurve.RemoveKey(animCurve.keys.Length - 1);
        Keyframe lastkf = animCurve.keys[animCurve.keys.Length - 1];
        lastkf.time = 5.0f;
        lastkf.value = 5;
        animCurve.AddKey(lastkf);
    }

    public void SetUseTime(float utime)
    {
        usetime = dir * utime;
    }

    void Update()
    {
        if (usetime < 0)
            usetime = 0;
        if (usetime > LifeTime)
            usetime = LifeTime;

        usetime += dir * Time.deltaTime;
        position = GetValue(usetime) / LifeTime;

        UpdatePosition();
        UpdateLook();
        if (usetime >= LifeTime || usetime <= 0)
        {
            if (wrapMode == WrapMode.PingPong)
                dir = -dir;
            else if (wrapMode == WrapMode.Loop)
                usetime = 0;
            else
                this.enabled = false;
        }
    }

    public float GetValue(float pos)
    {
        float v = animCurve.Evaluate(pos);
        return v;
    }

    void OnDrawGizmosSelected()
    {
        if (target)
        {
            Gizmos.color = new Color(0.5f, 0.4f, 0.25f, 0.5f);
            Gizmos.DrawLine(tf.position, debugLookPos);
        }
    }

    void UpdatePosition()
    {
        if (!curve)
            return;
        if (position < 0.0f)
            position = 0.0f;
        if (position > 1.0f)
            position = 1.0f;
        Vector3 curvePos = curve.GetPosition(position);
        if (tf.position != curvePos)
            tf.position = curvePos;
    }

    void UpdateLook()
    {
        if (!curve) return;
        Vector3 nToSet;
        Vector3 n;
        if (target)
        {
            n = (target.position - tf.position).normalized;
            if (tf.forward != n && n.magnitude > 0)
            {
                nToSet = Vector3.Slerp(tf.forward, n, targetFocusStrength * Time.deltaTime);
                if (!Application.isPlaying)
                    tf.forward = n;
                else
                    tf.forward = nToSet;
            }
            debugLookPos = target.position;
        }
        else if (curve && Changforward)
        {
            n = curve.GetForwardNormal(position, 0.01f);
            if (tf.forward != n && n.magnitude > 0)
            {
                nToSet = Vector3.Slerp(tf.forward, n, targetFocusStrength * Time.deltaTime);
                if (!Application.isPlaying)
                    tf.forward = n;
                else
                    tf.forward = nToSet;
            }
        }
    }

    void SetPositionOnCurve(float value)
    {
        position = value;
        Update();
    }

    void SetCurve(Curve desiredCurve)
    {
        curve = desiredCurve;
        Update();
    }

    void SetTarget(Transform desiredTarget)
    {
        target = desiredTarget;
        Update();
    }

}