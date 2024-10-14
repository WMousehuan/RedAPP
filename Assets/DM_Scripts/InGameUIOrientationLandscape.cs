/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class InGameUIOrientationLandscape : InGameUIOrientationVariable
{
	public Transform AnchorBoardPosition;

	public RectTransform TweenValueMainInfo;

	private float tweenValueMainInfoStartX;

	public override void Start()
	{
		base.Start();
		orientationType = OrientationType.Landscape;
	}

	public override void InitUITween()
	{
		base.InitUITween();
		if ((bool)TweenValueMainInfo)
		{
			Vector2 anchoredPosition = TweenValueMainInfo.anchoredPosition;
			tweenValueMainInfoStartX = anchoredPosition.x;
			RectTransform tweenValueMainInfo = TweenValueMainInfo;
			Vector2 anchoredPosition2 = TweenValueMainInfo.anchoredPosition;
			tweenValueMainInfo.anchoredPosition = new Vector2(-400f, anchoredPosition2.y);
		}
	}

	public override void ShowUITween()
	{
		base.ShowUITween();
		if ((bool)TweenValueMainInfo)
		{
			TweenValueMainInfo.DOAnchorPosX(tweenValueMainInfoStartX, 0.6f, snapping: true).SetEase(Ease.OutBack).SetDelay(0.8f);
		}
	}

	public Vector3 GetBoardPosition()
	{
		if (Camera.main == null || GameMain.main.UIGameCamera == null || AnchorBoardPosition == null)
		{
			return Vector3.zero;
		}
		return Camera.main.ViewportToWorldPoint(GameMain.main.UIGameCamera.WorldToViewportPoint(AnchorBoardPosition.transform.position));
	}
}
