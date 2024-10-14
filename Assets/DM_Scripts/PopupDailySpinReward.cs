
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

public class PopupDailySpinReward : Popup
{
	private static readonly int MaxItem = 8;

	private static readonly int RetryCoinCount = 5;

	private static float appEventPopupOpenTime;

	private static bool appEventRewardCollectablePopupOpenTime;

	private static bool appEventReceivedReward;

	public RectTransform BGRectTransform;

	public float duration = 5f;

	private bool enableButton = true;

	public Image[] ImageRewardItems = new Image[MaxItem];

	public GameObject[] ObjsSpinButton;

	private AudioSource loopSound;

	public int numOfRotation = 10;

	private bool onNetwork;

	private Vector2 resizePosMax = Vector2.zero;

	private Vector2 resizePosMin = Vector2.zero;

	public GameObject rouletteBody;

	public GameObject selectBar;

	public Text[] TextRewardItemValues = new Text[MaxItem];

	public Text TextRetryCoinCount;

	public GameObject objCloseButton;

	private Tweener tweenBody;

	private Tweener tweenSelectBar;

	private int selectedRewardItemDataIndex = -1;

	private dailySpinItemData[] itemData = new dailySpinItemData[8]
	{
		new dailySpinItemData(ServerItemIndex.Coin, 50, 1),
		new dailySpinItemData(ServerItemIndex.BoosterCandyPack, 1, 2),
		new dailySpinItemData(ServerItemIndex.Coin, 7, 120),
		new dailySpinItemData(ServerItemIndex.BoosterVBomb, 1, 30),
		new dailySpinItemData(ServerItemIndex.Coin, 4, 220),
		new dailySpinItemData(ServerItemIndex.BoosterHBomb, 1, 30),
		new dailySpinItemData(ServerItemIndex.Coin, 2, 517),
		new dailySpinItemData(ServerItemIndex.BoosterHammer, 1, 80)
	};

	public override void Start()
	{
		appEventPopupOpenTime = Time.realtimeSinceStartup;
		if (AppEventCommonParameters.IsDifferentDay(MonoSingleton<PlayerDataManager>.Instance.lastRecvDailyBonusDateTime))
		{
			appEventRewardCollectablePopupOpenTime = true;
			ObjsSpinButton[0].SetActive(value: true);
			ObjsSpinButton[1].SetActive(value: false);
			objCloseButton.SetActive(value: false);
		}
		else
		{
			appEventRewardCollectablePopupOpenTime = false;
			ObjsSpinButton[0].SetActive(value: false);
			ObjsSpinButton[1].SetActive(value: true);
			objCloseButton.SetActive(value: true);
		}
		for (int i = 0; i < MaxItem; i++)
		{
			ImageRewardItems[i].sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(itemData[i].rewardIndex);
			TextRewardItemValues[i].text = "x " + itemData[i].rewardCount;
		}
		TextRetryCoinCount.text = "x " + RetryCoinCount;
		base.Start();
	}

	public void OnPressSpin()
	{
		if (!enableButton)
		{
			return;
		}
		if (!AppEventCommonParameters.IsDifferentDay(MonoSingleton<PlayerDataManager>.Instance.lastRecvDailyBonusDateTime))
		{
			if (MonoSingleton<PlayerDataManager>.Instance.Coin < RetryCoinCount)
			{
				AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.DailySpin;
				AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
				MonoSingleton<PopupManager>.Instance.OpenPopupShopCoin();
				return;
			}
			MonoSingleton<PlayerDataManager>.Instance.DecreaseCoin(RetryCoinCount);
		}
		MonoSingleton<PopupManager>.Instance.DisableBackCloseEvent();
		MonoSingleton<PopupManager>.Instance.CurrentPopup.DisableBackKey = true;
		objCloseButton.SetActive(value: false);
		enableButton = false;
		DoRotateBoardTween();
	}

	private void EndSpin()
	{
		int num = 0;
		for (int i = 0; i < itemData.Length; i++)
		{
			num += itemData[i].prob;
		}
		int num2 = Random.Range(0, num);
		num = 0;
		for (int j = 0; j < itemData.Length; j++)
		{
			num += itemData[j].prob;
			if (num2 < num)
			{
				selectedRewardItemDataIndex = j;
				break;
			}
		}
		if ((bool)rouletteBody && (bool)selectBar)
		{
			rouletteBody.transform.localRotation = Quaternion.identity;
			selectBar.transform.localRotation = Quaternion.identity;
			Ease ease = Ease.OutCirc;
			rouletteBody.transform.DOLocalRotate(new Vector3(0f, 0f, 360 * numOfRotation + selectedRewardItemDataIndex * 45), duration, RotateMode.LocalAxisAdd).SetEase(ease).OnComplete(doSpinResult);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(selectBar.transform.DOLocalRotate(new Vector3(0f, 0f, -36f), 0.0625f, RotateMode.LocalAxisAdd).SetLoops(numOfRotation * 8 + selectedRewardItemDataIndex));
			sequence.SetEase(ease);
			sequence.Append(selectBar.transform.DOLocalRotate(new Vector3(0f, 0f, 36f), 0.0625f, RotateMode.LocalAxisAdd));
			sequence.Play();
			SoundSFX.Play("SR_Lobby_wheel_end");
		}
	}

	private void doSpinResult()
	{
		StartCoroutine(waitSpinResult());
	}

	private IEnumerator waitSpinResult()
	{
		yield return new WaitForSeconds(0.5f);
		appEventReceivedReward = true;
        MonoSingleton<AppEventManager>.Instance.SendAppEventDailySpinBonusCollected(AppEventManager.m_TempBox.AutoDailyRewardPopup, selectedRewardItemDataIndex + 1, (int)itemData[selectedRewardItemDataIndex].rewardIndex, itemData[selectedRewardItemDataIndex].rewardCount);
        MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(itemData[selectedRewardItemDataIndex].rewardIndex, itemData[selectedRewardItemDataIndex].rewardCount, AppEventManager.ItemEarnedBy.Daily_Spin_Bonus, -1, holdOnUpdateCoin: true);
        PopupRewardItems popupRewardItems = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupRewardItems, enableBackCloseButton: true, null, null, null, holdEventOK: false, holdEventNegative: false, isReserve: false, enableOverlapPopup: true) as PopupRewardItems;
		if ((bool)popupRewardItems)
		{
			popupRewardItems.SetData(itemData[selectedRewardItemDataIndex].rewardIndex, itemData[selectedRewardItemDataIndex].rewardCount);
		}
		if (AppEventCommonParameters.IsDifferentDay(MonoSingleton<PlayerDataManager>.Instance.lastRecvDailyBonusDateTime))
		{
			MonoSingleton<PlayerDataManager>.Instance.SetDailyBonusReceived();
		}
		ObjsSpinButton[0].SetActive(value: false);
		ObjsSpinButton[1].SetActive(value: true);
		objCloseButton.SetActive(value: true);
		enableButton = true;
		MonoSingleton<PopupManager>.Instance.CurrentPopup.DisableBackKey = false;
	}

	private void StopRouletteTween()
	{
		if (tweenBody != null && tweenBody.IsPlaying())
		{
			tweenBody.Kill();
		}
		if (tweenSelectBar != null && tweenSelectBar.IsPlaying())
		{
			tweenSelectBar.Kill();
		}
		if ((bool)loopSound)
		{
			loopSound.Stop();
		}
	}

	private void DoRotateBoardTween()
	{
		StopRouletteTween();
		int num = 8;
		rouletteBody.transform.localRotation = Quaternion.identity;
		tweenBody = rouletteBody.transform.DOLocalRotate(new Vector3(0f, 0f, 360 * num), 1f, RotateMode.LocalAxisAdd).OnComplete(EndSpin).SetEase(Ease.Linear)
			.SetDelay(0.01f);
		selectBar.transform.localRotation = Quaternion.identity;
		tweenSelectBar = selectBar.transform.DOLocalRotate(new Vector3(0f, 0f, -36f), 1f / (float)(num * 8)).SetLoops(num * 8, LoopType.Yoyo).SetEase(Ease.Linear)
			.SetDelay(0.01f);
		loopSound = SoundSFX.Play("SR_Lobby_wheel_loop");
	}

	private void Update()
	{
		resizePosMin = BGRectTransform.offsetMin;
		resizePosMax = BGRectTransform.offsetMax;
		if (Screen.width > Screen.height)
		{
			resizePosMin.x = 0f;
			resizePosMax.x = 0f;
		}
		else
		{
			resizePosMin.x = -100f;
			resizePosMax.x = 100f;
		}
		BGRectTransform.offsetMin = resizePosMin;
		BGRectTransform.offsetMax = resizePosMax;
	}

	public override void OnEventClose()
	{
		base.OnEventClose();
		//if (MonoSingleton<PlayerDataManager>.Instance.PayCount == 0 && MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo >= 50)
		//{
		//	MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupStarterPack);
		//}
		PeriodEventData.CheckAndOpenPopup();
	}
}
