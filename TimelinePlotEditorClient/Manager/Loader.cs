using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelAction = Model.Action;
using System;

public class Loader : MonoBehaviour {

    public static Loader Instance { get; private set; }
    public static void Init()
    {
        if (!Instance)
        {
            var go = new GameObject("ActionLoader");
            DontDestroyOnLoad(go);
            go.AddComponent<Loader>();
        }
    }
    private void Awake()
    {
        Instance = this;
    }


    private Dictionary<string, UnityEngine.Object> roleAssets=new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, TLEditorWww> roleAssetsWww = new Dictionary<string, TLEditorWww>();

    public void CreatAction(string actionName,Action<ModelAction> loadFinish)
    {
        XYCoroutineEngine.Execute(LoadActionObject(string.Format("res/action/{0}.action", actionName), loadFinish));
    }

    private IEnumerator LoadActionObject(string path,Action<ModelAction> loadFinish)
    {
        TLEditorWww roleWww = TLEditorWww.Create(path);
        while (!roleWww.Finished)
            yield return null;
        ModelAction actionConfig = roleWww.GetAsset() as Model.Action;
        if (actionConfig == null)
            Debug.LogError("Error, Load role Failed: " + path);
        else
            Debug.Log(actionConfig.ActionEvents.Length);
        roleWww.Unload();
        loadFinish(actionConfig);
    }



    public void CreateRoleObject(int apprid,Action<RoleObject> loadFinish)
    {
        XYCoroutineEngine.Execute(LoadRoleObject(string.Format("res/role/{0}.role", apprid), loadFinish));
    }

    private IEnumerator LoadRoleObject(string path, Action<RoleObject> loadFinish)
    {
        UnityEngine.Object role = null;
        TLEditorWww roleWww = null;

        if (!roleAssetsWww.TryGetValue(path, out roleWww))
        {
            roleWww = TLEditorWww.Create(path);
            roleAssetsWww.Add(path, roleWww);
        }

        while (!roleWww.Finished)
            yield return null;

        if (!roleAssets.TryGetValue(path, out role))
        {
            role = roleWww.GetCachedAsset() as GameObject;
            GameManager.ReplaceShader(role, string.Empty);
            roleAssets.Add(path, role);
        }

        if (role == null)
            Debug.LogError("Error, Load role Failed: " + path);
        else
        {
            Role roleConfig = (role as GameObject).GetComponent<Role>();
            if (roleConfig == null)
                Debug.Log("config==null");
            GameObject roleGo = ModelLoader.CreateRole(roleConfig).gameObject;
            var performer = roleGo.GetComponent<ActionPerformer>();
            RoleObject roleObj = roleGo.GetComponent<RoleObject>();
            roleObj.needAnimationButtonGui = false;
            roleObj.CheckAnimator();
            roleObj.enabled = true;
            //roleWww.Unload();
            loadFinish(roleObj);
        }
    }

    //private IEnumerator LoadRoleObject(string path, Action<RoleObject> loadFinish)
    //{
    //    UnityEngine.Object role = null;
    //    TLEditorWww roleWww = TLEditorWww.Create(path);

    //    roleWww = TLEditorWww.Create(path);

    //    while (!roleWww.Finished)
    //        yield return null;

    //    role = roleWww.GetAsset() as GameObject;
    //    GameManager.ReplaceShader(role, string.Empty);

    //    if (role == null)
    //        Debug.LogError("Error, Load role Failed: " + path);
    //    else
    //    {
    //        Role roleConfig = (role as GameObject).GetComponent<Role>();
    //        if (roleConfig == null)
    //            Debug.Log("config==null");
    //        GameObject roleGo = ModelLoader.CreateRole(roleConfig).gameObject;
    //        var performer = roleGo.GetComponent<ActionPerformer>();
    //        RoleObject roleObj = roleGo.GetComponent<RoleObject>();
    //        roleObj.needAnimationButtonGui = false;
    //        roleObj.CheckAnimator();
    //        roleObj.enabled = true;
    //        //roleWww.Unload();
    //        loadFinish(roleObj);
    //    }
    //}


    public void CreatTimeline(string name, Action<RoleObject> loadFinish)
    {
        XYCoroutineEngine.Execute(LoadTimeline(string.Format("res/timeline/{0}.timeline", name), loadFinish));
    }

    private IEnumerator LoadTimeline(string path, Action<RoleObject> loadFinish)
    {
        TLEditorWww roleWww = TLEditorWww.Create(path);
        while (!roleWww.Finished)
            yield return null;
        GameObject timeline = null;
        timeline = roleWww.GetAsset() as GameObject;
        if (timeline == null)
            Debug.LogError("Error, Load role Failed: " + path);
        else
        {
            Instantiate(timeline);
        }
    }



    public void CreatEffect(string name,Action<EffectObject> loadFinish,bool isUIEffect)
    {
        string path = string.Format("res/effect/{0}.go", name);
        if (isUIEffect)
            path = string.Format("res/effect/ui/{0}.go", name);
        XYCoroutineEngine.Execute(LoadEffect(path, loadFinish));
    } 

    private  IEnumerator LoadEffect(string path,Action<EffectObject> loadFinish)
    {
        TLEditorWww effectWWW = TLEditorWww.Create(path);
        while (!effectWWW.Finished)
            yield return null;
        EffectObject effObj = new EffectObject();
        effObj.OnCreate(effectWWW.GetAsset());
        GameManager.ReplaceShader(effObj.gameObject, string.Empty);
        effectWWW.Unload();
        loadFinish(effObj);
    }



    public void CreatTexture(string name,Action<Texture> loadFinish)
    {
        XYCoroutineEngine.Execute(LoadTexture(string.Format("res/ui/tex/{0}.tex", name), loadFinish));
    }

    private IEnumerator LoadTexture(string path, Action<Texture> loadFinish)
    {
        TLEditorWww effectWWW = TLEditorWww.Create(path);
        while (!effectWWW.Finished)
            yield return null;
        Texture texture = effectWWW.GetAsset() as Texture;
        yield return null;
        effectWWW.Unload();
        loadFinish(texture);
    }


    Queue<string> aniClipQueue = new Queue<string>();
    Queue<Action<AnimationClip>> loadFinishQueue = new Queue<Action<AnimationClip>>();
    TLEditorWww curAniWWW;
    public void CreatAniClips(string path,Action<AnimationClip> loadFinish)
    {
        aniClipQueue.Enqueue(path);
        loadFinishQueue.Enqueue(loadFinish);
        if (curAniWWW == null)
        {
            aniClipQueue.Dequeue();
            loadFinishQueue.Dequeue();
            XYCoroutineEngine.Execute(LoadAniClip(path, loadFinish));
        }
    }

    private IEnumerator LoadAniClip(string path, Action<AnimationClip> loadFinish)
    {
        curAniWWW = TLEditorWww.Create(path);
        while (!curAniWWW.Finished)
            yield return null;
        AnimationClip clip = curAniWWW.GetAsset() as AnimationClip;
        yield return null;
        curAniWWW.Unload();
        loadFinish(clip);
        curAniWWW = null;
        if (aniClipQueue.Count > 0)
        {
            XYCoroutineEngine.Execute(LoadAniClip(aniClipQueue.Dequeue(), loadFinishQueue.Dequeue()));
        }
    }


    public void CreatAudioClips(string name, Action<AudioClip> loadFinish)
    {
        XYCoroutineEngine.Execute(LoadAudioClip(string.Format("res/audio/{0}.audio", name), loadFinish));
    }

    private IEnumerator LoadAudioClip(string path, Action<AudioClip> loadFinish)
    {
        TLEditorWww www = TLEditorWww.Create(path);
        while (!www.Finished)
            yield return null;
        AudioClip clip = www.GetAsset() as AudioClip;
        yield return null;
        www.Unload();
        loadFinish(clip);
    }



    private void Update()
    {

    }
}
