/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using PathologicalGames;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIScoreTarget : MonoBehaviour
{
	public Image ImageRewardItemTarget;

	private List<int> itemIndex = new List<int>();

	private int reachedTargetPointIndex;

	private int[] scoreTargetPoint;

	public Text TextTargetScore;

	public void SetData(int[] starPoint, List<int> bonusRewardItemIndex)
	{
		itemIndex = bonusRewardItemIndex;
		ImageRewardItemTarget.sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite((ServerItemIndex)bonusRewardItemIndex[0]);
		ImageRewardItemTarget.gameObject.SetActive(value: true);
		scoreTargetPoint = starPoint;
		TextTargetScore.text = Utils.GetCurrencyNumberString(scoreTargetPoint[0]);
	}

	public void UpdateScore(int score)
	{
		if (scoreTargetPoint != null && reachedTargetPointIndex < scoreTargetPoint.Length && score >= scoreTargetPoint[reachedTargetPointIndex] && reachedTargetPointIndex < itemIndex.Count - 1)
		{
			reachedTargetPointIndex++;
			ChangeTarget(reachedTargetPointIndex);
		}
	}

	private void ChangeTarget(int targetPointIndex)
	{
		ImageRewardItemTarget.sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite((ServerItemIndex)itemIndex[targetPointIndex]);
		TextTargetScore.text = Utils.GetCurrencyNumberString(scoreTargetPoint[targetPointIndex]);
		if (base.gameObject.activeInHierarchy)
		{
			GameObject spawnEffectObject = SpawnStringEffect.GetSpawnEffectObject(SpawnStringEffectType.CollectBonusReward);
			if ((bool)spawnEffectObject)
			{
				spawnEffectObject.transform.parent = ImageRewardItemTarget.transform;
				spawnEffectObject.transform.localPosition = new Vector3(0f, 0f, -10f);
				PoolManager.PoolGameEffect.Despawn(spawnEffectObject.transform, 0.9f);
			}
		}
	}
}
