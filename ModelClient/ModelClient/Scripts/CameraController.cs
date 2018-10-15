using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    private Vector3 lastPlayerPos_ = new Vector3( -100, -100, -100 );

    public static CameraController Instance { get; private set; }

    public Transform targetTransform;

    private void Awake(){
        Instance = this;
       
    }

    private void LateUpdate()
    {
        if (targetTransform && targetTransform.position != lastPlayerPos_)
        {
            lastPlayerPos_ = targetTransform.position;
            this.transform.position = lastPlayerPos_ + new Vector3(0, 5, -5);
        }
        this.transform.localEulerAngles = new Vector3(45, 0, 0);
    }

}