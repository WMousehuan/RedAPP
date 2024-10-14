using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Canvas Group Alpha")]
public class TweenCanvasGroupAlpha : UITweener
{
	[Range(0f, 1f)]
	public float from = 1f;

	[Range(0f, 1f)]
	public float to = 1f;

	private bool mCached;

	private CanvasGroup mCG;

	[Obsolete("Use 'value' instead")]
	public float alpha
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

	public float value
	{
		get
		{
			if (!mCached)
			{
				Cache();
			}
			if (mCG != null)
			{
				return mCG.alpha;
			}
			return 0f;
		}
		set
		{
			if (!mCached)
			{
				Cache();
			}
			if (mCG != null)
			{
				mCG.alpha = value;
			}
		}
	}

	private void Cache()
	{
		mCached = true;
		mCG = GetComponent<CanvasGroup>();
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Mathf.Lerp(from, to, factor);
	}

	public static TweenCanvasGroupAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenCanvasGroupAlpha tweenCanvasGroupAlpha = UITweener.Begin<TweenCanvasGroupAlpha>(go, duration);
		tweenCanvasGroupAlpha.from = tweenCanvasGroupAlpha.value;
		tweenCanvasGroupAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenCanvasGroupAlpha.Sample(1f, isFinished: true);
			tweenCanvasGroupAlpha.enabled = false;
		}
		return tweenCanvasGroupAlpha;
	}

	public override void SetStartToCurrentValue()
	{
		from = value;
	}

	public override void SetEndToCurrentValue()
	{
		to = value;
	}
}
