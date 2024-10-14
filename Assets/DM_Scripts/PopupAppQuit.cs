
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class PopupAppQuit : PopupCombination
{
	public override void Start()
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventMISCTryToQuitByBackButton();
		base.Start();
	}

	public override void OnEventClose()
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventMISCPlayerRespondToQuitPopup("X button");
		base.OnEventClose();
	}

	public override void OnEventOK()
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventMISCPlayerRespondToQuitPopup("Quit");
		base.OnEventOK();
		if (MonoSingleton<ServerDataTable>.Instance.EnableAndroidAppQuitCrossPromotionPopup && CM_main.instance.isCrossPromotionSuceess && (bool)CM_main.instance.CM_LageBanner)
		{
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupAppQuitCrossPromotion);
		}
		else
		{
			Application.Quit();
		}
	}

	public override void OnEventNegative()
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventMISCPlayerRespondToQuitPopup("Keep Playing");
		base.OnEventNegative();
	}

	public void OnEventAppQuitByBackButton()
	{
		MonoSingleton<AppEventManager>.Instance.SendAppEventMISCPlayerRespondToQuitPopup("Back Button");
		if (MonoSingleton<ServerDataTable>.Instance.EnableAndroidAppQuitCrossPromotionPopup && CM_main.instance.isCrossPromotionSuceess && (bool)CM_main.instance.CM_LageBanner)
		{
			base.OnEventClose();
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupAppQuitCrossPromotion);
		}
		else
		{
			base.OnEventClose();
			Application.Quit();
		}
	}
}
