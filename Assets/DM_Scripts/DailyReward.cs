using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.DailyRewards.API;
using UnityEngine.UI;
using EasyUI.Toast;

public class DailyReward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Calendar.AddClickListener(CalendarButtonClicked);

    }

    private void CalendarButtonClicked(int dayNumber, int rewardValue, Sprite rewardSprite)
    {
        //        Debug.Log("Click " + dayNumber + " " + rewardValue);
        MonoSingleton<UIOverlayEffectManager>.Instance.ShowEffectRibbonFireworks();
        MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(rewardValue);
        Toast.Show("Successful Reward " + rewardValue + " coins!", 3f, ToastColor.Green);
        //reward += rewardValue;
        //UIRewardText.text = reward.ToString();
    }

    public void ShowRewardPannel()
    {
        SoundSFX.Play(SFXIndex.SlidePopupShow);
        Calendar.Show();
    }
}
