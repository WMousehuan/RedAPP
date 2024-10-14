
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine.UI;

public class PopupCommon : Popup
{
	public Text TextButton;

	public Text TextMessage;

	public Text TextTitle;

	public void SetText(string title, string message, string button = null)
	{
		if (!string.IsNullOrEmpty(title))
		{
			TextTitle.text = title;
		}
		if (!string.IsNullOrEmpty(message))
		{
			TextMessage.text = message;
		}
		if (TextButton != null && !string.IsNullOrEmpty(button))
		{
			TextButton.text = button;
		}
	}
}
