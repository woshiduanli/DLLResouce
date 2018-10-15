using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour {
    public GameObject target;
    public Camera Camera_;
    void Update()
    {
        if (!target || !Camera_)
            return;
        Camera_.transform.LookAt(target.transform);
    }
}
