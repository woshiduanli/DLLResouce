using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace G9ZUnityGUI {

    #region PropertyGrid用编辑控件基础对象 PropertyGridControlBase
    public class PropertyGridControlBase {
        protected System.Object obj_;
        protected PropertyInfo pi_;
        private string showName_;
        public string showName{ get{ return showName_; } }

        public virtual int sortValue{ get{ return 0; } }

        public PropertyGridControlBase( System.Object vObj, PropertyInfo vPi ) {
            obj_ = vObj;
            pi_ = vPi;
            CategoryAttribute[] catr = pi_.GetCustomAttributes( typeof( CategoryAttribute ), true ) as CategoryAttribute[];
            if ( catr.Length > 0 ) {
                showName_ = catr[ 0 ].Category;
            }
            else {
                showName_ = pi_.Name;
            }
        }

        public virtual void Draw() {
            GUILayout.BeginHorizontal();
            OnDrawName();
            if ( pi_.CanRead ) {
                System.Object result = OnDrawValue();
                if ( pi_.CanWrite ) {
                    pi_.SetValue( obj_, result, null );
                }
            }
            else {
                GUILayout.Button( "!Read" );
            }
            GUILayout.EndHorizontal();
        }

        protected virtual void OnDrawName() {
            GUILayout.Button( showName_ );
        }

        protected virtual System.Object OnDrawValue() {
            System.Object vValue = pi_.GetValue( obj_, null );
            GUILayout.Button( vValue.ToString() );
            return vValue;
        }
    }
    #endregion 

    #region 字符串编辑对象PGCString
    public class PGCString : PropertyGridControlBase {
        public override int sortValue{ get{ return 10; } }

        private string editingContent_;

        public PGCString( System.Object vObj, PropertyInfo vPi )
            : base( vObj, vPi ) {
        }

        protected override object OnDrawValue() {
            editingContent_ = GUILayout.TextArea( (string)pi_.GetValue( obj_, null ) );
            return editingContent_;
        }
    }
    #endregion

    #region Vector3编辑对象 PGCVector3
    public class PGCVector3 : PropertyGridControlBase {
        private float lastEditTime_ = 0.0f;
        public override int sortValue{ get{ return 1; } }

        private string editingContent_;

        public PGCVector3( System.Object vObj, PropertyInfo vPi )
            : base( vObj, vPi ) {
            editingContent_ = GameUtility.VectorToStr( (Vector3)pi_.GetValue( obj_, null ) );
        }

        protected override object OnDrawValue() {
            System.Object vValue = pi_.GetValue( obj_, null );
            string nowStr = GameUtility.VectorToStr( (Vector3)vValue );
            if ( Time.time - lastEditTime_ <= 2.0f ) {
                nowStr = editingContent_;
                editingContent_ = GUILayout.TextArea( nowStr );
                if ( editingContent_ != nowStr ) {
                    lastEditTime_ = Time.time;
                }
                Vector3 vRet = GameUtility.StrToVector3( editingContent_ );
                if ( vRet != Vector3.zero ) {
                    vValue = vRet;
                }
            }
            else {
                editingContent_ = GUILayout.TextArea( nowStr );
                if( editingContent_ != nowStr ){
                    lastEditTime_ = Time.time;
                }
            }
            return vValue;
        }
    }
    #endregion

    #region 布尔型编辑对象 PGCBool
    public class PGCBool : PropertyGridControlBase {
        public override int sortValue{ get{ return 20; } }

        public PGCBool( System.Object vObj, PropertyInfo vPi )
            : base( vObj, vPi ) {
        }

        protected override void OnDrawName() {
        }

        protected override object OnDrawValue() {
            bool bValue = (bool)pi_.GetValue( obj_, null );
            bValue = GUILayout.Toggle( bValue, this.showName );
            return bValue;
        }
    }
    #endregion 

    #region 枚举类型编辑对象PGCEnum
    public class PGCEnum : PropertyGridControlBase {
        public override int sortValue{ get{ return 5; } }

        private List<System.Object> enumValues_ = new List<object>();

        public PGCEnum( System.Object vObj, PropertyInfo vPi )
            : base( vObj, vPi ) {
            FieldInfo[] fis = vPi.PropertyType.GetFields();
            foreach( FieldInfo fi in fis ){
                if( fi.FieldType == vPi.PropertyType && fi.IsStatic ){
                    enumValues_.Add( fi.GetValue( null ) );
                }
            }
        }

        protected override object OnDrawValue() {
            GUI.SetNextControlName( pi_.Name );
            System.Object vValue = pi_.GetValue( obj_, null );
            if( GUILayout.Button( vValue.ToString() ) ){
                for( int i = 0; i < enumValues_.Count; i++ ){
                    if( enumValues_[i].ToString() == vValue.ToString() ){
                        if( i + 1 >= enumValues_.Count ){
                            vValue = enumValues_[0];
                        }
                        else{
                            vValue = enumValues_[i + 1];
                        }
                        break;
                    }
                }
            }
            return vValue;
        }
    }
    #endregion

    #region 数值型类型编辑对象(int,short,float等) PGCValue
    public class PGCValue : PropertyGridControlBase {
        public override int sortValue{ get{ return 15; } }

        private MethodInfo mi_;
        private string editingContent_;
        private System.Object lastValue_;

        public PGCValue( System.Object vObj, PropertyInfo vPi )
            : base( vObj, vPi ) {
            mi_ = vPi.PropertyType.GetMethod( "TryParse", new System.Type[] { typeof( string ), vPi.PropertyType.MakeByRefType() } );
            if ( mi_ == null ) {
                Debug.Log( "can't find TryParse" );
            }
            lastValue_ = pi_.GetValue( obj_, null );
            editingContent_ = lastValue_.ToString();
        }

        protected override object OnDrawValue() {
            System.Object vValue = pi_.GetValue( obj_, null );
            if( lastValue_.ToString() != vValue.ToString() ){
                editingContent_ = vValue.ToString();
                lastValue_ = vValue;
            }
            editingContent_ = GUILayout.TextField( editingContent_ );
            if ( mi_ != null ) {
                System.Object[] args = new System.Object[] { editingContent_, vValue };
                if ( (bool)mi_.Invoke( null, args ) ) {
                    vValue = args[ 1 ];
                    lastValue_ = vValue;
                }
            }
            return vValue;
            /*
            string nowStr = vValue.ToString();
                        if( Time.time - lastEditTime_ <= 2.0f ){
                            nowStr = editingContent_;
                            editingContent_ = GUILayout.TextField( nowStr );
                            if ( editingContent_ != nowStr ) {
                                lastEditTime_ = Time.time;
                            }
                            if ( mi_ != null ) {
                                System.Object[] args = new System.Object[] { editingContent_, vValue };
                                if ( (bool)mi_.Invoke( null, args ) ) {
                                    vValue = args[ 1 ];
                                }
                            }
                        }
                        else{
                            editingContent_ = GUILayout.TextField( nowStr );
                            if( editingContent_ != nowStr ){
                                lastEditTime_ = Time.time;
                            }
                        }
                        return vValue;*/
            
        }
    }
    #endregion

    #region UnityGUI用PropertyGrid控件 UGUIPropertyGrid
    public class UGUIPropertyGrid {
        private List<PropertyGridControlBase> pgCtrls_ = new List<PropertyGridControlBase>();

        private System.Object selectedObject_;
		//private Vector2 scrollPosition_ = Vector2.zero;
        public System.Object selectedObject {
            get { return selectedObject_; }
            set {
                selectedObject_ = value;
                pgCtrls_.Clear();
                if ( value != null ) {
                    PropertyInfo[] pi = value.GetType().GetProperties();
                    for ( int i = 0; i < pi.Length; i++ ) {
                        BrowsableAttribute[] baAttr = pi[i].GetCustomAttributes( typeof(BrowsableAttribute), true ) as BrowsableAttribute[];
                        if( baAttr.Length > 0 && !baAttr[0].Browsable ){
                            continue;
                        }
                        if ( pi[ i ].PropertyType == typeof( string ) ) {
                            pgCtrls_.Add( new PGCString( selectedObject_, pi[ i ] ) );
                        }
                        else if ( pi[ i ].PropertyType == typeof( Vector3 ) ) {
                            pgCtrls_.Add( new PGCVector3( selectedObject_, pi[ i ] ) );
                        }
                        else if( pi[ i ].PropertyType == typeof(bool) ){
                            pgCtrls_.Add( new PGCBool( selectedObject_, pi[ i ] ) );
                        }
                        else if ( pi[ i ].PropertyType.IsEnum ) {
                            pgCtrls_.Add( new PGCEnum( selectedObject_, pi[ i ] ) );
                        }
                        else if ( pi[ i ].PropertyType.BaseType == typeof( System.ValueType ) ) {
                            pgCtrls_.Add( new PGCValue( selectedObject_, pi[ i ] ) );
                        }
                        else {
                            pgCtrls_.Add( new PropertyGridControlBase( selectedObject_, pi[ i ] ) );
                        }
                    }
//                     pgCtrls_.Sort(
//                         delegate(PropertyGridControlBase e1, PropertyGridControlBase e2) {
//                             return e2.sortValue - e1.sortValue;
//                         }
//                         );
                }
            }
        }

        public void OnGUI() {
            if ( selectedObject_ == null ) {
                return;
            }
            //scrollPosition_ = GUILayout.BeginScrollView( scrollPosition_, GUILayout.ExpandWidth( true ) );
            foreach ( PropertyGridControlBase pgc in pgCtrls_ ) {
                pgc.Draw();
            }
            //GUILayout.EndScrollView();
        }
    }
    #endregion

    #region 滑动确定控件
    public class UGUIConfirmSlider{
        private float lastChangeTime_ = 0.0f;
        private float curValue_ = 0.0f;
        private float width_ = 20.0f;
        private string showName_ = string.Empty;
        public UGUIConfirmSlider( string vName, float vWidth ){
            showName_ = vName;
            width_ = vWidth;
        }

        public bool OnDraw(){
            GUILayout.BeginHorizontal();
            GUILayout.Label( showName_ );
            float newValue = GUILayout.HorizontalSlider( curValue_, 0.0f, 1.0f, GUILayout.MaxWidth( width_ ) );
            GUILayout.EndHorizontal();
            if( newValue != curValue_ ){
                lastChangeTime_ = Time.time;
            }
            curValue_ = newValue;
            if( Time.time - lastChangeTime_ > 0.5f ){
                curValue_ = 0.0f;
            }
            if( curValue_ >= 1.0f ){
                curValue_ = 0.0f;
                return true;
            }
            else{
                return false;
            }
        }
    }
    #endregion

    #region 长按确定控件
    public class UGUIConfirmButton {
        private float pushLasting_ = 0.0f;
        private float confirmNeedTime_ = 0.0f;
        private string showName_ = string.Empty;
        private int falseCount_ = 0;
        private bool done_ = false;
        private float miniSize = 10.0f;

        public UGUIConfirmButton( string vName, float vConfirmNeedTime ) {
            confirmNeedTime_ = vConfirmNeedTime;
            pushLasting_ = confirmNeedTime_;
            showName_ = vName;
            miniSize = vName.Length * 20.0f;
        }

        public bool OnDraw() {
            string info;
            if( done_ ){
                info = "OK";
            }
            else{
                info = pushLasting_ >= confirmNeedTime_ ? showName_ : pushLasting_.ToString("F1");
            }
            if ( GUILayout.RepeatButton( info, GUILayout.MinWidth( miniSize ) ) ) {
                falseCount_ = 0;
                if( !done_ ){
                    pushLasting_ -= Time.deltaTime;
                    if ( pushLasting_ <= 0.0 ) {
                        pushLasting_ = confirmNeedTime_;
                        done_ = true;
                        return true;
                    }
                }
            }
            else {
                falseCount_++;
                if( falseCount_ > 5 ){
                    pushLasting_ = confirmNeedTime_;
                    done_ = false;
                }
            }
            return false;
        }
    }
    #endregion
}
