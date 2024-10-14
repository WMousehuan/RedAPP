using UnityEngine;

public class BannerUpdateNotification : Banner
{
	private readonly string strAndroidURL = "https://play.google.com/store/apps/details?id=com.cookapps.beagles.ng003";

	private readonly string strIOSUrl = "https://itunes.apple.com/app/candy-blast-chocolate-splash/id1361435055";

	private bool isSendedAppEvent;

	private string storeVersion;

	public override void CloseBanner()
	{
		base.CloseBanner();
	}

	public void PressGoToMarketButton()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.OpenURL(strIOSUrl);
		}
		else if (Application.platform == RuntimePlatform.Android && GlobalSetting.storeMarket == StoreMarket.GooglePlay)
		{
			Application.OpenURL(strAndroidURL);
		}
	}
}
