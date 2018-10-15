using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig : MonoBehaviour
{
    public CamereConfig CamereCfg;
    public ShadowConfig ShadowCfg;

    [System.Serializable]
    public class CamereConfig
    {
        public Vector3 Angle;
        public float Height;
    }

    [System.Serializable]
    public class ShadowConfig
    {
        public Vector3 Angle;
        public Vector3 Postion;
        public float Alpha;

        public List<GameObject> Terrains;
    }

    [System.Serializable]
    public class AssetConfig
    {
        public List<TextureConfig> Configs;
        public Material Material;
        public string Shader;
    }

    [System.Serializable]
    public class TextureConfig
    {
        public string Texture;
        public string PropertyName;
    }

    public List<AssetConfig> Mainbuilding = new List<AssetConfig>();
    public List<AssetConfig> Other = new List<AssetConfig>();
}

public class JumpConfig : MonoBehaviour
{
    public bool Once;
    public Transform EndPos;
    public float JumpPower = 5;
    public float JumpNum = 1;
    public float Duration = 1;
    public CTrigger cTrigger;
}
