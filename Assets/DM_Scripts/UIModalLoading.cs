using System;
using UnityEngine;

public class UIModalLoading : MonoBehaviour
{
	public enum FadeState
	{
		None,
		FadeIn,
		FadeOut
	}

	private readonly float FadeInTimes = 0.4f;

	private readonly float FadeOutTimes = 0.4f;

	public FadeState CurFadeState;

	private float fadeDeltaTime;

	private Action OnCompleteFadeIn;

	private Action OnCompleteFadeOut;

	public CanvasGroup PanelObject;

	private void Update()
	{
		if (CurFadeState == FadeState.FadeIn)
		{
			fadeDeltaTime += Time.deltaTime;
			if (fadeDeltaTime > FadeInTimes)
			{
				fadeDeltaTime = FadeInTimes;
			}
			PanelObject.alpha = Mathf.Lerp(0f, 1f, fadeDeltaTime / FadeInTimes);
			if (PanelObject.alpha == 1f)
			{
				CurFadeState = FadeState.None;
				if (OnCompleteFadeIn != null)
				{
					OnCompleteFadeIn();
				}
			}
		}
		else
		{
			if (CurFadeState != FadeState.FadeOut)
			{
				return;
			}
			fadeDeltaTime += Time.deltaTime;
			if (fadeDeltaTime > FadeOutTimes)
			{
				fadeDeltaTime = FadeOutTimes;
			}
			PanelObject.alpha = 1f - Mathf.Lerp(0f, 1f, fadeDeltaTime / FadeOutTimes);
			if (PanelObject.alpha == 0f)
			{
				base.gameObject.SetActive(value: false);
				CurFadeState = FadeState.None;
				if (OnCompleteFadeOut != null)
				{
					OnCompleteFadeOut();
				}
			}
		}
	}

	public void FadeIn(Action onEventFadeInEnd = null)
	{
		fadeDeltaTime = 0f;
		if ((bool)PanelObject)
		{
			PanelObject.alpha = 0f;
		}
		CurFadeState = FadeState.FadeIn;
		OnCompleteFadeIn = onEventFadeInEnd;
		base.gameObject.SetActive(value: true);
	}

	public void FadeOut(Action onEventFadeOutEnd = null)
	{
		fadeDeltaTime = 0f;
		PanelObject.alpha = 1f;
		CurFadeState = FadeState.FadeOut;
		OnCompleteFadeOut = onEventFadeOutEnd;
	}
}
