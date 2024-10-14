using System;

[Serializable]
public class ReqPacketBase : PacketBase
{
	public string device;

	public string device_id;

	public string userversion;

	public string signed_key;

	public string access_token;

	public int order;

	public int zip;

	public ReqPacketBase()
	{
		device = NetCommonData.GetStoreMarket();
		device_id = PlayerDataManager.GetDeviceID();
		userversion = GlobalSetting.ConfigData.AppVersionNumber.ToString();
		//if (FacebookManager.fbAccessToken == null)
		//{
		//	access_token = string.Empty;
		//}
		//else
		//{
		//	access_token = FacebookManager.fbAccessToken;
		//}
	}
}
