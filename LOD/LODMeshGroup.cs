using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODMeshGroup : MonoBehaviour
{
    public int LodLevels = 2;
    public float[] compress = new float[] { 0.75f, 1.5f };
    public float smallObjectsValue = 5;

    void Awake()
    {
        Check();
    }

    void Start()
    {
        Check();
    }

    void Update()
    {
        Check();
    }
    private void Check()
    {
        if (this.transform.childCount == 0)
        {
            Object.DestroyImmediate(this, true);
        }
    }
}
