/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoosterShuffle : Booster
{
	public static int shuffleEffectType = 2;

	public GameObject freeObj;

	private int itemCount = 1;

	private int itemPrice = 3;

	public GameObject priceObj;

	private bool usingBooster;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Caching()
	{
		base.Caching();
		if (freeObj == null)
		{
			freeObj = base.transform.Find("item_free").gameObject;
		}
		if (priceObj == null)
		{
			GameObject gameObject = base.transform.Find("Button_buy_bg").gameObject;
			GameObject gameObject2 = base.transform.Find("PriceSetForPreBooster").gameObject;
			gameObject.SetActive(value: false);
			gameObject2.SetActive(value: false);
			priceObj = gameObject;
		}
	}

	public override void ForceStart()
	{
		boosterType = BoosterType.Shuffle;
		base.ForceStart();
		if (freeObj != null)
		{
			freeObj.SetActive(value: true);
		}
		if (priceObj != null)
		{
			priceObj.SetActive(value: true);
			Text component = priceObj.transform.Find("Text").GetComponent<Text>();
			component.text = "x 3";
		}
	}

	public override void UseBooster(bool isTutorial = false)
	{
		if (CheckEnableBoosterForShuffle(isTutorial))
		{
			base.UseBooster(isTutorial);
		}
	}

	public override void UpdateTextBoosterCount()
	{
		base.UpdateTextBoosterCount();
		if ((bool)freeObj)
		{
			freeObj.SetActive(value: false);
		}
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType] == 0)
		{
			if ((bool)priceObj)
			{
				priceObj.SetActive(value: true);
			}
			buttonNumberObj.SetActive(value: false);
		}
		else
		{
			if ((bool)priceObj)
			{
				priceObj.SetActive(value: false);
			}
			buttonNumberObj.SetActive(value: true);
		}
		buttonBuy.SetActive(value: false);
	}

	protected bool CheckEnableBoosterForShuffle(bool isTutorial = false)
	{
		if (!isTutorial)
		{
			if (!GameMain.main.CanINextTurn())
			{
				return false;
			}
			if (!GameMain.main.isPlaying)
			{
				return false;
			}
			if (!GameMain.main.CanIWait())
			{
				return false;
			}
			if (GameMain.main.isGameResult)
			{
				return false;
			}
			if (GameMain.main.CurrentTurn == VSTurn.CPU)
			{
				return false;
			}
			if (GameMain.main.isConnectedSweetRoad)
			{
				return false;
			}
			if (usingBooster)
			{
				return false;
			}
		}
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType] > 0)
		{
			StartCoroutine(UseBooster());
			return true;
		}
		if (MonoSingleton<IRVManager>.Instance.CurrentNetStatus == InternetReachabilityVerifier.Status.Offline)
		{
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupConnectionLost);
			return false;
		}
		OnPressBuyItem();
		return false;
	}

	public override void CancelBooster()
	{
		base.CancelBooster();
		ControlAssistant.main.ReleasePressedChip();
		onSelect = false;
		selectEffect.SetActive(value: false);
		guide.gameObject.SetActive(value: false);
	}

	private IEnumerator UseBooster()
	{
		usingBooster = true;
		UpdateTextBoosterCount();
		selectEffect.SetActive(value: true);
		guide.gameObject.SetActive(value: true);
		guide.TurnOnOnlyOneBoosterUI(uiIndex);
		guide.textGuide.text = string.Empty;
		guide.SetIconOff();
		guide.SetBackGroundOff();
		if ((bool)priceObj)
		{
			priceObj.SetActive(value: false);
		}
		yield return StartCoroutine(Utils.WaitFor(GameMain.main.CanIWait, 0.1f));
		GameMain.main.isPlaying = true;
		SoundSFX.Play(SFXIndex.Shuffle);
		yield return StartCoroutine(GameMain.main.ShuffleBooster());
		CompleteUseBooster();
		selectEffect.SetActive(value: false);
		guide.gameObject.SetActive(value: false);
		guide.SetIconOn();
		guide.SetBackGroundOn();
		UpdateTextBoosterCount();
		ControlAssistant.main.ReleasePressedChip();
		onSelect = false;
		usingBooster = false;
	}

	public void OnPressBuyItem()
	{
		if (MonoSingleton<PlayerDataManager>.Instance.Coin >= itemPrice)
		{
			Run();
			MonoSingleton<PlayerDataManager>.Instance.DecreaseCoin(itemPrice);
		}
		else
		{
			AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.InGameItem;
			AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
			MonoSingleton<PopupManager>.Instance.OpenPopupShopCoin();
		}
	}

	private void Run()
	{
		int boosterType = (int)base.boosterType;
		if (boosterType > 2)
		{
			boosterType = 2;
		}
		MonoSingleton<PlayerDataManager>.Instance.IncreaseBoosterData(base.boosterType, itemCount, AppEventManager.ItemEarnedBy.Purchased_with_Coins);
		SoundSFX.Play(SFXIndex.Shuffle);
		StartCoroutine(UseBooster());
	}
}
