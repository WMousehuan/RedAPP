
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class LiteDailyBonusTable
{
	public int coinValueMin;

	public int coinValueMax;

	public int boosterHammerValueMin;

	public int boosterHammerValueMax;

	public int boosterCandyPackValueMin;

	public int boosterCandyPackValueMax;

	public int priorityCoin;

	public int prioriyBoosterHammer;

	public int priorityBoosterCandyPack;

	public void SetTableFromOptionTable()
	{
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusCoinMin", ref coinValueMin);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusCoinMax", ref coinValueMax);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusHammerMin", ref boosterHammerValueMin);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusHammerMax", ref boosterHammerValueMax);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusCandyPackMin", ref boosterCandyPackValueMin);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusCandyPackMax", ref boosterCandyPackValueMax);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusPriorityCoin", ref priorityCoin);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusPriorityHammer", ref prioriyBoosterHammer);
		MonoSingleton<ServerDataTable>.Instance.SetOptionValue("DailyBonusPriorityCandyPack", ref priorityBoosterCandyPack);
	}

	public RewardItemData GetRandRewardItemData()
	{
		RewardItemData rewardItemData = new RewardItemData(ServerItemIndex.None, 0);
		int max = priorityCoin + prioriyBoosterHammer + priorityBoosterCandyPack;
		int num = Random.Range(0, max);
		if (num < priorityCoin)
		{
			rewardItemData.item_index = ServerItemIndex.Coin;
			rewardItemData.value = Random.Range(coinValueMin, coinValueMax + 1);
		}
		else if (num < priorityCoin + prioriyBoosterHammer)
		{
			rewardItemData.item_index = ServerItemIndex.BoosterHammer;
			rewardItemData.value = Random.Range(boosterHammerValueMin, boosterHammerValueMax + 1);
		}
		else
		{
			rewardItemData.item_index = ServerItemIndex.BoosterCandyPack;
			rewardItemData.value = Random.Range(boosterCandyPackValueMin, boosterCandyPackValueMax + 1);
		}
		return rewardItemData;
	}
}
