
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PopupBoosterPack : Popup, IAppEventPurchaseFunnel
{
	public enum BoosterPackType
	{
		CandyPack,
		Hammer,
		HBomb,
		VBomb
	}

	public BoosterPackType boosterPackType;

	public float priceDollar;

	public int rewardBoosterCount;

	public int rewardCoinCount;

	public Image ImageRewardBoosterType;

	public Text TextRewardBoosterCount;

	public Text TextRewardCoinCount;

	private AppEventManager.PurchaseReachedStep purchaseReachedStep;

	public Text textOldPrice;

	public Text textRatedPrice;

	//private PurchaseEventArgs purchaseEvent;

	private AppEventManager.PurchaseTypeOfPopup purchasePopupType;

	private AppEventManager.CoinPurchasedProductType coinPurchaseProductType;

//	private IAPManager.StoreProductType storeProductType;

	private ServerItemIndex rewardBoosterType;

	public void SendAppEventPurchaseFunnel(int beforeCoin, AppEventManager.PurchaseReachedStep step)
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelConversionRate(purchasePopupType, step, beforeCoin, 0, priceDollar, (int)(DateTime.Now - PopupOpenDateTime).TotalSeconds);
	}

	public override void Start()
	{
		if (boosterPackType == BoosterPackType.CandyPack)
		{
			purchasePopupType = AppEventManager.PurchaseTypeOfPopup.BoosterPackBomb;
			coinPurchaseProductType = AppEventManager.CoinPurchasedProductType.BoosterPackBomb;
			//storeProductType = IAPManager.StoreProductType.BoosterPackBomb;
			rewardBoosterType = ServerItemIndex.BoosterCandyPack;
		}
		else if (boosterPackType == BoosterPackType.Hammer)
		{
			purchasePopupType = AppEventManager.PurchaseTypeOfPopup.BoosterPackHammer;
			coinPurchaseProductType = AppEventManager.CoinPurchasedProductType.BoosterPackHammer;
			//storeProductType = IAPManager.StoreProductType.BoosterPackHammer;
			rewardBoosterType = ServerItemIndex.BoosterHammer;
		}
		else if (boosterPackType == BoosterPackType.HBomb)
		{
			purchasePopupType = AppEventManager.PurchaseTypeOfPopup.BoosterPackHBomb;
			coinPurchaseProductType = AppEventManager.CoinPurchasedProductType.BoosterPackHBomb;
			//storeProductType = IAPManager.StoreProductType.BoosterPackHBomb;
			rewardBoosterType = ServerItemIndex.BoosterHBomb;
		}
		else if (boosterPackType == BoosterPackType.VBomb)
		{
			purchasePopupType = AppEventManager.PurchaseTypeOfPopup.BoosterPackVBomb;
			coinPurchaseProductType = AppEventManager.CoinPurchasedProductType.BoosterPackVBomb;
			//storeProductType = IAPManager.StoreProductType.BoosterPackVBomb;
			rewardBoosterType = ServerItemIndex.BoosterVBomb;
		}
		AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.Null;
		purchaseReachedStep = AppEventManager.PurchaseReachedStep.PopupOpened;
		base.Start();
		MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: true);
#if ENABLE_IAP
		Product product = MonoSingleton<IAPManager>.Instance.GetProduct((int)storeProductType);
		product = MonoSingleton<IAPManager>.Instance.GetProduct((int)storeProductType);
		if (product != null)
		{
			textRatedPrice.text = MonoSingleton<IAPManager>.Instance.GetPrettyLocalizedPriceString(product.metadata.localizedPriceString);
		}
#endif

		if ((bool)ImageRewardBoosterType)
		{
			ImageRewardBoosterType.sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(rewardBoosterType);
		}
		if ((bool)TextRewardBoosterCount)
		{
			TextRewardBoosterCount.text = rewardBoosterCount.ToString();
		}
		if ((bool)TextRewardCoinCount)
		{
			TextRewardCoinCount.text = rewardCoinCount.ToString();
		}
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelPopupShown(purchasePopupType);
	}

	public void OnPressButtonBuy()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelButtonClicked(purchasePopupType, 0, priceDollar, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
#if ENABLE_IAP
		if (MonoSingleton<IAPManager>.Instance.BuyProduct((int)storeProductType, OnPurchaseSuccess, OnPurchaseFailed))
		{
			MonoSingleton<UIManager>.Instance.ShowLoading();
		}
		else
#endif
		{
			MonoSingleton<UIManager>.Instance.HideLoading();
		}


		MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(rewardBoosterType, rewardBoosterCount, AppEventManager.ItemEarnedBy.Package_Product);
	}

	
	public override void OnEventClose()
	{
		base.OnEventClose();
		if (purchaseReachedStep != AppEventManager.PurchaseReachedStep.Purchased)
		{
			SendAppEventPurchaseFunnel(MonoSingleton<PlayerDataManager>.Instance.Coin, purchaseReachedStep);
		}
		MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: false);
		PopupInGameItemStore popupInGameItemStore = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupInGameItemStore) as PopupInGameItemStore;
		popupInGameItemStore.SetPopup(ServerDataTable.GetBoosterTypeFromServerItemIndex(rewardBoosterType));
	}
}
