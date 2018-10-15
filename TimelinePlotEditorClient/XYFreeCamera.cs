using UnityEngine;

public class XYFreeCamera : MonoBehaviour {

    public float ScrollSpeed = 15;
    public float ScrollFastSpeed = 30;
    public float TurnSpeed = 60;
    public bool canrotate = false;

    [SerializeField]
    public static float MoveSpeed=1;

    //private Vector3 CenterPoint = Vector3.zero;

    void Reset() {
        ScrollSpeed = 15;
        ScrollFastSpeed = 30;
        TurnSpeed = 60;
    }

    void Start() {
        GenerateCenterPoint();
    }

    public void GenerateCenterPoint()
    {
		 //RaycastHit rayHit;
		 //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayHit))
		 //{
		 //    CenterPoint = rayHit.point;
		 //}
    }

    void Update() {
        float zoom;

        if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ){
            zoom = Input.GetAxisRaw( "Mouse ScrollWheel" ) * ScrollFastSpeed;
        }
        else{
            zoom = Input.GetAxisRaw( "Mouse ScrollWheel" ) * ScrollSpeed;
        }

        if( zoom != 0 ) {
            transform.Translate( 0, -zoom, zoom);
        }

        if (Input.GetMouseButtonDown(1))
        {
            canrotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            canrotate = false;
        }

        float rotation = Input.GetAxisRaw( "XRotate" ) * Time.deltaTime * ( Input.GetKey( KeyCode.LeftShift ) ? TurnSpeed / 2 : TurnSpeed ) ;

        if (rotation != 0 && canrotate)
        {
            this.transform.Rotate( Vector3.up, -rotation );
        }

        //float x = Input.GetAxisRaw( "Horizontal" ) * Time.deltaTime;
        //float y = Input.GetAxisRaw( "Vertical" ) * Time.deltaTime;

        //if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ) {
        //    x *= FastSpeed;
        //    y *= FastSpeed;
        //}
        //else {
        //    x *= NormalSpeed;
        //    y *= NormalSpeed;
        //}

        //if( x != 0 ) {
        //    transform.Translate( x*NormalSpeed, 0, 0 );
        //    GenerateCenterPoint();
        //}
        //if( y != 0 ) {
        //    Vector3 forward = transform.forward;
        //    forward.y = 0;
        //    transform.position = transform.position + forward * y*NormalSpeed;
        //    GenerateCenterPoint();
        //}
 
    }
}
