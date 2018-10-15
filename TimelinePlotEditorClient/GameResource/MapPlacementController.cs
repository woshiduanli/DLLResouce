using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
[System.Serializable]
public class MapPlacementController : MonoBehaviour
{
    public static MapPlacementController instance = null;
    public static MapReference curMap = null;
    //public static MapReference
    public static string textResourcesPath = Directory.GetCurrentDirectory() + "/resources";
    private const int SelectableLayer = XYClientCommon.StandingLayer | XYDefines.Layer.Mask.Player;

    void Start()
    {
        RenderSettings.fog = false;
        instance = this;
        
        GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");
        foreach (GameObject terrain in terrains)
            if (terrain)
            {
                Collider co = terrain.GetComponent<Collider>();
                if (co == null)
                    continue;
                this.gameObject.transform.position = new Vector3(co.bounds.size.x / 2, 3.5f, co.bounds.size.z / 2);
                break;
            }
    }

    private Vector3 mouseButton1DownPos;
    private Vector3 lastFrameMousePosition_ = Vector3.zero;
    private GameObject selectedObj;
    private GameObject dragObject;
    private void LateUpdate()
    {
        if (CameraControll.Instance.controlType== CameraControlType.FollowMp)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
            if (Physics.Raycast(ray, out hit, 2000f,LayerMask.GetMask("Terrain")))
                mouseButton1DownPos = hit.point;

        if (Input.GetMouseButtonUp(1))
        {
            if (Physics.Raycast(ray, out hit, 2000f,LayerMask.GetMask("Terrain")) && Vector3.Distance(hit.point, mouseButton1DownPos) <= 0.1)
            {
                if (loadType == 0)
                    World.Instance.AddMainplayer(playerConfigId, roleobj =>
                        roleobj.transform.position = hit.point
                    );
                else if (loadType == 1)
                    TimelineManager.Instance.GenerateMovePoint(hit.point);
                else
                    World.Instance.CreatRoleObjIfHaveNot(playerConfigId, roleobj =>
                        roleobj.transform.position = hit.point
                    );
            }
        }

        if (CameraControll.Instance.controlType == CameraControlType.Manual)
        {
            if (Input.GetMouseButtonUp(0))
            {
                dragObject = null;
                lastFrameMousePosition_ = Vector3.zero;
                if (Physics.Raycast(ray, out hit,2000f, LayerMask.GetMask("Player")))
                    selectedObj = hit.transform.GetComponentInParent<RoleObject>().gameObject;
                else if (Physics.Raycast(ray, out hit,2000f, LayerMask.GetMask("WayPoint")))
                    selectedObj = hit.transform.gameObject;
                else
                    selectedObj = null;
            }
            if (Input.GetMouseButtonDown(0))
            {
                lastFrameMousePosition_ = Input.mousePosition;
                if (Physics.Raycast(ray, out hit,2000f, LayerMask.GetMask("Player")))
                    dragObject = hit.transform.GetComponentInParent<RoleObject>().gameObject;
                else if (Physics.Raycast(ray, out hit,2000f, LayerMask.GetMask("WayPoint")))
                    dragObject = hit.transform.gameObject;
                else
                    dragObject = null;
            }
        }
    }

    private void CameraMovementHandle()
    {
        if (lastFrameMousePosition_ == Vector3.zero)
        {
            lastFrameMousePosition_ = Input.mousePosition;
        }
        if (lastFrameMousePosition_ != Input.mousePosition)
        {
            Vector3 newPos = (lastFrameMousePosition_ - Input.mousePosition) * this.transform.position.y / 1000.0f;
            this.transform.Translate(newPos.x * XYFreeCamera.MoveSpeed, 0, newPos.y * XYFreeCamera.MoveSpeed);
            lastFrameMousePosition_ = Input.mousePosition;
        }
    }


    private bool showGui = true;
    int playerConfigId=0;
    int loadType = 0;//0:mainplayer 1:pathPoint 2:other
    private string timelineName="";
    private Rect roleLoadWindowRect=new Rect(100,0,0,0);
    private Rect timeLineLoadWindowRect = new Rect(400, 0, 0, 0);
    private Rect selectRoleObjWindowRect = new Rect(800, 0, 0, 0);
    void OnGUI()
    {
        showGui=GUI.Toggle(new Rect(0, 0, 60, 20), showGui, "显示UI");
        if (!showGui)
            return;
        roleLoadWindowRect = GUILayout.Window(1, roleLoadWindowRect, DrawRoleLoadWindow, "角色加载", GUILayout.Width(300));
        timeLineLoadWindowRect = GUILayout.Window(2, timeLineLoadWindowRect, DrawTimelineLoadWindow, "Timeline加载", GUILayout.Width(300));
        if (selectedObj != null)
            selectRoleObjWindowRect = GUILayout.Window(3, selectRoleObjWindowRect, DrawSelectRoleObjWindow, "选中物体编辑", GUILayout.Width(300));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Event.current.type == EventType.MouseDrag && Input.GetMouseButton(0))
        {
            if (dragObject != null)
            {
                if (Physics.Raycast(ray, out hit, 2000.0f, LayerMask.GetMask("Terrain")))
                    dragObject.transform.position = hit.point;
            }
            else
                CameraMovementHandle();
        }
    }

    private void DrawSelectRoleObjWindow(int id)
    {
        GUILayout.Label("位置：");
        selectedObj.transform.position = GUILayout.TextField(selectedObj.transform.position.VectorToString()).ToVector3();
        
    }

    private void DrawTimelineLoadWindow(int id)
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timeline名称:");
        timelineName = GUILayout.TextField(timelineName);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("加载"))
        {
            TimelineManager.Instance.LoadTimeline(timelineName);
        }
        if (GUILayout.Button("播放"))
        {
            TimelineManager.Instance.PlayTimeLine(timelineName);
        }
        if (GUILayout.Button("保存"))
        {
            TimelineManager.Instance.SavePrefab(timelineName);
        }
        if (GUILayout.Button("暂停"))
        {
            TimelineManager.Instance.PauseTimeline(timelineName);
        }

        GUILayout.EndVertical();
    }

    private void DrawRoleLoadWindow(int id)
    {
        loadType = GUILayout.SelectionGrid(loadType, new string[] { "主角","路径点", "其他" }, 3);
        CameraControll.Instance.controlType = (CameraControlType)GUILayout.SelectionGrid((int)CameraControll.Instance.controlType,
            new string[] { "跟随", "手动","CineMachine" }, 3);
        GUILayout.BeginHorizontal();
        GUILayout.Label("角色配置ID：", GUILayout.Width(90));
        playerConfigId = Convert.ToInt32(GUILayout.TextField(playerConfigId.ToString(), GUILayout.Width(60)));
        GUILayout.EndHorizontal();
    }

}