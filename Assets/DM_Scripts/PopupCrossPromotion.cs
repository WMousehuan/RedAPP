
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class PopupCrossPromotion : PopupCombination
{
	public RawImage ImageBanner;

	private bool isPressDownloadButton;

	public override void Start()
	{
		if (!CM_main.instance.isCrossPromotionSuceess || !CM_main.instance.CM_LageBanner)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ImageBanner.texture = CM_main.instance.CM_LageBanner;
		base.Start();
	}

	public override void OnEventClose()
	{
		if (!isPressDownloadButton)
		{
			MonoSingleton<AppEventManager>.Instance.SendAppEventCookappsCrossPromotionConversionRate(isClose: true, "Corsspromotion Icon");
		}
		base.OnEventClose();
	}

	public override void OnEventOK()
	{
		isPressDownloadButton = true;
		MonoSingleton<AppEventManager>.Instance.SendAppEventCookappsCrossPromotionConversionRate(isClose: false, "Corsspromotion Icon");
		base.OnEventOK();
		Application.OpenURL(CM_main.instance.TargetAppUrl);
	}
}
