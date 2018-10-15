using UnityEngine;

public class XYFreeCamera : MonoBehaviour {

    public float NormalSpeed = 32;
    public float FastSpeed = 64;
    float TurnSpeed = 60;
    public bool canrotate = false;
	//private Vector3 CenterPoint = Vector3.zero;

    void Reset() {
        NormalSpeed = 32;
        FastSpeed = 64;
        TurnSpeed = 90;
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
            zoom = Input.GetAxisRaw( "Mouse ScrollWheel" ) * NormalSpeed;
        }
        else{
            zoom = Input.GetAxisRaw( "Mouse ScrollWheel" ) * FastSpeed;
        }

        if( zoom != 0 ) {
            transform.Translate( 0, -zoom, zoom);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            canrotate = !canrotate;
        }

        float rotation = Input.GetAxisRaw( "XRotate" ) * Time.deltaTime * ( Input.GetKey( KeyCode.LeftShift ) ? TurnSpeed / 2 : TurnSpeed ) ;

        if (rotation != 0 && canrotate)
        {
            this.transform.Rotate( Vector3.up, -rotation );
        }

        float x = Input.GetAxisRaw( "Horizontal" ) * Time.deltaTime;
        float y = Input.GetAxisRaw( "Vertical" ) * Time.deltaTime;

        if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ) {
            x *= FastSpeed;
            y *= FastSpeed;
        }
        else {
            x *= NormalSpeed;
            y *= NormalSpeed;
        }

        if( x != 0 ) {
            transform.Translate( x, 0, 0 );
            GenerateCenterPoint();
        }
        if( y != 0 ) {
            Vector3 forward = transform.forward;
            forward.y = 0;
            transform.position = transform.position + forward * y;
            GenerateCenterPoint();
        }
 
    }
}
