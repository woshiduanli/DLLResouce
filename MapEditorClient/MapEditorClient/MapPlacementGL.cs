using UnityEngine;
using System.Collections;
using System.IO;
using MapPlacement;

public class MapPlacementGL : MonoBehaviour {
    public GUISkin skin;
    private MPRole curSelectRole { get { return MapPlacementController.instance.curSelectObj as MPRole; } }
    private MPItem curSelectItem { get { return MapPlacementController.instance.curSelectObj as MPItem; } }
    private Vector3[] dirForQuadsDraw_ = new Vector3[4];
    private Vector3 lastDrawAngle_ = Vector3.forward;
    private static Material material_;
    void Start() {
        dirForQuadsDraw_[0] = new Vector3( -0.25f, 0, 0.25f );
        dirForQuadsDraw_[1] = new Vector3( 0.25f, 0, 0.25f );
        dirForQuadsDraw_[2] = new Vector3( 0.25f, 0, -0.25f );
        dirForQuadsDraw_[3] = new Vector3( -0.25f, 0, -0.25f );
        CreateMaterial();
    }

    private void CreateMaterial()
    {
        if (!material_)
        {
            //material_ = new Material(
            //    "Shader \"Lines/Colored Blended\" {" +
            //    "SubShader { Pass {" +
            //    "   BindChannels { Bind \"Color\",color }" +
            //    "   Blend SrcAlpha OneMinusSrcAlpha" +
            //    "   ZWrite Off Cull Off Fog { Mode Off } ZTest Always" +
            //    "} } }");
            //material_.hideFlags = HideFlags.HideAndDontSave;
            //material_.shader.hideFlags = HideFlags.HideAndDontSave;
            //Object.DontDestroyOnLoad(material_);
            material_ = new Material(Shader.Find("LinesColored Blended"));
        }
    }

    public void BeforeUseGL()
    {
        material_.SetPass(0);
    }

    void DrawRolePath( MPRole mpr ){
        BeforeUseGL();
        GL.Color( Color.red );
        GL.Begin( GL.LINES );
        {
            GL.Vertex( mpr.position );
            GL.Vertex( mpr.routeList[ 0 ].position );

            for ( int i = 0; i < mpr.routeList.Count - 1; i++ ) {
                GL.Vertex( mpr.routeList[ i ].position );
                GL.Vertex( mpr.routeList[ i + 1 ].position );
            }
        }
        GL.End();
        GL.Color( Color.blue );
        GL.Begin( GL.QUADS );
        for ( int i = 0; i < mpr.routeList.Count; i++ ) {
            Vector3 orgPos = mpr.routeList[ i ].position;
            if ( MapPlacementController.instance.curSelectRoutePoint == mpr.routeList[ i ] ) {
                GL.Color( Color.yellow );
            }
            else {
                GL.Color( Color.blue );
            }
            for ( int k = 0; k < 4; k++ ) {
                GL.Vertex( orgPos + dirForQuadsDraw_[ k ] );
            }
        }
        GL.End();
    }

    void NormalDraw(){
        BeforeUseGL();
        if ( MapPlacementController.instance.curSelectObj != null ) {
            Vector3 orgPos = MapPlacementController.instance.curSelectObj.position;
            GL.Color( Color.cyan );
            GL.Begin( GL.QUADS );
            for ( int k = 0; k < 4; k++ ) {
                GL.Vertex( orgPos + dirForQuadsDraw_[ k ] * -0.2f );
            }
            GL.End();
        }
        if ( this.curSelectRole != null && curSelectRole.routePointCount > 0 ) {
            DrawRolePath( this.curSelectRole );
        }
        else if ( this.curSelectItem != null && curSelectItem.radius > 0 ) {
            BeforeUseGL();
            GL.Color( Color.yellow );
            GL.Begin( GL.LINES );
            GL.Vertex( curSelectItem.position );
            lastDrawAngle_ = Quaternion.Euler( 0.0f, 180.0f * Time.deltaTime, 0.0f ) * lastDrawAngle_;
            Vector3 destPos = XYClientCommon.AdjustToGround( curSelectItem.position + lastDrawAngle_ * curSelectItem.radius );
            GL.Vertex( destPos );

            GL.End();
        }
    }

    void DrawAllPath(){
        foreach( MPObject mpo in MapPlacementController.instance ){
            if( mpo is MPRole ){
                MPRole mpr = mpo as MPRole;
                if( mpr.routePointCount > 0 ){
                    DrawRolePath( mpr );
                }
            }
        }
    }
    void OnPostRender() {
        if( MapPlacementController.instance.AllRoutePointMode ){
            DrawAllPath();
        }
        else{
            NormalDraw();
        }
        GL.Clear(false, false, Color.clear);
    }
}
