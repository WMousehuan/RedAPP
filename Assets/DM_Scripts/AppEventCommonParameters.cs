/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */

using AppEventCommonParam;

#if ENABLE_ANTI_CHEAT
using CodeStage.AntiCheat.ObscuredTypes;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

public class AppEventCommonParameters
{
	public static string adNetworkName = string.Empty;

	public static string adCampaignName = string.Empty;

	public static void SetCommonParamList(Dictionary<string, object> dicParam, ParamType[] paramList, ParamTypeIncGID[] paramGidList, int gid)
	{
		if (paramList != null)
		{
			for (int i = 0; i < paramList.Length; i++)
			{
				SetParam(dicParam, paramList[i]);
			}
		}
		if (paramGidList != null)
		{
			for (int j = 0; j < paramGidList.Length; j++)
			{
				SetParamIncGID(dicParam, paramGidList[j], gid);
			}
		}
	}

	public static void SetParamIncGID(Dictionary<string, object> dicParam, ParamTypeIncGID paramType, int gid)
	{
		int num = (ServerDataTable.GetLimitLevel() != MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo) ? MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo : (MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo + 1);
		string key = paramType.ToString();
		switch (paramType)
		{
		case ParamTypeIncGID.STAGE_NUMBER:
			break;
		case ParamTypeIncGID.STAGE_NUMBER_LAST_PLAYED:
			break;
		case ParamTypeIncGID.LEVEL_NUMBER:
			dicParam[key] = gid;
			break;
		case ParamTypeIncGID.LEVEL_NUMBER_LAST_PLAYED:
			dicParam[key] = gid;
			break;
		case ParamTypeIncGID.LEVEL_GAME_MODE:
			dicParam[key] = MapData.GetGoalTarget(gid).ToString();
			break;
		case ParamTypeIncGID.LEVEL_PLAY_TYPE:
			dicParam[key] = MonoSingleton<AppEventManager>.Instance.GetLevelPlayType(gid);
			break;
		case ParamTypeIncGID.IS_FIRST_CLEAR:
			dicParam[key] = ((num != gid) ? "NO" : "YES");
			break;
		case ParamTypeIncGID.IS_THIS_LEVEL_EVER_CLEARED:
			dicParam[key] = ((gid == -1) ? "N/A" : ((num <= gid) ? "NO" : "YES"));
			break;
		}
	}

	public static void SetParam(Dictionary<string, object> dicParam, ParamType paramType)
	{
		string key = paramType.ToString();
		int num = 0;
		int num2 = (ServerDataTable.GetLimitLevel() != MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo) ? MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo : (MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo + 1);
		switch (paramType)
		{
		case ParamType.PLAYER_UID:
			break;
		case ParamType.PLAYER_LOGGED_IN_AS:
			break;
		case ParamType.PLAYER_ITEM_REMAINS_CANDYCRANE:
			break;
		case ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE:
			break;
		case ParamType.BUFFS_APPLIED:
			break;
		case ParamType.AVERAGE_STARS_EARNED:
			break;
		case ParamType.TIME_STAMP_DAY:
			dicParam[key] = DateTime.Today.ToString("ddd").ToUpper();
			break;
		case ParamType.TIME_STAMP_TIME_IN_A_DAY:
			dicParam[key] = $"{DateTime.Now.TimeOfDay.Hours:00}:{DateTime.Now.TimeOfDay.Minutes / 30 * 30:00}";
			break;
		case ParamType.PLATFORM:
			dicParam[key] = GetPlatformName();
			break;
		case ParamType.PLAYER_LIFE_REMAINS:
			dicParam[key] = num;
			break;
		case ParamType.PLAYER_ITEM_REMAINS_MAGICHAMMER:
			dicParam[key] = (int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[0];
			break;
		case ParamType.PLAYER_ITEM_REMAINS_CANDYBOMB:
			dicParam[key] = (int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[1];
			break;
		case ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR:
			dicParam[key] = MonoSingleton<PlayerDataManager>.Instance.PayTotalDollar;
			break;
		case ParamType.PLAYER_TOTAL_PURCHASE_COUNT:
			dicParam[key] = MonoSingleton<PlayerDataManager>.Instance.PayCount;
			break;
		case ParamType.PLAYER_AVERAGE_PRICE_PER_PURCHASE:
			if (MonoSingleton<PlayerDataManager>.Instance.PayCount > 0)
			{
				dicParam[key] = (float)Math.Round(MonoSingleton<PlayerDataManager>.Instance.PayTotalDollar / (float)MonoSingleton<PlayerDataManager>.Instance.PayCount, 1, MidpointRounding.AwayFromZero);
			}
			else
			{
				dicParam[key] = 0;
			}
			break;
		case ParamType.PLAYER_DAYS_SINCE_INSTALL:
			dicParam[key] = GetDurationAfterAccountCreated();
			break;
		case ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE:
			dicParam[key] = ((MonoSingleton<PlayerDataManager>.Instance.PayCount != 0) ? GetDurationDays(MonoSingleton<PlayerDataManager>.Instance.LastPayUnixTimeStamp) : (-1));
			break;
		case ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN:
		{
			int num5 = 0;
			num5 = ((DateTime.Now < MonoSingleton<PlayerDataManager>.Instance.lastLoginDateTime) ? (-101) : ((!MonoSingleton<PlayerDataManager>.Instance.IsFirstAppInstall) ? GetDurationLocalDays(MonoSingleton<PlayerDataManager>.Instance.lastLoginDateTime) : (-1)));
			dicParam[key] = num5;
			break;
		}
		case ParamType.PLAYER_DATE_OF_INSTALL:
#if ENABLE_ANTI_CHEAT
			    dicParam[key] = Utils.UnixTimeStampToDateTime(ObscuredPrefs.GetInt("WDate", 0)).ToString("yyyyMMdd");
#else
                dicParam[key] = Utils.UnixTimeStampToDateTime(CPlayerPrefs.GetInt("WDate", 0)).ToString("yyyyMMdd");
#endif
                break;
		case ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE:
			dicParam[key] = num2;
			break;
		case ParamType.PLAYER_NUMBER_OF_GAME_FRIENDS:
			dicParam[key] = 0;
			break;
		case ParamType.DID_PLAYER_EVER_PAY:
			dicParam[key] = ((MonoSingleton<PlayerDataManager>.Instance.PayCount <= 0) ? "NO" : "YES");
			break;
		case ParamType.IS_FIRST_SESSION:
			dicParam[key] = ((!MonoSingleton<PlayerDataManager>.Instance.IsFirstSession) ? "NO" : "YES");
			break;
		case ParamType.SESSION_TIME_IN_SECONDS:
			dicParam[key] = (int)(Math.Ceiling((float)(Utils.ConvertToTimestamp(DateTime.UtcNow) - MonoSingleton<AppEventManager>.Instance.startTimeAfterLogin) / 10f) * 10.0);
			break;
		case ParamType.SESSION_TIME_IN_MINUTES:
			dicParam[key] = (int)(Math.Ceiling((float)(Utils.ConvertToTimestamp(DateTime.UtcNow) - MonoSingleton<AppEventManager>.Instance.startTimeAfterLogin) / 10f) * 10.0) / 60;
			break;
		case ParamType.SESSION_COUNT_PLAY:
			dicParam[key] = AppEventManager.m_TempBox.sessionGamePlayCount;
			break;
		case ParamType.SESSION_COUNT_CLEAR:
			dicParam[key] = AppEventManager.m_TempBox.sessionGameClearCount;
			break;
		case ParamType.SESSION_COUNT_FAIL:
			dicParam[key] = AppEventManager.m_TempBox.sessionGameFailCount;
			break;
		case ParamType.SESSION_SPENT_COIN:
			dicParam[key] = AppEventManager.m_TempBox.TotalSpentCoin;
			break;
		case ParamType.SESSION_SPENT_INGAME_ITEMS:
			dicParam[key] = AppEventManager.m_TempBox.TotalSpendInGameItemsCount;
			break;
		case ParamType.ORIENTATION:
			dicParam[key] = ((Screen.width <= Screen.height) ? "Portrait" : "Landscape");
			break;
		case ParamType.NEEDED_COINS_FOR_WHAT:
			dicParam[key] = GetCoinCategroyString(AppEventManager.m_TempBox.coinCategory);
			break;
		case ParamType.WHERE:
			dicParam[key] = GetWhereSceneType(MonoSingleton<SceneControlManager>.Instance.CurrentSceneType);
			break;
		case ParamType.OS_VERSION:
			dicParam[key] = SystemInfo.operatingSystem;
			break;
		case ParamType.DEVICE_MODEL:
			dicParam[key] = SystemInfo.deviceModel;
			break;
		case ParamType.SCREEN_RESOLUTION:
			dicParam[key] = Screen.width + "x" + Screen.height;
			break;
		case ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END:
		{
			string sessionEndDateTimeString2 = PlayerDataManager.GetSessionEndDateTimeString();
			if (sessionEndDateTimeString2 == string.Empty)
			{
				dicParam[key] = -1;
				break;
			}
			int num4 = (int)(DateTime.Now - Convert.ToDateTime(sessionEndDateTimeString2)).TotalHours;
			if (num4 < 0)
			{
				num4 = -100;
			}
			dicParam[key] = num4;
			break;
		}
		case ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END:
		{
			string sessionEndDateTimeString = PlayerDataManager.GetSessionEndDateTimeString();
			if (sessionEndDateTimeString == string.Empty)
			{
				dicParam[key] = -1;
				break;
			}
			int num3 = 0;
			DateTime dateTime = Convert.ToDateTime(sessionEndDateTimeString);
			num3 = ((!(DateTime.Now < dateTime)) ? GetDurationLocalDays(dateTime) : (-101));
			dicParam[key] = num3;
			break;
		}
		case ParamType.PREVIOUSLY_CLEARED_COUNT:
			dicParam[key] = AppEventManager.m_TempBox.prevGameClearCount;
			break;
		case ParamType.PREVIOUSLY_FAIL_COUNT:
			dicParam[key] = AppEventManager.m_TempBox.prevGameFailCount;
			break;
		//case ParamType.VERSION_MOBILE_CLIENT:
		//	dicParam[key] = GlobalSetting.ConfigData.AppVersion + "." + BuildNumber.GetJenkinsBuildVersion();
		//	break;
		case ParamType.IS_ORGANIC_PRESUMABLY:
			dicParam[key] = ((!string.IsNullOrEmpty(adNetworkName)) ? "NO" : "YES");
			break;
		case ParamType.AD_NETWORK:
			dicParam[key] = ((!string.IsNullOrEmpty(adNetworkName)) ? adNetworkName : "N/A");
			break;
		case ParamType.AD_CAMPAIGN_NAME:
			dicParam[key] = ((!string.IsNullOrEmpty(adCampaignName)) ? adCampaignName : "N/A");
			break;
		case ParamType.PLAYER_COIN_BALANCE_BEFORE_EVENT:
			dicParam[key] = MonoSingleton<PlayerDataManager>.Instance.Coin;
			break;
		case ParamType.PLAYER_WATER_REMAINS_BEFORE_EVENT:
		case ParamType.PLAYER_RESOURCE_REMAINS_WATER:
			dicParam[key] = 0;
			break;
		case ParamType.PLAYER_STARCANDY_REMAINS_BEFORE_EVNET:
		case ParamType.PLAYER_RESOURCE_REMAINS_STARCANDY:
			dicParam[key] = 0;
			break;
		case ParamType.DYNAMIC_BALANCING:
			dicParam[key] = "NORMAL";
			break;
		case ParamType.NUMBER_OF_ITEMS_USED:
			dicParam[key] = AppEventManager.m_TempBox.listUsedBoosterOrder.Count;
			break;
		}
	}

	private static string GetPlatformName()
	{
		if (GlobalSetting.storeMarket == StoreMarket.AmazonStore)
		{
			return "Mobile Amazon";
		}
		if (GlobalSetting.storeMarket == StoreMarket.GooglePlay)
		{
			return "Mobile Android";
		}
		if (GlobalSetting.storeMarket == StoreMarket.OneStore)
		{
			return "Mobile OneStore";
		}
		if (GlobalSetting.storeMarket == StoreMarket.AppStore)
		{
			return "Mobile iOS";
		}
		return "N/A";
	}

	public static int GetDurationAfterAccountCreated()
	{
#if ENABLE_ANTI_CHEAT
		int @int = ObscuredPrefs.GetInt("WDate", 0);
#else
		int @int = CPlayerPrefs.GetInt("WDate", 0);
#endif
        if (@int > 0)
		{
			DateTime dateTime = Utils.UnixTimeStampToDateTime(@int).ToLocalTime();
			DateTime now = DateTime.Now;
			if (dateTime >= now || (dateTime.Year == now.Year && dateTime.Month == now.Month && dateTime.Day == now.Day))
			{
				return 0;
			}
			return Mathf.Max(0, (int)((DateTime.Now - dateTime).TotalDays + 1.0));
		}
		return -1;
	}

	public static int GetDurationDays(int oldUnixTimeStamp)
	{
		DateTime t = Utils.UnixTimeStampToDateTime(oldUnixTimeStamp);
		DateTime utcNow = DateTime.UtcNow;
		if (t >= utcNow || (t.Year == utcNow.Year && t.Month == utcNow.Month && t.Day == utcNow.Day))
		{
			return 0;
		}
		return Mathf.Max(0, (int)(DateTime.UtcNow.Date - t.Date).TotalDays);
	}

	public static int GetDurationLocalDays(DateTime oldDateTime)
	{
		DateTime now = DateTime.Now;
		if (oldDateTime >= now || (oldDateTime.Year == now.Year && oldDateTime.Month == now.Month && oldDateTime.Day == now.Day))
		{
			return 0;
		}
		return Mathf.Max(0, (int)(DateTime.Now.Date - oldDateTime.Date).TotalDays);
	}

	public static bool IsDifferentDay(DateTime oldDateTime)
	{
		DateTime now = DateTime.Now;
		if (oldDateTime >= now || (oldDateTime.Year == now.Year && oldDateTime.Month == now.Month && oldDateTime.Day == now.Day))
		{
			return false;
		}
		return true;
	}

	public static string GetBoosterName(Booster.BoosterType boosterType)
	{
		switch (boosterType)
		{
		case Booster.BoosterType.Hammer:
			return "Magic Hammer";
		case Booster.BoosterType.CandyPack:
			return "Candy Bomb";
		case Booster.BoosterType.Shuffle:
			return "Sweet Shuffle";
		case Booster.BoosterType.Move5:
			return "Move+5";
		case Booster.BoosterType.Move1:
			return "Move+1";
		case Booster.BoosterType.HBomb:
			return "H Bomb";
		case Booster.BoosterType.VBomb:
			return "V Bomb";
		default:
			return "NONE";
		}
	}

	public static int GetCoinValueByServerItemIndex(int iid)
	{
		switch (iid)
		{
		case 7:
			if (MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop.ContainsKey(7))
			{
				return MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop[7].normal_price;
			}
			return 0;
		case 8:
			if (MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop.ContainsKey(8))
			{
				return MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop[8].normal_price;
			}
			return 0;
		case 1:
			return 1;
		default:
			return 0;
		}
	}

	public static string GetWhereSceneType(SceneType sceneType)
	{
		switch (sceneType)
		{
		case SceneType.Game:
			return "In-game";
		case SceneType.Lobby:
			return "Lobby";
		default:
			return "Etc";
		}
	}

	public static string GetCoinCategroyString(AppEventManager.CoinCategory byWhere)
	{
		switch (byWhere)
		{
		case AppEventManager.CoinCategory.DailySpin:
			return "Daily Spin";
		case AppEventManager.CoinCategory.EventPopup:
			return "Event Popup";
		case AppEventManager.CoinCategory.InGameItem:
			return "In-Game Item";
		case AppEventManager.CoinCategory.Life:
			return "Life";
		case AppEventManager.CoinCategory.Move:
			return "Move+";
		case AppEventManager.CoinCategory.PlayBonusGame:
			return "Play Bonus Game";
		case AppEventManager.CoinCategory.AutomaticPopup:
			return "Automatic Popup";
		case AppEventManager.CoinCategory.PreBooster:
			return "Pre Booster";
		case AppEventManager.CoinCategory.DailyBonusLevel:
			return "Daily Bonus Level Game";
		default:
			return "N/A";
		}
	}

	public static string GetServerItemIndexName(ServerItemIndex itemIndex)
	{
		return itemIndex.ToString();
	}

	public static string GetServerItemRewardType(ServerItemIndex itemIndex, bool forStepSpin = false)
	{
		switch (itemIndex)
		{
		case ServerItemIndex.None:
			if (forStepSpin)
			{
				return "Feature : Coin Spin";
			}
			return "None";
		case ServerItemIndex.Coin:
		case ServerItemIndex.BoosterHammer:
		case ServerItemIndex.BoosterCandyPack:
		case ServerItemIndex.BoosterHBomb:
		case ServerItemIndex.BoosterVBomb:
			return itemIndex.ToString();
		default:
			return "ETC";
		}
	}

	public static string GerServerItemNameForAppEvent(ServerItemIndex itemIndex)
	{
		switch (itemIndex)
		{
		case ServerItemIndex.None:
			return "Feature : Coin Spin";
		case ServerItemIndex.BoosterCandyPack:
			return "Candy Pack";
		case ServerItemIndex.BoosterHammer:
			return "Magic Hammer";
		case ServerItemIndex.Coin:
			return itemIndex.ToString();
		default:
			return "ETC";
		}
	}

	public static string GetServerItemRewardTypeSpecific(ServerItemIndex itemIndex, int rewardCount, int buffTime = 0)
	{
		return GerServerItemNameForAppEvent(itemIndex) + " +" + rewardCount;
	}

	public static string GetServerItemRewardTypeSpecificV2(ServerItemIndex itemIndex, int rewardCount)
	{
		return GerServerItemNameForAppEvent(itemIndex) + " +" + rewardCount;
	}

	public static string GetYesOrNo(bool value)
	{
		if (value)
		{
			return "YES";
		}
		return "NO";
	}
}
