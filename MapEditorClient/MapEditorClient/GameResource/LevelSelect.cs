using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//[ExecuteInEditMode()]
public class LevelSelect : MonoBehaviour
{
    public static LevelSelect Instance { get; private set; }
    private static Rect rcWnd_ = new Rect(0, 0, 0, 0);
    private Rect NpcPlacementWindowRect_ = new Rect(0, 0, 0, 0);
    private bool dataInfoEntered_;

    private MapReference[] mrs_;
    private List<string> mapsName=new List<string>();
    private Vector2 scrollPosition_;
  
    private void Awake()
    {
        GameManager.CreateBaseComponets();
    }


    private void NpcPlacementWindow(int winID)
    {
        if (mrs_ != null)
        {
            scrollPosition_ = GUILayout.BeginScrollView(scrollPosition_);
            {
                foreach (MapReference mr in mrs_)
                {
                    string showName = string.Format("{0}_{1}", mr.ID, mr.Name);
                    if (GUILayout.Button(showName))
                    { 
                        MapPlacementController.curMap = mr;
                        XYCoroutineEngine.Execute(GameManager.LoadLevel(mr.FileName, mr));
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        GUI.DragWindow(new Rect(0, 0, 1000, 20));
    }

    private void DataBaseInfoWindow(int windID)
    {
        MapPlacementController.textResourcesPath = GUILayout.TextArea(MapPlacementController.textResourcesPath);
        if (GUILayout.Button("Use Text Data"))
        {
            GameDataHelper.ReloadDataFromFile(MapPlacementController.textResourcesPath, EditionType.ALL, false);
            dataInfoEntered_ = true;
            //mrs_=LoadMapInfoFromTxt().ToArray();
            mrs_ = Global.mapr_mgr.ToArray();
        }
        GUILayout.Space(10);
    }

    void OnGUI()
    {
        if (dataInfoEntered_)
        {
            NpcPlacementWindowRect_ = GUILayout.Window(1, NpcPlacementWindowRect_, NpcPlacementWindow, "选择地图",GUILayout.Height(400), GUILayout.Width(300));
        }
        else
        {
            rcWnd_ = GUILayout.Window(2, rcWnd_, DataBaseInfoWindow, "数据库信息", GUILayout.Width(100));
        }
    }

    public List<MapReference> LoadMapInfoFromTxt()
    {
        List<MapReference> mapInfos = new List<MapReference>();
        StreamReader reader= File.OpenText(MapPlacementController.textResourcesPath + "/Map.txt");
        string line;
        while ((line = reader.ReadLine())!=null)
        {
            string[] values=line.Split('\t');
            if (values.Length > 5)
            {
                mapInfos.Add(new MapReference()
                {
                    ID = int.Parse(values[1]),
                    Name=values[3],
                    FileName=values[5],

                });
            }
        }
        return mapInfos;
    }
}