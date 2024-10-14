using UnityEngine;

public class GlobalSetting
{
	public static ConfigJson ConfigData = new ConfigJson();

	public static string iOSAppID = "";

	public static readonly int FPS = 50;

	public static readonly int LOW_FPS = 30;

	public static readonly StoreMarket storeMarket = StoreMarket.GooglePlay;

	public static void LoadConfigData()
	{
		string path = "config.json.android";
		TextAsset textAsset = Resources.Load<TextAsset>(path);
		if (textAsset != null)
		{
			ConfigData = JsonUtility.FromJson<ConfigJson>(textAsset.text);
			string[] array = ConfigData.AppVersion.Split('.');
			ConfigData.AppVersionNumber = int.Parse(array[0]) * 100000 + int.Parse(array[1]) * 100 + int.Parse(array[2]);
			ConfigData.AppContentVersion = array[0] + "." + array[1];
			ConfigData.AppVersionCode = int.Parse(array[0]) * 100000;
		}
	}

	public static SystemLanguage GetSystemLanguage()
	{
		return Application.systemLanguage;
	}
}
