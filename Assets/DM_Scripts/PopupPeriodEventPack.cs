using System;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PopupPeriodEventPack : Popup, IAppEventPurchaseFunnel
{
	public PeriodEvent periodEventType;

	public float priceDollarForLog;

	public int discountRate;

	public ServerItemIndex[] rewardBoosterType;

	public int[] rewardBoosterCount;

	public int rewardCoinCount;

	public Image[] ImageRewardBoosterType;

	public Text[] TextRewardBoosterCount;

	public Text TextRewardCoinCount;

	public Text TextRemainTime;

	private AppEventManager.PurchaseReachedStep purchaseReachedStep;

	public Text TextOldPrice;

	public Text TextRatedPrice;

	public Text TextDiscountRate;

	//private PurchaseEventArgs purchaseEvent;

	private AppEventManager.PurchaseTypeOfPopup purchasePopupType;

	private AppEventManager.CoinPurchasedProductType coinPurchaseProductType;

//	private IAPManager.StoreProductType storeProductType;

	private DateTime endDateTime;

	public void SendAppEventPurchaseFunnel(int beforeCoin, AppEventManager.PurchaseReachedStep step)
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelConversionRate(purchasePopupType, step, beforeCoin, 0, priceDollarForLog, (int)(DateTime.Now - PopupOpenDateTime).TotalSeconds);
	}

	public override void Start()
	{
		if (periodEventType == PeriodEvent.Christmas)
		{
			purchasePopupType = AppEventManager.PurchaseTypeOfPopup.PeriodEventChristmasPack;
			coinPurchaseProductType = AppEventManager.CoinPurchasedProductType.PeriodEventChristmasPack;
			//storeProductType = IAPManager.StoreProductType.PeriodEventChristmasPack;
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
			float num = (float)product.metadata.localizedPrice / ((float)(100 - discountRate) / 100f);
			if (TextOldPrice != null)
			{
				TextOldPrice.text = product.metadata.isoCurrencyCode + " " + num;
			}
			TextRatedPrice.text = MonoSingleton<IAPManager>.Instance.GetPrettyLocalizedPriceString(product.metadata.localizedPriceString);
		}
#endif
		for (int i = 0; i < rewardBoosterType.Length; i++)
		{
			ImageRewardBoosterType[i].sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(rewardBoosterType[i]);
			TextRewardBoosterCount[i].text = "x" + rewardBoosterCount[i];
		}
		if ((bool)TextRewardCoinCount)
		{
			TextRewardCoinCount.text = "x" + rewardCoinCount;
		}
		if ((bool)TextDiscountRate)
		{
			TextDiscountRate.text = $"{discountRate}% off";
		}
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelPopupShown(purchasePopupType);
		endDateTime = PeriodEventData.GetPeriodEndDateTime(periodEventType);
	}

	private void Update()
	{
		if ((bool)TextRemainTime)
		{
			int time = Mathf.Max(0, (int)(endDateTime - DateTime.Now).TotalSeconds);
			TextRemainTime.text = Utils.GetTimeFormat(time);
		}
	}

	public void OnPressButtonBuy()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelButtonClicked(purchasePopupType, 0, priceDollarForLog, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
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
	}

	//public void OnPurchaseSuccess(PurchaseEventArgs e = null)
	//{
	//	purchaseEvent = e;
	//	MonoSingleton<UIManager>.Instance.HideLoading();
	//	int coin = MonoSingleton<PlayerDataManager>.Instance.Coin;
	//	for (int i = 0; i < rewardBoosterType.Length; i++)
	//	{
	//		MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(rewardBoosterType[i], rewardBoosterCount[i], AppEventManager.ItemEarnedBy.Package_Product);
	//	}
	//	if (rewardCoinCount > 0)
	//	{
	//		MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(ServerItemIndex.Coin, rewardCoinCount, AppEventManager.ItemEarnedBy.Package_Product);
	//	}
	//	AppEventManager.m_TempBox.coinProductType = coinPurchaseProductType;
	//	int getCoin = MonoSingleton<PlayerDataManager>.Instance.Coin - coin;
	//	int gid = (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType != SceneType.Game) ? (-1) : MapData.main.gid;
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventCoinPurchased(gid, coin, getCoin, 0, priceDollarForLog);
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchased(gid, coin, getCoin, 0, priceDollarForLog);
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventCoinEarned(gid, coin, getCoin, MonoSingleton<PlayerDataManager>.Instance.Coin, AppEventManager.ItemEarnedBy.Purchased_with_Real_Money);
	//	if (MonoSingleton<PlayerDataManager>.Instance.PayCount == 0)
	//	{
	//		MonoSingleton<AppEventManager>.Instance.SendAppEventFirstTimePurchase(gid, coin, getCoin, 0, priceDollarForLog, 0);
	//	}
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelConfirmed(purchasePopupType, coin, 0, priceDollarForLog, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
	//	MonoSingleton<PlayerDataManager>.Instance.PayCount++;
	//	purchaseReachedStep = AppEventManager.PurchaseReachedStep.Purchased;
	//	SendAppEventPurchaseFunnel(coin, AppEventManager.PurchaseReachedStep.Purchased);
	//	OnEventClose();
	//	PeriodEventData.SetBuyPeriodEventPack(periodEventType);
	//	MonoSingleton<PlayerDataManager>.Instance.SavePayInfo();
	//}

	//public void OnPurchaseFailed(PurchaseFailureReason reason = PurchaseFailureReason.Unknown)
	//{
	//	MonoSingleton<UIManager>.Instance.HideLoading();
	//	if (reason != PurchaseFailureReason.UserCancelled)
	//	{
	//		MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupInAppPurchaseFailed);
	//		return;
	//	}
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelCanceled(purchasePopupType, 0, priceDollarForLog, (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
	//	purchaseReachedStep = AppEventManager.PurchaseReachedStep.ClickedPurchaseButton;
	//}

	public override void OnEventClose()
	{
		base.OnEventClose();
		if (purchaseReachedStep != AppEventManager.PurchaseReachedStep.Purchased)
		{
			SendAppEventPurchaseFunnel(MonoSingleton<PlayerDataManager>.Instance.Coin, purchaseReachedStep);
		}
		PeriodEventData.OnEventClosePeriodEventPopup();
	}
}
