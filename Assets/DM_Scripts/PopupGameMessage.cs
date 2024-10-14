
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameMessage : Popup
{
	public RectTransform BGRectTransform;

	private Coroutine CoroutineShowTween;

	private float fromY;

	private bool isShowing;

	public GameObject ObjectMainView;

	private Vector2 resizePosMax = Vector2.zero;

	private Vector2 resizePosMin = Vector2.zero;

	private bool stopShowDelay;

	public Text TextMessage;

	private bool tweenIsOver;

	public float tweenValueInTime = 2f;

	public float tweenValueOutTime = 1.4f;

	public float tweenValueShowingTime = 2f;

	public override void Start()
	{
		base.Start();
		Application.targetFrameRate = GlobalSetting.FPS;
		fromY = -300 - Screen.height / 2;
		ObjectMainView.transform.localPosition = new Vector3(0f, fromY, 0f);
		CoroutineShowTween = StartCoroutine(ShowTween());
	}

	private IEnumerator ShowTween()
	{
		if (m_PopupType == PopupType.PopupGameMessageStart)
		{
			yield return new WaitForSeconds(UISceneLoading.FadeOutDefaultTimes);
		}
		ObjectMainView.transform.DOLocalMoveY(0f, tweenValueInTime).SetEase(Ease.OutCubic);
		SoundSFX.Play(SFXIndex.SlidePopupShow);
		yield return new WaitForSeconds(tweenValueInTime);
		isShowing = true;
		yield return new WaitForSeconds(tweenValueShowingTime);
		isShowing = false;
		if (!stopShowDelay)
		{
			StartCoroutine(HideTween());
		}
	}

	private IEnumerator HideTween()
	{
		ObjectMainView.transform.DOLocalMoveY(fromY, tweenValueOutTime).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(tweenValueOutTime * 0.25f);
		SoundSFX.Play(SFXIndex.SlidePopupHide);
		yield return new WaitForSeconds(tweenValueOutTime * 0.75f);
		tweenIsOver = true;
		if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType != SceneType.Game)
		{
			eventClose = null;
		}
		OnEventClose();
	}

	public override void OnEventClose()
	{
		if (isShowing && !stopShowDelay)
		{
			stopShowDelay = true;
			StartCoroutine(HideTween());
			return;
		}
		if (tweenIsOver)
		{
			base.OnEventClose();
		}
		Application.targetFrameRate = GlobalSetting.LOW_FPS;
	}

	private void Update()
	{
		resizePosMin = BGRectTransform.offsetMin;
		resizePosMax = BGRectTransform.offsetMax;
		if (Screen.width > Screen.height)
		{
			resizePosMin.x = 128f;
			resizePosMax.x = -128f;
		}
		else
		{
			resizePosMin.x = -100f;
			resizePosMax.x = 100f;
		}
		BGRectTransform.offsetMin = resizePosMin;
		BGRectTransform.offsetMax = resizePosMax;
	}

	public override void SoundPlayShow()
	{
	}

	public override void SoundPlayHide()
	{
	}
}
