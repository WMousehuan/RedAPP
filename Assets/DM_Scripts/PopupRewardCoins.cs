
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupRewardCoins : Popup
{
	

	public int RewardCount = 10;
    
	

	public override void Start()
	{
		base.Start();
		SoundSFX.Play(SFXIndex.DailyBonusGet);
		MonoSingleton<UIOverlayEffectManager>.Instance.ShowEffectRibbonFireworks();
		
			MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(base.transform, new Vector2(0f, 100f), Coins, 10);
		
	}


    void Coins()
    {
		MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(10);
	}


	

	public override void OnEventClose()
	{
		base.OnEventClose();
		UIManager.holdOnUpdateCoin = false;
	}

	public void OnPressClaim()
	{
		OnEventClose();
	}

	public void OnClickClaim()
	{
		//MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(base.transform, new Vector2(0f, 100f), null, 10);
		//MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(10);
		base.OnEventClose();
	}


	
}
