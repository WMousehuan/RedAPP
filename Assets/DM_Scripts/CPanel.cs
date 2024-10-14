/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class CPanel : MonoBehaviour
{
	public static int uiAnimation;

	private string currentClip = string.Empty;

	public string hide;

	public string show;

	public void SetActive(bool a)
	{
		if (base.gameObject.activeSelf == a)
		{
			return;
		}
		currentClip = string.Empty;
		if (!a)
		{
			if (!(hide != string.Empty))
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			currentClip = hide;
		}
		if (a)
		{
			base.gameObject.SetActive(value: true);
			if (!(show != string.Empty))
			{
				return;
			}
			currentClip = show;
			Update();
		}
		if (!(currentClip == string.Empty))
		{
			GetComponent<Animation>().Play(currentClip);
			GetComponent<Animation>()[currentClip].time = 0f;
			uiAnimation++;
		}
	}

	public virtual void Update()
	{
		if (currentClip == string.Empty)
		{
			return;
		}
		GetComponent<Animation>()[currentClip].time += Mathf.Min(Time.unscaledDeltaTime, Time.maximumDeltaTime);
		GetComponent<Animation>()[currentClip].enabled = true;
		GetComponent<Animation>().Sample();
		GetComponent<Animation>()[currentClip].enabled = false;
		if (GetComponent<Animation>()[currentClip].time >= GetComponent<Animation>()[currentClip].length)
		{
			if (currentClip == hide)
			{
				base.gameObject.SetActive(value: false);
			}
			currentClip = string.Empty;
			uiAnimation--;
		}
	}
}
