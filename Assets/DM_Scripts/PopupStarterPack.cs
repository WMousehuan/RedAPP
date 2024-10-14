using System;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PopupStarterPack : Popup, IAppEventPurchaseFunnel
{
	private readonly int packageIndex = 1;

	private AppEventManager.PurchaseReachedStep purchaseReachedStep;

	public Text textOldPrice;

	public Text textRatedPrice;

	public Text[] textRewardCount = new Text[3];

	//private PurchaseEventArgs purchaseEvent;

	private readonly ServerItemIndex[] rewardType = new ServerItemIndex[5]
	{
		ServerItemIndex.Coin,
		ServerItemIndex.BoosterHammer,
		ServerItemIndex.BoosterCandyPack,
		ServerItemIndex.BoosterHBomb,
		ServerItemIndex.BoosterVBomb
	};

	private readonly int[] rewardCount = new int[5]
	{
		20,
		1,
		1,
		1,
		1
	};

	public void SendAppEventPurchaseFunnel(int beforeCoin, AppEventManager.PurchaseReachedStep step)
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelConversionRate(AppEventManager.PurchaseTypeOfPopup.Starter_Pack_Popup, step, beforeCoin, 0, GetPriceDollar(), (int)(DateTime.Now - PopupOpenDateTime).TotalSeconds);
	}

	public override void Start()
	{
		AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.Null;
		purchaseReachedStep = AppEventManager.PurchaseReachedStep.PopupOpened;
		base.Start();
		MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: true);
#if ENABLE_IAP
		Product product = MonoSingleton<IAPManager>.Instance.GetProduct(5);
		if (product != null)
		{
			textRatedPrice.text = MonoSingleton<IAPManager>.Instance.GetPrettyLocalizedPriceString(product.metadata.localizedPriceString);
		}
#endif
		for (int i = 0; i < rewardCount.Length; i++)
		{
			textRewardCount[i].text = "x" + rewardCount[i];
		}
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelPopupShown(AppEventManager.PurchaseTypeOfPopup.Starter_Pack_Popup);
	}

	public void OnPressButtonBuy()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelButtonClicked(AppEventManager.PurchaseTypeOfPopup.Starter_Pack_Popup, 0, GetPriceDollar(), (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
#if ENABLE_IAP
		if (MonoSingleton<IAPManager>.Instance.BuyProduct(5, OnPurchaseSuccess, OnPurchaseFailed))
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
	//	for (int i = 0; i < rewardCount.Length; i++)
	//	{
	//		MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(rewardType[i], rewardCount[i], AppEventManager.ItemEarnedBy.Package_Product);
	//	}
	//	AppEventManager.m_TempBox.coinProductType = AppEventManager.CoinPurchasedProductType.Starter_Pack_1;
	//	int getCoin = MonoSingleton<PlayerDataManager>.Instance.Coin - coin;
	//	int gid = (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType != SceneType.Game) ? (-1) : MapData.main.gid;
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventCoinPurchased(gid, coin, getCoin, 0, GetPriceDollar());
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchased(gid, coin, getCoin, 0, GetPriceDollar());
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventCoinEarned(gid, coin, getCoin, MonoSingleton<PlayerDataManager>.Instance.Coin, AppEventManager.ItemEarnedBy.Purchased_with_Real_Money);
	//	if (MonoSingleton<PlayerDataManager>.Instance.PayCount == 0)
	//	{
	//		MonoSingleton<AppEventManager>.Instance.SendAppEventFirstTimePurchase(gid, coin, getCoin, 0, GetPriceDollar(), 0);
	//	}
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelConfirmed(AppEventManager.PurchaseTypeOfPopup.Starter_Pack_Popup, coin, 0, GetPriceDollar(), (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
	//	MonoSingleton<PlayerDataManager>.Instance.PayCount++;
	//	MonoSingleton<PlayerDataManager>.Instance.EnabledDoubleShopCoin = true;
	//	MonoSingleton<UIManager>.Instance.ShowDoubleShopCoinAlarm(show: true);
	//	purchaseReachedStep = AppEventManager.PurchaseReachedStep.Purchased;
	//	SendAppEventPurchaseFunnel(coin, AppEventManager.PurchaseReachedStep.Purchased);
	//	OnEventClose();
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
	//	MonoSingleton<AppEventManager>.Instance.SendAppEventPurchaseFunnelCanceled(AppEventManager.PurchaseTypeOfPopup.Starter_Pack_Popup, 0, GetPriceDollar(), (int)(DateTime.Now - AppEventManager.m_TempBox.PurchaseFunnelStepElapsedTime).TotalSeconds);
	//	purchaseReachedStep = AppEventManager.PurchaseReachedStep.ClickedPurchaseButton;
	//}

	public override void OnEventClose()
	{
		base.OnEventClose();
		MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: false);
		if (purchaseReachedStep != AppEventManager.PurchaseReachedStep.Purchased)
		{
			SendAppEventPurchaseFunnel(MonoSingleton<PlayerDataManager>.Instance.Coin, purchaseReachedStep);
		}
		if (MonoSingleton<PlayerDataManager>.Instance.EnabledDoubleShopCoin)
		{
			AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCoinDoubleShop);
		}
	}

	private float GetPriceDollar()
	{
		return 2.9f;
	}
}
