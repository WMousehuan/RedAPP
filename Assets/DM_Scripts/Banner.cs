using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Banner : MonoBehaviour
{
	public RectTransform BGRectTransform;

	private float fromY;

	private bool isShowing;

	public GameObject ObjectMainView;

	private Vector2 resizePosMax = Vector2.zero;

	private Vector2 resizePosMin = Vector2.zero;

	private readonly bool stopShowDelay;

	public float tweenValueInTime = 0.4f;

	public float tweenValueOutTime = 0.45f;

	public float tweenValueShowingTime = 2f;

	public void Start()
	{
		fromY = 55f;
		GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		ObjectMainView.transform.localPosition = new Vector3(0f, fromY, 0f);
		StartCoroutine(ShowTween());
	}

	private IEnumerator ShowTween()
	{
		ObjectMainView.transform.DOLocalMoveY(0f - fromY, tweenValueInTime).SetEase(Ease.OutCubic);
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
		CloseBanner();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void CloseBanner()
	{
		MonoSingleton<BannerManager>.Instance.Close();
	}

	private void Update()
	{
		resizePosMax = BGRectTransform.offsetMax;
		resizePosMin = BGRectTransform.offsetMin;
		if (Screen.width > Screen.height)
		{
			resizePosMax.x = -300f;
			resizePosMin.x = 300f;
		}
		else
		{
			resizePosMax.x = 0f;
			resizePosMin.x = 0f;
		}
		BGRectTransform.offsetMax = resizePosMax;
		BGRectTransform.offsetMin = resizePosMin;
	}
}
