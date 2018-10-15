using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }
    public static void Init()
    {
        if (!Instance)
        {
            var go = new GameObject("World");
            DontDestroyOnLoad(go);
            go.AddComponent<World>();
        }
    }

    //private int snRecord;
    public RoleObject mainPlayer;
    //private int mpSn;
    private Dictionary<RoleData, RoleObject> autoLoadRoles;
    private List<EffectObject> effects;

    private Dictionary<int, RoleObject> manualLoadRoles;

    private void Awake()
    {
        Instance = this;
        //snRecord = 1;
        autoLoadRoles = new Dictionary<RoleData, RoleObject>();
        manualLoadRoles = new Dictionary<int, RoleObject>();
        effects = new List<EffectObject>();
    }

    public void CreatRoleObjIfHaveNot(RoleData roledata, Action<RoleObject> loadFinish = null)
    {
        RoleReference roleRef = Global.role_mgr.GetReference(roledata.Id);
        Loader.Instance.CreateRoleObject(roleRef.Apprid, a =>
        {
            GameUtility.SetLayerRecusive(8, a.gameObject);
            //roledata.Sn = snRecord;
            //snRecord++;
            autoLoadRoles.Add(roledata, a);
            var agent = a.gameObject.AddComponent<NavMeshAgent>();
            InitNavMesh(agent);
            if (loadFinish != null)
                loadFinish(a);
        });
    }

    private void InitNavMesh(NavMeshAgent nav_)
    {
        nav_.acceleration = 5000;
        nav_.radius = 0.1f;
        nav_.speed = 4;
        nav_.stoppingDistance = 0f;
        nav_.angularSpeed = 3600;
        nav_.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        nav_.enabled = false;
    }

    public void AddMainplayer(RoleData roledata,Action<RoleObject> loadFinish = null)
    {
        if (mainPlayer)
        {
            autoLoadRoles.Remove(roledata);
            Destroy(mainPlayer.gameObject);
        }
        mainPlayer = null;
        RoleReference roleRef = Global.role_mgr.GetReference(roledata.Id);
        Loader.Instance.CreateRoleObject(roleRef.Apprid, a =>
        {
            GameUtility.SetLayerRecusive(8, a.gameObject);
            a.gameObject.AddComponent<MianPlayControl>();
            mainPlayer = a;
            loadFinish(a);
            //mpSn = snRecord;
            //roledata.Sn = mpSn;
            autoLoadRoles.Add(roledata, a);
            //snRecord++;
        });
    }

    public void ClearRole()
    {
        foreach(RoleData roleData in autoLoadRoles.Keys)
        {
            RoleObject roleObj = autoLoadRoles[roleData];
            //roles.Remove(roleData);
            if(roleObj)
                GameObject.Destroy(roleObj.gameObject);
        }
        autoLoadRoles.Clear();
    }

    public void ClearTimelineReside()
    {
        ClearRole();
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }
        effects.Clear();
    }

    public void RemoveRole(RoleObject roleObj)
    {
       foreach (var pair in autoLoadRoles)
       {
            if (pair.Value == roleObj)
            {
                autoLoadRoles.Remove(pair.Key);
                break;
            }
       }
        foreach (var pair in manualLoadRoles)
        {
            if (pair.Value == roleObj)
            {
                manualLoadRoles.Remove(pair.Key);
                break;
            }
        }
        GameObject.Destroy(roleObj.gameObject);
    }

    public void CreatRoleObjIfHaveNot(int id, Action<RoleObject> loadFinish = null)
    {
        RoleReference roleRef = Global.role_mgr.GetReference(id);
        Loader.Instance.CreateRoleObject(roleRef.Apprid, a =>
        {
            GameUtility.SetLayerRecusive(8, a.gameObject);
            var agent = a.gameObject.AddComponent<NavMeshAgent>();
            InitNavMesh(agent);
            manualLoadRoles[id] = a;
            if (loadFinish != null)
                loadFinish(a);
        });
    }

    public void AddMainplayer(int id, Action<RoleObject> loadFinish = null)
    {
        if (mainPlayer)
            return;
        RoleReference roleRef = Global.role_mgr.GetReference(id);
        Loader.Instance.CreateRoleObject(roleRef.Apprid, a =>
        {
            GameUtility.SetLayerRecusive(8, a.gameObject);
            a.gameObject.AddComponent<MianPlayControl>();
            mainPlayer = a;
            loadFinish(a);
        });
    }

    public RoleObject GetRoleObj(RoleData data)
    {
        if (autoLoadRoles.ContainsKey(data))
            return autoLoadRoles[data];
        return null;
    }

    public RoleObject GetAutoLoadRole(int id)
    {
        foreach (var data in autoLoadRoles)
        {
            if (data.Key.Id == id)
                return data.Value;
        }
        return null;
    }

    public RoleObject GetRoleObj(int id)
    {
        if (manualLoadRoles.ContainsKey(id))
            return manualLoadRoles[id];
        return null;
    }


    public void AddEffect(EffectObject effect)
    {
        if(!effects.Contains(effect))
            effects.Add(effect);
    }

    public void DestroyEffect(EffectObject effect)
    {
        if (!effects.Contains(effect))
            effects.Remove(effect);
        GameObject.DestroyImmediate(effect.gameObject);
    }


    private List<EffectObject> needDestroy = new List<EffectObject>();
    private void Update()
    {
        foreach(RoleData roleData in autoLoadRoles.Keys)
        {
            if(autoLoadRoles[roleData])
                roleData.transform.position = autoLoadRoles[roleData].transform.position;
        }

        for(int i=0;i<effects.Count;i++)
        {
            effects[i].OnUpdate();
            if (effects[i].isGoDestroy)
            {
                effects.RemoveAt(i);
            }
        }
    }
}

