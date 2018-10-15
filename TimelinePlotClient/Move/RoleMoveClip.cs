using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

public enum MoveMotion
{
    Walk,
    Run
}

public class RoleMoveClip : PlayableAsset
{

    public List<Vector3> points;
    public MoveWay moveWay;
    public RotateWay rotateWay;
    public MoveMotion moveMotion;
    [HideInInspector]
    public RoleData roleData;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        PlayableDirector director = owner.GetComponent<PlayableDirector>();
        var playable = ScriptPlayable<RoleMovePlayable>.Create(graph);
        RoleMovePlayable movePlayable = playable.GetBehaviour();
        if (movePlayable == null)
            return playable;
        movePlayable.roleData = roleData;
        movePlayable.points = points;
        movePlayable.moveWay = moveWay;
        movePlayable.rotateWay = rotateWay;
        movePlayable.moveMotion = moveMotion;
        movePlayable.executer = BehaviourExecuterFactory.GetMoveExecuter(movePlayable);
        if (movePlayable.executer == null)
            return playable;
        movePlayable.executer.OnPlayableCreate(playable);
        return playable;
    }


}



public class RoleMovePlayable : MYPlayableBehaviour
{
    public RoleData roleData;
    public List<Vector3> points;
    public MoveWay moveWay;
    public RotateWay rotateWay;
    public MoveMotion moveMotion;
    public MoveClipCurve curve;

    public override void OnMYBehaviourStart(Playable playable)
    {
        List<Vector3> temp = new List<Vector3>(points);
        curve = new MoveClipCurve(temp);
        if (executer == null)
            return;
        executer.OnBehaviourStart(playable);
    }

    public void AddPlayerCurrentPos(Vector3 pos)
    {
        if (points == null)
            return;
        List<Vector3> temp = new List<Vector3>(points);
        temp.Insert(0, pos);
        if (curve == null)
            curve = new MoveClipCurve(temp);
        else
            curve.RefreshPoint(temp);
    }
}


public enum MoveWay
{
    NavMesh,
    Other
}

public enum RotateWay
{
    Hard,
    Soft
}


public class MoveClipCurve
{
    private List<Vector3> points = new List<Vector3>();


    public MoveClipCurve(List<Vector3> points)
    {
        this.points = points;
        if (points.Count > 0)
        {
            points.Insert(0, points[0]);
            points.Add(points[points.Count - 1]);
        }
    }

    public void RefreshPoint(List<Vector3> points)
    {
        this.points = points;
        if (points.Count > 0)
        {
            points.Insert(0, points[0]);
            points.Add(points[points.Count - 1]);
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
