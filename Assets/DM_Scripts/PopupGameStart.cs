
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameStart : Popup, IAppEventAdWatchingFunnel
{
	public enum PoupGameStartType
	{
		GameStart,
		GameRetry,
		GameResult
	}

	public RectTransform MainView;

	public Text TextLevel;

	[Header("Target")]
	public GameObject targetRoot;

	public Text TextGoal;

	public Text TextScore;

	[Header("Star")]
	public GameObject starRoot;

	public GameObject[] ObjsStar;

	public GameObject[] objStarIdleEffect;

	public GameObject[] objGetStarEffect;

	public GameObject[] objStarIdleEffectForAdd;

	public GameObject[] objGetStarEffectForAdd;

	public GameObject PrefabStarIdleEffect;

	public GameObject PrefabGetStarEffect;

	[Space(10f)]
	[Header("Buttons")]
	public GameObject buttonPlay;

	public GameObject groupNext;

	[Space(10f)]
	public PoupGameStartType m_Type;

	private int nStartLevel;

	private AudioSource GameClearLoopSound;

	public static AppEventManager.AdAccessedBy accessedBy = AppEventManager.AdAccessedBy.Levelball_from_Lobby;

	private AppEventManager.AdCompletedStepReached adCompletedStepReached;

	private AppEventManager.AdCompletedStepReached adCompletedStepReachedForGuestAds;

	private int beforeStarIndex;

	public void SendAppEventAdWatchingFunnel(int beforeLife, AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReached = step;
		MonoSingleton<AppEventManager>.Instance.SendAppEventAdWatchingFunnel(step, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds, beforeLife, MonoSingleton<PlayerDataManager>.Instance.Coin, AdRewardType.PlayWithItem, 1, AppEventManager.AdAccessedBy.Levelball_from_Lobby);
	}

	public void SetAdCompletedStepReached(AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReached = step;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
		{
			StartCoroutine(waitGetStarEffect(0.5f, 3));
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void OnEnable()
	{
		SetAdCompletedStepReachedForGuestBonus(AppEventManager.AdCompletedStepReached.None);
		base.transform.localScale = Vector3.one;
		base.OnEnable();
	}

	private void OnDestroy()
	{
		if ((bool)GameClearLoopSound)
		{
			GameClearLoopSound.Stop();
			GameClearLoopSound = null;
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		RemoveStarEffect();
		if ((bool)GameClearLoopSound)
		{
			GameClearLoopSound.Stop();
			GameClearLoopSound = null;
		}
	}

	public void SetPopupLevelStart(int level, GoalTarget target, int starCount, PoupGameStartType type, bool isHardLevel)
	{
		TextScore.text = string.Empty;
		m_Type = type;
		nStartLevel = level;
		if ((bool)TextLevel)
		{
			TextLevel.text = "Level " + level;
		}
		if (!isHardLevel)
		{
			switch (target)
			{
			case GoalTarget.NotAssign:
			case GoalTarget.Score:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, (!MapData.IsCollectMakeSpecial(level)) ? "Popup_LevelInfo_CollectMode" : "Popup_LevelInfo_CollectMakeSpecial");
				break;
			case GoalTarget.BringDown:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_BringDownMode");
				break;
			case GoalTarget.SweetRoad:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_SweetRoadMode");
				break;
			case GoalTarget.RescueVS:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_WitchDefeatMode");
				break;
			case GoalTarget.RescueMouse:
			case GoalTarget.RescueGingerMan:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_GingerRescueMode");
				break;
			case GoalTarget.Jelly:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_JellyMode");
				break;
			case GoalTarget.Digging:
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextGoal, "Popup_LevelInfo_DiggingMode");
				break;
			}
		}
		TurnOffAllObject();
		buttonPlay.SetActive(value: true);
		starRoot.SetActive(value: false);
	}

	public void SetPopupLevelSuccess(int score, int level, GoalTarget target, int starCount, bool isLogined, PoupGameStartType type)
	{
		AppEventManager.m_TempBox.stageClearedPopupClosedAction = AppEventManager.StageClearPopupClosedAction.Close;
		SetAdCompletedStepReached(AppEventManager.AdCompletedStepReached.None);
		MonoSingleton<UIManager>.Instance.HideCoinCurrentMenuLayer();
		m_Type = type;
		nStartLevel = level;
		if ((bool)TextLevel)
		{
			TextLevel.text = "Level " + level;
		}
		TextGoal.text = string.Empty;
		TextScore.text = string.Empty;
		TurnOffAllObject();
		starRoot.SetActive(value: true);
		groupNext.SetActive(value: true);
		StartCoroutine(GameClearScoreAnimation(score, isHardLevel: false));
		StartCoroutine(waitGetStarEffect(0.5f, starCount));
	}

	private IEnumerator waitPopupOpenCompletely(GameObject waitingObj)
	{
		waitingObj.SetActive(value: false);
		yield return new WaitForSeconds(tweenTimeToOpen);
		waitingObj.SetActive(value: true);
	}

	private IEnumerator GameClearScoreAnimation(int targetScore, bool isHardLevel)
	{
		int currentScore = 0;
		float lerpScoreTimeDuration = 0f;
		GameClearLoopSound = SoundSFX.Play(SFXIndex.GameClearPopupScoreCountLoop, loop: true);
		while (currentScore < targetScore)
		{
			currentScore = (int)Mathf.Lerp(0f, targetScore, lerpScoreTimeDuration);
			lerpScoreTimeDuration += Time.deltaTime * 0.8f;
			TextScore.text = Utils.GetCurrencyNumberString(currentScore);
			yield return null;
		}
		if ((bool)GameClearLoopSound)
		{
			GameClearLoopSound.Stop();
			GameClearLoopSound = null;
			SoundSFX.Play(SFXIndex.GameClearPopupScoreCountEnd);
		}
	}

	private void TurnOffAllObject()
	{
		buttonPlay.SetActive(value: false);
		groupNext.SetActive(value: false);
	}

	private void RemoveGetStarEffectForAdd()
	{
		if (objGetStarEffectForAdd != null && objGetStarEffectForAdd.Length > 0)
		{
			for (int i = 0; i < objGetStarEffectForAdd.Length; i++)
			{
				if ((bool)objGetStarEffectForAdd[i])
				{
					UnityEngine.Object.Destroy(objGetStarEffectForAdd[i]);
				}
			}
		}
		objGetStarEffectForAdd = null;
	}

	private void RemoveIdleStarEffectForAdd()
	{
		if (objStarIdleEffectForAdd != null && objStarIdleEffectForAdd.Length > 0)
		{
			for (int i = 0; i < objStarIdleEffectForAdd.Length; i++)
			{
				if ((bool)objStarIdleEffectForAdd[i])
				{
					UnityEngine.Object.Destroy(objStarIdleEffectForAdd[i]);
				}
			}
		}
		objStarIdleEffectForAdd = null;
	}

	private IEnumerator waitIdleStarEffectForAdd(float delay, int starCount)
	{
		yield return new WaitForSeconds(delay);
		CreateStarIdleEffectForAdd(starCount);
	}

	private void CreateStarIdleEffectForAdd(int starCount)
	{
		RemoveIdleStarEffectForAdd();
		RemoveGetStarEffectForAdd();
		objStarIdleEffectForAdd = new GameObject[starCount];
		objGetStarEffectForAdd = new GameObject[starCount];
		for (int i = 0; i < ObjsStar.Length; i++)
		{
			if (i < starCount && i > beforeStarIndex)
			{
				ObjsStar[i].SetActive(value: true);
				objStarIdleEffectForAdd[i] = UnityEngine.Object.Instantiate(PrefabStarIdleEffect);
				objStarIdleEffectForAdd[i].transform.SetParent(ObjsStar[i].transform, worldPositionStays: false);
				objStarIdleEffectForAdd[i].transform.localPosition = Vector3.zero;
			}
			else if (i == beforeStarIndex)
			{
				ObjsStar[i].SetActive(value: true);
			}
			else
			{
				ObjsStar[i].SetActive(value: false);
			}
		}
	}

	private IEnumerator waitGetStarEffectForAdd(float delay, int starCount)
	{
		yield return new WaitForSeconds(delay);
		RemoveIdleStarEffectForAdd();
		RemoveGetStarEffectForAdd();
		objGetStarEffectForAdd = new GameObject[starCount];
		objStarIdleEffectForAdd = new GameObject[starCount];
		for (int i = 0; i < ObjsStar.Length; i++)
		{
			if (i < starCount && i > beforeStarIndex)
			{
				ObjsStar[i].SetActive(value: true);
				ObjsStar[i].GetComponent<Image>().enabled = false;
				if (objGetStarEffectForAdd != null)
				{
					objGetStarEffectForAdd[i] = UnityEngine.Object.Instantiate(PrefabGetStarEffect);
					objGetStarEffectForAdd[i].transform.SetParent(ObjsStar[i].transform, worldPositionStays: false);
					objGetStarEffectForAdd[i].transform.localPosition = Vector3.zero;
					SoundSFX.Play((SFXIndex)(16 + i), loop: false, 0.25f);
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return new WaitForSeconds(0.667f);
		if (objGetStarEffectForAdd != null)
		{
			for (int j = 0; j < ObjsStar.Length; j++)
			{
				if (j < starCount && j > beforeStarIndex)
				{
					UnityEngine.Object.Destroy(objGetStarEffectForAdd[j]);
					ObjsStar[j].GetComponent<Image>().enabled = true;
				}
			}
		}
		StartCoroutine(waitIdleStarEffectForAdd(0.1f, starCount));
	}

	private void RemoveStarEffect()
	{
		for (int i = 0; i < ObjsStar.Length; i++)
		{
			ObjsStar[i].SetActive(value: false);
		}
		if (objStarIdleEffect != null && objStarIdleEffect.Length > 0)
		{
			for (int j = 0; j < objStarIdleEffect.Length; j++)
			{
				if ((bool)objStarIdleEffect[j])
				{
					UnityEngine.Object.Destroy(objStarIdleEffect[j]);
				}
			}
		}
		objStarIdleEffect = null;
		if (objGetStarEffect != null && objGetStarEffect.Length > 0)
		{
			for (int k = 0; k < objGetStarEffect.Length; k++)
			{
				if (objGetStarEffect[k] != null)
				{
					UnityEngine.Object.Destroy(objGetStarEffect[k]);
				}
			}
		}
		objGetStarEffect = null;
		RemoveIdleStarEffectForAdd();
		RemoveGetStarEffectForAdd();
	}

	private IEnumerator waitIdleStarEffect(float delay, int starCount)
	{
		yield return new WaitForSeconds(delay);
		CreateStarIdleEffect(starCount);
	}

	private void CreateStarIdleEffect(int starCount)
	{
		RemoveStarEffect();
		objStarIdleEffect = new GameObject[starCount];
		for (int i = 0; i < ObjsStar.Length; i++)
		{
			if (i < starCount)
			{
				ObjsStar[i].SetActive(value: true);
				objStarIdleEffect[i] = UnityEngine.Object.Instantiate(PrefabStarIdleEffect);
				objStarIdleEffect[i].transform.SetParent(ObjsStar[i].transform, worldPositionStays: false);
				objStarIdleEffect[i].transform.localPosition = Vector3.zero;
			}
			else
			{
				ObjsStar[i].SetActive(value: false);
			}
		}
	}

	private IEnumerator waitGetStarEffect(float delay, int starCount)
	{
		yield return new WaitForSeconds(delay);
		RemoveStarEffect();
		objGetStarEffect = new GameObject[starCount];
		for (int i = 0; i < ObjsStar.Length; i++)
		{
			if (i < starCount)
			{
				ObjsStar[i].SetActive(value: true);
				ObjsStar[i].GetComponent<Image>().enabled = false;
				objGetStarEffect[i] = UnityEngine.Object.Instantiate(PrefabGetStarEffect);
				objGetStarEffect[i].transform.SetParent(ObjsStar[i].transform, worldPositionStays: false);
				objGetStarEffect[i].transform.localPosition = Vector3.zero;
				SoundSFX.Play((SFXIndex)(16 + i), loop: false, 0.25f);
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return new WaitForSeconds(0.667f);
		for (int j = 0; j < ObjsStar.Length; j++)
		{
			if (j < starCount)
			{
				UnityEngine.Object.Destroy(objGetStarEffect[j]);
				ObjsStar[j].GetComponent<Image>().enabled = true;
			}
		}
		StartCoroutine(waitIdleStarEffect(0.1f, starCount));
	}

	public override void OnEventOK()
	{
		base.OnEventOK();
		if (m_Type == PoupGameStartType.GameResult)
		{
			if (AppEventManager.m_TempBox.stageClearedPopupClosedAction != AppEventManager.StageClearPopupClosedAction.Invite)
			{
				AppEventManager.m_TempBox.stageClearedPopupClosedAction = AppEventManager.StageClearPopupClosedAction.Next;
			}
			MonoSingleton<AppEventManager>.Instance.SendAppEventPopupLevelCleared();
//            FindObjectOfType<AdManager>().ShowAdmobRewardVideo();
        }
	}

	public override void OnEventClose()
	{
		if (m_Type == PoupGameStartType.GameResult && AppEventManager.m_TempBox.stageClearedPopupClosedAction != 0)
		{
			MonoSingleton<AppEventManager>.Instance.SendAppEventPopupLevelCleared();
		}
		base.OnEventClose();
		// FindObjectOfType<AdManager>().ShowAdmobInterstitial();
		//Advertisements.Instance.ShowInterstitial();
    }

	public void OnEventButtonGamePlay(bool forceStart = false)
	{
		eventClose = null;
		OnEventClose();
		MonoSingleton<PlayerDataManager>.Instance.lastPlayedLevel = nStartLevel;
		MapData.main = new MapData(nStartLevel);
		if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Lobby)
		{
			//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Game, SceneChangeEffect.Color);
			//GameMain.CompleteGameStart();
			//return;
		}
		MonoSingleton<PopupManager>.Instance.CloseAllPopup();
		MonoSingleton<SceneControlManager>.Instance.RemoveCurrentScene();
		//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Game, SceneChangeEffect.Color);
		GameMain.CompleteGameStart();
	}

	public void SetAdCompletedStepReachedForGuestBonus(AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReachedForGuestAds = step;
	}

	public void SendAppEventAdWatchingFunnelForGuestBonus(int beforeLife, AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReachedForGuestAds = step;
		MonoSingleton<AppEventManager>.Instance.SendAppEventAdWatchingFunnel(step, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds, beforeLife, MonoSingleton<PlayerDataManager>.Instance.Coin, AdRewardType.GuestBonus, 3, AppEventManager.AdAccessedBy.Guest_GameStartPopup_RankingUI);
	}

	public void SendAppEventAdWatchingFunnelForPreBooster(int beforeLife, AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReached = step;
		MonoSingleton<AppEventManager>.Instance.SendAppEventAdWatchingFunnel(step, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds, beforeLife, MonoSingleton<PlayerDataManager>.Instance.Coin, AdRewardType.PreBooster, 3, AppEventManager.AdAccessedBy.Levelball_from_Lobby);
	}

	public void SendAppEventAdWatchingFunnelForStar(int beforeLife, AppEventManager.AdCompletedStepReached step)
	{
		adCompletedStepReached = step;
		MonoSingleton<AppEventManager>.Instance.SendAppEventAdWatchingFunnel(step, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds, beforeLife, MonoSingleton<PlayerDataManager>.Instance.Coin, AdRewardType.Star, 3, AppEventManager.AdAccessedBy.Level_Result_Popup);
	}

	public void OnPressNextByAD()
	{
       // FindObjectOfType<AdManager>().ShowAdmobRewardVideo();
        GameMain.rewardMove3ByADStart = true;
				OnEventOK();
				MonoSingleton<AppEventManager>.Instance.SendAppEventAdCompleted(MonoSingleton<PlayerDataManager>.Instance.Coin, MonoSingleton<PlayerDataManager>.Instance.Coin, AdRewardType.StartMove3, 3, AppEventManager.AdAccessedBy.Level_Result_Popup);
		
	}
}
