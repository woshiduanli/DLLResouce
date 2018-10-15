using UnityEngine;
using System.Collections;

public class FaqiControll : MonoBehaviour
{
    private Transform transform_ = null;
    string clippath = string.Empty;
    GameObject child;
    public static void CreateFaqi(string path, GameObject go)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        GameObject parent = new GameObject("faqi");
        parent.transform.parent = go.transform.parent;
        parent.transform.localPosition = Vector3.zero;
        parent.transform.localEulerAngles = Vector3.zero;
        parent.transform.localScale = Vector3.one;

        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;

        FaqiControll fc = parent.AddComponent<FaqiControll>();
        fc.child=go;
        fc.clippath = path;
    }

    void Start()
    {
        XYSingleAssetLoader.AsyncLoad(clippath, AnimationLoad, null);
        transform_ = this.transform;
    }

    private void AnimationLoad(object contex, UnityEngine.Object res)
    {
        if (res)
        {
            AnimationClip ac = res as AnimationClip;
            if (!ac)
            {
                return;
            }
            if (!child.GetComponent<Animation>())
            {
                child.AddComponent(typeof(Animation));
            }
            ac.name = "round";
            child.GetComponent<Animation>().AddClip(ac, ac.name);
            child.GetComponent<Animation>()[ac.name].wrapMode = WrapMode.Loop;
            child.GetComponent<Animation>().CrossFade(ac.name);
        }

    }

    void LateUpdate()
    {
        transform_.position = transform_.parent.transform.position;
        transform_.eulerAngles = Vector3.zero;
    }
}
