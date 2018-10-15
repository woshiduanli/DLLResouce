using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    GameObject mapPlacement;
    XYObstacleCamera obstacleCamera_;
    public static void CreateBaseComponets()
    {
        var gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
    }

    void Awake()
    {
        AddMapPlacementComponents();
    }

    void OnApplicationQuit() 
    {
        if (obstacleCamera_)
        obstacleCamera_.obstacle_filename += "back";
        obstacleCamera_.SaveObstacleToFile();
    }

    private void AddMapPlacementComponents()
    {
        if (!mapPlacement)
        {
            mapPlacement = new GameObject("MapPlacement");
            var ObstacleCamera = new GameObject("XYObstacleCamera");
            ObstacleCamera.tag = XYDefines.Tag.MainCamera;

            ObstacleCamera.transform.parent = mapPlacement.transform;
            mapPlacement.transform.localPosition = Vector3.zero;
            ObstacleCamera.transform.localPosition = Vector3.zero;
            ObstacleCamera.transform.localEulerAngles = new Vector3(45.0f, 0.0f, 0.0f);

            var fc = mapPlacement.AddComponent<XYFreeCamera>();
            fc.NormalSpeed = 5;
            fc.FastSpeed = 5;

            var camera = ObstacleCamera.AddComponent<Camera>();
            obstacleCamera_ = ObstacleCamera.AddComponent<XYObstacleCamera>();
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.backgroundColor = Color.gray;
            camera.fieldOfView = 60;
            camera.nearClipPlane = 0.3f;
            camera.farClipPlane = 1000;
        }
    }

    #region 添加场景碰撞
    public static void AddAllSceneColliders()
    {
        Debug.Log("设置场景固定碰撞...");
        int totalAdded = 0;
        GameObject[] gameObjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gameObjs)
        {
            int result = AddColliderToGameObject(go);
            if (result == -1)
            {
                break;
            }
            else
            {
                totalAdded += result;
            }
        }
        Debug.Log(string.Format("设置结束，共设置:{0}", totalAdded));
    }

    private static int AddColliderToGameObject(GameObject go)
    {
        bool added = false;
        if (go.name.Contains("collider"))
        {
            MeshFilter mf = go.GetComponent(typeof(MeshFilter)) as MeshFilter;
            if (mf && mf.sharedMesh)
            {
                go.SetActive(true);
                if (go.GetComponent<Renderer>())
                    go.GetComponent<Renderer>().enabled = false;

                MeshCollider mc = go.GetComponent<MeshCollider>();
                if (!mc)
                {
                    mc = go.AddComponent<MeshCollider>() as MeshCollider;
                    added = true;
                }
                if (mc)
                {
                    mc.sharedMesh = mf.sharedMesh;
                }
                else
                {
                    Debug.Log(string.Format("MeshCollider没有找到:{0}", go.name));
                    Debug.LogError("Can't add MeshCollider, progress stopped", go);
                    return -1;
                }
            }
            else
            {
                Debug.Log(string.Format("MeshFilter或Mesh没有找到:{0}", go.name));
                Debug.LogError("MeshFilter or Mesh miss, progress stopped", go);
                return -1;
            }
        }
        return added ? 1 : 0;
    }
    #endregion
}
