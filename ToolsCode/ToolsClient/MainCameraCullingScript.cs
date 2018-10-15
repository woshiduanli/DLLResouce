using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MainCameraCullingScript : MonoBehaviour
{
    public List<GameObject> cullings = new List<GameObject>(); 
    private List<GameObject> needShowCullings = new List<GameObject>(); 

    public static float showDistance = 30.0f;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().enabled = true;

        Invoke("SetAllCullingsActiveToFalse", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        SetOtherColliderActive(other,true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetOtherColliderActive(other,false);
    }

    private void SetOtherColliderActive(Collider other,bool isShow)
    {
        if (other.gameObject.layer == 18)
        {
            float distance = (other.gameObject.transform.position - transform.position).magnitude;
            distance = Mathf.Abs(distance);

            if (distance > showDistance && !isShow)
            {
                other.transform.FindChild("Root").gameObject.SetActive(false);
            }
            else
            {
                other.transform.FindChild("Root").gameObject.SetActive(true);

                if (needShowCullings != null)
                {
                    needShowCullings.Add(other.gameObject);
                }
            }
        }
    }

    private void SetAllCullingsActiveToFalse()
    {
        List<GameObject> needMissCullings = new List<GameObject>();

        foreach (GameObject culling in cullings)
        {
            if (culling != null)
            {
                needMissCullings.Add(culling);
            }
        }

        foreach (GameObject needShow in needShowCullings)
        {
            for(int i = 0;i<needMissCullings.Count;i++)
            {
                if (needMissCullings[i] == needShow)
                {
                    needMissCullings.Remove(needShow);
                }
            }
        }

        foreach (GameObject needMiss in needMissCullings)
        {
            if(needMiss != null)
                needMiss.transform.FindChild("Root").gameObject.SetActive(false);
        }

        needShowCullings.Clear();
        needShowCullings = null;
    }
}
