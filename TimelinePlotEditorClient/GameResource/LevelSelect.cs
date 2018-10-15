using System.Collections.Generic;
using UnityEngine;
using System.IO;

//[ExecuteInEditMode()]
public class LevelSelect : MonoBehaviour
{
    public static LevelSelect Instance { get; private set; }
    private Rect chooseMapWindowRect = new Rect(0, 0, 0, 0);

    private MapReference[] mrs_;
    private List<string> mapsName=new List<string>();
    private Vector2 scrollPosition_;
  
    private void Awake()
    {
        GameManager.CreateBaseComponets();
    }

    public void Start()
    {
        GameDataHelper.ReloadDataFromFile(MapPlacementController.textResourcesPath, EditionType.ALL, false);
        mrs_ = LoadMapInfoFromTxt().ToArray();
        mrs_ = Global.mapr_mgr.ToArray();
    }

    private void DrawChooseMapWindow(int winID)
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
                        XYCoroutineEngine.Execute(GameManager.instance_.LoadLevel(mr.FileName, mr));
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        GUI.DragWindow(new Rect(0, 0, 1000, 20));
    }

    void OnGUI()
    {
        chooseMapWindowRect = GUILayout.Window(1, chooseMapWindowRect, DrawChooseMapWindow, "选择地图", GUILayout.Height(400), GUILayout.Width(300));
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