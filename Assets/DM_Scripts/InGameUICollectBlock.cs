/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameUICollectBlock : MonoBehaviour
{
	protected CollectBlockType collectType;

	public Image ImageTarget;

	public GameObject ObjCheck;

	private int targetCount;

	public Text TextTargetCount;

	public virtual void SetData(CollectBlockType _collectType, int _targetCount, Sprite _targetSprite)
	{
		ObjCheck.SetActive(value: false);
		targetCount = _targetCount;
		collectType = _collectType;
		ImageTarget.rectTransform.sizeDelta = new Vector2(_targetSprite.rect.width, _targetSprite.rect.height);
		ImageTarget.sprite = _targetSprite;
		UpdateTargetCount(targetCount);
	}

	public virtual void UpdateTargetCount(int _targetCount, int param1 = 0)
	{
		if (collectType == CollectBlockType.SweetRoadConnect)
		{
			TextTargetCount.gameObject.SetActive(value: false);
			if (GameMain.main.isConnectedOnlySweetRoad && (bool)ObjCheck)
			{
				ObjCheck.SetActive(value: true);
			}
			return;
		}
		if (_targetCount < 0)
		{
			_targetCount = 0;
		}
		if ((bool)TextTargetCount)
		{
			if (MonoSingleton<SceneControlManager>.Instance.OldSceneType != SceneType.MapTool)
			{
				TextTargetCount.text = ((collectType != CollectBlockType.RescueFriend) ? _targetCount.ToString() : BoardManager.main.remainRescueFriendInBoard.ToString());
			}
			else
			{
				TextTargetCount.text = _targetCount.ToString();
			}
			Sequence sequence = DOTween.Sequence();
			sequence.Append(TextTargetCount.transform.DOScale(2f, 0.2f));
			sequence.Append(TextTargetCount.transform.DOScale(1f, 0.2f));
			sequence.Play();
			if (_targetCount == 0 && (bool)ObjCheck)
			{
				ObjCheck.SetActive(value: true);
			}
		}
	}

	public virtual void Reset()
	{
		ObjCheck.SetActive(value: false);
		if (MonoSingleton<SceneControlManager>.Instance.OldSceneType != SceneType.MapTool)
		{
			TextTargetCount.text = ((collectType != CollectBlockType.RescueFriend) ? targetCount.ToString() : BoardManager.main.remainRescueFriendInBoard.ToString());
		}
		else
		{
			TextTargetCount.text = targetCount.ToString();
		}
	}
}
