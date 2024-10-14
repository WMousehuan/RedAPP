/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameQuit : Popup
{
	public RectTransform BGRectTransform;

	private bool decreaseLife;

	public GameObject ObjectMainView;

	public GameObject ObjQuitButton;

	public GameObject ObjRetryButton;

	public GameObject ObjRetryButtonResume;

	public GameObject ObjTextPos;

	private Vector2 resizePosMax = Vector2.zero;

	private Vector2 resizePosMin = Vector2.zero;

	public Text TextTitle;

	public float tweenValueInTime = 2f;

	public Image ImageBackBlocking;

	public override void Start()
	{
		ImageBackBlocking.gameObject.SetActive(value: true);
		ImageBackBlocking.color = new Color(0f, 0f, 0f, 0f);
		ImageBackBlocking.DOFade(0.7f, 0.5f).SetDelay(tweenValueInTime / 2f);
		base.Start();
		//TextTitle.font = MonoSingleton<UIManager>.Instance.GetLocalizeFont();
		SetQuitButton(decreaseLife);
		TextTitle.text = "Do you really want to quit?";
		float y = -150 - Screen.height / 2;
		ObjectMainView.transform.localPosition = new Vector3(0f, y, 0f);
		ObjectMainView.transform.DOLocalMoveY(0f, tweenValueInTime).SetEase(Ease.OutCubic);
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

	private void SetQuitButton(bool decreaseLife)
	{
		ObjTextPos.transform.localPosition = new Vector3(0f, -47f, 0f);
		ObjRetryButtonResume.SetActive(value: false);
		ObjRetryButton.SetActive(value: true);
		if (decreaseLife)
		{
			ObjTextPos.transform.localPosition = new Vector3(47f, -47f, 0f);
			ObjQuitButton.transform.localPosition = new Vector3(-110f, -92f, 0f);
		}
	}

	public void OnPressButtonRetry()
	{
		BoardManager.main.RemoveTutorialOutlineEffect();
		SoundSFX.Play(SFXIndex.ButtonClick);
		GameMain.main.OnEventGameRetry();
	}
}
