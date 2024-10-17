
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;
using Gley.MobileAds;

public class PopupRewardItems : Popup
{
	public Transform gridRoot;

	public Image[] ImageRewardItem;

	public GameObject[] items;

	private readonly ServerItemIndex[] rewardItemIndex = new ServerItemIndex[3];

	public Text[] TextRewardValue;

	public int[] RewardCount;

	private RewardItemData recvedRewardItemData;

	public override void Start()
	{
		base.Start();
		SoundSFX.Play(SFXIndex.DailyBonusGet);
		MonoSingleton<UIOverlayEffectManager>.Instance.ShowEffectRibbonFireworks();
		if (RewardCount.Length > 0 && rewardItemIndex[0] == ServerItemIndex.Coin)
		{
			MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(base.transform, new Vector2(0f, 100f), null, RewardCount[0]);
		}
	}

	private void SetGrid(int count = 1)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i < count)
			{
				items[i].SetActive(value: true);
			}
			else
			{
				items[i].SetActive(value: false);
			}
		}
		switch (count)
		{
		case 1:
		{
			Transform transform3 = gridRoot;
			Vector3 localPosition5 = gridRoot.localPosition;
			float y3 = localPosition5.y;
			Vector3 localPosition6 = gridRoot.localPosition;
			transform3.localPosition = new Vector3(0f, y3, localPosition6.z);
			break;
		}
		case 2:
		{
			Transform transform2 = gridRoot;
			Vector3 localPosition3 = gridRoot.localPosition;
			float y2 = localPosition3.y;
			Vector3 localPosition4 = gridRoot.localPosition;
			transform2.localPosition = new Vector3(-100f, y2, localPosition4.z);
			break;
		}
		case 3:
		{
			Transform transform = gridRoot;
			Vector3 localPosition = gridRoot.localPosition;
			float y = localPosition.y;
			Vector3 localPosition2 = gridRoot.localPosition;
			transform.localPosition = new Vector3(-200f, y, localPosition2.z);
			break;
		}
		}
	}

	public void SetData(int item_index, int value, int buffTimeSec = 0)
	{
		SetGrid();
		rewardItemIndex[0] = (ServerItemIndex)item_index;
		ImageRewardItem[0].sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(rewardItemIndex[0]);
		if (item_index == 7 || item_index == 8 || item_index == 1)
		{
			TextRewardValue[0].text = "x " + value;
		}
		else
		{
			TextRewardValue[0].transform.parent.gameObject.SetActive(value: false);
		}
	}

	public void SetData(ServerItemIndex item_index, int value = 1)
	{
		SetGrid();
		SetItem(0, item_index, value);
	}

	private void SetItem(int index, ServerItemIndex item_index, int value = 1)
	{
		rewardItemIndex[index] = item_index;
		ImageRewardItem[index].sprite = MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(rewardItemIndex[index]);
		TextRewardValue[index].text = PlayerDataManager.GetRewardCountValue(item_index, value);
		RewardCount[index] = value;
	}

	public void SetData(RewardItemData[] data)
	{
		int num = Mathf.Min(3, data.Length);
		recvedRewardItemData = data[0];
		SetGrid(num);
		for (int i = 0; i < num; i++)
		{
			SetItem(i, data[i].item_index, data[i].value);
		}
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

	public void OnPressDoubleRewardForWatchAD()
	{
		
		APIMobileAds.ShowRewardedVideo(CompleteMethod);
    }


	private void CompleteMethod(bool completed)
	{
		
		if (completed == true)
		{
			MonoSingleton<PlayerDataManager>.Instance.RewardServerItem(recvedRewardItemData.item_index, recvedRewardItemData.value, AppEventManager.ItemEarnedBy.Daily_Bonus_Double_Watch_AD, -1, holdOnUpdateCoin: true);
			base.OnEventClose();
			PopupRewardItems popupRewardItems = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupRewardItems, enableBackCloseButton: false) as PopupRewardItems;
			popupRewardItems.SetData(new RewardItemData[1]
			{
				recvedRewardItemData
			});
		}
		else
		{
#if UNITY_EDITOR
			MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(10);
			base.OnEventClose();
			PopupRewardItems popupRewardItems = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupRewardItems, enableBackCloseButton: true) as PopupRewardItems;
			//popupRewardItems.SetData(new RewardItemData[1]
			//{
			//	recvedRewardItemData
			//});
#endif
			Debug.Log("No Reward");
		}
	}

	
}
