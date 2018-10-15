using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UGUITween/Tween Height")]
public class TweenHeight : UITweener
{
	public float from = 100;
	public float to = 100;

    RectTransform mTrans;

	public RectTransform cachedTrans { get { if (mTrans == null) mTrans = GetComponent<RectTransform>(); return mTrans; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
    {
        get
        {
            return cachedTrans.sizeDelta.y;
        }
        set
        {
            Vector2 size = cachedTrans.sizeDelta;
            size.y = value;
            cachedTrans.sizeDelta = size;
        }
    }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = Mathf.RoundToInt(from * (1f - factor) + to * factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenHeight Begin (RectTransform tf, float duration, int height)
	{
		TweenHeight comp = UITweener.Begin<TweenHeight>(tf.gameObject, duration);
		comp.from = tf.sizeDelta.y;
		comp.to = height;

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
