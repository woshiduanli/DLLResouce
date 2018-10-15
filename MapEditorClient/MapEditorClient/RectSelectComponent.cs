using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapPlacement;
using System;
using UnityEditor;


public class SelectorInfo {
    public GameObject gameObject;
    private Renderer[] renders;
    private List<Color> materialColors = new List<Color>();
    public Bounds bounds;
    public MPObject mapObject;

    public SelectorInfo(MPObject obj) {
        this.mapObject = obj;
        this.gameObject = obj.mapGO;
        this.renders = this.gameObject.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renders.Length; i++) {
            this.bounds.Encapsulate(this.renders[i].bounds);
            this.materialColors.Add(this.renders[i].material.color);
        }
    }

    public void Select() {
        for (int i = 0; i < renders.Length; i++) {
            renders[i].material.color = Color.green;
        }
    }

    public void Unselect() {
        for (int i = 0; i < renders.Length; i++) {
            renders[i].material.color = materialColors[i];
        }
    }
}

public class RectSelectComponent : MonoBehaviour {
    private Material _mat;
    private bool isSelectBegin = false;
    private Vector3 selectStartPos = Vector3.zero;
    private Vector3 selectEndPos = Vector3.zero;
    private bool isWantCopy = false;

    private List<SelectorInfo> datas = new List<SelectorInfo>();
    private List<SelectorInfo> selects = new List<SelectorInfo>();

    public Func<IEnumerator<MPObject>> getMapObjectsEvent = null;
    public Func<List<MPObject>, List<MPObject>> copyMapObjectsOp = null;
    public Action<MPObject> deleteMapObjectsOp = null;
    public Action<List<MPObject>> dragMapObjectsOp = null;
    public Action<List<MPObject>> dragBeginMapObjectsOp = null;
    public Action<List<MPObject>> dragEndMapObjectsOp = null;

    void Start() {
        if (!_mat) {
            //_mat = new Material("Shader \"Lines/Colored Blended\" {" + "SubShader { Pass { " + "    Blend SrcAlpha OneMinusSrcAlpha " + " ZWrite Off Cull Off Fog { Mode Off } " + "    BindChannels {" + "      Bind \"vertex\", vertex Bind \"color\", color }" + "} } }");
            _mat = new Material(Shader.Find("LinesColored Blended"));
        }
    }

    void OnDestroy() {
        Destroy(_mat);
    }

    private List<MPObject> GetMapObjects() {
        List<MPObject> mapObjects = new List<MPObject>();
        if (getMapObjectsEvent == null)
            return mapObjects;

        IEnumerator<MPObject> mpEnumerator = getMapObjectsEvent();
        while (mpEnumerator.MoveNext())
            mapObjects.Add(mpEnumerator.Current);
        return mapObjects;
    }

    private void RefreshSelectorInfos() {
        List<MPObject> temp = GetMapObjects();

        //add
        for (int i = 0; i < temp.Count; i++) {
            if (!Contains(temp[i]))
                datas.Add(new SelectorInfo(temp[i]));
        }

        //remove
        for (int i = datas.Count - 1; i >= 0; i--) {
            if (!temp.Contains(datas[i].mapObject))
                datas.Remove(datas[i]);
        }
    }

    private bool Contains(MPObject mpObj) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].mapObject == mpObj)
                return true;
        }

        return false;
    }

    // Update is called once per frame
    void OnGUI() {
        if (Input.GetMouseButtonDown(2)) {
            RefreshSelectorInfos();

            isSelectBegin = true;
            selectStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) {
            selectEndPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2)) {
            isSelectBegin = false;
            Vector3 center = (selectStartPos + selectEndPos) * 0.5f;
            Vector3 size = new Vector3(Mathf.Abs(selectStartPos.x - selectEndPos.x), Mathf.Abs(selectStartPos.y - selectEndPos.y), 0);
            Bounds bounds = new Bounds(center, size);

            CheckBounds(bounds);
        }


        if (isCombinationKey(EventModifiers.Alt, KeyCode.None,EventType.MouseDown)) {
            RefreshSelectorInfos();
            isSelectBegin = true;
            selectStartPos = Input.mousePosition;
            selectEndPos = Input.mousePosition;
        }
        if (isCombinationKey(EventModifiers.Alt, KeyCode.None, EventType.MouseDrag)) {
            selectEndPos = Input.mousePosition;
        }
        if (isCombinationKey(EventModifiers.Alt, KeyCode.None, EventType.MouseUp)) {
            isSelectBegin = false;
            Vector3 center = (selectStartPos + selectEndPos) * 0.5f;
            Vector3 size = new Vector3(Mathf.Abs(selectStartPos.x - selectEndPos.x), Mathf.Abs(selectStartPos.y - selectEndPos.y), 0);
            Bounds bounds = new Bounds(center, size);

            CheckBounds(bounds);
        }
        if (isCombinationKey(EventModifiers.Control, KeyCode.C, EventType.KeyDown)) {
            isWantCopy = true;
        }
        if (isCombinationKey(EventModifiers.Control, KeyCode.V, EventType.KeyDown)) {
            if (isWantCopy && selects.Count > 0 && copyMapObjectsOp != null) {
                List<MPObject> temp = new List<MPObject>();
                for (int i = 0; i < selects.Count; i++)
                {
                    temp.Add(selects[i].mapObject);
                    selects[i].Unselect();
                }
                List<MPObject> mapobjs = copyMapObjectsOp(temp);

                for (int i=0;i<selects.Count;i++)
                {
                    RefreshSelectorInfos();
                    selects[i].Select();
                }
                UnityEngine.Object[] objs = new UnityEngine.Object[mapobjs.Count];
                for (int i = 0; i < mapobjs.Count;i++ ) {
                    objs[i] = mapobjs[i].mapGO;
                }
                Selection.objects = objs;
            }
        }
        if (isCombinationKey(EventModifiers.None, KeyCode.Delete, EventType.KeyDown)) {
            if (deleteMapObjectsOp != null) {
                for (int i = 0; i < selects.Count; i++)
                    deleteMapObjectsOp(selects[i].mapObject);
            }
            selects.Clear();
        }
        if (isCombinationKey(EventModifiers.Shift, KeyCode.None, EventType.MouseDown)) {
            if (dragMapObjectsOp != null) {
                List<MPObject> temp = new List<MPObject>();
                for (int i = 0; i < selects.Count; i++)
                    temp.Add(selects[i].mapObject);
                dragMapObjectsOp(temp);
            }
        }
        if (isCombinationKey(EventModifiers.Shift, KeyCode.None, EventType.MouseDown)) {
            if (dragBeginMapObjectsOp != null) {
                List<MPObject> temp = new List<MPObject>();
                for (int i = 0; i < selects.Count; i++)
                    temp.Add(selects[i].mapObject);
                dragBeginMapObjectsOp(temp);
            }
        }
        if (isCombinationKey(EventModifiers.Shift, KeyCode.None, EventType.MouseUp)) {
            if (dragEndMapObjectsOp != null) {
                List<MPObject> temp = new List<MPObject>();
                for (int i = 0; i < selects.Count; i++)
                    temp.Add(selects[i].mapObject);
                dragEndMapObjectsOp(temp);
            }
        }
    }

    private bool isCombinationKey(EventModifiers prekey, KeyCode postkey, EventType postkeyevent) {
        if (prekey != EventModifiers.None) {
            bool eventDown = (Event.current.modifiers & prekey) != 0;
            if (eventDown && Event.current.rawType == postkeyevent && Event.current.keyCode == postkey) {
                Event.current.Use();

                //if (postkey != KeyCode.None)
                //    Debug.Log(string.Format("{0} + {1}", prekey.ToString(), postkey.ToString()));
                //else
                //    Debug.Log(string.Format("{0} + {1}", prekey.ToString(), postkeyevent.ToString()));

                return true;
            }
        } else {
            if (Event.current.rawType == postkeyevent && Event.current.keyCode == postkey) {
                Event.current.Use();

                //if (postkey != KeyCode.None)
                //    Debug.Log(string.Format("{0}", postkey.ToString()));
                //else
                //    Debug.Log(string.Format("{0}", postkeyevent.ToString()));

                return true;
            }
        }
        return false;
    }

    void DrawBox(Vector3 p1, Vector3 p2) {
        _mat.SetPass(0);
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.QUADS);
        GL.Color(new Color(Color.green.r, Color.green.g, Color.green.b, 0.1f));
        GL.Vertex3(p1.x, p1.y, 0);
        GL.Vertex3(p2.x, p1.y, 0);
        GL.Vertex3(p2.x, p2.y, 0);
        GL.Vertex3(p1.x, p2.y, 0);
        GL.End();

        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex(p1);
        GL.Vertex3(p1.x, p2.y, 0);
        GL.Vertex3(p1.x, p2.y, 0);
        GL.Vertex(p2);
        GL.Vertex(p2);
        GL.Vertex3(p2.x, p1.y, 0);
        GL.Vertex3(p2.x, p1.y, 0);
        GL.Vertex(p1);
        GL.End();
        GL.PopMatrix();
    }

    void OnPostRender() {
        if (isSelectBegin) {
            DrawBox(selectStartPos, selectEndPos);
        }
    }

    void CheckBounds(Bounds bounds) {
        selects.Clear();
        isWantCopy = false;
        for (int i = 0; i < datas.Count; i++) {
            GameObject _cubeObj = datas[i].gameObject;
            if (_cubeObj) {
                Vector3 pos = Camera.main.WorldToScreenPoint(_cubeObj.transform.position);
                pos.z = bounds.center.z;
                if (bounds.Contains(pos)) {
                    selects.Add(datas[i]);
                }
            }
        }

        for (int i = 0; i < datas.Count; i++) {
            if(selects.Contains(datas[i]))
                datas[i].Select();
            else
                datas[i].Unselect();
        }

        //Selection activtiy
        UnityEngine.Object[] selectobjs = new UnityEngine.Object[selects.Count];
        for (int i = 0; i < selects.Count; i++) {
            selectobjs[i] = selects[i].gameObject;
        }
        Selection.objects = selectobjs;
    }
}