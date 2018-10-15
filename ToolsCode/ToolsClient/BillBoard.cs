using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    public bool Lock;
    public Transform TheCamera;
    void Start()
    {
        if (!TheCamera)
            TheCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = TheCamera.transform.rotation;
        if (Lock)
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }
}
