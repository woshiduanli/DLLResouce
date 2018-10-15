using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FixedPitchCamera : MonoBehaviour{
    private const float MAX_DELTATIME = 1f/30f;

    /*
     * 在以玩家为原点的坐标系中
     *		yaw 原点到摄像机的偏航角（围绕Y轴的转角，正Z方向为0度，正X方向为90度）
     *		pitch 原点到摄像机的仰角（与XZ平面的夹角）
     */
    private static float yawCurrent_ = 225;
    private static float yaw_ = 225; // 摄像机相当于世界Z轴的偏航角
    private static float pitch_; // 摄像机相当于世界XZ平面的仰角
    private static float distance_ = 8; // 摄像机到玩家的距离

    private static float lastPlayerYaw_ = -400; // 上一次玩家的偏航角
    private static Vector3 lastPlayerPos_ = new Vector3( -100, -100, -100 ); // 上一次玩家的位置

    public static readonly int CAMERA_COLLISION_IGNORE_LAYERS = 0;

    public static readonly int MIN_CAMERA_SPEED = 60;
    public static readonly int MAX_CAMERA_SPEED = 360;
    public static readonly int DEFAULT_CAMERA_SPEED = 260;
    public static bool IsCameraShock = true;
    public bool AutoBehind = false; // 当玩家移动、转动时，是否自动将摄像机移到玩家身后？
    public float MaxAngle = 45f; // 最高可以将摄像机移到哪个垂直角度
    public float MaxDistance = 16; // 最大距离
    public float MiniAngle = 25f; //-15f;	// 最低可以将摄像机移到哪个垂直角度
    public float MiniDistance = 4f; // 最小距离
    public Transform Target;
    public float TargetYOffset = 0.9f; // 被观察点的偏移
    private float cameraPosSmooth_;
    private float deltaTime_;
    private bool mouseBothDowning_ = false;

    private bool mouseLeftDowning_, mouseRightDowning_;
    private Camera thisCamera_;

    public static FixedPitchCamera Instance { get; private set; }

    public static float Distance{
        get { return distance_; }
    }


    public Camera Camera{
        get { return thisCamera_; }
    }

    public float Yaw{
        get { return yawCurrent_; }
    }


    public bool IsMouseBothDowning{
        get { return mouseBothDowning_; }
    }

    public bool IsMouseLeftDowning{
        get { return mouseLeftDowning_; }
    }

    public bool IsMouseRightDowning{
        get { return mouseRightDowning_; }
    }

    public bool IsMouserAnyDowning{
        get { return mouseLeftDowning_ || mouseRightDowning_; }
    }

    public static void AdjustYaw( float value ){
        yaw_ = value;
    }

    // 检测摄像机碰撞时，忽略哪些层


    // ---- methods

    private IEnumerator Start(){
        //AfterPlayerUpdate( ts, true );
        yield return null;
    }

    private void Awake(){
        Instance = this;
        thisCamera_ = GetComponent<Camera>();
        thisCamera_.nearClipPlane = 0.3f;
    }

    public Vector3 GetCameraForward(){
        return Quaternion.Euler( 0, Yaw - 180, 0 )*Vector3.forward;
    }

    private static float ClampAngle( float angle, float minValue, float maxValue ){
        return Mathf.Clamp( RepeatAngle( angle ), minValue, maxValue );
    }

    private static float RepeatAngle( float angle ){
        if( angle <= -360 ){
            angle += 360;
        }
        else if( angle >= 360 ){
            angle -= 360;
        }
        return angle;
    }

    private void AdjustAngleFromUserMouseDrag( float x, float y ){
        if( x != 0 ){
            //yawCurrent_ = yaw_ = RepeatAngle(yawCurrent_ + X * CustomSetting_Client.Instance.CameraSpeedEx);
            yawCurrent_ = yaw_ = RepeatAngle( yawCurrent_ + x*10 );
        }
        /*if ( y != 0 ) {
            //pitch_ = ClampAngle(pitch_ - y * CustomSetting_Client.Instance.CameraSpeedEx, MIN_VERT_ANGLE, MAX_VERT_ANGLE);
            pitch_ = ClampAngle( pitch_ - y * speed, MiniAngle, MaxAngle );
        }*/
    }

    // 根据鼠标滚轮和+-键调整摄像机与目标的距离
    private void AdjustDistanceFromInput(){
        float wheelDelta = Input.GetAxis( "Mouse ScrollWheel" );
        if( Math.Abs( 0 - wheelDelta ) > float.Epsilon ){
            distance_ = Mathf.Clamp( distance_ - wheelDelta*300*deltaTime_, MiniDistance, MaxDistance );
        }
    }

    private static Vector3 CalcCameraPos( Vector3 targetPos, float radiansYaw, float radiansPitch, float distance ){
        if( distance != 0 ){
            float sinPitch = Mathf.Sin( radiansPitch );
            float cosPitch = Mathf.Cos( radiansPitch );
            float cosYaw = Mathf.Cos( radiansYaw );
            float sinYaw = Mathf.Sin( radiansYaw );
            float v = cosPitch*distance;
            targetPos.z += v*cosYaw;
            targetPos.y += sinPitch*distance;
            targetPos.x += v*sinYaw;
        }
        return targetPos;
    }

    private static float CalcCurrentYaw( float current, float expected, float deltaTime ){
        float delta = expected - current;
        if( delta == 0f || delta == 360f ){
            return expected;
        }
        delta = RepeatAngle( Mathf.Abs( delta ) );
        float sqr = delta*delta;
        if( sqr == 0 ){
            return expected;
        }
        float smooth = 360/sqr;
        if( float.IsNaN( smooth ) ){
            return expected;
        }
        if( smooth < 10 ){
            smooth = 10;
        }
        return Mathf.LerpAngle( current, expected, deltaTime*smooth );
    }

    private void Change( Transform player, bool noSmooth ){
        if( mouseLeftDowning_ || mouseRightDowning_ ){
            yawCurrent_ = yaw_;
        }
        else{
            yawCurrent_ = CalcCurrentYaw( yawCurrent_, yaw_, deltaTime_ );
        }
        //
        Vector3 targetPos = player.position;
        targetPos.y += TargetYOffset;

        pitch_ = MiniAngle + ( distance_/( MaxDistance - MiniDistance ) )*( MaxAngle - MiniAngle );

        Vector3 cameraPos = CalcCameraPos( targetPos, yawCurrent_*Mathf.Deg2Rad, pitch_*Mathf.Deg2Rad, distance_ );
        Vector3 cameraCurrentPos = transform.position;

        // 摄像机位置不变，直接返回
        if( cameraPos == cameraCurrentPos ){
            return;
        }
        SetCameraPos( transform, cameraCurrentPos, cameraPos, noSmooth );
        transform.LookAt( targetPos );
    }

    private void SetCameraPos( Transform ts, Vector3 currentPos, Vector3 setTo, bool noSmooth ){
        if( !noSmooth ){
            if( ( setTo - currentPos ).sqrMagnitude < 12*12 ){
                if( cameraPosSmooth_ < 1 ){
                    cameraPosSmooth_ += deltaTime_*5f;
                    ts.position = Vector3.Lerp( currentPos, setTo, cameraPosSmooth_ );
                    return;
                }
            }
        }
        ts.position = setTo;
        //cameraPosSmooth_ = 0;
    }

    // 在玩家移动之前被调用，计算摄像机应该在的位置
    public float BeforePlayerUpdate( float deltaTime ){
        deltaTime_ = deltaTime;
        if( deltaTime_ > MAX_DELTATIME ){
            deltaTime_ = MAX_DELTATIME;
        }
        //
        AdjustDistanceFromInput();
        mouseLeftDowning_ = Input.GetMouseButton( 0 ); // XYInput.GetMouseButton(XYInput.MouseButton.Left);
        mouseRightDowning_ = Input.GetMouseButton( 1 );

        float x, y = 0;
        //锁定视觉游戏，y轴不允许调整
        if( mouseLeftDowning_ || mouseRightDowning_ ){
            x = Input.GetAxisRaw( "Mouse X" );
            //y = Input.GetAxisRaw( "Mouse Y" );
            AdjustAngleFromUserMouseDrag( x, y );
        }

        return yaw_;
    }

    public void OnPlayerLookAt( Transform player ){
        lastPlayerPos_ = player.transform.position;
        lastPlayerYaw_ = player.eulerAngles.y;
    }

    private void AfterPlayerUpdate( Transform player, bool noSmooth ){
        //if (deltaTime > MAX_DELTATIME) {
        //    deltaTime = MAX_DELTATIME;
        //}
        Vector3 playerPos = player.transform.position;
        float playerYaw = player.eulerAngles.y;
        if( mouseBothDowning_ ){
            lastPlayerPos_ = playerPos;
            lastPlayerYaw_ = playerYaw;
        }
        else{
            // 先判断玩家是否已经移动过
            bool targetChanged = ( lastPlayerPos_.x != playerPos.x ) || ( lastPlayerPos_.z != playerPos.z );
            if( targetChanged ){
                lastPlayerPos_ = playerPos;
            }
            // 判断玩家是否转身
            if( lastPlayerYaw_ != playerYaw ){
                lastPlayerYaw_ = playerYaw;
                targetChanged = true;
            }
        }
        Change( player, noSmooth );
    }

    private void LateUpdate(){
        if( Target ){
            BeforePlayerUpdate( Time.deltaTime );
            AfterPlayerUpdate( Target, true );
        }
    }

}