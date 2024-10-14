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
using dotmob.sr.maptool;
#if FACEBOOK_SDK
using Facebook.Unity;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

public class AppEventManager : MonoSingleton<AppEventManager>
{
	public enum InitialLaunchFunnelParameter
	{
		Null,
		Guest,
		Facebook,
		Level,
		StepValue,
		Automatic,
		Manual
	}

	public enum InitialLaunchFunnelSteps
	{
		_01AppLaunched,
		_02TitleScreenIsDisplayed,
		_03GameStartButtonClicked,
		_04LoadingResourcesFinished,
		_05PlayerEnteredFirstLevel,
		_05_1TutorialLevel1Finished,
		_05_2FirstLevelPlayFinished,
		_06Level2Cleared,
		_07Level3Cleared,
		_08Level4Cleared,
		_09Level5Cleared,
		_10_1Level6Cleared,
		_10TutorialLevel6Finished,
		_11Level7Cleared,
		_12Level8Cleared,
		_13TutorialLevel9Start,
		_13_1MagicHammerClicked_1,
		_13_2MagicHammerClicked_2,
		_13_3MagicHammerBuyed,
		_13_4MagicHammerClicked_3,
		_13_5MagicHammerUsed,
		_13_6MagicHammerClicked_4,
		_13_7MagicHammerClicked_5,
		_13_8Level9Cleared,
		_14Level10Cleared,
		_15TutorialLevel11Finished,
		_15_1Level11Cleared,
		_16TutorialLevel17Start,
		_16_1CandyCraneClicked_1,
		_16_2CandyCraneClicked_2,
		_16_3CandyCraneBuyed,
		_16_4CandyCraneClicked_3,
		_16_5MCandyCraneUsed,
		_16_6MCandyCraneUsed_2,
		_16_7CandyCraneClicked_4,
		_16_8CandyCraneClicked_5,
		_16_9Level17Cleared,
		_17TutorialLevel20Finished,
		_17_1Level20Cleared,
		_18TutorialLobbyLevel21Start,
		_18_1TutorialFreeCoinClicked,
		_18_2TutorialFreeCoinClicked_2,
		_18_3TutorialFreeCoinBuyed,
		_18_4TutorialFreeCoinCleared,
		_19TutorialLevel30Finished,
		_19_1Level30Cleared,
		_20_RatingStart,
		_20_1RatingCleared,
		_21TutorialLevel32Start,
		_21_1CandyPackClicked_1,
		_21_2CandyPackClicked_2,
		_21_3CandyPackBuyed,
		_21_4CandyPackClicked_3,
		_21_5CandyPackUsed,
		_21_6CandyPackClicked_4,
		_21_7CandyPackClicked_5,
		_21_8Level32Cleared,
		_22TutorialLevel33Finished,
		_22_1Level33Cleared,
		_23TutorialLevel34Start,
		_23_1PreBoosterClicked_1,
		_23_2PreBoosterClicked_2,
		_23_3Level34Cleared,
		_24TutorialLevel40Start,
		_24_1ShuffleClicked_1,
		_24_2ShuffleUsed,
		_24_3ShuffleCleared,
		_24_4Level40Cleared,
		_25TutorialLevel45Start,
		_25_1Level45Cleared,
		_81NoMoreLives,
		_89OldPlayerLoggedIntoFacebook,
		_89_1OldPlayerRestartAsAGuest,
		_90LevelCleared,
		_91LevelFailed
	}

	public enum FacebookLoginFromWhere
	{
		Auto_Popup__x__Cleared_Level_,
		Auto_Popup__x__No_More_Lives,
		Title_Screen,
		Message_Center,
		Heart_Request_Popup,
		Bonus_Level_Popup,
		Leader_Board__x__Game_Start_Popup,
		Leader_Board__x__Game_Result_Popup,
		Leader_Board__x__Lobby_Popup,
		Option_Popup,
		Get_Help_Request_Popup,
		ETC
	}

	public enum SessionStartedAs
	{
		Resumed,
		Relaunched,
		First_Launch
	}

	public enum CoinCategory
	{
		Null,
		InGameItem,
		Life,
		Move,
		DailySpin,
		PlayBonusGame,
		EventPopup,
		AutomaticPopup,
		PreBooster,
		DailyBonusLevel,
		RandomBox
	}

	public enum CoinPurchasedProductType
	{
		Regular_Coin_Pack,
		Starter_Pack_1,
		Cohort_Sale_Pack,
		Cohort_Bonus_Item_Pack,
		Unlimited_Life_Pack,
		Buyback_Pack,
		Ingame_Item_Pack,
		Push__x__1_More_Expensive_Proposal,
		Push__x__1_More_Ingame_Item_Pack,
		Push__x__1_Regular_Coin_Pack,
		Double_Coin_Pack,
		Step_Double_Coin_Pack,
		Super_Sale_Coin_Pack,
		BoosterPackBomb,
		BoosterPackHammer,
		BoosterPackHBomb,
		BoosterPackVBomb,
		BoosterPackBomb2,
		PeriodEventChristmasPack,
		Etc
	}

	public enum ItemEarnedBy
	{
		Initial_Coin_for_New_Player,
		Timed_Replenish__x__While_Gone,
		Timed_Replenish__x__While_Playing,
		Free_Replenish__x__First_Time_No_More_Lives,
		Free_Lives_for_New_Player,
		Free_Replenish,
		Initial_Water_for_New_Player,
		Initial_Items_When_Unlocked,
		Initial_Star_Candy_for_New_Player,
		Level_Clear_Reward,
		Viral_Alien,
		Daily_Bonus_Level_Reward,
		Purchased_with_Real_Money,
		Purchased_with_Coins,
		Bonus_Level_Reward,
		Daily_Bonus,
		Daily_Bonus_Double_Watch_AD,
		Event_Reward,
		CS_Reward,
		Fanpange_Reward,
		Welcome_Back_Bonus,
		Become_a_Hero_Reward,
		Facebook_Login_Reward,
		Daily_Spin_Bonus,
		AD_Watching_Reward,
		Package_Product,
		Fast_Level_Achievement_Reward,
		Friends_Help,
		Hard_Level_Reward,
		Metagame__x__Tree_Reward,
		Rating_Reward,
		OneCoin,
		Step_Spin_Bonus,
		Reward_Life_Message,
		Guest_Bonus,
		Random_Box,
		League,
		Second_Level_Clear_Reward,
		Quest_Reward,
		Quest_All_Clear_Reward,
		Step_Spin_Tutorial,
		Other_Reward
	}

	public enum StageClearPopupClosedAction
	{
		None,
		Next,
		Invite,
		Close
	}

	public enum LifeAccessedBy
	{
		Life_Icon_from_Lobby,
		No_more_life_Automatic_Popup,
		Message_Center,
		Friends_Ranking_UI,
		Etc
	}

	public enum AdAccessedBy
	{
		Life_Icon_from_Lobby,
		No_more_life_Popup,
		No_more_life_Automatic_Popup,
		BounsLevelBall_from_Lobby,
		BounsLevelBall_Automatic_Popup,
		Levelball_from_Lobby,
		GameStart_Automatic_Popup,
		Coin_Store_Popup,
		Coin_Store_Automatic_Popup,
		Coin_ADReward_Popup,
		AD_Booster_Icon_from_Ingame,
		AD_Package_Popup,
		AlienMessage,
		OneCoin_RewardAD_Retry_popup,
		AD_Help_Popup,
		Level_Result_Popup,
		LevelReward_Popup,
		Guest_GameStartPopup_RankingUI,
		LifeMessage,
		StepSpinPopup,
		StepSpinTutorialPopup,
		Level_Clear_Click_Next_Button,
		Etc
	}

	public enum AdCompletedStepReached
	{
		None,
		Popup_Opened,
		Clicked_ADWatching_Button,
		Completed
	}

	public enum GetPermissionActionType
	{
		Impression,
		Response_Confirm,
		Response_Later
	}

	public enum GetPermissionAccessedBy
	{
		AcceptDialog,
		AutomaticPopup,
		InvitePopup,
		LeaderBoard_GameStart,
		LeaderBoard_GameEnd,
		LeaderBoard_LobbyIcon,
		GetHelp,
		AskForLives,
		MessageCenter,
		ETC
	}

	public enum ItemType
	{
		NONE,
		Move5,
		MagicHammer,
		CandyCrane,
		CandyPack
	}

	public enum LevelPlayType
	{
		Normal,
		Hard_Level,
		Bonus_Level,
		Friend_Help__y__Normal,
		Friend_Help__y__Hard_Level,
		One_Coin,
		Etc,
		Daily_Bonus_Level,
		NULL,
		Max
	}

	public enum PlayerAction
	{
		Game_Over,
		Player_Did_Quit,
		Terminated_Unfinished
	}

	public enum OfferCondition
	{
		NULL,
		Right_after_First_Purchase,
		Need_Coins_for_Something,
		Long_Time_No_Pay
	}

	public enum PurchaseTypeOfPopup
	{
		Regual_Coin_Store,
		Starter_Pack_Popup,
		Cohort_Sale_Popup__x__2nd_Purchase,
		Unlimited_Life_Pack,
		Buyback_Pack,
		Ingame_Item_Pack,
		Push__x__1_More_Expensive_Proposal,
		Push__x__1_More_Ingame_Item_Pack,
		Push__x__1_Regular_Coin_Store,
		Double_Coin_Store,
		Step_Double_Coin_Store,
		BoosterPackBomb,
		BoosterPackHammer,
		BoosterPackHBomb,
		BoosterPackVBomb,
		BoosterPackBomb2,
		PeriodEventChristmasPack,
		Others
	}

	public enum PurchaseReachedStep
	{
		PopupOpened,
		ClickedPurchaseButton,
		Purchased
	}

	public enum FreeMovePackPurchasedBy
	{
		Free__z_5_Moves_for_First_Time_Failure
	}

	public enum InvitePopupAccessedBy
	{
		Automatic_Popup,
		Life_Request_Popup,
		Message_Center,
		Ranking_UI,
		Bonus_Level,
		Chain_Letter,
		Etc
	}

	public enum InviteReachedStep
	{
		Popup_Opened,
		Clicked_Invite_Button,
		Completed
	}

	public enum MetaGameAccessedBy
	{
		Lobby_Icon,
		Automatic_Event
	}

	public enum SpinCost
	{
		Free,
		AD,
		Coin,
		Tutorial_AD,
		ETC
	}

	public enum LobbyButtonType
	{
		Buff,
		Cross_Promotion,
		One_Coin_Clear,
		Daily_Bonus_Level,
		Cohort_Pack,
		Step_Spin,
		Quest,
		Sweet_Garden,
		League,
		Heart,
		Coin,
		Cookie_Point,
		World_Map,
		Ranking,
		Message_Box,
		ETC
	}

	public enum ButtonLocation
	{
		Left,
		Right,
		Top,
		Bottom,
		Full_Screen,
		ETC
	}

	public bool isCheater;

	private static readonly string appName = "SR_M";

	public int startTimeAfterLogin;

	public static AppEventTempBox m_TempBox = new AppEventTempBox();

	private readonly Dictionary<int, string> IntialLaunchFunnel_Steps_Dic = new Dictionary<int, string>
	{
		{
			0,
			"01. App Launched"
		},
		{
			1,
			"02. Title Screen is Disaplyed"
		},
		{
			2,
			"03. Game Start Button Clicked"
		},
		{
			3,
			"04. Loading Resources Finished"
		},
		{
			4,
			"05. Player Entered First Level"
		},
		{
			5,
			"05-1. Tutorial : Level 1 Finished"
		},
		{
			6,
			"05-2. First Level Play Finished"
		},
		{
			7,
			"06. Level 2 Cleared"
		},
		{
			8,
			"07. Level 3 Cleared"
		},
		{
			9,
			"08. Level 4 Cleared"
		},
		{
			10,
			"09. Level 5 Cleared"
		},
		{
			11,
			"10-1. Level 6 Cleared"
		},
		{
			12,
			"10. Tutorial : Level 6 Finished"
		},
		{
			13,
			"11. Level 7 Cleared"
		},
		{
			14,
			"12. Level 8 Cleared"
		},
		{
			15,
			"13. Tutorial: Level 9 Start"
		},
		{
			16,
			"13-1. Magic Hammer Clicked-1"
		},
		{
			17,
			"13-2. Magic Hammer Clicked-2"
		},
		{
			18,
			"13-3. Magic Hammer Buyed"
		},
		{
			19,
			"13-4. Magic Hammer Clicked-3"
		},
		{
			20,
			"13-5. Magic Hammer Used"
		},
		{
			21,
			"13-6. Magic Hammer Clicked-4"
		},
		{
			22,
			"13-7. Magic Hammer Clicked-5"
		},
		{
			23,
			"13-8. Level 9 Cleared"
		},
		{
			24,
			"14. Level 10 Cleared"
		},
		{
			25,
			"15. Tutorial : Level 11 Finished"
		},
		{
			26,
			"15-1. Level 11 Cleared"
		},
		{
			27,
			"16. Tutorial : Level 17 Start"
		},
		{
			28,
			"16-1. Candy Crane Clicked-1"
		},
		{
			29,
			"13-2. Candy Crane Clicked-2"
		},
		{
			30,
			"16-3. Candy Crane Buyed"
		},
		{
			31,
			"16-4. Candy Crane Clicked-3"
		},
		{
			32,
			"16-5. Candy Crane Used"
		},
		{
			33,
			"16-6. Candy Crane Used-2"
		},
		{
			34,
			"16-7. Candy Crane Clicked-4"
		},
		{
			35,
			"16-8. Candy Crane Clicked-5"
		},
		{
			36,
			"16-9. Level 17 Cleared"
		},
		{
			37,
			"17. Tutorial : Level 20 Cleared"
		},
		{
			38,
			"17-1. Level 20 Cleared"
		},
		{
			39,
			"18. Tutorial: Lobby Level 21 Start"
		},
		{
			40,
			"18-1. Tutorial: Free Coin Clicked"
		},
		{
			41,
			"18-2. Tutorial: Free Coin Clicked-2"
		},
		{
			42,
			"18-3. Tutorial: Free Coin Buyed"
		},
		{
			43,
			"18-4. Tutorial: Free Coin Cleared"
		},
		{
			44,
			"19. Tutorial : Level 30 Finished"
		},
		{
			45,
			"19-1. Level 30 Cleared"
		},
		{
			46,
			"20. Tutorial : Rating Start"
		},
		{
			47,
			"20-1. Rating Cleared"
		},
		{
			48,
			"21. Tutorial: Level 32 Start"
		},
		{
			49,
			"21-1. Candy Pack Clicked-1"
		},
		{
			50,
			"21-2. Candy Pack Clicked-2"
		},
		{
			51,
			"21-3. Candy Pack Buyed"
		},
		{
			52,
			"21-4. Candy Pack Clicked-3"
		},
		{
			53,
			"21-5. Candy Pack Used"
		},
		{
			54,
			"21-6. Candy Pack Clicked-4"
		},
		{
			55,
			"21-7. Candy Pack Clicked-5"
		},
		{
			56,
			"21-8. Level 32 Cleared"
		},
		{
			57,
			"22. Tutorial : Level 33 Finished"
		},
		{
			58,
			"22-1. Level 33 Cleared"
		},
		{
			59,
			"23. Tutorial: Level 34 Start"
		},
		{
			60,
			"23-1. Pre Booster Clicked-1"
		},
		{
			61,
			"23-2. Pre Booster Clicked-2"
		},
		{
			62,
			"23-3. Level 34 Cleared"
		},
		{
			63,
			"24. Tutorial: Level 40 Start"
		},
		{
			64,
			"24-1. Shuffle Clicked-1"
		},
		{
			65,
			"24-2. Shuffle Clicked-2"
		},
		{
			66,
			"24-3. Shuffle Cleared"
		},
		{
			67,
			"24-4. Level 40 Cleared"
		},
		{
			68,
			"25. Tutorial: Level 45 Start"
		},
		{
			69,
			"25-1. Level 45 Cleared"
		},
		{
			70,
			"81. No More Lives"
		},
		{
			71,
			"89. Old Player Logged into Facebook"
		},
		{
			72,
			"89-1. Old Player Restart as a Guest"
		},
		{
			73,
			"90. Level Cleared"
		},
		{
			74,
			"91. Level Failed"
		}
	};

	private DateTime timeLastStep = DateTime.Now;

	public List<string> pastStep = new List<string>();

	public Dictionary<int, int> tryCount = new Dictionary<int, int>();

	private void Start()
	{
		startTimeAfterLogin = Utils.ConvertToTimestamp(DateTime.UtcNow);
		m_TempBox.IsFirstLevelFail = ((PlayerPrefs.GetInt("FirstLevelFail", 0) == 1) ? true : false);
		m_TempBox.IsFirstLifeZero = ((PlayerPrefs.GetInt("FirstLifeZero", 0) == 1) ? true : false);
		m_TempBox.countFailStreak = PlayerPrefs.GetInt("CountFailStreak", 0);
		m_TempBox.DungeonOpenCount = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCDungeonOpenCount), 0);
		m_TempBox.FirstGetTokenTime = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCFirstGetTokenTime), 0);
		m_TempBox.TokenCountSwitch = ((PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCTokenCountSwitch), 0) == 1) ? true : false);
		m_TempBox.LevelCountForTokenTime = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCLevelCountForTokenTime), 1);
		m_TempBox.DungeonLevelPlayCount = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCDungeonLevelPlayCount), 0);
		m_TempBox.DungeonLevelFailCount = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCDungeonLevelFailCount), 0);
		m_TempBox.MidRewardCountInDungeon = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCMidRewardCountInDungeon), 0);
		m_TempBox.MidRewardCountInEvent = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCMidRewardCountInEvent), 0);
		m_TempBox.MidRewardFailedCount = PlayerPrefs.GetInt(LocalDataString.GetString(StringIndex.OCCMidRewardFailedCount), 0);
	}

	public void SendAppEventDailyPlayerStatus()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LOGGED_IN_AS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_AVERAGE_PRICE_PER_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DATE_OF_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_ITEM_REMAINS_MAGICHAMMER);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_ITEM_REMAINS_CANDYCRANE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_ITEM_REMAINS_CANDYBOMB);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_NUMBER_OF_GAME_FRIENDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_RESOURCE_REMAINS_WATER);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_RESOURCE_REMAINS_STARCANDY);
		SendAppEvent("DAILY_PLAYER_STATUS_M_2", MonoSingleton<PlayerDataManager>.Instance.Coin, dictionary);
	}

	public void SendAppEventPlayerSessionStart(int durationHour, SessionStartedAs sessionStartedAs)
	{
		CheckCheater();
		ParamType[] paramList = new ParamType[17]
		{
			ParamType.PLATFORM,
			ParamType.OS_VERSION,
			ParamType.DEVICE_MODEL,
			ParamType.SCREEN_RESOLUTION,
			ParamType.PLAYER_UID,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR,
			ParamType.PLAYER_TOTAL_PURCHASE_COUNT,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.IS_FIRST_SESSION,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		string text = (string)(dictionary["IS_PUSH_ALLOWED"] = "N/A");
		dictionary["PLAYER_HOURS_SINCE_LAST_SESSION_END"] = durationHour;
		dictionary["SESSION_STARTED_AS"] = GetLogTypeString(sessionStartedAs.ToString());
		dictionary["PLAYER_TOTAL_ADS_COUNT"] = MonoSingleton<PlayerDataManager>.Instance.adsCount;
		int adsCount = MonoSingleton<PlayerDataManager>.Instance.adsCount;
		int durationAfterAccountCreated = AppEventCommonParameters.GetDurationAfterAccountCreated();
		int num = 0;
		num = ((durationAfterAccountCreated != 0) ? ((adsCount == 0) ? (-1000) : ((adsCount <= durationAfterAccountCreated) ? (-1 * (durationAfterAccountCreated / adsCount)) : (adsCount / durationAfterAccountCreated))) : 0);
		dictionary["ADS_LOYALTY"] = num;
		SendAppEvent("PLAYER_SESSION_START_M", null, dictionary);
	}

	public void SendAppEventPlayerADSource()
	{
		ParamType[] paramList = new ParamType[8]
		{
			ParamType.IS_ORGANIC_PRESUMABLY,
			ParamType.AD_NETWORK,
			ParamType.AD_CAMPAIGN_NAME,
			ParamType.PLATFORM,
			ParamType.PLAYER_UID,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_DATE_OF_INSTALL,
			ParamType.DID_PLAYER_EVER_PAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		SendAppEvent("SEGMENT_UA_SOURCE_M", null, dictionary);
	}

	public void SendAppEventMovePackPurchased(Booster.BoosterType type, int gid, int movePackPriceCoin, int continueCount, int userCoinBefore, int userCoinAfter, int[] countOfEachTargetRemainCount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string text = (string)(dictionary["SPECIFIC_TYPE"] = ((type != Booster.BoosterType.Move1) ? ("5 Moves for " + movePackPriceCoin + " Coins") : ("1 Moves for " + movePackPriceCoin + " Coins")));
		dictionary["PRICE_IN_COINS"] = movePackPriceCoin;
		dictionary["PURCHASE_COUNT_INCLUDING_THIS"] = continueCount + 1;
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		dictionary["PLAYER_COIN_BALANCE_AFTER_EVENT"] = userCoinAfter;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		int num = 0;
		for (int i = 0; i < countOfEachTargetRemainCount.Length; i++)
		{
			num += countOfEachTargetRemainCount[i];
		}
		dictionary["LEVEL_OBJECTIVE_LEFT"] = num;
		int num2 = 0;
		for (int j = 0; j < m_TempBox.listUsedBoosterOrder.Count; j++)
		{
			if (m_TempBox.listUsedBoosterOrder[j] != Booster.BoosterType.Move5 && m_TempBox.listUsedBoosterOrder[j] != Booster.BoosterType.Move1)
			{
				num2++;
			}
		}
		dictionary["COUNT_ALREADY_SPENT_INGAME_ITEM"] = num2;
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		if (m_TempBox.isPurchaseInGameOver)
		{
			if (MonoSingleton<PopupManager>.Instance.CurrentPopupType == PopupType.PopupGameOver)
			{
				dictionary["THROUGH_WHERE"] = "Game Over Screen - Only 1 left";
			}
			else
			{
				dictionary["THROUGH_WHERE"] = "Game Over Screen - Normal";
			}
		}
		else
		{
			dictionary["THROUGH_WHERE"] = "Ingame Icon";
		}
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		dictionary["MOVE_PACK_PURCHASED_POPUP_TYPE"] = ((MonoSingleton<PopupManager>.Instance.CurrentPopupType != PopupType.PopupGameOver) ? "More than one" : "Just One");
		SendAppEvent("MOVE_PACK_PURCHASED_M", movePackPriceCoin, dictionary);
	}

	public void SendAppEventFreeMovePackPurchased(int gid, int moveCount, int userCoinBefore, FreeMovePackPurchasedBy purchasedBy)
	{
		ParamType[] paramList = new ParamType[10]
		{
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR,
			ParamType.PLAYER_TOTAL_PURCHASE_COUNT,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLAYER_UID
		};
		ParamTypeIncGID[] paramGidList = new ParamTypeIncGID[4]
		{
			ParamTypeIncGID.LEVEL_NUMBER,
			ParamTypeIncGID.STAGE_NUMBER,
			ParamTypeIncGID.LEVEL_PLAY_TYPE,
			ParamTypeIncGID.LEVEL_GAME_MODE
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, paramGidList, gid);
		dictionary["NUMBER_OF_MOVES_GIVEN"] = moveCount;
		dictionary["PURCHASED_BY"] = GetLogTypeString(purchasedBy.ToString());
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		SendAppEvent("FREE_MOVE_PACK_PURCHASED_M", null, dictionary);
	}

	public void SendAppEventLoginWithFacebook()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string empty = string.Empty;
		empty = (string)(dictionary["LOGGED_IN_TYPE"] = ((m_TempBox.loggedInType == 0) ? "Played as Guest" : ((m_TempBox.loggedInType == 1) ? "Never Played Before" : ((m_TempBox.loggedInType == 2) ? "Synced with Other FB ID" : ((m_TempBox.loggedInType != 3) ? "N/A" : "Existing Account")))));
		dictionary["IS_FACEBOOK_ACCOUNT_NEW"] = ((m_TempBox.isExistFacebookAcount != 1) ? "YES" : "NO");
		dictionary["IS_PLAYER_ACCOUNT_NEW"] = ((m_TempBox.isExistPlayerAccount != 1) ? "YES" : "NO");
		dictionary["FROM_WHERE"] = GetLogTypeString(m_TempBox.fbLoginWhere.ToString());
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		SendAppEvent("LOGGED_IN_WITH_FACEBOOK_ACCOUNT_M", null, dictionary);
	}

	public void SendAppEventLevelStart(int gid, int prevPlayCount, int prevClearCount, int prevFailCount, int prevStar, int prevHighScore)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
		dictionary["PREVIOUSLY_PLAYED_COUNT"] = prevPlayCount;
		dictionary["PREVIOUSLY_CLEARED_COUNT"] = prevClearCount;
		dictionary["PREVIOUSLY_FAIL_COUNT"] = prevFailCount;
		dictionary["PREVIOUS_RECORD_NUMBER_OF_STARS"] = prevStar;
		dictionary["PREVIOUS_RECORD_HIGH_SCORE"] = prevHighScore;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		SendAppEvent("LEVEL_STARTED_M", null, dictionary);
	}

	public void SendAppEventLevelClear(int gid, int getStarCount, int getScore, int remainMoveCount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.IS_FIRST_CLEAR, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		dictionary["GAME_RESULT_NUMBER_OF_STARS"] = getStarCount;
		dictionary["GAME_RESULT_SCORE"] = getScore;
		dictionary["PREVIOUSLY_PLAYED_COUNT"] = m_TempBox.prevGamePlayCount;
		dictionary["PREVIOUSLY_FAIL_COUNT"] = m_TempBox.prevGameFailCount;
		dictionary["PREVIOUS_RECORD_NUMBER_OF_STARS"] = m_TempBox.prevGameStar;
		dictionary["PLAYTIME_IN_SECONDS"] = Mathf.Max(0, (int)(Math.Round((DateTime.Now - m_TempBox.dateTimeGameStart).TotalSeconds / 10.0, 1, MidpointRounding.AwayFromZero) * 10.0));
		dictionary["NUMBER_OF_MOVES_LEFT"] = remainMoveCount;
		dictionary["NUMBER_OF_ITEMS_USED"] = m_TempBox.listUsedBoosterOrder.Count;
		for (int i = 0; i < 5; i++)
		{
			string value = (i >= m_TempBox.listUsedBoosterOrder.Count) ? "NONE" : AppEventCommonParameters.GetBoosterName(m_TempBox.listUsedBoosterOrder[i]);
			dictionary["ITEM_TYPE_USED_" + (i + 1)] = value;
		}
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		dictionary["AREA_TUTORIAL"] = ((m_TempBox.didAreaTutorialPlay == 0) ? "N/A" : ((m_TempBox.didAreaTutorialPlay != 1) ? "NO" : "YES"));
		SendAppEvent("LEVEL_CLEARED_M", getStarCount, dictionary);
	}

	public void SendAppEventPopupLevelCleared()
	{
		string value = string.Empty;
		if (m_TempBox.stageClearedPopupClosedAction != 0)
		{
			if (m_TempBox.stageClearedPopupClosedAction == StageClearPopupClosedAction.Next)
			{
				value = "Next";
			}
			else if (m_TempBox.stageClearedPopupClosedAction == StageClearPopupClosedAction.Invite)
			{
				value = "Invite";
			}
			else if (m_TempBox.stageClearedPopupClosedAction == StageClearPopupClosedAction.Close)
			{
				value = "X";
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["PLAYER_ACTION"] = value;
			dictionary["PREVIOUSLY_PLAYED_COUNT"] = m_TempBox.prevGamePlayCount;
			SendAppEvent("POPUP_LEVEL_CLEARED", 0f, dictionary);
		}
	}

	public void SendAppEventLevelFail(int gid, int getStarCount, int getScore, int remainMoveCount, int[] countOfEachTargetRemainCount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		int num = 0;
		for (int i = 0; i < countOfEachTargetRemainCount.Length; i++)
		{
			num += countOfEachTargetRemainCount[i];
		}
		dictionary["LEVEL_OBJECTIVE_LEFT"] = num;
		dictionary["NUMBER_OF_ITEMS_USED"] = m_TempBox.listUsedBoosterOrder.Count;
		dictionary["GAME_RESULT_SCORE"] = getScore;
		dictionary["PLAYTIME_IN_SECONDS"] = Mathf.Max(0, (int)(Math.Round((DateTime.Now - m_TempBox.dateTimeGameStart).TotalSeconds / 10.0, 1, MidpointRounding.AwayFromZero) * 10.0));
		dictionary["NUMBER_OF_MOVES_LEFT"] = remainMoveCount;
		dictionary["DID_PLAYER_QUIT"] = ((m_TempBox.didPlayerQuit == -1) ? "YES" : ((m_TempBox.didPlayerQuit != 0) ? "YES-retry" : "NO"));
		dictionary["AREA_TUTORIAL"] = ((m_TempBox.didAreaTutorialPlay == 0) ? "N/A" : ((m_TempBox.didAreaTutorialPlay != 1) ? "NO" : "YES"));
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		float value = 100f;
		if (m_TempBox.totalCollectCount > 0)
		{
			value = 1f - (float)num / (float)m_TempBox.totalCollectCount;
		}
		SendAppEvent("LEVEL_FAILED_M", value, dictionary);
	}

	public void SendAppEventLevelClearScore(int gid, int getStarCount, int getScore, int remainMoveCount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.IS_FIRST_CLEAR, gid);
		dictionary["GAME_RESULT_NUMBER_OF_STARS"] = getStarCount;
		dictionary["GAME_RESULT_SCORE"] = getScore;
		dictionary["PREVIOUSLY_PLAYED_COUNT"] = m_TempBox.prevGamePlayCount;
		dictionary["PREVIOUSLY_FAIL_COUNT"] = m_TempBox.prevGameFailCount;
		dictionary["PREVIOUS_RECORD_NUMBER_OF_STARS"] = m_TempBox.prevGameStar;
		dictionary["IS_HIGH_SCORE"] = ((m_TempBox.prevGameBestScore == 0) ? "N/A" : ((getScore <= m_TempBox.prevGameBestScore) ? "NO" : "YES"));
		dictionary["NUMBER_OF_MOVES_LEFT"] = remainMoveCount;
		dictionary["NUMBER_OF_ITEMS_USED"] = m_TempBox.listUsedBoosterOrder.Count;
		for (int i = 0; i < 5; i++)
		{
			string value = (i >= m_TempBox.listUsedBoosterOrder.Count) ? "NONE" : AppEventCommonParameters.GetBoosterName(m_TempBox.listUsedBoosterOrder[i]);
			dictionary["ITEM_TYPE_USED_" + (i + 1)] = value;
		}
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		SendAppEvent("LEVEL_CLEAR_SCORE_M", getScore, dictionary);
	}

	public void SendAppEventLevelResetWithoutUsingMove(int gid)
	{
		ParamType[] paramList = new ParamType[6]
		{
			ParamType.PLATFORM,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_LOGGED_IN_AS
		};
		ParamTypeIncGID[] paramGidList = new ParamTypeIncGID[3]
		{
			ParamTypeIncGID.LEVEL_NUMBER,
			ParamTypeIncGID.STAGE_NUMBER,
			ParamTypeIncGID.LEVEL_GAME_MODE
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, paramGidList, gid);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		SendAppEvent("LEVEL_RESET_WITHOUT_USING_MOVE_M", null, dictionary);
	}

	public void SendAppEventCoinPurchased(int gid, int userCoinBefore, int getCoin, int listIndexByPosition, float priceDollar)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		dictionary["HOW_MANY_COINS"] = getCoin;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		AppEventCommonParameters.SetParam(dictionary, ParamType.NEEDED_COINS_FOR_WHAT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.WHERE);
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRODUCT_TYPE"] = GetLogTypeString(m_TempBox.coinProductType.ToString());
		dictionary["IS_FIRST_TIME_PURCHASE"] = ((MonoSingleton<PlayerDataManager>.Instance.PayCount != 0) ? "NO" : "YES");
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_CLEAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		SendAppEvent("COIN_PURCHASED_M", getCoin, dictionary);
	}

	public void SendAppEventPurchased(int gid, int userCoinBefore, int getCoin, int listIndexByPosition, float priceDollar)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		dictionary["HOW_MANY_COINS"] = getCoin;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		AppEventCommonParameters.SetParam(dictionary, ParamType.NEEDED_COINS_FOR_WHAT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.WHERE);
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRODUCT_TYPE"] = GetLogTypeString(m_TempBox.coinProductType.ToString());
		dictionary["IS_FIRST_TIME_PURCHASE"] = ((MonoSingleton<PlayerDataManager>.Instance.PayCount != 0) ? "NO" : "YES");
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
		SendAppEvent("PURCHASES_M", priceDollar, dictionary);
	}

	public void SendAppEventCoinEarned(int gid, int userCoinBefore, int getCoin, int userCoinAfter, ItemEarnedBy rewardEarnedBy)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		dictionary["PLAYER_COIN_BALANCE_AFTER_EVENT"] = userCoinAfter;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.IS_THIS_LEVEL_EVER_CLEARED, gid);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		dictionary["HOW_MANY_COINS"] = getCoin;
		dictionary["EARNED_BY"] = GetLogTypeString(rewardEarnedBy.ToString());
		SendAppEvent("COIN_EARNED_M", getCoin, dictionary);
	}

	public void SendAppEventCoinConsumed(int usedCoin, int gid, int userCoinBefore, int userCoinAfter, CoinCategory byWhere, string specific_type, int iid)
	{
		m_TempBox.TotalSpentCoin += usedCoin;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		dictionary["PLAYER_COIN_BALANCE_AFTER_EVENT"] = userCoinAfter;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.IS_THIS_LEVEL_EVER_CLEARED, gid);
		dictionary["CATEGORY"] = AppEventCommonParameters.GetCoinCategroyString(byWhere);
		dictionary["SPECIFIC_TYPE"] = $"{specific_type} ({iid})";
		dictionary["HOW_MANY_COINS"] = usedCoin;
		AppEventCommonParameters.SetParam(dictionary, ParamType.WHERE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_CLEAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		dictionary["LEVEL_PLAY_TYPE"] = GetLevelPlayTypeWithoutMapDataMain(gid);
		SendAppEvent("COIN_CONSUMED_M", usedCoin, dictionary);
	}

	public void SendAppEventGetPermissionFriendList(GetPermissionActionType actionType)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["ACTION_TYPE"] = actionType.ToString();
		dictionary["ACCESSED_BY"] = m_TempBox.permissionAccessedBy.ToString();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
	}

	public void SendAppEventGetPermissionPublish(GetPermissionActionType actionType)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Clear();
		float value = (actionType == GetPermissionActionType.Impression) ? 1 : (-1);
		dictionary["ACTION_TYPE"] = actionType.ToString();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		SendAppEvent("FB_API_POPUP_POSTING_M", value, dictionary);
	}

	public void SendAppEventInGameItemUsed(int gid, int price, int prevNumOfItem, Booster.BoosterType itemType, int givenMoveCount, int remainMoveCount, int[] countOfEachTargetRemainCount)
	{
		m_TempBox.TotalSpendInGameItemsCount++;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = price;
		dictionary["ITEM_TYPE"] = AppEventCommonParameters.GetBoosterName(itemType);
		dictionary["ITEM_PRICE_IN_COIN"] = price;
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DYNAMIC_BALANCING);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		dictionary["NUMBER_OF_THIS_ITEM_IN_STOCK"] = prevNumOfItem;
		dictionary["NUMBER_OF_ITEMS_ALREADY_USED"] = m_TempBox.NumOfItemsAlreadyUsedInLevel++;
		dictionary["ITEM_TYPE_USED_RIGHT_BEFORE"] = m_TempBox.itemTypeUsedRightBefore.ToString();
		int num = 0;
		for (int i = 0; i < countOfEachTargetRemainCount.Length; i++)
		{
			num += countOfEachTargetRemainCount[i];
		}
		dictionary["LEVEL_OBJECTIVE_LEFT"] = num;
		dictionary["NUMBER_OF_MOVES_GIVEN"] = givenMoveCount;
		dictionary["NUMBER_OF_MOVES_LEFT"] = remainMoveCount;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.BUFFS_APPLIED);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		SendAppEvent("ITEM_INGAME_USED_M", value, dictionary);
		m_TempBox.itemTypeUsedRightBefore = itemType;
	}

	public void SendAppEventInGameItemEarned(Booster.BoosterType itemType, ItemEarnedBy earnedBy, int NumOfEarnedItem, int price, int prevNumOfItem)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = NumOfEarnedItem;
		dictionary["ITEM_TYPE"] = AppEventCommonParameters.GetBoosterName(itemType);
		dictionary["HOW_MANY_ITEMS"] = NumOfEarnedItem;
		dictionary["EARNED_BY"] = GetLogTypeString(earnedBy.ToString());
		dictionary["NUMBER_OF_THIS_ITEM_IN_STOCK"] = prevNumOfItem;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		SendAppEvent("INGAME_ITEM_EARNED_M", value, dictionary);
	}

	public void SendAppEventFreeInGameItemUsed(int gid, int prevNumOfItem, Booster.BoosterType itemType, int givenMoveCount, int remainMoveCount, int[] countOfEachTargetRemainCount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["ITEM_TYPE"] = AppEventCommonParameters.GetBoosterName(itemType);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_GAME_MODE, gid);
		dictionary["NUMBER_OF_THIS_ITEM_IN_STOCK"] = prevNumOfItem;
		int num = 0;
		for (int i = 0; i < countOfEachTargetRemainCount.Length; i++)
		{
			num += countOfEachTargetRemainCount[i];
		}
		dictionary["LEVEL_OBJECTIVE_LEFT"] = num;
		dictionary["NUMBER_OF_MOVES_GIVEN"] = givenMoveCount;
		dictionary["NUMBER_OF_MOVES_LEFT"] = remainMoveCount;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		SendAppEvent("FREE_INGAME_ITEM_USED_M", null, dictionary);
	}

	public void SendAppEventFirstTimePurchase(int gid, int userCoinBefore, int getCoin, int listIndexByPosition, float priceDollar, int iid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DATE_OF_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		dictionary["HOW_MANY_COINS"] = getCoin;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		AppEventCommonParameters.SetParam(dictionary, ParamType.NEEDED_COINS_FOR_WHAT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.WHERE);
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRODUCT_TYPE"] = GetLogTypeString(m_TempBox.coinProductType.ToString());
		dictionary["PRODUCT_ID_INTERNAL"] = iid.ToString();
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_CLEAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		SendAppEvent("FIRST_TIME_PURCHASE_M", (int)dictionary["PLAYER_DAYS_SINCE_INSTALL"], dictionary);
	}

	public void SendAppEventFirstTimeLevelFailed(int gid)
	{
		PlayerPrefs.SetInt("FirstLevelFail", 1);
		m_TempBox.IsFirstLevelFail = true;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = gid;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LOGGED_IN_AS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.IS_FIRST_SESSION);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		SendAppEvent("FIRST_TIME_LEVEL_FAILED_M", value, dictionary);
	}

	public void SendAppEventFirstTimeNoMoreLives(int gid)
	{
		PlayerPrefs.SetInt("FirstLifeZero", 1);
		m_TempBox.IsFirstLifeZero = true;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LOGGED_IN_AS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER_LAST_PLAYED, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER_LAST_PLAYED, gid);
		dictionary["COUNT_FAIL_STREAK"] = m_TempBox.countFailStreak;
		AppEventCommonParameters.SetParam(dictionary, ParamType.IS_FIRST_SESSION);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_CLEAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		SendAppEvent("FIRST_TIME_NO_MORE_LIVES_M", value, dictionary);
	}

	public void SendAppEventLifeEarned(int getLife, ItemEarnedBy earnedBy)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = getLife;
		dictionary["EARNED_BY"] = GetLogTypeString(earnedBy.ToString());
		dictionary["HOW_MANY_LIVES"] = getLife;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		SendAppEvent("LIFE_EARNED_M", value, dictionary);
	}

	public void SendAppEventLifeConsumed(PlayerAction playerAction, int gid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = -1f;
		dictionary["PLAYER_ACTION"] = GetLogTypeString(playerAction.ToString());
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		SendAppEvent("LIFE_CONSUMED_M", value, dictionary);
	}

	public void SendAppEventLifePurchased(int getLife, int userCoinBefore, int price, int lastPlayedLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float value = getLife;
		dictionary["HOW_MANY_LIVES"] = getLife;
		dictionary["PRICE_IN_COIN"] = price;
		dictionary["DID_CLEAR_LAST_PLAYED_LEVEL"] = ((m_TempBox.didClearLastPlayedLevel == 0) ? "NO" : ((m_TempBox.didClearLastPlayedLevel != 1) ? "N/A" : "YES"));
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER_LAST_PLAYED, lastPlayedLevel);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER_LAST_PLAYED, lastPlayedLevel);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		SendAppEvent("LIFE_PURCHASED_M", value, dictionary);
	}

	public void SendAppEventLifeNoMoreLives(int lastPlayedLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER_LAST_PLAYED, lastPlayedLevel);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER_LAST_PLAYED, lastPlayedLevel);
		dictionary["COUNT_FAIL_STREAK"] = m_TempBox.countFailStreak;
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_CLEAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		SendAppEvent("LIFE_NO_MORE_LIVES_M", null, dictionary);
	}

	public void SendAppEventLifeRequestToFriends(int requestCount, int requestAvailableCount, int isAll, bool isManualSelect)
	{
		ParamType[] paramList = new ParamType[8]
		{
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.WHERE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_UID
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["ACCESSED_BY"] = GetLogTypeString(m_TempBox.askLifeAccessedBy.ToString());
		string value = "N/A";
		switch (isAll)
		{
		case 1:
			value = "YES";
			break;
		case 0:
			value = "NO";
			break;
		}
		dictionary["IS_ALL_OPTION_SELECTED"] = value;
		string value2 = "N/A";
		if (isAll != -1)
		{
			value2 = ((!isManualSelect) ? "NO" : "YES");
		}
		dictionary["DID_SELECTED_FRIENDS_MANUALY"] = value2;
		dictionary["NUMBER_OF_FRIENDS_TO_SEND"] = requestCount;
		dictionary["NUMBER_OF_FRIENDS_AVAILABLE_TO_SEND"] = requestAvailableCount;
		SendAppEvent("LIFE_REQUEST_SENT_M", requestCount, dictionary);
	}

	public void SendAppEventLifeSendSent(int sentCount, int sentAvailableCount, int isCheckAll)
	{
		ParamType[] paramList = new ParamType[6]
		{
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_UID
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["ACCESSED_BY"] = GetLogTypeString(m_TempBox.sendLifeAccessedBy.ToString());
		string value = "N/A";
		switch (isCheckAll)
		{
		case 1:
			value = "YES";
			break;
		case 0:
			value = "NO";
			break;
		}
		dictionary["IS_ALL_OPTION_SELECTED"] = value;
		string value2 = "N/A";
		switch (isCheckAll)
		{
		case 0:
			value2 = "YES";
			break;
		case 1:
			value2 = "NO";
			break;
		}
		dictionary["DID_SELECTED_FRIENDS_MANUALY"] = value2;
		dictionary["NUMBER_OF_FRIENDS_TO_SEND"] = sentCount;
		dictionary["NUMBER_OF_FRIENDS_AVAILABLE_TO_SEND"] = sentAvailableCount;
		SendAppEvent("LIFE_SEND_SENT_M", sentCount, dictionary);
	}

	public void SendAppEventTimeSpent(SceneType sceneTypeWhere, int gid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = (int)(DateTime.Now - m_TempBox.dateTimeSpendStart).TotalSeconds;
		m_TempBox.dateTimeSpendStart = DateTime.Now;
		if (num < 600 && num >= 0)
		{
			dictionary["TIME_SPENT_IN_SECOND"] = num;
			dictionary["WHERE"] = AppEventCommonParameters.GetWhereSceneType(sceneTypeWhere);
			AppEventCommonParameters.SetParam(dictionary, ParamType.ORIENTATION);
			AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
			AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
			AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_PLAY_TYPE, gid);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
			AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
			if ((MonoSingleton<PlayerDataManager>.Instance.IsOnSoundBGM || MonoSingleton<PlayerDataManager>.Instance.IsOnSoundEffect)
                /* && AndroidNativeUtils.GetMediaVolume() > 0*/
                //@TODO NATIVE
                //&& AndroidNativeFunctions.GetTotalVolume() > 0
                )
            {
				dictionary["IS_SOUND_ON"] = "YES";
			}
			else
			{
				dictionary["IS_SOUND_ON"] = "NO";
			}
			SendAppEvent("TIME_SPENT_M_2", num, dictionary);
		}
	}

	public void SendAppEventPurchaseFunnelConversionRate(PurchaseTypeOfPopup popupType, PurchaseReachedStep step, int userCoinBefore, int listIndexByPosition, float priceDollar, int popupOpenDurationTimeSec)
	{
		ParamType[] paramList = new ParamType[8]
		{
			ParamType.PLAYER_UID,
			ParamType.PLATFORM,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_TIME_IN_SECONDS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		float value = (step == PurchaseReachedStep.Purchased) ? 1 : 0;
		dictionary["PURCHASE_STEP_REACHED"] = $"{(int)(step + 1)}. {GetLogTypeString(step.ToString())}";
		dictionary["TYPE_OF_POPUP"] = GetLogTypeString(popupType.ToString());
		AppEventCommonParameters.SetParam(dictionary, ParamType.WHERE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.NEEDED_COINS_FOR_WHAT);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		dictionary["TIME_ELAPSED_SINCE_POPUP_OPENED"] = popupOpenDurationTimeSec;
		SendAppEvent("PURCHASE_FUNNEL_CONVERSION_RATE_M_1", value, dictionary);
	}

	public void SendAppEventPurchaseFunnelPopupShown(PurchaseTypeOfPopup popupType)
	{
		m_TempBox.PurchaseFunnelStepElapsedTime = DateTime.Now;
		ParamType[] paramList = new ParamType[11]
		{
			ParamType.WHERE,
			ParamType.NEEDED_COINS_FOR_WHAT,
			ParamType.PLAYER_UID,
			ParamType.PLATFORM,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["TYPE_OF_POPUP"] = GetLogTypeString(popupType.ToString());
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		SendAppEvent("PURCHASE_FUNNEL_POPUP_SHOWN_M_1", null, dictionary);
	}

	public void SendAppEventPurchaseFunnelButtonClicked(PurchaseTypeOfPopup popupType, int listIndexByPosition, float priceDollar, int popupOpenDurationTimeSec)
	{
		m_TempBox.PurchaseFunnelStepElapsedTime = DateTime.Now;
		ParamType[] paramList = new ParamType[10]
		{
			ParamType.WHERE,
			ParamType.NEEDED_COINS_FOR_WHAT,
			ParamType.PLAYER_UID,
			ParamType.PLATFORM,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_TIME_IN_SECONDS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["TYPE_OF_POPUP"] = GetLogTypeString(popupType.ToString());
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		dictionary["TIME_ELAPSED_SINCE_LAST_STEP_IN_SEC"] = popupOpenDurationTimeSec;
		SendAppEvent("PURCHASE_FUNNEL_BUTTON_CLICKED_M_1", null, dictionary);
	}

	public void SendAppEventPurchaseFunnelCanceled(PurchaseTypeOfPopup popupType, int listIndexByPosition, float priceDollar, int popupOpenDurationTimeSec)
	{
		m_TempBox.PurchaseFunnelStepElapsedTime = DateTime.Now;
		ParamType[] paramList = new ParamType[10]
		{
			ParamType.WHERE,
			ParamType.NEEDED_COINS_FOR_WHAT,
			ParamType.PLAYER_UID,
			ParamType.PLATFORM,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_TIME_IN_SECONDS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["TYPE_OF_POPUP"] = GetLogTypeString(popupType.ToString());
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		dictionary["TIME_ELAPSED_SINCE_LAST_STEP_IN_SEC"] = popupOpenDurationTimeSec;
		SendAppEvent("PURCHASE_FUNNEL_CANCELED_M_1", null, dictionary);
	}

	public void SendAppEventPurchaseFunnelConfirmed(PurchaseTypeOfPopup popupType, int userCoinBefore, int listIndexByPosition, float priceDollar, int popupOpenDurationTimeSec)
	{
		m_TempBox.PurchaseFunnelStepElapsedTime = DateTime.Now;
		ParamType[] paramList = new ParamType[10]
		{
			ParamType.WHERE,
			ParamType.NEEDED_COINS_FOR_WHAT,
			ParamType.PLAYER_UID,
			ParamType.PLATFORM,
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_TIME_IN_SECONDS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["TYPE_OF_POPUP"] = GetLogTypeString(popupType.ToString());
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = userCoinBefore;
		dictionary["POSITION_IN_THE_LIST"] = listIndexByPosition;
		dictionary["PRICE_IN_DOLLAR"] = priceDollar;
		dictionary["TIME_ELAPSED_SINCE_LAST_STEP_IN_SEC"] = popupOpenDurationTimeSec;
		SendAppEvent("PURCHASE_FUNNEL_CONFIRMED_M_1", null, dictionary);
	}

	public void SendAppEventCreateSpecialBlock(int gid, bool isClearStage)
	{
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.BUFFS_APPLIED,
			ParamType.PLAYER_UID,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		ParamTypeIncGID[] paramGidList = new ParamTypeIncGID[5]
		{
			ParamTypeIncGID.STAGE_NUMBER,
			ParamTypeIncGID.LEVEL_NUMBER,
			ParamTypeIncGID.LEVEL_PLAY_TYPE,
			ParamTypeIncGID.LEVEL_GAME_MODE,
			ParamTypeIncGID.IS_THIS_LEVEL_EVER_CLEARED
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, paramGidList, gid);
		dictionary["DID_CLEAR"] = ((!isClearStage) ? "NO" : "YES");
		dictionary["NUMBER_OF_ITEMS_USED"] = m_TempBox.listUsedBoosterOrder.Count;
	}

	public void SendAppEventMISCWorldMapObjectClicked(int episodeNo, string clickedObjectName)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		dictionary["EPISODE_NUMBER"] = episodeNo;
		dictionary["OBJECT_NUMBER"] = clickedObjectName;
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		SendAppEvent("MISC_WORLD_MAP_OBJECT_CLICKED_M", null, dictionary);
	}

	public void SendAppEventMISCUnMatchedMove(GameFailResultReason resultReason, int gid)
	{
		if (m_TempBox.GameUnMatchedMoveBlockCount != 0)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["HOW_MANY_TIMES"] = m_TempBox.GameUnMatchedMoveBlockCount;
			switch (resultReason)
			{
			case GameFailResultReason.Clear:
				dictionary["FINISHED_LEVEL_AS"] = "Cleared";
				break;
			case GameFailResultReason.UserManualQuitButton:
				dictionary["FINISHED_LEVEL_AS"] = "Dropped";
				break;
			default:
				dictionary["FINISHED_LEVEL_AS"] = "Failed";
				break;
			}
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
			AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
			AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
			dictionary["USED_HOW_MANY_MOVES"] = m_TempBox.UsedMoveCount;
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_LOGIN);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
			SendAppEvent("MISC_PERFORM_UNMATCHED_MOVE_M", m_TempBox.GameUnMatchedMoveBlockCount, dictionary);
		}
	}

	public void SendAppEventShuffleOccurred(int gid, bool isClearStage, int shuffleCount)
	{
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.BUFFS_APPLIED,
			ParamType.PLAYER_UID,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		ParamTypeIncGID[] paramGidList = new ParamTypeIncGID[5]
		{
			ParamTypeIncGID.STAGE_NUMBER,
			ParamTypeIncGID.LEVEL_NUMBER,
			ParamTypeIncGID.LEVEL_PLAY_TYPE,
			ParamTypeIncGID.LEVEL_GAME_MODE,
			ParamTypeIncGID.IS_THIS_LEVEL_EVER_CLEARED
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, paramGidList, gid);
		dictionary["HOW_MANY_TIMES"] = shuffleCount;
		dictionary["DID_CLEAR"] = ((!isClearStage) ? "NO" : "YES");
		dictionary["NUMBER_OF_ITEMS_USED"] = m_TempBox.listUsedBoosterOrder.Count;
		SendAppEvent("MISC_SHUFFLE_OCCURRED_M", shuffleCount, dictionary);
	}

	public void SendAppEventMISCUseIOSShortcutItemMenu(int shortCutIndex)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_PLAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_COUNT_FAIL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LIFE_REMAINS);
		string text = "N/A";
		switch (shortCutIndex)
		{
		default:
			return;
		case 1:
			text = "Play Now";
			break;
		case 2:
			text = "Message Center";
			break;
		case 3:
			text = "Star Ranking";
			break;
		}
		dictionary["SHORT_CUT_MENU_NAME"] = text;
		SendAppEvent("MISC_IOS_SHORTCUT_MENU_START_M", null, dictionary);
	}

	public void SendAppEventMISCExceptionGretelsTreeTutorial()
	{
		ParamType[] paramList = new ParamType[8]
		{
			ParamType.PLAYER_UID,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_COIN_BALANCE_BEFORE_EVENT,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.PLAYER_RESOURCE_REMAINS_WATER,
			ParamType.PLAYER_RESOURCE_REMAINS_STARCANDY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		SendAppEvent("MISC_TREE_TUTORIAL_PROBLEM_M", null, dictionary);
	}

	public void SendAppEventMISCUpdatePopup(bool DidPushUpdateButton)
	{
		int num = DidPushUpdateButton ? 1 : 0;
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLATFORM
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["IS_FORCED_UPDATE"] = ((!m_TempBox.forceUpdate) ? "NO" : "YES");
		dictionary["DID_PUSH_UPDATE_BUTTON"] = ((!DidPushUpdateButton) ? "NO" : "YES");
		m_TempBox.isOpenedUpdatePopup = false;
		SendAppEvent("MISC_UPDATE_POPUP_M", num, dictionary);
	}

	public void SendAppEventMISCUnexpectedRewardItem(int rewardItemIndex, ItemEarnedBy earnedBy)
	{
		ParamType[] paramList = new ParamType[7]
		{
			ParamType.PLAYER_UID,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLATFORM
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["WHAT_FEATURE"] = earnedBy.ToString();
		dictionary["INDEX"] = rewardItemIndex.ToString();
		dictionary["SPECIFIC_APP_VERSION"] = GlobalSetting.ConfigData.AppVersion;
		SendAppEvent("MISC_UNEXPECTED_REWARD_M", null, dictionary);
	}

	public void SendAppEventMISCOfflinePlayOccured(int playOfflineCount)
	{
		ParamType[] paramList = new ParamType[6]
		{
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_UID,
			ParamType.PLATFORM
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["HOW_MANY_PLAYS_OFFINE"] = playOfflineCount;
		SendAppEvent("MISC_OFFLINE_PLAY_OCCURED_M", playOfflineCount, dictionary);
	}

	public void SendAppEventCookappsCrossPromotionConversionRate(bool isClose, string accessBy)
	{
		ParamType[] paramList = new ParamType[11]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLATFORM,
			ParamType.PLAYER_LOGGED_IN_AS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		dictionary["GAME_NAME"] = CM_main.instance.TargetProjectCode;
		dictionary["ACCESSED_BY"] = accessBy;
		dictionary["CROSSPROMOTION_STEP_REACHED"] = ((!isClose) ? "2.Clicked Download Button" : "1. Popup Opened");
		float value = (!isClose) ? 1 : 0;
		SendAppEvent("CROSS_PROMOTION_CONVERSION_RATE_M", value, dictionary);
	}

	public void SendAppEventMISCUpdateSlideConversionRate(bool DidUpdate, string storeVersion)
	{
		ParamType[] paramList = new ParamType[7]
		{
			ParamType.PLAYER_UID,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLATFORM,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.DID_PLAYER_EVER_PAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["UPDATED_COMPLETED_STEP_REACHED"] = ((!DidUpdate) ? "1. Update Slide Shown" : "2. Push Update Button");
		dictionary["VERSION_MOBILE_LATEST"] = storeVersion;
		float value = DidUpdate ? 1 : 0;
		SendAppEvent("MISC_UPDATE_SLIDE_CONVERSION_RATE_M", value, dictionary);
	}

	public void SendAppEventMISCTryToQuitByBackButton()
	{
		ParamType[] paramList = new ParamType[13]
		{
			ParamType.PLAYER_UID,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR,
			ParamType.PLAYER_TOTAL_PURCHASE_COUNT,
			ParamType.SESSION_TIME_IN_MINUTES,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		SendAppEvent("MISC_TRY_TO_QUIT_BY_BACK_BUTTON_M", null, dictionary);
	}

	public void SendAppEventMISCPlayerRespondToQuitPopup(string respond)
	{
		ParamType[] paramList = new ParamType[13]
		{
			ParamType.PLAYER_UID,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR,
			ParamType.PLAYER_TOTAL_PURCHASE_COUNT,
			ParamType.SESSION_TIME_IN_MINUTES,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["PLAYER_RESPONSE"] = respond;
		int num = 0;
		if (respond == "Quit" || respond == "Back Button")
		{
			num = 1;
		}
		SendAppEvent("MISC_PLAYER_RESPOND_TO_QUIT_POPUP_M", num, dictionary);
	}

	private string GetLogTypeString(string enumString)
	{
		if (string.Compare(enumString, "NULL") == 0)
		{
			return "N/A";
		}
		string text = enumString.Replace('_', ' ');
		text = text.Replace(" x ", ":");
		text = text.Replace(" y ", "-");
		text = text.Replace(" z ", "+");
		if (enumString == FacebookLoginFromWhere.Auto_Popup__x__Cleared_Level_.ToString())
		{
			return text + (MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo - 1);
		}
		return text;
	}

	public string GetLevelPlayType(int gid)
	{
		if (gid <= 0)
		{
			return GetLogTypeString(LevelPlayType.NULL.ToString());
		}
		LevelPlayType levelPlayType = LevelPlayType.Etc;
		return GetLogTypeString(LevelPlayType.Normal.ToString());
	}

	public string GetLevelPlayTypeWithoutMapDataMain(int gid)
	{
		if (gid <= 0)
		{
			return GetLogTypeString(LevelPlayType.NULL.ToString());
		}
		MapData mapData = null;
		if (MapData.main != null)
		{
			mapData = MapData.main;
		}
		else
		{
			mapData = new MapData(gid);
		}
		LevelPlayType levelPlayType = LevelPlayType.Etc;
		return GetLogTypeString(LevelPlayType.Normal.ToString());
	}

	public void SendAppEventPushRemoteNoti(string pushId, string title, string message, Dictionary<string, string> dict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["PUSH_ID"] = ((!string.IsNullOrEmpty(pushId)) ? pushId : "N/A");
		dictionary["PUSH_TITLE"] = title;
		dictionary["PUSH_MESSAGE"] = message;
		foreach (string key in dict.Keys)
		{
			dictionary["PUSH_JSON_PARAM_" + key.ToUpper()] = dict[key];
		}
		dictionary["SESSION_STARTED_AS"] = GetLogTypeString(m_TempBox.remotePushAs.ToString());
		if (m_TempBox.remotePushAs == SessionStartedAs.Resumed)
		{
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
			AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LOGGED_IN_AS);
			AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		}
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END);
		SendAppEvent("PUSH_NOTI_GLOBAL_OPENED_M", null, dictionary);
	}

	public void SendAppEventPushNoti(int index, string message)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["PUSH_ID"] = GetLocalNotiPushID(index);
		dictionary["PUSH_MESSAGE"] = message;
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_PURCHASE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_TOTAL_PURCHASE_COUNT);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LOGGED_IN_AS);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLATFORM);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END);
		SendAppEvent("PUSH_NOTI_LOCAL_OPENED_M", null, dictionary);
	}

	private string GetLocalNotiPushID(int index)
	{
		string result = string.Empty;
		switch (index)
		{
		case 1:
			result = "RETENTION_D00";
			break;
		case 2:
			result = "RETENTION_D01";
			break;
		case 3:
			result = "RETENTION_D02";
			break;
		case 4:
			result = "RETENTION_D03";
			break;
		case 6:
			result = "LIFE_RESTORED_1";
			break;
		case 7:
			result = "LIFE_LEFT_INBOX";
			break;
		case 8:
			result = "LIFE_RESTORED_FULL";
			break;
		}
		if (index >= 104 && index <= 128)
		{
			result = $"RETENTION_D{index - 100:D2}";
		}
		else if (index >= 200 && index < 300)
		{
			result = "ENGAGEMENT_LUNCH_TIME";
		}
		else if (index >= 300 && index < 400)
		{
			result = "ENGAGEMENT_DINNER_TIME";
		}
		else if (index >= 400 && index < 500)
		{
			result = "ENGAGEMENT_BED_TIME";
		}
		return result;
	}

	public void SendAppEvent(string appEventName, float? valueToSum = default(float?), Dictionary<string, object> parameters = null)
	{
#if FACEBOOK_SDK
		if (!isCheater && !MapToolManager.IsMapTool && FB.IsInitialized)
		{
			if (parameters != null)
			{
				parameters["VERSION_MOBILE_CLIENT"] = GlobalSetting.ConfigData.AppContentVersion;
			}
			FB.LogAppEvent(appEventName, valueToSum, parameters);
		}
#endif
	}

	private static void SendAppEventToCookappsGamesLogger(string eventName, Dictionary<string, object> dicParam, float? valueToSum = default(float?))
	{
	}

	private static void SendAppEventToCookappsGamesLogger(Dictionary<string, string> formatForSending)
	{
	}

	private void CheckCheater()
	{
		isCheater = false;
		for (int i = 0; i < MonoSingleton<PlayerDataManager>.Instance.BoosterCount.Length; i++)
		{
			if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[i] >= 1000)
			{
				isCheater = true;
			}
		}
	}

	public void SendAppEventAdCompleted(int beforeLife, int beforeCoin, AdRewardType adRewardType, int rewardCount, AdAccessedBy adAccessedBy)
	{
		ParamType[] paramList = new ParamType[13]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END,
			ParamType.WHERE,
			ParamType.PLATFORM,
			ParamType.PLAYER_LOGGED_IN_AS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = beforeCoin;
		if (adRewardType == AdRewardType.Package || adRewardType == AdRewardType.Help || adRewardType == AdRewardType.OneCoin)
		{
			dictionary["SHOWN_TO_WHOM"] = "ETC";
		}
		List<string> list = new List<string>();
		string value = "Etc";
		string value2 = string.Empty;
		switch (adRewardType)
		{
		case AdRewardType.Life:
		case AdRewardType.MessageLife:
			value = "Life";
			value2 = "Life +" + rewardCount;
			break;
		case AdRewardType.BonusLevelKey:
			value = "BounsKey";
			value2 = "BounsKey +" + rewardCount;
			break;
		case AdRewardType.Coin:
			value = "Coin";
			value2 = "Coin +" + rewardCount;
			break;
		case AdRewardType.ViralTime:
			value = "Time";
			value2 = "Time -10:00 ";
			break;
		case AdRewardType.OneCoin:
			value = "OneCoinRetry";
			value2 = "OneCoinRetry";
			break;
		case AdRewardType.Help:
			value = "None";
			value2 = "None";
			break;
		case AdRewardType.Star:
			value = "Star, Cookie";
			value2 = "Star +1, Cookie +10";
			break;
		case AdRewardType.SpecificTime:
			value = "Time";
			value2 = "Time +5:00:00";
			break;
		}
		dictionary["ACCESSED_BY"] = GetLogTypeString(adAccessedBy.ToString());
#if ENABLE_ANTI_CHEAT
		dictionary["DID_PLAYER_EVER_WATCH_AD"] = ((!ObscuredPrefs.GetBool("DID_PLAYER_EVER_WATCH_AD", defaultValue: false)) ? "NO" : "YES");
#else
		dictionary["DID_PLAYER_EVER_WATCH_AD"] = ((!CPlayerPrefs.GetBool("DID_PLAYER_EVER_WATCH_AD", defaultValue: false)) ? "NO" : "YES");
#endif
        dictionary["AD_REWARD_TYPE"] = value;
		dictionary["AD_REWARD_TYPE_SPECIFIC"] = value2;
		SendAppEvent("REWARD_AD_WATCHING_COMPLETED_M_7", null, dictionary);
	}

	public void SendAppEventAdWatchingFunnel(AdCompletedStepReached step, int popupOpenDurationTimeSec, int beforeLife, int beforeCoin, AdRewardType adRewardType = AdRewardType.Life, int rewardCount = 1, AdAccessedBy adAccessedBy = AdAccessedBy.Life_Icon_from_Lobby)
	{
		ParamType[] paramList = new ParamType[13]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END,
			ParamType.WHERE,
			ParamType.PLATFORM,
			ParamType.PLAYER_LOGGED_IN_AS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		if (adRewardType == AdRewardType.Package || adRewardType == AdRewardType.Help || adRewardType == AdRewardType.OneCoin)
		{
			dictionary["SHOWN_TO_WHOM"] = "ETC";
		}
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = beforeCoin;
		dictionary["TIME_ELAPSED_SINCE_POPUP_OPENED"] = popupOpenDurationTimeSec;
		string value = "Etc";
		string value2 = string.Empty;
		List<string> list = new List<string>();
		switch (adRewardType)
		{
		case AdRewardType.Life:
		case AdRewardType.MessageLife:
			value = "Life";
			value2 = "Life +" + rewardCount;
			break;
		case AdRewardType.BonusLevelKey:
			value = "BounsKey";
			value2 = "BounsKey +" + rewardCount;
			break;
		case AdRewardType.Coin:
			value = "Coin";
			value2 = "Coin +" + rewardCount;
			break;
		case AdRewardType.ViralTime:
			value = "Time";
			value2 = "Time -10:00 ";
			break;
		case AdRewardType.OneCoin:
			value = "OneCoinRetry";
			value2 = "OneCoinRetry";
			break;
		case AdRewardType.Help:
			value = "None";
			value2 = "None";
			break;
		case AdRewardType.Star:
			value = "Star, Cookie";
			value2 = "Star +1, Cookie +10";
			break;
		case AdRewardType.SpecificTime:
			value = "Time";
			value2 = "Time +5:00:00";
			break;
		}
#if ENABLE_ANTI_CHEAT
		dictionary["DID_PLAYER_EVER_WATCH_AD"] = ((!ObscuredPrefs.GetBool("DID_PLAYER_EVER_WATCH_AD", defaultValue: false)) ? "NO" : "YES");
#else
        dictionary["DID_PLAYER_EVER_WATCH_AD"] = ((!CPlayerPrefs.GetBool("DID_PLAYER_EVER_WATCH_AD", defaultValue: false)) ? "NO" : "YES");
#endif
        dictionary["ACCESSED_BY"] = GetLogTypeString(adAccessedBy.ToString());
		dictionary["AD_REWARD_TYPE"] = value;
		dictionary["AD_REWARD_TYPE_SPECIFIC"] = value2;
		dictionary["AD_COMPLETED_STEP_REACHED"] = $"{(int)step}. {GetLogTypeString(step.ToString())}";
		float value3 = (step == AdCompletedStepReached.Completed) ? 1 : 0;
		SendAppEvent("REWARD_AD_CONVERSION_RATE_M_7", value3, dictionary);
	}

	public void SendAppEventShowIntetstitialAD(AdAccessedBy adAccessedBy)
	{
		ParamType[] paramList = new ParamType[13]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.SESSION_COUNT_PLAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE,
			ParamType.PLAYER_HOURS_SINCE_LAST_SESSION_END,
			ParamType.PLAYER_DAYS_SINCE_LAST_SESSION_END,
			ParamType.WHERE,
			ParamType.PLATFORM,
			ParamType.PLAYER_LOGGED_IN_AS
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["ACCESSED_BY"] = GetLogTypeString(adAccessedBy.ToString());
		SendAppEvent("INTERSTITIAL_AD_WATCHING_M", null, dictionary);
	}

	public void SendAppEventPreBoosterUsed(int gid, int boosterTypeIndex)
	{
		ParamType[] paramList = new ParamType[5]
		{
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR,
			ParamType.BUFFS_APPLIED
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		string text = string.Empty;
		switch (boosterTypeIndex)
		{
		case 0:
			text += "+3 MOVE";
			break;
		case 1:
			text += "2*2 block +2";
			break;
		case 2:
			text += "3*3 block +2";
			break;
		}
		dictionary["BOOSTER_TYPE"] = text;
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		SendAppEvent("PREBOOSTER_USED", null, dictionary);
	}

	public void SendAppEventPreBoosterPurchased(int gid, string boosterType)
	{
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.PLAYER_LIFE_REMAINS,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_TOTAL_SPENT_IN_DOLLAR
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["BOOSTER_TYPE"] = boosterType;
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		SendAppEvent("PREBOOSTER_PURCHASED", null, dictionary);
	}

	public void SendAppEventPreBoosterBuyAndStock()
	{
		SendAppEvent("PREBOOSTER_BUY_AND_STOCK", 1f);
	}

	public void SendAppEventLevelRatingAnswer(int gid, bool isGood, int getStar)
	{
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PREVIOUSLY_FAIL_COUNT
		};
		float value = isGood ? 1 : 0;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		dictionary["HOW_MANY_STARS"] = getStar;
		dictionary["WHAT_BUTTON_CLICKED"] = ((!isGood) ? "BAD" : "GOOD");
		SendAppEvent("LEVEL_RATING_ANSWER_M", value, dictionary);
	}

	public void SendAppEventLevelRatingConversionRate(int gid, bool isRespond, int getStar)
	{
		ParamType[] paramList = new ParamType[4]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLAYER_DAYS_SINCE_INSTALL,
			ParamType.PREVIOUSLY_FAIL_COUNT
		};
		float value = isRespond ? 1 : 0;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.STAGE_NUMBER, gid);
		AppEventCommonParameters.SetParamIncGID(dictionary, ParamTypeIncGID.LEVEL_NUMBER, gid);
		dictionary["DID_PLAYER_RESPOND"] = ((!isRespond) ? "NO" : "YES");
		dictionary["HOW_MANY_STARS"] = getStar;
		SendAppEvent("LEVEL_RATING_CONVERSION_RATE_M", value, dictionary);
	}

	public void AppsFlyerAppEvent(string eventName, Dictionary<string, string> eventValue)
	{
#if APPSFLYER_SDK
        AppsFlyer.trackRichEvent(eventName, eventValue);
        string str = "********AppEvent : " + eventName + " { ";
        foreach (KeyValuePair<string, string> item in eventValue)
        {
            str += item.Key;
            str += ":";
            str += item.Value;
            str += ", ";
        }
#endif
    }

	public void SendAppEventDailySpinBonusCollected(bool isAutoPopup, int spinIndex, int iid, int rewardValue)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		float num = 0f;
		TimeSpan timeSpan = DateTime.Now - MonoSingleton<PlayerDataManager>.Instance.lastRecvDailyBonusDateTime;
		num = (float)timeSpan.TotalHours / 24f;
		dictionary["DAYS_SINCE_LAST_BONUS_COLLECTED"] = (int)timeSpan.TotalDays;
		dictionary["REWARD_INDEX"] = spinIndex;
		dictionary["REWARD_TYPE"] = AppEventCommonParameters.GetServerItemRewardType((ServerItemIndex)iid, forStepSpin: true);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE);
		AppEventCommonParameters.SetParam(dictionary, ParamType.DID_PLAYER_EVER_PAY);
		dictionary["ACCESSED_BY"] = ((!isAutoPopup) ? "Icon in Lobby" : "Automatic Popup");
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_DAYS_SINCE_INSTALL);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.TIME_STAMP_TIME_IN_A_DAY);
		AppEventCommonParameters.SetParam(dictionary, ParamType.PLAYER_UID);
		dictionary["PLAYER_COIN_BALANCE_BEFORE_EVENT"] = MonoSingleton<PlayerDataManager>.Instance.Coin;
		AppEventCommonParameters.SetParam(dictionary, ParamType.SESSION_TIME_IN_SECONDS);
		SendAppEvent("DAILY_SPIN_COLLECTED", num, dictionary);
	}

	public void SendAppEventRandomBoxResult(int howManyBoxesOpened)
	{
		ParamType[] paramList = new ParamType[7]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLATFORM,
			ParamType.SESSION_TIME_IN_MINUTES,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		dictionary["HOW_MANY_BOXES_OPENED"] = howManyBoxesOpened;
		float value = (howManyBoxesOpened == 2) ? 1 : 0;
		SendAppEvent("RANDOMBOX_CONVERSION_M", value, dictionary);
	}

	public void SendAppEventRandomBoxClosedByX()
	{
		ParamType[] paramList = new ParamType[9]
		{
			ParamType.PLAYER_UID,
			ParamType.DID_PLAYER_EVER_PAY,
			ParamType.PLATFORM,
			ParamType.SESSION_TIME_IN_MINUTES,
			ParamType.PLAYER_LOGGED_IN_AS,
			ParamType.TIME_STAMP_DAY,
			ParamType.TIME_STAMP_TIME_IN_A_DAY,
			ParamType.PLAYER_LEVEL_HIGHEST_PLAYABLE,
			ParamType.PLAYER_STAGE_HIGHEST_PLAYABLE
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AppEventCommonParameters.SetCommonParamList(dictionary, paramList, null, 0);
		SendAppEvent("RANDOMBOX_CLOSED_BY_X", null, dictionary);
	}
}
