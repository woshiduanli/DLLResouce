using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour 
{
    public float timeScale = 0.3f;
    void OnEnable()
    {
        Time.timeScale = timeScale;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
}
