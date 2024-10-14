
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BoosterButton : MonoBehaviour
{
	public enum BoosterButtonType
	{
		Page,
		Message
	}

	private Button button;

	public BoosterButtonType type;

	public string value;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(delegate
		{
			OnClick();
		});
	}

	private void OnClick()
	{
		if (type == BoosterButtonType.Message)
		{
			SendMessage(value, SendMessageOptions.DontRequireReceiver);
		}
		if (type == BoosterButtonType.Page)
		{
			UIServer.main.ShowPage(value);
		}
	}
}
