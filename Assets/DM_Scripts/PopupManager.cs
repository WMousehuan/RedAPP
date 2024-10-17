
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupManager : MonoSingleton<PopupManager>
{
	public bool IsActive;

	public GameObject PrefabCombinationBG;

	[SerializeField]
	private GameObject PrefabPopupList;

	private PopupList m_PopupList;

	public PopupType CurrentPopupType = PopupType.PopupNone;

	public Popup CurrentPopup;

	public CanvasScaler canvasScaler;
	public Camera PopupCamera;

	public GameObject ObjBackBlocking;

	public EventTrigger EventTriggerBackBlocking;

	public Transform ParentPopupGroup;

	public float blurFadeInTime = 1.5f;

	public float blurFadeOutTime = 1.5f;

	public List<Popup> listOpenedPopupObject = new List<Popup>();

	public CanvasScaler popupCanvasScaler;

	public override void Awake()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(PrefabPopupList);
		gameObject.transform.SetParent(base.transform);
		m_PopupList = gameObject.GetComponent<PopupList>();
		base.Awake();
	}

	private void Start()
	{
		ObjBackBlocking.SetActive(value: false);
    }

    private void Update()
    {
        canvasScaler.matchWidthOrHeight = Mathf.Lerp(1, 0, ((Screen.height / (float)Screen.width) - (4 / 3f)) / ((16 / 9f) - (4 / 3f)));

    }

    public void CloseAllPopup(bool UseCloseEvent = false)
    {
        for (int num = listOpenedPopupObject.Count - 1; num >= 0; num--)
		{
			try
			{
				Close();
				if (UseCloseEvent && listOpenedPopupObject[num].eventClose != null)
				{
					listOpenedPopupObject[num].eventClose();
				}
			}
			catch
			{
			}
		}
		listOpenedPopupObject.Clear();
		IsActive = false;
		ObjBackBlocking.SetActive(value: false);
		CurrentPopupType = PopupType.PopupNone;
		CurrentPopup = null;
	}

	private bool IsOpendPopupObject(PopupType type)
	{
		for (int i = 0; i < listOpenedPopupObject.Count; i++)
		{
			if (listOpenedPopupObject[i].m_PopupType == type)
			{
				return true;
			}
		}
		return false;
	}

	public Popup OpenCommonPopup(PopupType type, string title, string message, Action actionCloseEvent = null, Action actionEvent = null, Action actionNegativeEvent = null, bool enableBackCloseButton = true, bool disableBackKey = false, bool enableBlocking = true)
	{
		bool enableBackCloseButton2 = enableBackCloseButton;
		bool disableBackKey2 = disableBackKey;
		PopupCommon popupCommon = Open(type, enableBackCloseButton2, actionCloseEvent, actionEvent, actionNegativeEvent, holdEventOK: false, holdEventNegative: false, isReserve: false, enableOverlapPopup: false, disableBackKey2) as PopupCommon;
		if ((bool)popupCommon)
		{
			popupCommon.SetText(title, message);
		}
		return popupCommon;
	}

	public Popup Open(PopupType type, bool enableBackCloseButton = true, Action actionCloseEvent = null, Action actionEvent = null, Action actionNegativeEvent = null, bool holdEventOK = false, bool holdEventNegative = false, bool isReserve = false, bool enableOverlapPopup = false, bool disableBackKey = false, bool enableBlocking = true)
	{
		print(type.ToString());
		if (type == CurrentPopupType && CurrentPopup != null)
		{
			return CurrentPopup;
		}

        Popup popup = null;
		GameObject popup2 = m_PopupList.GetPopup(type);
		if (popup2 == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(popup2);
		if (gameObject == null)
		{
			return null;
		}
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(value: true);
		}
		popup = gameObject.GetComponent<Popup>();
		popupCanvasScaler.enabled = false;
		popupCanvasScaler.enabled = true;
		if ((bool)popup)
        {
            popup.EnableOverlapPopup = enableOverlapPopup;
			popup.DisableBackKey = disableBackKey;
			popup.transform.SetParent(ParentPopupGroup, worldPositionStays: false);
			popup.transform.localPosition = Vector3.zero;
			popup.Open(type, actionEvent, actionNegativeEvent, actionCloseEvent, holdEventOK, holdEventNegative);
			IsActive = true;
			if (popup.EnableOverlapPopup)
			{
				ObjBackBlocking.transform.SetSiblingIndex(popup.transform.GetSiblingIndex() - 1);
			}
			//listOpenedPopupObject.Add(popup);
			popup.enableBackBlockingClose = enableBackCloseButton;
			CurrentPopup = popup;
			CurrentPopupType = popup.m_PopupType;
			CurrentPopup.SoundPlayShow();
			if (CurrentPopup.enableBackBlockingClose)
			{
				EnableBackCloseEvent();
			}
			else
			{
				DisableBackCloseEvent();
			}
			if (enableBlocking)
			{
				ObjBackBlocking.SetActive(value: true);
			}
			UITweenList component = CurrentPopup.gameObject.GetComponent<UITweenList>();
			if ((bool)component)
			{
				component.PlayTweenListOpen();
            }
        }
        return popup;
    }
    public T Open<T>(PopupType type) where T : Popup
    {
        Popup popup = Open(type);
        return (T)popup;
    }

    public void Close()
    {
		bool flag = true;
		bool flag2 = false;
		if ((bool)CurrentPopup)
		{
			flag2 = CurrentPopup.EnableOverlapPopup;
			if (CurrentPopup.EnableOverlapPopup)
			{
				ObjBackBlocking.transform.SetAsFirstSibling();
			}
			CurrentPopup.SoundPlayHide();
			flag = CurrentPopup.DoBackBlur;
			if (CurrentPopup.DontDestroy)
			{
				CurrentPopup.gameObject.SetActive(value: false);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(CurrentPopup.gameObject);
			}
		}
		if (listOpenedPopupObject.Count > 0)
		{
			listOpenedPopupObject.RemoveAt(listOpenedPopupObject.Count - 1);
		}
		if (listOpenedPopupObject.Count > 0)
		{
			Popup popup = listOpenedPopupObject[listOpenedPopupObject.Count - 1];
			if ((bool)popup)
			{
				popup.gameObject.SetActive(value: true);
				CurrentPopupType = popup.m_PopupType;
				CurrentPopup = popup;
				popupCanvasScaler.enabled = false;
				popupCanvasScaler.enabled = true;
				if (CurrentPopup.enableBackBlockingClose)
				{
					EnableBackCloseEvent();
				}
				else
				{
					DisableBackCloseEvent();
				}
				return;
			}
		}
		IsActive = false;
		ObjBackBlocking.SetActive(value: false);
		CurrentPopupType = PopupType.PopupNone;
		CurrentPopup = null;
	}

	public void OnPressBlocker()
	{
		if ((bool)CurrentPopup && CurrentPopup.UseClosingByBlocker)
		{
			CurrentPopup.OnEventClose();
		}
	}

	public void EnableBackCloseEvent()
	{
		EventTriggerBackBlocking.triggers[0].callback.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
	}

	public void DisableBackCloseEvent()
	{
		EventTriggerBackBlocking.triggers[0].callback.SetPersistentListenerState(0, UnityEventCallState.Off);
	}

	public void OnOffBackBlocker(bool isOn)
	{
		ObjBackBlocking.SetActive(isOn);
	}

	public Transform GetBackBlockingTransform()
	{
		return ObjBackBlocking.transform;
	}

	public bool IsOpenPopupNow()
	{
		if (listOpenedPopupObject.Count == 0)
		{
			return false;
		}
		return true;
	}

	public void OpenPopupShopCoin()
	{
		if (MonoSingleton<PlayerDataManager>.Instance.EnabledDoubleShopCoin)
		{
			AppEventManager.m_TempBox.adAccessedBy = AppEventManager.AdAccessedBy.Coin_Store_Automatic_Popup;
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCoinDoubleShop);
		}
		else
		{
			MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupShopCoin);
		}
	}
}
