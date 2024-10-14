using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDownloadEpisode : MonoBehaviour
{
	public int currentDownloadBundleIndex;

	private string downloadBundleName = string.Empty;

	public Image ImageEpisodeIcon;

	[HideInInspector]
	public bool isCancel;

	[HideInInspector]
	public bool isDone;

	[HideInInspector]
	public bool isNetError;

	public GameObject ObjCloseButton;

	public GameObject ObjRetryButton;

	public Text TextEpisodeNo;

	public Text TextProgress;

	public Text TextMessage;

	private int totalDependencyFileCount = 1;

	public void SetData(int episodeNo, string bundleName, bool enableCloseButton = true)
	{
		downloadBundleName = bundleName;
		string langValue = MonoSingleton<ServerDataTable>.Instance.GetLangValue("EPISODE_NAME_" + episodeNo);
		if (string.IsNullOrEmpty(langValue))
		{
			TextEpisodeNo.text = "Episode " + episodeNo;
		}
		else
		{
			TextEpisodeNo.text = langValue;
		}
		//if (BundleManager.instance.Manifest != null)
		//{
		//	totalDependencyFileCount = 1;
		//	string[] allDependencies = BundleManager.instance.Manifest.GetAllDependencies(bundleName);
		//	if (allDependencies != null)
		//	{
		//		totalDependencyFileCount += allDependencies.Length;
		//	}
		//}
		ObjCloseButton.SetActive(enableCloseButton);
		SetStateStart();
		MonoSingleton<UIManager>.Instance.HideLoading();
		//StartCoroutine(DownloadAssetBundle());
	}

	public void SetProgress(float value)
	{
		TextProgress.text = $"{Mathf.Floor(value * 100f)} %";
		ImageEpisodeIcon.fillAmount = value;
		if (totalDependencyFileCount > 1)
		{
			TextProgress.text += $"({currentDownloadBundleIndex + 1}/{totalDependencyFileCount})";
		}
	}

	public void OnPressButtonCancel()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		StopAllCoroutines();
		isCancel = true;
		isDone = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnPressButtonRetry()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		StopAllCoroutines();
		//StartCoroutine(DownloadAssetBundle());
	}

	private void SetStateStart()
	{
		ObjRetryButton.SetActive(value: false);
		SetProgress(0f);
		MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextMessage, "Popup_ResoureceDown_Des1");
	}

	private void SetStateRetry()
	{
		ObjRetryButton.SetActive(value: true);
		MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextMessage, "Popup_ResoureceDown_Des2");
	}

	//private IEnumerator DownloadAssetBundle()
	//{
	//	while (!Caching.ready)
	//	{
	//		yield return null;
	//	}
	//	BundleManager.instance.uiDownloadEpsiode = this;
	//	yield return StartCoroutine(BundleManager.instance.LoadBundle(downloadBundleName));
	//	if (!BundleManager.instance.IsBundleLoaded(downloadBundleName))
	//	{
	//		SetStateRetry();
	//		yield break;
	//	}
	//	BundleManager.instance.uiDownloadEpsiode = null;
	//	yield return new WaitForSeconds(1f);
	//	isDone = true;
	//	UnityEngine.Object.Destroy(base.gameObject, 0.5f);
	//}
}
