public class NetCommonData
{
	public static string CurrentGameToken;

	public static string GetStoreMarket()
	{
		return GetStoreMarket(GlobalSetting.storeMarket);
	}

	public static string GetStoreMarket(StoreMarket storeMarket)
	{
		switch (storeMarket)
		{
		case StoreMarket.AmazonStore:
			return "amazon";
		case StoreMarket.GooglePlay:
			return "android";
		case StoreMarket.AppStore:
			return "ios";
		case StoreMarket.OneStore:
			return "onestore";
		default:
			return "android";
		}
	}
}
