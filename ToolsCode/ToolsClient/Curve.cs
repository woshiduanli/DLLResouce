using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Curve : MonoBehaviour
{
    public bool ResetPoints = false;
    public List<Vector3> points = new List<Vector3>();

    [ExecuteInEditMode]
    void Update()
    {
        UpdatePoints();
    }

    void UpdatePoints()
    {
        if (ResetPoints)
            return;
        ResetPoints = true;
        if (transform.childCount <= 0)
        {
            GameObject newObject = new GameObject("Point0");
            newObject.AddComponent<CurvePoint>();
            newObject.transform.parent = transform;
            newObject.transform.localPosition = Vector3.zero;
        }
        points.Clear();
        foreach (Transform t in transform)
        {
            points.Add(t.position);
            break;
        }
        foreach (Transform t in transform)
        {
            points.Add(t.position);
        }
        points.Add(points[points.Count - 1]);
        //int i = 0;
        //foreach (Transform t in points)
        //{
        //    if (i == 0)
        //    {
        //        i++;
        //        continue;
        //    }
        //    if (i == points.Count - 1)
        //    {
        //        i++;
        //        continue;
        //    }
        //    if (t == gameObject.transform)
        //    {
        //        i++;
        //        continue;
        //    }
        //    string newName = "Point" + i;
        //    string currentName = t.gameObject.name;
        //    if (currentName != newName)
        //    {
        //        t.gameObject.name = newName;
        //    }
        //    i++;
        //}
    }

    void OnDrawGizmos()
    {
        if (points == null) return;
        if (points.Count < 3) return;
        Vector3 lastPos = transform.position;
        bool isFirst = true;
        lastPos = points[0];
        Gizmos.color = Color.blue;
        for (float i = 0.0f; i < points.Count * 8.0f; i++)
        {
            //Transform p = points[(int)(i / 8.0f)];
            float ratio = i / (points.Count * 8.0f);
            Vector3 pos = GetPosition(ratio);
            if (!isFirst)
            {
                Gizmos.DrawLine(lastPos, pos);
            }
            lastPos = pos;
            isFirst = false;
        }
    }

    public Vector3 GetForwardNormal(float p, float sampleDist)
    {
        float curveLength = GetLength();
        Vector3 pos = GetPosition(p);
        Vector3 frontPos = GetPosition(p + (sampleDist / curveLength));
        Vector3 backPos = GetPosition(p - (sampleDist / curveLength));
        Vector3 frontNormal = (frontPos - pos).normalized;
        Vector3 backNormal = (backPos - pos).normalized;
        Vector3 normal = Vector3.Slerp(frontNormal, -backNormal, 0.5f);
        normal.Normalize();
        return normal;
    }

    public Vector3 GetPosition(float pos)
    {
        return GetPosition(pos, true);
    }

    public Vector3 GetPosition(float pos, bool clamp)
    {
        if (points.Count == 0)
            return Vector3.zero;
        if (clamp)
        {
            pos = Mathf.Clamp(pos, 0.0f, 1.0f);
        }

        try
        {
            int numSections = points.Count - 3;
            if (numSections <= 0) return points[0];
            int currPt = Mathf.Min(Mathf.FloorToInt(pos * numSections), numSections - 1);
            if (currPt < 0 || currPt >= points.Count)
                return points[points.Count - 1];
            float u = pos * numSections - currPt;
            Vector3 a = points[currPt];
            Vector3 b = points[currPt + 1];
            Vector3 c = points[currPt + 2];
            Vector3 d = points[currPt + 3];
            return 0.5f * ((-a + 3.0f * b - 3.0f * c + d) * (u * u * u) + (2.0f * a - 5.0f * b + 4.0f * c - d) * (u * u) + (-a + c) * u + 2.0f * b);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
            return Vector3.zero;
        }

    }

    public float GetLength()
    {
        if (points.Count < 3) return 0.0f;
        float l = 0;
        for (int i = 1; i < points.Count - 2; i++)
        {
            if (points[i] == null || points[i + 1] == null) return 0;
            l += Vector3.Distance(points[i], points[i + 1]);
        }
        return l;
    }

    public static Vector3 Interpolate(Vector3[] p, float pos)
    {
        int numSections = p.Length - 3;
        if (numSections <= 0) return Vector3.zero;
        int currPt = Mathf.Min(Mathf.FloorToInt(pos * numSections), numSections - 1);
        float u = pos * numSections - currPt;
        Vector3 a = p[currPt];
        Vector3 b = p[currPt + 1];
        Vector3 c = p[currPt + 2];
        Vector3 d = p[currPt + 3];
        return 0.5f * ((-a + 3.0f * b - 3.0f * c + d) * (u * u * u) + (2.0f * a - 5.0f * b + 4.0f * c - d) * (u * u) + (-a + c) * u + 2.0f * b);
    }

}