/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class ItemsInGameStore : MonoBehaviour
{
	private Booster.BoosterType boosterType;

	public Button ButtonBuy;

	private int buyingItemListIndex;

	private int itemCount;

	private PacketDataSpecItemData itemData;

	private int itemPrice;

	public GameObject ObjLabelEffect;

	public GameObject[] ObjsBoosterIcon = new GameObject[5];

	public GameObject[] ObjsMark;

	public GameObject[] ObjsMarkBG;

	public Text TextCost;

	public Text TextCount;

	public Text TextDiscount;

	public void SetItem(PacketDataSpecItemData _itemData, Booster.BoosterType _boosterType, int _buyingItemListIndex)
	{
		buyingItemListIndex = _buyingItemListIndex;
		itemData = _itemData;
		boosterType = _boosterType;
		GameObject[] objsBoosterIcon = ObjsBoosterIcon;
		foreach (GameObject gameObject in objsBoosterIcon)
		{
			if ((bool)gameObject)
			{
				gameObject.SetActive(value: false);
			}
		}
		ObjsBoosterIcon[(int)boosterType].SetActive(value: true);
		GameObject[] objsMark = ObjsMark;
		foreach (GameObject gameObject2 in objsMark)
		{
			gameObject2.SetActive(value: false);
		}
		if (itemData.mark >= 2)
		{
			ObjsMark[itemData.mark - 2].SetActive(value: true);
		}
		GameObject[] objsMarkBG = ObjsMarkBG;
		foreach (GameObject gameObject3 in objsMarkBG)
		{
			gameObject3.SetActive(value: false);
		}
		if (itemData.mark >= 2)
		{
			ObjsMarkBG[itemData.mark - 2].SetActive(value: true);
			//ObjLabelEffect.SetActive(value: true);
		}
		if (itemData.sale_rate > 0)
		{
			TextDiscount.gameObject.SetActive(value: true);
			TextDiscount.text = $"{itemData.sale_rate}%\noff";
		}
		else
		{
			TextDiscount.gameObject.SetActive(value: false);
		}
		if (itemData.onsale == 1)
		{
			TextCount.text = itemData.sale_cnt.ToString();
			TextCost.text = itemData.sale_price.ToString();
		}
		else
		{
			TextCount.text ="x"+itemData.normal_cnt.ToString();
			TextCost.text = itemData.normal_price.ToString();
		}
	}

	public void OnPressBuyItem()
	{
		if (itemData == null)
		{
			return;
		}
		itemPrice = itemData.normal_price;
		itemCount = itemData.normal_cnt;
		if (itemData.onsale == 1)
		{
			itemPrice = itemData.sale_price;
			itemCount = itemData.sale_cnt;
		}
		if (MonoSingleton<PlayerDataManager>.Instance.Coin >= itemPrice)
		{
			int coin = MonoSingleton<PlayerDataManager>.Instance.Coin;
			string specific_type = $"{AppEventCommonParameters.GetBoosterName(boosterType)} Pack {buyingItemListIndex + 1} : {itemCount} Items";
			MonoSingleton<AppEventManager>.Instance.SendAppEventCoinConsumed(itemPrice, MapData.main.gid, MonoSingleton<PlayerDataManager>.Instance.Coin, MonoSingleton<PlayerDataManager>.Instance.Coin - itemPrice, AppEventManager.CoinCategory.InGameItem, specific_type, itemData.iid);
			MonoSingleton<PlayerDataManager>.Instance.DecreaseCoin(itemPrice);
			SpawnStringEffectType effectType;
			switch (boosterType)
			{
			case Booster.BoosterType.Hammer:
				effectType = SpawnStringEffectType.SuccessBuyMagicHammer;
				break;
			case Booster.BoosterType.CandyPack:
				effectType = SpawnStringEffectType.SuccessBuyCandyPack;
				break;
			case Booster.BoosterType.HBomb:
				effectType = SpawnStringEffectType.SuccessBuyBoosterHBomb;
				break;
			case Booster.BoosterType.VBomb:
				effectType = SpawnStringEffectType.SuccessBuyBoosterVBomb;
				break;
			default:
				effectType = SpawnStringEffectType.SuccessBuyMagicHammer;
				break;
			}
			CPanelGameUI.Instance.ThrowPurchasedBoosterItemEffect(effectType, (int)boosterType, itemCount, AppEventManager.ItemEarnedBy.Purchased_with_Coins);
			if (MonoSingleton<PopupManager>.Instance.CurrentPopupType == PopupType.PopupInGameItemStore)
			{
				MonoSingleton<PopupManager>.Instance.CurrentPopup.OnEventClose();
			}
			else
			{
				MonoSingleton<UIManager>.Instance.HideCoinCurrentMenuLayer();
			}
		}
		else
		{
			AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.InGameItem;
			AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
			MonoSingleton<PopupManager>.Instance.OpenPopupShopCoin();
		}
	}
}
