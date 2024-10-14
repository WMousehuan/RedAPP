
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using dotmob;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameOver : Popup
{
	public RectTransform BGRectTransform;

	private PacketDataSpecItemData buyItem = new PacketDataSpecItemData();

	public GameObject ObjectMainView;

	public GameObject ObjNormalButton;

	public RectTransform PosButtonCancel;

	public GameObject PrefabCollect;

	public GameObject PrefabCollectTreasure;

	private Vector2 resizePosMax = Vector2.zero;

	private Vector2 resizePosMin = Vector2.zero;

	public Transform TargetList;

	public Text TextCostValue;

	private bool tweeningCompleted;

	public float tweenValueInTime = 2f;

	private void Update()
	{
		resizePosMin = BGRectTransform.offsetMin;
		resizePosMax = BGRectTransform.offsetMax;
		if (Screen.width > Screen.height)
		{
			resizePosMin.x = 128f;
			resizePosMax.x = -128f;
			PosButtonCancel.anchoredPosition = new Vector2(-23f, -18f);
		}
		else
		{
			resizePosMin.x = -40f;
			resizePosMax.x = 40f;
			PosButtonCancel.anchoredPosition = new Vector2(-80f, -14f);
		}
		BGRectTransform.offsetMin = resizePosMin;
		BGRectTransform.offsetMax = resizePosMax;
		if (tweeningCompleted)
		{
			if (Screen.width > Screen.height)
			{
				ObjectMainView.transform.SetLocalPositionY(0f);
			}
			else
			{
				ObjectMainView.transform.SetLocalPositionY(250f);
			}
		}
	}

	public override void Start()
	{
		base.Start();
		ObjNormalButton.SetActive(value: true);
		PosButtonCancel.gameObject.SetActive(value: true);
		tweeningCompleted = false;
		float y = -150 - Screen.height / 2;
		ObjectMainView.transform.localPosition = new Vector3(0f, y, 0f);
		if (Screen.width > Screen.height)
		{
			ObjectMainView.transform.DOLocalMoveY(0f, tweenValueInTime).SetEase(Ease.OutCubic).OnComplete(delegate
			{
				tweeningCompleted = true;
			});
		}
		else
		{
			ObjectMainView.transform.DOLocalMoveY(254f, tweenValueInTime).SetEase(Ease.OutCubic).OnComplete(delegate
			{
				tweeningCompleted = true;
			});
		}
		if (TargetList != null)
		{
			if (GameMain.main.GetGameOverPoupupType() == PopupType.PopupGameOver)
			{
				for (int i = 0; i < CPanelGameUI.Instance.varPortrait.dicCollectObjs.Count; i++)
				{
					CollectBlockType collectBlockType = MapData.main.collectBlocks[i].GetCollectBlockType();
					int num = GameMain.main.countOfEachTargetCount[(int)collectBlockType];
					if (num <= 0)
					{
						continue;
					}
					GameObject gameObject = Object.Instantiate(PrefabCollect);
					if (!gameObject)
					{
						continue;
					}
					CollectBlockByGameOver component = gameObject.GetComponent<CollectBlockByGameOver>();
					if ((bool)component)
					{
						component.gameObject.transform.SetParent(TargetList, worldPositionStays: false);
						Sprite sprite;
						switch (collectBlockType)
						{
						case CollectBlockType.NormalRed:
						case CollectBlockType.NormalOrange:
						case CollectBlockType.NormalYellow:
						case CollectBlockType.NormalGreen:
						case CollectBlockType.NormalBlue:
						case CollectBlockType.NormalPurple:
							sprite = Resources.Load<Sprite>(CPanelGameUI.GetCollectIconNormalBlockName(MapData.main.collectBlocks[i].blockType));
							break;
						default:
							sprite = Resources.Load<Sprite>("UI/CollectIcon/" + MapData.main.collectBlocks[i].blockType);
							break;
						case CollectBlockType.Null:
							continue;
						}
						if (sprite != null)
						{
							component.SetData(collectBlockType, num, MapData.main.collectBlocks[i].count, sprite);
						}
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(component);
					}
				}
			}
			else
			{
				for (int j = 0; j < CPanelGameUI.Instance.varPortrait.dicCollectObjs.Count; j++)
				{
					GameObject gameObject2 = Object.Instantiate(PrefabCollect);
					if (!gameObject2)
					{
						continue;
					}
					CollectBlockByGameOver component2 = gameObject2.GetComponent<CollectBlockByGameOver>();
					if ((bool)component2)
					{
						component2.gameObject.transform.SetParent(TargetList, worldPositionStays: false);
						CollectBlockType collectBlockType2 = MapData.main.collectBlocks[j].GetCollectBlockType();
						int remainCount = GameMain.main.countOfEachTargetCount[(int)collectBlockType2];
						Sprite sprite2;
						switch (collectBlockType2)
						{
						case CollectBlockType.NormalRed:
						case CollectBlockType.NormalOrange:
						case CollectBlockType.NormalYellow:
						case CollectBlockType.NormalGreen:
						case CollectBlockType.NormalBlue:
						case CollectBlockType.NormalPurple:
							sprite2 = Resources.Load<Sprite>(CPanelGameUI.GetCollectIconNormalBlockName(MapData.main.collectBlocks[j].blockType));
							break;
						default:
							sprite2 = Resources.Load<Sprite>("UI/CollectIcon/" + MapData.main.collectBlocks[j].blockType);
							break;
						case CollectBlockType.Null:
							continue;
						}
						if (sprite2 != null)
						{
							component2.SetData(collectBlockType2, remainCount, MapData.main.collectBlocks[j].count, sprite2);
						}
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(component2);
					}
				}
			}
		}
		if (MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop.ContainsKey(6))
		{
			buyItem = MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop[6];
			MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: true);
			TextCostValue.text = buyItem.normal_price.ToString();
		}
	}

	public override void SoundPlayShow()
	{
		SoundSFX.Play(SFXIndex.PopupOpenEffectSlide);
	}

	public override void OnEventOK()
	{
		if (MonoSingleton<PlayerDataManager>.Instance.Coin < buyItem.normal_price)
		{
			AppEventManager.m_TempBox.coinCategory = AppEventManager.CoinCategory.Move;
			AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
			MonoSingleton<PopupManager>.Instance.OpenPopupShopCoin();
			return;
		}
		    int coin = MonoSingleton<PlayerDataManager>.Instance.Coin;
		    MonoSingleton<PlayerDataManager>.Instance.DecreaseCoin(buyItem.normal_price);
		    int coin2 = MonoSingleton<PlayerDataManager>.Instance.Coin;
		    AppEventManager.m_TempBox.isPurchaseInGameOver = true;
		    AppEventManager.m_TempBox.listUsedBoosterOrder.Add(Booster.BoosterType.Move5);
		MonoSingleton<AppEventManager>.Instance.SendAppEventMovePackPurchased(Booster.BoosterType.Move5, MapData.main.gid, MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop[6].normal_price, AppEventManager.m_TempBox.gameContinueCount, coin, coin2, GameMain.main.countOfEachTargetCount);
		MonoSingleton<AppEventManager>.Instance.SendAppEventCoinConsumed(buyItem.normal_price, MapData.main.gid, coin, coin2, AppEventManager.CoinCategory.Move, "Move+ Pack : 5 Moves", buyItem.iid);
		MonoSingleton<AppEventManager>.Instance.SendAppEventInGameItemUsed(MapData.main.gid, buyItem.normal_price, 0, Booster.BoosterType.Move5, MapData.main.moveCount, GameMain.main.MoveCount, GameMain.main.countOfEachTargetCount);

		//CPanelGameUI.Instance.ThrowPurchasedItemEffect(SpawnStringEffectType.TextEffect5Move, 5);

//#if UNITY_EDITOR
		GameMain.main.IncreaseMoveCount(5);
		
		GameMain.main.DoGameContinue();
//#endif


		AppEventManager.m_TempBox.gameContinueCount++;
		MonoSingleton<UIManager>.Instance.HideCoinCurrentMenuLayer();
		MonoSingleton<UIManager>.Instance.HideLoading();
		eventClose = null;
		base.OnEventOK();
	}
}
