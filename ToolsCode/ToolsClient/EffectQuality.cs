using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectQuality : MonoBehaviour
{
    public GameObject[] HighQua;
    public bool Show;
    private bool value;

    private void OnEnable()
    {
        if (HighQua == null || HighQua.Length == 0)
        {
            Destroy(this);
            return;
        }
        Do();
    }

    private void OnDisable()
    {
        value = false;
    }

    private void Update()
    {
        if (value == Show)
            return;

        value = Show;
        Do();
    }

    private void Do()
    {
        value = Show;
        for (int i = 0; i < HighQua.Length; i++)
        {
            GameObject go = HighQua[i];
            if (go && go.activeSelf != Show)
                go.SetActive(Show);
        }
    }

}

public class EffectControll : MonoBehaviour
{
    public GameObject[] HighQua;
    public static bool ShowHigh = true;
    private bool value;

    private void OnEnable()
    {
        if (HighQua == null || HighQua.Length == 0)
        {
            Destroy(this);
            return;
        }
        Do();
    }

    private void OnDisable()
    {
        value = false;
    }


    private void Update()
    {
        if (value == ShowHigh)
            return;

        value = ShowHigh;
        Do();
    }

    private void Do()
    {
        value = ShowHigh;
        for (int i = 0; i < HighQua.Length; i++)
            SetActive(HighQua[i]);
    }

    private void SetActive(GameObject go)
    {
        if (go && go.activeSelf != ShowHigh)
            go.SetActive(ShowHigh);
    }
}

public class SoundControll : MonoBehaviour
{
    public AudioSource[] Sounds;
    public static float Volume = 1;
    private float value;

    private void OnEnable()
    {
        if (Sounds == null || Sounds.Length == 0)
        {
            Destroy(this);
            return;
        }
        Do();
    }

    private void OnDisable()
    {
        value = 0;
    }

    private void Update()
    {
        if (value == Volume)
            return;
        value = Volume;
        Do();
    }

    private void Do()
    {
        value = Volume;
        for (int i = 0; i < Sounds.Length; i++)
        {
            AudioSource so = Sounds[i];
            if (Volume <= 0.05f)
            {
                if (so.isPlaying)
                    so.Stop();
            }
            else
            {
                if (!so.isPlaying)
                    so.Play();
                if (so.volume != Volume)
                    so.volume = Volume;
            }
        }
    }
}

public class MusicControll : MonoBehaviour
{
    public AudioSource[] Sounds;
    public static float Volume = 1;
    private float value;

    private void OnEnable()
    {
        if (Sounds == null || Sounds.Length == 0)
        {
            Destroy(this);
            return;
        }
        Do();
    }

    private void OnDisable()
    {
        value = 0;
    }

    private void Update()
    {
        if (value == Volume)
            return;
        value = Volume;
        Do();
    }

    private void Do()
    {
        value = Volume;
        for (int i = 0; i < Sounds.Length; i++)
        {
            AudioSource so = Sounds[i];
            if (Volume <= 0.05f)
            {
                if (so.isPlaying)
                    so.Stop();
            }
            else
            {
                if (!so.isPlaying)
                    so.Play();
                if (so.volume != Volume)
                    so.volume = Volume;
            }
        }
    }
}

