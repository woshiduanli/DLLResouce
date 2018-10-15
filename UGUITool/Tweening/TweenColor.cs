using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UGUITween/Tween Color")]
public class TweenColor : UITweener
{
	public Color from = Color.white;
	public Color to = Color.white;

	bool mCached = false;
    MaskableGraphic mImage;
	Material mMat;
	Light mLight;
	SpriteRenderer mSr;

	void Cache ()
	{
		mCached = true;
        mImage = GetComponent<MaskableGraphic>();
		if (mImage != null) return;

		mSr = GetComponent<SpriteRenderer>();
		if (mSr != null) return;

        Renderer ren = GetComponent<Renderer>();

        if (ren != null)
		{
			mMat = ren.material;
			return;
        }


		mLight = GetComponent<Light>();
		if (mLight == null) mImage = GetComponentInChildren<CImage>();
	}

	[System.Obsolete("Use 'value' instead")]
	public Color color { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Color value
	{
		get
		{
			if (!mCached) Cache();
			if (mImage != null) return mImage.color;
			if (mMat != null) return mMat.color;
			if (mSr != null) return mSr.color;
			if (mLight != null) return mLight.color;
			return Color.black;
		}
		set
		{
			if (!mCached) Cache();
			if (mImage != null) mImage.color = value;
			else if (mMat != null) mMat.color = value;
			else if (mSr != null) mSr.color = value;
			else if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenColor Begin (GameObject go, float duration, Color color)
	{
		TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
		comp.from = comp.value;
		comp.to = color;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}