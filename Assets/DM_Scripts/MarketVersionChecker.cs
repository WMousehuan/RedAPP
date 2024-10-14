using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class MarketVersionChecker : MonoBehaviour
{
	public enum VersionState
	{
		None,
		OldVersion,
		EqualMarketVersion
	}

	public static string appBundleId;

	public static string currentVersion;

	private readonly string AndroidRegex = "Current Version</div><span[^>]*?><div><span[^>]*?>(.*?)</span></div>";

	private readonly string AndroidUrl = "https://play.google.com/store/apps/details?id=";

	private readonly string iOSRegex = "\"version\":\"(.*?)\",";

	private readonly string iOSUrl = "https://itunes.apple.com/lookup?bundleId=";

	private string storeVersion;

	private bool storeVersionReady;

	[HideInInspector]
	public VersionState CurrentVersionState;

	public void Initialize()
	{
		appBundleId = Application.identifier;
		currentVersion = GlobalSetting.ConfigData.AppVersion;
		StartCoroutine(ProcessGettingStoreVersion());
	}

	private IEnumerator ProcessGettingStoreVersion()
	{
		storeVersion = string.Empty;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string url = AndroidUrl;
		string versionRegex = AndroidRegex;
		WWW www = new WWW(url + appBundleId);
		yield return www;
		Regex regex = new Regex(versionRegex);
		Match v = regex.Match(www.text);
		if (v.Groups.Count > 1)
		{
			string version = v.Groups[1].ToString().Replace(" ", string.Empty);
			versionReady(version);
		}
	}

	private void versionReady(string version)
	{
		storeVersion = version;
		storeVersionReady = true;
		UpdateVersionState();
	}

	public string GetStoreVersion()
	{
		if (storeVersionReady)
		{
			return storeVersion;
		}
		return null;
	}

	private void UpdateVersionState()
	{
		CurrentVersionState = VersionState.None;
		if (!storeVersionReady)
		{
			return;
		}
		string[] array = currentVersion.Split('.');
		string[] array2 = storeVersion.Split('.');
		if (array.Length < 3 || array2.Length < 3)
		{
			return;
		}
		int[] array3 = new int[3];
		int[] array4 = new int[3];
		for (int i = 0; i < 3; i++)
		{
			array3[i] = 0;
			int.TryParse(array[i], out array3[i]);
			array4[i] = 0;
			int.TryParse(array2[i], out array4[i]);
		}
		for (int j = 0; j < 3; j++)
		{
			if (array3[j] > array4[j])
			{
				return;
			}
			if (array3[j] < array4[j])
			{
				CurrentVersionState = VersionState.OldVersion;
//				BannerUpdateNotification bannerUpdateNotification = MonoSingleton<BannerManager>.Instance.Open(BannerType.BannerUpdate) as BannerUpdateNotification;
				return;
			}
		}
		CurrentVersionState = VersionState.EqualMarketVersion;
	}
}
