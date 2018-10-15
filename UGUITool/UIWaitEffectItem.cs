using UnityEngine;
using System.Collections;

public class UIWaitEffectItem : MonoBehaviour 
{
    public CImage Sprite_;
    public float targetAlpha;
    public UIWaitEffectItem Front;
    public float duration = 0.2f;

    void OnEnable()
    {
        move();
    }

    public void move()
    {
        targetAlpha = Front.Sprite_.color.a;
        Invoke("AlphaFinished", duration);
    }

    void SetTrans(UIWaitEffectItem traget)
    {
        Color co = traget.Sprite_.color;
        co.a = traget.targetAlpha;
        traget.Sprite_.color = co;
    }

    void AlphaFinished()
    {
        CancelInvoke("AlphaFinished");
        SetTrans(this);
        move(); 
    }
}
