
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupCombination : Popup
{
	[Tooltip("Desc Key")]
	public string keyTextDesc;

	[Tooltip("Title Key")]
	public string keyTextTitle;

	public bool showBGImage;

	public bool showCloseButton = true;

	[Tooltip("Popup Desc String. if keyTextDesc == null && TextDesc != null then,")]
	public string strDesc;

	[Space(10f)]
	[Tooltip("Popup TItle String. if keyTextTitle == null then,")]
	public string strTitle;

	public Text TextDesc;

	private void Awake()
	{
		base.gameObject.SetActive(value: true);
	}

	public override void Start()
	{
		GameObject prefabCombinationBG = MonoSingleton<PopupManager>.Instance.PrefabCombinationBG;
		if ((bool)prefabCombinationBG)
		{
			GameObject gameObject = Object.Instantiate(prefabCombinationBG);
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.SetAsFirstSibling();
			Button componentInChildren = gameObject.GetComponentInChildren<Button>();
			if ((bool)componentInChildren)
			{
				if (!showCloseButton)
				{
					componentInChildren.gameObject.SetActive(value: false);
					MonoSingleton<PopupManager>.Instance.DisableBackCloseEvent();
				}
				else
				{
					componentInChildren.onClick.AddListener(OnEventClose);
				}
				Vector3 localScale = componentInChildren.transform.localScale;
				componentInChildren.transform.localScale = Vector3.zero;
				componentInChildren.transform.DOScale(localScale, 0.3f).SetEase(Ease.OutBack).SetDelay((!ShowOpenCloseTweenEffect) ? (tweeningMaxDelay + totalListDelay + 0.1f) : (0.2f + (tweeningMaxDelay + totalListDelay + 0.1f)));
			}
			Text componentInChildren2 = gameObject.GetComponentInChildren<Text>();
			if ((bool)componentInChildren2)
			{
				if (!string.IsNullOrEmpty(keyTextTitle))
				{
					MonoSingleton<ServerDataTable>.Instance.SetLangValue(componentInChildren2, keyTextTitle);
				}
				else if (!string.IsNullOrEmpty(strTitle))
				{
					componentInChildren2.text = strTitle;
				}
			}
		}
		if ((bool)TextDesc)
		{
			if (!string.IsNullOrEmpty(keyTextDesc))
			{
				MonoSingleton<ServerDataTable>.Instance.SetLangValue(TextDesc, keyTextDesc);
			}
			else if (!string.IsNullOrEmpty(strDesc))
			{
				TextDesc.text = strDesc;
			}
		}
		base.Start();
	}
}
