/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIBoosterUsingGuide : MonoBehaviour
{
	public GameObject bg;

	public Transform boardTopLocalPositionAboutGuide;

	public List<Button> boosterButtons = new List<Button>();

	public Image boosterIcon;

	public Image boosterIconShadow;

	public GameObject circle;

	private bool registComplete;

	public Sprite[] SpriteBoosterIcon;

	private int tempDir = 1;

	public Text textGuide;

	private void OnEnable()
	{
		if (registComplete)
		{
			for (int i = 0; i < boosterButtons.Count; i++)
			{
				boosterButtons[i].transform.Find("Button_number").gameObject.SetActive(value: false);
			}
			if (boardTopLocalPositionAboutGuide != null)
			{
				RectTransform component = GetComponent<RectTransform>();
				Vector3 localPosition = boardTopLocalPositionAboutGuide.localPosition;
				component.sizeDelta = new Vector2(0f, Mathf.Abs(localPosition.y));
			}
		}
	}

	private void OnDisable()
	{
		if (!registComplete)
		{
			return;
		}
		for (int i = 0; i < boosterButtons.Count; i++)
		{
			boosterButtons[i].interactable = true;
			Booster component = boosterButtons[i].GetComponent<Booster>();
			GameObject gameObject = component.transform.Find("Button_number").gameObject;
			if (component.boosterType == Booster.BoosterType.Shuffle)
			{
				if ((int)MonoSingleton<PlayerDataManager>.Instance.BoosterCount[(int)component.boosterType] == 0)
				{
					gameObject.SetActive(value: false);
				}
				else
				{
					gameObject.SetActive(value: true);
				}
			}
			else
			{
				gameObject.SetActive(value: true);
			}
		}
	}

	public void RegistBoosterButton(Button _boosterButton)
	{
		registComplete = true;
		boosterButtons.Add(_boosterButton);
	}

	public void SetIconImage(Booster.BoosterType boosterType)
	{
		Sprite sprite = SpriteBoosterIcon[(int)boosterType];
		if (sprite != null)
		{
			if ((bool)boosterIcon)
			{
				boosterIcon.sprite = sprite;
				boosterIcon.SetNativeSize();
			}
			if ((bool)boosterIconShadow)
			{
				boosterIconShadow.sprite = sprite;
				boosterIconShadow.SetNativeSize();
			}
		}
	}

	public void SetIconOff()
	{
		if ((bool)boosterIcon)
		{
			boosterIcon.enabled = false;
		}
		if ((bool)boosterIconShadow)
		{
			boosterIconShadow.enabled = false;
		}
	}

	public void SetIconOn()
	{
		if ((bool)boosterIcon)
		{
			boosterIcon.enabled = true;
		}
		if ((bool)boosterIconShadow)
		{
			boosterIconShadow.enabled = true;
		}
	}

	public void SetBackGroundOn()
	{
		bg.SetActive(value: true);
	}

	public void SetBackGroundOff()
	{
		bg.SetActive(value: false);
	}

	public void TurnOnOnlyOneBoosterUI(int uiIndex)
	{
		boosterIcon.gameObject.SetActive(value: true);
		Vector3 localPosition = textGuide.transform.localPosition;
		localPosition.x += 0.001f * (float)tempDir;
		tempDir = -tempDir;
		textGuide.transform.localPosition = localPosition;
		for (int i = 0; i < boosterButtons.Count; i++)
		{
			boosterButtons[i].interactable = false;
		}
		boosterButtons[uiIndex].interactable = true;
		if (circle != null)
		{
			circle.SetActive(value: true);
			circle.transform.position = boosterButtons[uiIndex].transform.position;
		}
	}
}
