/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */

using UnityEngine;

public class ButtonLinkToCSUrl : MonoBehaviour
{
	private void Start()
	{
	}

	public void OnPressButton()
	{
		Application.OpenURL(MonoSingleton<ServerDataTable>.Instance.faqCsUrl);
	}
}
