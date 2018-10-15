using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour {
    private Transform cameraGOtf;
    public float distance = 8;
    private Vector2 oldPosition1; 
    private Vector2 oldPosition2;
    public Vector3 rotate = new Vector3(55, 0, 0);
    public static CameraControll Instance;
	void Start ()
    {
        cameraGOtf = this.transform;
        Instance = this;
	}

    void LateUpdate()
    {
        //if (!MianPlayControl.Instance)
        //{
        //    return;
        //}
        var rotation = Quaternion.Euler(rotate);
        Vector3 pos = Vector3.zero;
        pos = MianPlayControl.Instance.transform.position;
        var position = rotation * (new Vector3(0, 0, -distance)) + pos;
        cameraGOtf.eulerAngles = rotate;
        cameraGOtf.position = position;
    }

}
