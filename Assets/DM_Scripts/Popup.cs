
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
	public List<GameObject> CloseTweeningButtons = new List<GameObject>();

	[Space(10f)]
	[Header("Tweening")]
	public List<GameObject> DefaultTweeningButtons = new List<GameObject>();

	protected Dictionary<GameObject, Vector3> dicTweeningBaseScale = new Dictionary<GameObject, Vector3>();

	public bool DoBackBlur = true;

	public bool UseClosingByBlocker = true;

	public bool DontDestroy;

	[HideInInspector]
	public bool enableBackBlockingClose;

	[HideInInspector]
	public bool EnableOverlapPopup;

	public Action eventClose;

	protected Action eventNegative;

	protected Action eventOK;

	protected bool holdEventNegative;

	protected bool holdEventOK;

	[Space(10f)]
	[Header("Layout")]
	public List<GameObject> layoutObjects = new List<GameObject>();

	private readonly List<PopupLayout> layouts = new List<PopupLayout>();

	public List<GameObject> ListTweenObjects = new List<GameObject>();

	public float listTweenObjectsDelay;

	[HideInInspector]
	public PopupType m_PopupType = PopupType.PopupNone;

	private Vector3 orgScale = Vector3.one;

	public DateTime PopupOpenDateTime = DateTime.Now;

	public bool ShowOpenCloseTweenEffect = true;

	protected float totalListDelay;

	protected float tweeningMaxDelay;

	public List<TweeningPopupObject> tweeningObjects = new List<TweeningPopupObject>();

	protected float tweenTimeToOpen = 0.4f;

	[HideInInspector]
	public bool DisableBackKey;

	public virtual void Start()
	{
		if (!DontDestroy)
		{
			StartPopupTween();
		}
	}

	public virtual void OnEnable()
	{
		if (PopupManager.Instance.CurrentPopup != this)
		{
			PopupManager.Instance.CurrentPopup = this;
			if (PopupManager.Instance.listOpenedPopupObject.Contains(this))
			{
				PopupManager.Instance.listOpenedPopupObject.Remove(this);
            }
            PopupManager.Instance.listOpenedPopupObject.Add(this);
        }
		if (DontDestroy)
		{
			StartPopupTween();
		}
	}

	protected void StartPopupTween()
	{
		PopupOpenDateTime = DateTime.Now;
		if (ShowOpenCloseTweenEffect)
		{
			Application.targetFrameRate = GlobalSetting.FPS;
			orgScale = base.transform.localScale;
			base.transform.localScale = new Vector3(0f, 0f, 0f);
			base.transform.DOScale(orgScale, tweenTimeToOpen).SetEase(Ease.OutBack).OnComplete(delegate
			{
				base.transform.localScale = orgScale;
				if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Game)
				{
					Application.targetFrameRate = GlobalSetting.LOW_FPS;
				}
			});
		}
		tweeningMaxDelay = 0f;
		for (int i = 0; i < tweeningObjects.Count; i++)
		{
			if (tweeningObjects[i].obj != null && tweeningObjects[i].obj.activeSelf && tweeningObjects[i].delay > tweeningMaxDelay)
			{
				tweeningMaxDelay = tweeningObjects[i].delay;
			}
		}
		totalListDelay = tweeningMaxDelay + listTweenObjectsDelay;
		for (int j = 0; j < ListTweenObjects.Count; j++)
		{
			if (ListTweenObjects[j] != null && ListTweenObjects[j].activeSelf)
			{
				totalListDelay += 0.1f;
			}
		}
		StartTweeningObjects();
	}

	private void StartTweeningObjects()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < DefaultTweeningButtons.Count; i++)
		{
			if (DefaultTweeningButtons[i] != null && DefaultTweeningButtons[i].activeSelf)
			{
				TweenScaleObject(DefaultTweeningButtons[i], 0.3f, 0f);
			}
		}
		if (DefaultTweeningButtons.Count > 0)
		{
			SoundSFX.PlayCap(SFXIndex.PopupOpenEffectLow);
		}
		for (int j = 0; j < tweeningObjects.Count; j++)
		{
			if (tweeningObjects[j].obj != null && tweeningObjects[j].obj.activeSelf)
			{
				TweenScaleObject(tweeningObjects[j].obj, tweeningObjects[j].duration, tweeningObjects[j].delay);
			}
		}
		if (tweeningObjects.Count > 0)
		{
			SoundSFX.PlayCapDelay(this, tweeningObjects[0].delay, SFXIndex.PopupOpenEffectMid);
		}
		for (int k = 0; k < CloseTweeningButtons.Count; k++)
		{
			if (CloseTweeningButtons[k] != null && CloseTweeningButtons[k].activeSelf)
			{
				TweenScaleObject(CloseTweeningButtons[k], 0.3f, tweeningMaxDelay + 0.1f);
			}
		}
		if (CloseTweeningButtons.Count > 0)
		{
			SoundSFX.PlayCapDelay(this, tweeningMaxDelay + 0.2f, SFXIndex.PopupOpenEffectHigh);
		}
		float num = tweeningMaxDelay + 0.1f + listTweenObjectsDelay;
		for (int l = 0; l < ListTweenObjects.Count; l++)
		{
			if (ListTweenObjects[l] != null && ListTweenObjects[l].activeSelf)
			{
				TweenScaleObject(ListTweenObjects[l], 0.3f, num);
				if (l == ListTweenObjects.Count - 1)
				{
					SFXIndex index = SFXIndex.PopupOpenEffectLow;
					float delay = num;
					SoundSFX.Play(index, loop: false, delay);
				}
				else
				{
					SFXIndex index = SFXIndex.PopupOpenEffectHigh;
					float delay = num;
					SoundSFX.Play(index, loop: false, delay);
				}
				num += 0.1f;
			}
		}
	}

	private void TweenScaleObject(GameObject obj, float duration, float delay)
	{
		if (!dicTweeningBaseScale.ContainsKey(obj))
		{
			dicTweeningBaseScale.Add(obj, obj.transform.localScale);
		}
		obj.transform.localScale = Vector3.zero;
		obj.transform.DOScale(dicTweeningBaseScale[obj], duration).SetEase(Ease.OutBack).SetDelay((!ShowOpenCloseTweenEffect) ? delay : (0.2f + delay));
	}

	public virtual void Open(PopupType popupType, Action actionEvent = null, Action actionNegativeEvent = null, Action actionCloseEvent = null, bool holdEventOK = false, bool holdEventNegative = false)
	{
		m_PopupType = popupType;
		eventOK = actionEvent;
		eventNegative = actionNegativeEvent;
		eventClose = actionCloseEvent;
		this.holdEventOK = holdEventOK;
		this.holdEventNegative = holdEventNegative;
	}
    public virtual void OnEventOpenOtherPopup(string popupType)
    {
        SoundSFX.Play(SFXIndex.ButtonClick);
        PopupManager.Instance.Open(Enum.Parse<PopupType>(popupType));
    }
    public virtual void SoundPlayShow()
	{
		if (m_PopupType != PopupType.PopupRewardItems)
		{
			SoundSFX.Play(SFXIndex.ZoomPopupShow);
		}
	}

	public virtual void SoundPlayHide()
	{
	}

	public virtual void OnEventOK()
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		if (!holdEventOK)
		{
			MonoSingleton<PopupManager>.Instance.Close();
		}
        eventOK?.Invoke();
        if (eventOK != null) UnityEngine.Debug.Log("Popup.OnEventOK " + eventOK.Method.Name);
    }

    public virtual void OnEventNegative()
	{
		if (!holdEventNegative)
		{
			MonoSingleton<PopupManager>.Instance.Close();
		}
		if (eventNegative != null)
		{
			eventNegative();
		}
	}

	public virtual void OnEventClose()
	{
		SoundSFX.Play(SFXIndex.PopupHideAfterClick);
		MonoSingleton<PopupManager>.Instance.Close();
		if (eventClose != null)
		{
			eventClose();
		}
	}

	public void ChangeLayout(LayoutType type)
	{
		foreach (GameObject layoutObject in layoutObjects)
		{
			layoutObject.SetActive(value: false);
		}
		for (int i = 0; i < layouts.Count; i++)
		{
			if (type == layouts[i].type)
			{
				layouts[i].obj.SetActive(value: true);
				if (layouts[i].posAndScale != Vector4.zero)
				{
					layouts[i].obj.transform.localPosition = new Vector3(layouts[i].posAndScale.x, layouts[i].posAndScale.y, 0f);
					layouts[i].obj.GetComponent<RectTransform>().sizeDelta = new Vector2(layouts[i].posAndScale.z, layouts[i].posAndScale.w);
				}
			}
		}
	}

	public void SetLayout(LayoutType type, GameObject obj, Vector4 posAndScale = default(Vector4))
	{
		PopupLayout popupLayout = new PopupLayout();
		popupLayout.type = type;
		popupLayout.obj = obj;
		popupLayout.posAndScale = posAndScale;
		layouts.Add(popupLayout);
	}
    private void OnDestroy()
    {
		print(this.name);
    }
}
