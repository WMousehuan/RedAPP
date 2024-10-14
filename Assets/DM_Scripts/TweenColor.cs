using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	public Color from = Color.white;

	public Color to = Color.white;

	private bool mCached;

	private Light mLight;

	private CanvasRenderer mCr;

	[Obsolete("Use 'value' instead")]
	public Color color
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public Color value
	{
		get
		{
			if (!mCached)
			{
				Cache();
			}
			if (mCr != null)
			{
				return mCr.GetColor();
			}
			return Color.white;
		}
		set
		{
			if (!mCached)
			{
				Cache();
			}
			if (mCr != null)
			{
				mCr.SetColor(value);
			}
			else if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	private void Cache()
	{
		mCached = true;
		mCr = GetComponent<CanvasRenderer>();
		if (!(mCr != null))
		{
			mLight = GetComponent<Light>();
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Color.Lerp(from, to, factor);
	}

	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, isFinished: true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		from = value;
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		to = value;
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		value = from;
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		value = to;
	}
}
