using UnityEngine;
using System.Collections;

public class MianPlayControl : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent nav_;
    public UnityEngine.AI.NavMeshAgent nav { get { return nav_; } }
    private static MianPlayControl instance_;
    public static MianPlayControl Instance { get { return instance_; } }
    void Start()
    {
        instance_ = this;
        nav_ = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
        nav_.acceleration = 5000;
        nav_.radius = 0.1f;
        nav_.speed = 4;
        nav_.angularSpeed = 360;
        nav_.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
    }
	

	// Update is called once per frame
	void Update ()
    {
        Vector3 inputpos = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            inputpos = Input.mousePosition;
        }
        if (inputpos == Vector3.zero)
        {
            return;
        }
        RaycastHit hit;
        Ray roleDetectRay = Camera.main.ScreenPointToRay(inputpos);
        if (Physics.Raycast(roleDetectRay, out hit, 1000.0f, XYDefines.Layer.Mask.Terrain))
        {
            nav_.SetDestination(hit.point);
        }  
	}
}
