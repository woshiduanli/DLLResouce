using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class CameraControll : MonoBehaviour {

    public CameraControlType controlType= CameraControlType.Manual;

    private Transform cameraGOtf;
    public float distance = 8;
    private Vector2 oldPosition1; 
    private Vector2 oldPosition2;
    public Vector3 rotate;
    public static CameraControll Instance;
    public Camera camera;

    private List<CameraImageEffect> images = new List<CameraImageEffect>();
    private CameraImageEffect[] effects = new CameraImageEffect[(int)ImageEffect.MAX];

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        cameraGOtf = this.transform;
        camera = GetComponent<Camera>();

        effects[(int)ImageEffect.Gray] = new CameraGrayEffect(this);
        effects[(int)ImageEffect.Blur] = new CameraBlurEffect(this);
        effects[(int)ImageEffect.RadialBlur] = new CameraRadialBlur(this);
        effects[(int)ImageEffect.Shock] = new CameraShockEffect(this);
        effects[(int)ImageEffect.ScreenBlack] = new ScreenBlackEffect(this);
    }

    void LateUpdate()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].Update(this))
            {
                images.Remove(images[i]);
                break;
            }
            images[i].Update(this);
        }

        if (controlType == CameraControlType.Manual)
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(45, 0, 0);
            return;
        }
        if (controlType == CameraControlType.CineMachine)
            return;
        if (MianPlayControl.Instance == null)
            return;
        rotate = new Vector3(55, 0, 0);
        transform.eulerAngles = rotate;
        var rotation = Quaternion.Euler(rotate);
        Vector3 pos = Vector3.zero;
        pos = MianPlayControl.Instance.transform.position;
        var position = rotation * (new Vector3(0, 0, -distance)) + pos;
        cameraGOtf.eulerAngles = rotate;
        cameraGOtf.position = position;
    }


    #region ÉãÏñ»úÐ§¹û£¨»ÒÆÁ ÕðÆµ Ö±ÏßÄ£ºý Ô¶¾°Ä£ºý£©

    public void RemoveImageEffect(ImageEffect ie)
    {
        for (int i = 0; i < images.Count; i++)
        {
            CameraImageEffect effect = images[i];
            if (effect.state == ie)
            {
                effect.Leave(this);
                images.Remove(effect);
                break;
            }
        }
    }

    public void AddImageEffect(ImageEffect efType, GameObject effGo, float value)
    {
        //Debug.Log(string.Format("addimageeffect:{0}", effGo));
        OnEnterEffect(efType, effGo, value);
    }

    private CameraImageEffect AddImageEffect(ImageEffect ie)
    {
        CameraImageEffect effect = null;
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].state == ie)
            {
                effect = images[i];
                break;
            }
        }
        if (effect == null)
        {
            effect = effects[(int)ie];
            if (effect.Enter(this))
                images.Add(effect);
        }
        return effect;
    }
   
    private void OnEnterEffect(ImageEffect efType,GameObject effGo,float value)
    {
        switch (efType)
        {
            case ImageEffect.Shock:
                {
                    CameraShock(effGo,value);
                }
                break;
            case ImageEffect.ScreenBlack:
                {
                    EnterScreenBlack(value);
                }
                break;
            case ImageEffect.Gray:
            case ImageEffect.RadialBlur:
            case ImageEffect.Blur:
                {
                    AddImageEffect(efType);
                }
                break;
        }
    }

    private void OnLeaveEffect(ImageEffect efType)
    {
        RemoveImageEffect(efType);
    }

    private void CameraShock(GameObject Shockobj,float shockFrequency)
    {
        Debug.Log(string.Format("CameraShock:{0}", Shockobj));
        if (!Shockobj)
            return;
        CameraShockEffect shock = AddImageEffect(ImageEffect.Shock) as CameraShockEffect;
        shock.CameraShockTF = Shockobj.transform;
        shock.shockFrequency = shockFrequency;
    }

    private void EnterScreenBlack(float value)
    {
        ScreenBlackEffect sb = AddImageEffect(ImageEffect.ScreenBlack) as ScreenBlackEffect;
        sb.duration = value;
    }

    public class CameraImageEffect
    {
        public ImageEffect state = ImageEffect.MAX;
        protected bool stop = true;
        protected bool isdone = false;

        public CameraImageEffect(CameraControll owner)
        {
            state = ImageEffect.MAX;
        }

        public bool Update(CameraControll owner)
        {
            if (stop)
                return true;
            if (!isdone)
                Enter(owner);
            if (!Check(owner))
            {
                Leave(owner);
                return true;
            }
            OnUpdate(owner);
            return false;
        }

        public virtual void OnUpdate(CameraControll owner) { }
        public virtual bool Enter(CameraControll owner) { stop = false; isdone = true; return true; }
        public virtual void Leave(CameraControll owner) { stop = true; isdone = false; }
        public virtual bool Check(CameraControll owner) { return true; }
    }

    /// <summary>
    /// ÉãÏñ»úÕð¶¯
    /// </summary>
    public class CameraShockEffect : CameraImageEffect
    {
        private Vector3 originalPos;
        private Vector3 Shockpos = Vector3.zero;
        public Transform CameraShockTF;
        public CinemachineBrain brain;
        private Transform cameraTF;
        public float shockFrequency=0.1f;
        private bool addPosInThisFrame;
        private float timer;

        public CameraShockEffect(CameraControll owner) : base(owner)
        {
            state = ImageEffect.Shock;
            cameraTF = owner.cameraGOtf;
        }

        public override bool Enter(CameraControll owner)
        {
            if (CameraShockTF == null)
            {
                stop = false;
                isdone = false;
                return true;
            }
            cameraTF = owner.cameraGOtf;
            Shockpos = CameraShockTF.localPosition;
            if (brain == null)
                brain = owner.GetComponent<CinemachineBrain>();
            if (brain != null && brain.enabled==true)
            {
                if (brain.ActiveVirtualCamera != null && brain.ActiveVirtualCamera.VirtualCameraGameObject.activeSelf)
                    cameraTF = brain.ActiveVirtualCamera.VirtualCameraGameObject.transform;
            }
            originalPos = cameraTF.parent.position;
            return base.Enter(owner);
        }

        public override void OnUpdate(CameraControll owner)
        {
            timer += Time.deltaTime;
            if (timer < shockFrequency)
                return;
            timer = 0;
            
            if (addPosInThisFrame)
                cameraTF.parent.position += Shockpos;
            else
                cameraTF.parent.position -= Shockpos;
            addPosInThisFrame = !addPosInThisFrame;
        }

        public override bool Check(CameraControll owner)
        {
            return CameraShockTF;
        }

        public override void Leave(CameraControll owner)
        {
            base.Leave(owner);
            Debug.Log("leave");
            Debug.Log(originalPos);
            cameraTF.parent.position = originalPos;
        }
    }

    /// <summary>
    /// ËÀÍö»ÒÆÁ
    /// </summary>
    public class CameraGrayEffect : CameraImageEffect
    {
        private GrayscaleEffect Grayscale;

        public CameraGrayEffect(CameraControll owner)
            : base(owner)
        {
            state = ImageEffect.Gray;
        }
        public override bool Enter(CameraControll owner)
        {
            base.Enter(owner);
            if (!Grayscale)
                Grayscale =owner.gameObject.AddComponent<GrayscaleEffect>();
            Grayscale.enabled = true;
            Shader shader = Shader.Find("MOYU/Grayscale");
            Grayscale.shader = shader;
            isdone = true;
            return true;
        }

        public override void Leave(CameraControll owner)
        {
            base.Leave(owner);
            if (!Grayscale)
                return;
            Grayscale.enabled = false;
        }
    }

    public class GrayscaleEffect : ImageEffectBase
    {
        public Texture textureRamp;
        public float rampOffset;

        // Called by camera to apply image effect
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetTexture("_RampTex", textureRamp);
            material.SetFloat("_RampOffset", rampOffset);
            Graphics.Blit(source, destination, material);
        }
    }

    /// <summary>
    /// Ö±ÏßÄ£ºý
    /// </summary>
    public class CameraRadialBlur : CameraImageEffect
    {
        private RadialBlur rb;
        public float value = 0.125f;

        public CameraRadialBlur(CameraControll owner) : base(owner)
        {
            state = ImageEffect.RadialBlur;
        }

        public override bool Enter(CameraControll owner)
        {
            base.Enter(owner);
            if (rb && rb.enabled)
                return true;
            if (!rb)
                rb = owner.gameObject.AddComponent<RadialBlur>();
            rb.enabled = true;
            rb.Intensity = value;
            isdone = true;
            return true;
        }


        public override void Leave(CameraControll owner)
        {
            base.Leave(owner);
            if (!rb)
                return;
            rb.enabled = false;
        }

    }

    /// <summary>
    /// ±³¾°Ä£ºý
    /// </summary>
    public class CameraBlurEffect : CameraImageEffect
    {
        public BlurEffect blur;
        private GameObject BlurCa;
        private Camera BlurCamera;
        public CameraBlurEffect(CameraControll owner)
            : base(owner)
        {
            state = ImageEffect.Blur;
        }
        public override bool Enter(CameraControll owner)
        {
            if (!Check(owner))
                return false;
            base.Enter(owner);
            if (!blur)
                blur = owner.gameObject.AddComponent<BlurEffect>();
            if (!BlurCa)
            {
                BlurCa = owner.AddChild(owner.camera.gameObject);
                BlurCa.name = "BlurCa";
                BlurCamera = BlurCa.AddComponent<Camera>();
                BlurCamera.nearClipPlane = 0.1f;
                BlurCamera.farClipPlane = 100;
                BlurCamera.fieldOfView = Camera.main.fieldOfView;
                BlurCamera.depth = owner.camera.depth + 1;
                BlurCamera.useOcclusionCulling = false;
                BlurCamera.allowHDR = false;
                BlurCamera.clearFlags = CameraClearFlags.Depth;
                BlurCamera.renderingPath = owner.camera.renderingPath;
            }
            BlurCamera.cullingMask =  XYDefines.Layer.MainPlayer;
            owner.camera.cullingMask ^= XYDefines.Layer.MainPlayer; ;
            BlurCa.SetActive(true);
            blur.enabled = true;
            isdone = true;
            return true;
        }

        public override void Leave(CameraControll owner)
        {
            base.Leave(owner);
            if (blur)
                blur.enabled = false;
            if (BlurCa)
                BlurCa.SetActive(false);
            owner.camera.cullingMask |= XYDefines.Layer.MainPlayer; ;
        }
    }
    

    /// <summary>
    /// Ñ¹ÆÁ
    /// </summary>
    public class ScreenBlackEffect : CameraImageEffect
    {
        private GameObject BlackCa;
        public float duration;
        public ScreenBlackEffect(CameraControll owner) : base(owner)
        {
            state = ImageEffect.ScreenBlack;
        }
        public override bool Enter(CameraControll owner)
        {
            if (!Check(owner))
                return false;
            base.Enter(owner);
            if (!BlackCa)
            {
                BlackCa = owner.AddChild(owner.camera.gameObject);
                BlackCa.name = "BlurCa";
                Camera ca = BlackCa.AddComponent<Camera>();
                ca.nearClipPlane = 0.1f;
                ca.farClipPlane = 100;
                ca.fieldOfView = Camera.main.fieldOfView;
                ca.depth = owner.camera.depth + 1;
                ca.useOcclusionCulling = false;
                ca.allowHDR = false;
               // ca.cullingMask = CDefines.Layer.Mask.MainPlayer;
                ca.clearFlags = CameraClearFlags.Depth;
                ca.renderingPath = owner.camera.renderingPath;
            }
            //owner.camera.cullingMask ^= CDefines.Layer.Mask.MainPlayer;
            BlackCa.SetActive(true);
            isdone = true;
            return true;
        }

        public override void Leave(CameraControll owner)
        {
            base.Leave(owner);
            //if (BlackCa)
            //    CClientCommon.DestroyImmediate(BlackCa);
            BlackCa = null;
            //owner.camera.cullingMask |= CDefines.Layer.Mask.MainPlayer;
            duration = 0;
        }
    }

    public GameObject AddChild(GameObject parent)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }
    #endregion

}

public enum CameraControlType
{
    FollowMp,
    Manual,
    CineMachine
}

public enum ImageEffect
{
    Shock = 0,
    Gray,
    RadialBlur,
    Blur,
    ScreenBlack,
    MAX,
}



