using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISceneLoading : MonoBehaviour
{
	public enum FadeState
	{
		None,
		FadeIn,
		FadeOut
	}

	public static readonly float FadeOutDefaultTimes = 1.0f;

	private readonly float FadeInDefaultTimes = 0.4f;

	private readonly float FadeInMarioTimes = 0.6f;

	private readonly float FadeInMarioTimesAfter = 0.24f;

	private readonly float FadeInMarioTimesPausing = 0.4f;

	private readonly float FadeOutMarioTimes = 0.6f;

	public FadeState CurFadeState;

	public Camera currentCamera;

	private SceneChangeEffect currentChangeEffect;

	private CanvasGroup currentPanel;

	private float fadeDeltaTime;

	private float fadeInTime;

	private float fadeOutTime;

	public Image ImageColorFade;

	private Action OnCompleteFadeIn;

	private Action OnCompleteFadeOut;

	public Camera OverlayCameraBlank;

	public CanvasGroup[] PanelsGroupChangeEffect;

	private void Awake()
	{
		//for (int i = 0; i < PanelsGroupChangeEffect.Length; i++)
		//{
		//	PanelsGroupChangeEffect[i].gameObject.SetActive(value: false);
		//}
	}

	private void Update()
	{
		if (CurFadeState == FadeState.FadeIn)
		{
			fadeDeltaTime += Time.deltaTime;
			if (fadeDeltaTime > fadeInTime)
			{
				fadeDeltaTime = fadeInTime;
			}
			currentPanel.alpha = Mathf.Lerp(0f, 1f, fadeDeltaTime / fadeInTime);
			if (currentPanel.alpha >= 0.95f)
			{
				CurFadeState = FadeState.None;
                OnCompleteFadeIn?.Invoke();
            }
		}
		else
		{
			if (CurFadeState != FadeState.FadeOut)
			{
				return;
			}
			fadeDeltaTime += Time.deltaTime;
			if (fadeDeltaTime > fadeOutTime)
			{
				fadeDeltaTime = fadeOutTime;
			}
			currentPanel.alpha = 1f - Mathf.Lerp(0f, 1f, fadeDeltaTime / fadeOutTime);
			if (currentPanel.alpha <= 0.05f)
			{
				base.gameObject.SetActive(value: false);
				CurFadeState = FadeState.None;
                OnCompleteFadeOut?.Invoke();
            }
        }
	}

	public IEnumerator FadeIn(SceneType newSceneType, SceneChangeEffect effect, Action onEventFadeInEnd = null, Color fadeColor = default(Color))
	{
		//Debug.Log("CHAY VAO DAY");
		if (!(this == null))
		{
			currentChangeEffect = effect;
			currentPanel = PanelsGroupChangeEffect[(int)effect];
			for (int i = 0; i < PanelsGroupChangeEffect.Length; i++)
			{
				PanelsGroupChangeEffect[i].gameObject.SetActive(value: false);
			}
			currentPanel.gameObject.SetActive(value: true);
			fadeInTime = FadeInDefaultTimes;
			fadeOutTime = FadeOutDefaultTimes;
			base.gameObject.SetActive(value: true);
			ImageColorFade.color = fadeColor;
			fadeDeltaTime = 0f;
			currentPanel.alpha = 0f;
			CurFadeState = FadeState.FadeIn;
			OnCompleteFadeIn = onEventFadeInEnd;
		}
		yield break;
	}

	public void FadeOut(Action onEventFadeOutEnd = null)
	{
		fadeDeltaTime = 0f;
		if ((bool)currentPanel)
		{
			currentPanel.alpha = 1f;
		}
		CurFadeState = FadeState.FadeOut;
		OnCompleteFadeOut = onEventFadeOutEnd;
		if (currentPanel == null)
		{
			CurFadeState = FadeState.None;
		}
	}
}
