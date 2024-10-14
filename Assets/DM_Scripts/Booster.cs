
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */


 using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
	public enum BoosterType
	{
		Hammer,
		CandyPack,
		Shuffle,
		HBomb,
		VBomb,
		Max,
		Move5,
		Move1,
		NONE
	}

	public static Color colorDarkBrown = new Color(37f / 51f, 59f / 255f, 0f);

	public static Color colorDarkGreen = new Color(41f / 255f, 113f / 255f, 44f / 255f);

	public Image bgImage;

	public BoosterType boosterType;

	public GameObject buttonAdObj;

	public GameObject buttonBuy;

	public GameObject highLight;

	public GameObject frameBgObj;

	public GameObject buttonNumberObj;

	public Transform CenterPosition;

	public InGameUIBoosterUsingGuide guide;

	public Image itemImage;

	private bool mIsTutorial;

	protected bool onSelect;

	public GameObject selectEffect;

	public GameObject selectFail;

	public Text TextBoosterCount;

	protected int uiIndex;

    private void OnEnable()
    {
    }

    protected virtual void Caching()
	{
		if (TextBoosterCount == null)
		{
			TextBoosterCount = base.transform.Find("Button_number/ImageYellow/Text").GetComponent<Text>();
		}
		if (buttonBuy == null)
		{
			buttonBuy = base.transform.Find("Button_buy").gameObject;
		}
		if (selectEffect == null)
		{
			selectEffect = base.transform.Find("Eff_Item_use").gameObject;
		}
		if (selectFail == null)
		{
			selectFail = base.transform.Find("Eff_Itemuse_fail").gameObject;
		}

        guide = CPanelGameUI.Instance.BoosterGuide;

        if (CenterPosition == null)
		{
			CenterPosition = base.transform.Find("bg");
		}
		if (bgImage == null)
		{
			bgImage = base.transform.Find("bg").GetComponent<Image>();
		}
		if (itemImage == null)
		{
			itemImage = base.transform.Find("Item").GetComponent<Image>();
		}
		if (frameBgObj == null)
		{
			frameBgObj = base.transform.Find("frame_bg").gameObject;
		}
		if (buttonNumberObj == null)
		{
			buttonNumberObj = base.transform.Find("Button_number").gameObject;
		}
		if (highLight == null)
		{
			highLight = base.transform.Find("Image_highlight").gameObject;
		}
		if (buttonAdObj == null)
		{
			buttonAdObj = base.transform.Find("ButtonAd").gameObject;
		}
	}

	protected Sprite GetItemSprite()
	{
		Sprite sprite = null;
		switch (boosterType)
		{
		case BoosterType.Hammer:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterHammer);
		case BoosterType.CandyPack:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterCandyPack);
		case BoosterType.Shuffle:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterShuffle);
		case BoosterType.HBomb:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterHBomb);
		case BoosterType.VBomb:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterVBomb);
		default:
			return MonoSingleton<UIManager>.Instance.GetServerRewardItemTypeSprite(ServerItemIndex.BoosterHammer);
		}
	}

	private void SetImage()
	{
		Sprite itemSprite = GetItemSprite();
		itemImage.SetNativeSize();
		if (itemSprite != null)
		{
			itemImage.sprite = itemSprite;
			itemImage.SetNativeSize();
			itemImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		bgImage.enabled = false;
	}

	protected virtual void Start()
	{
	}

	public virtual void ForceStart()
	{
		Caching();
		SetImage();
		if (buttonAdObj != null)
		{
			buttonAdObj.SetActive(value: false);
		}
		if (buttonBuy != null)
		{
			buttonBuy.SetActive(value: false);
		}
		if (guide != null)
		{
			guide.RegistBoosterButton(GetComponent<Button>());
			guide.gameObject.SetActive(value: false);
		}
		if (selectEffect != null)
		{
			float num = Screen.height;
			float num2 = Screen.width;
			if (Screen.width > Screen.height)
			{
				num = Screen.width;
				num2 = Screen.height;
			}
			float num3 = num2 / num * 3f;
			selectEffect.gameObject.layer = base.transform.parent.gameObject.layer;
			ParticleSystem[] componentsInChildren = selectEffect.transform.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = base.transform.parent.gameObject.layer;
				if (componentsInChildren[i].gameObject.layer == LayerMask.NameToLayer("UIInGame"))
				{
					componentsInChildren[i].transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
				}
				else
				{
					componentsInChildren[i].transform.localScale = new Vector3(num3, num3, num3);
				}
			}
			GameObject gameObject = selectEffect.transform.Find("Check").gameObject;
			gameObject.SetActive(value: false);
		}
		if (base.transform.parent.gameObject.layer == LayerMask.NameToLayer("UIInGame"))
		{
			MonoSingleton<UIManager>.Instance.eventCancelBooster += CancelBooster;
		}
		GameObject gameObject2 = buttonNumberObj.transform.Find("ImageGreen").gameObject;
		GameObject gameObject3 = buttonNumberObj.transform.Find("ImageYellow").gameObject;
//		Outline component = buttonNumberObj.transform.Find("Text").GetComponent<Outline>();
//		Shadow component2 = buttonNumberObj.transform.Find("Text").GetComponent<Shadow>();
		highLight.SetActive(value: false);
		frameBgObj.SetActive(value: false);
		buttonNumberObj.transform.localPosition = new Vector3(30f, -21f, 0f);
		//component.effectColor = colorDarkBrown;
		//component2.effectColor = colorDarkBrown;
		gameObject2.SetActive(value: false);
		gameObject3.SetActive(value: true);
	}

	public bool IsLocked()
	{
		return false;
	}

	public virtual void SetLockImage()
	{
		Sprite lockSprite = MonoSingleton<UIManager>.Instance.GetLockSprite();
		if (lockSprite != null && !(itemImage == null))
		{
			itemImage.SetNativeSize();
			frameBgObj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.76f);
			itemImage.sprite = lockSprite;
			itemImage.SetNativeSize();
			itemImage.transform.localScale = Vector3.one;
		}
	}

	public void SetUiIndex(int _uiIndex)
	{
		uiIndex = _uiIndex;
	}

	private void OnDestroy()
	{
		if (base.transform.parent.gameObject.layer == LayerMask.NameToLayer("UIInGame"))
		{
			MonoSingleton<UIManager>.Instance.eventCancelBooster -= CancelBooster;
		}
	}

	protected virtual void Update()
	{
		if ((Application.isEditor || Application.platform == RuntimePlatform.Android) && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			CancelBooster();
		}
	}

	private void OrientationChange(ScreenOrientation orientation)
	{
		if (onSelect)
		{
			CancelBooster();
		}
	}

	public virtual void UseBooster(bool isTutorial = false)
	{
		mIsTutorial = isTutorial;
		Application.targetFrameRate = GlobalSetting.FPS;
		CPanelGameUI.Instance.isPlayingBoosterItem = true;
		if (isTutorial)
		{
			onSelect = false;
		}
	}

	public virtual void CancelBooster()
	{
		Application.targetFrameRate = GlobalSetting.LOW_FPS;
		CPanelGameUI.Instance.isPlayingBoosterItem = false;
		CPanelGameUI.Instance.UpdateTextBoosterCount();
	}

	protected bool CheckEnableBoosterForPreBooster(bool isTutorial = false)
	{
		if (!isTutorial)
		{
			if (!GameMain.main.isPlaying)
			{
				return false;
			}
			if (!GameMain.main.CanIWait())
			{
				return false;
			}
			if (GameMain.main.isGameResult)
			{
				return false;
			}
			if (GameMain.main.CurrentTurn == VSTurn.CPU)
			{
				return false;
			}
			if (GameMain.main.isConnectedSweetRoad)
			{
				return false;
			}
		}
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType] > 0)
		{
			return true;
		}
		if (MonoSingleton<IRVManager>.Instance.CurrentNetStatus == InternetReachabilityVerifier.Status.Offline)
		{
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupConnectionLost);
			return false;
		}
		return false;
	}

	protected bool CheckEnableBooster(bool isTutorial = false)
	{
		if (!isTutorial)
		{
			if (!GameMain.main.isPlaying)
			{
				return false;
			}
			if (!GameMain.main.CanIWait())
			{
				return false;
			}
			if (GameMain.main.isGameResult)
			{
				return false;
			}
			if (GameMain.main.CurrentTurn == VSTurn.CPU)
			{
				return false;
			}
			if (GameMain.main.isConnectedSweetRoad)
			{
				return false;
			}
		}
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType] > 0)
		{
			return true;
		}
		if (MonoSingleton<IRVManager>.Instance.CurrentNetStatus == InternetReachabilityVerifier.Status.Offline)
		{
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupConnectionLost);
			return false;
		}
		bool flag = true;
		//if (PlayerPrefs.GetInt("BuyBoosterV2" + boosterType, 0) == 0)
		//{
		//	if (boosterType == BoosterType.CandyPack)
		//	{
		//		MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupBoosterPackCandyPack);
		//		flag = false;
		//	}
		//	else if (boosterType == BoosterType.Hammer)
		//	{
		//		MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupBoosterPackHammer);
		//		flag = false;
		//	}
		//	else if (boosterType == BoosterType.HBomb)
		//	{
		//		MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupBoosterPackHBomb);
		//		flag = false;
		//	}
		//	else if (boosterType == BoosterType.VBomb)
		//	{
		//		MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupBoosterPackVBomb);
		//		flag = false;
		//	}
		//}
		if (flag)
		{
			PopupInGameItemStore popupInGameItemStore = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupInGameItemStore) as PopupInGameItemStore;
			popupInGameItemStore.SetPopup(boosterType);
		}
		return false;
	}

	public virtual void UpdateTextBoosterCount()
	{
		if (buttonBuy != null)
		{
			buttonBuy.SetActive(value: false);
		}
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType] == 0)
		{
			if (buttonBuy != null)
			{
				buttonBuy.SetActive(value: true);
			}
		}
		else if (buttonBuy != null)
		{
			buttonBuy.SetActive(value: false);
		}
		if (TextBoosterCount != null)
		{
			TextBoosterCount.text = MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType].ToString();
		}
	}

	protected void CompleteUseBooster()
	{
		GameMain.main.IsGettingItem = true;
		Application.targetFrameRate = GlobalSetting.LOW_FPS;
		int num = (int)boosterType;
		GameMain.main.IsUseBooster = true;
		if (GameMain.main.UsedBoosterCountForSync != null && (int)boosterType < GameMain.main.UsedBoosterCountForSync.Length)
		{
			GameMain.main.UsedBoosterCountForSync[num]++;
		}
		if (num <= 2)
		{
			GameMain.main.UsedBoosterCount[num]++;
		}
		AppEventManager.m_TempBox.listUsedBoosterOrder.Add(boosterType);
		CPanelGameUI.Instance.isPlayingBoosterItem = false;
		MonoSingleton<PlayerDataManager>.Instance.BoosterCount[num] = Mathf.Max(0, (int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[num] - 1);
		MonoSingleton<PlayerDataManager>.Instance.SaveBoosterData();
		if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[num] == 0)
		{
			buttonBuy.SetActive(value: true);
		}
		else
		{
			buttonBuy.SetActive(value: false);
		}
		if (boosterType == BoosterType.Shuffle)
		{
			buttonBuy.SetActive(value: false);
		}
		TextBoosterCount.text = MonoSingleton<PlayerDataManager>.Instance.BoosterCount[num].ToString();
		GameMain.main.EventCounter();
		CompleteUseItem();
	}

	private void CompleteUseItem()
	{
		if (GameMain.main.UsedBoosterCountForSync != null && (int)boosterType < GameMain.main.UsedBoosterCountForSync.Length)
		{
			GameMain.main.UsedBoosterCountForSync[(int)boosterType]--;
		}
		int num = 0;
		int boosterItemIndex = MonoSingleton<ServerDataTable>.Instance.GetBoosterItemIndex(boosterType);
		if (MonoSingleton<PlayerDataManager>.Instance.dicBoosterItemList.ContainsKey(boosterItemIndex) && MonoSingleton<PlayerDataManager>.Instance.dicBoosterItemList[boosterItemIndex].Count > 0 && MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop.ContainsKey(MonoSingleton<PlayerDataManager>.Instance.dicBoosterItemList[boosterItemIndex][0]))
		{
			num = MonoSingleton<ServerDataTable>.Instance.m_dicTableItemShop[MonoSingleton<PlayerDataManager>.Instance.dicBoosterItemList[boosterItemIndex][0]].normal_price;
			MonoSingleton<AppEventManager>.Instance.SendAppEventInGameItemUsed(MapData.main.gid, num, MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType], boosterType, MapData.main.moveCount, GameMain.main.MoveCount, GameMain.main.countOfEachTargetCount);
			if (mIsTutorial)
			{
				MonoSingleton<AppEventManager>.Instance.SendAppEventFreeInGameItemUsed(MapData.main.gid, MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)boosterType], boosterType, MapData.main.moveCount, GameMain.main.MoveCount, GameMain.main.countOfEachTargetCount);
			}
		}
	}

	protected IEnumerator EffectSelectFail(Transform targetPos)
	{
		GameObject effSelectFail = UnityEngine.Object.Instantiate(selectFail);
		effSelectFail.SetActive(value: true);
		Transform transform = effSelectFail.transform;
		Vector3 position = targetPos.transform.position;
		float x = position.x;
		Vector3 position2 = targetPos.transform.position;
		transform.position = new Vector2(x, position2.y);
		yield return new WaitForSeconds(0.5f);
		UnityEngine.Object.Destroy(effSelectFail);
	}
}
