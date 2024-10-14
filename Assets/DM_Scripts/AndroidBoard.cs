/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */

using UnityEngine;

internal class AndroidBoard : IBoard
{
	private AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");

	public void SetText(string str)
	{
		cb.CallStatic("setText", str);
	}

	public string GetText()
	{
		return cb.CallStatic<string>("getText", new object[0]);
	}
}
