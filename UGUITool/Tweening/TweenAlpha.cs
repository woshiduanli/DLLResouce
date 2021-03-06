using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("UGUITween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;

	bool mCached = false;
	Material mMat;
	MaskableGraphic mImage;
    CanvasGroup mGroup;
	void Cache ()
	{
		mCached = true;
        mGroup = GetComponent<CanvasGroup>();
        if (!mGroup)
        {
            mImage = GetComponent<MaskableGraphic>();

            if (!mImage)
            {
                Renderer ren = GetComponent<Renderer>();
                if (ren != null)
                    mMat = ren.material;
            }
        }

	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
	{
		get
		{
			if (!mCached) 
                Cache();
            if (mGroup)
                return mGroup.alpha;
			if (mImage != null) 
                return mImage.color.a;
			return mMat != null ? mMat.color.a : 1f;
		}
		set
		{
			if (!mCached) 
                Cache();
            if (mGroup)
                mGroup.alpha = value;
			else if (mImage != null)
			{
				Color c = mImage.color;
				c.a = value;
                mImage.color = c;
			}
			else if (mMat != null)
			{
				Color c = mMat.color;
				c.a = value;
				mMat.color = c;
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
