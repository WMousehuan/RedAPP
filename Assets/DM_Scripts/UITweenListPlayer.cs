using UnityEngine;

public class UITweenListPlayer : MonoBehaviour
{
	public UITweenList[] arrUITweenList;

	public void PlayTweenListOpen()
	{
		for (int i = 0; i < arrUITweenList.Length; i++)
		{
			arrUITweenList[i].PlayTweenListOpen();
		}
	}

	public void PlayTweenListClose()
	{
		for (int i = 0; i < arrUITweenList.Length; i++)
		{
			arrUITweenList[i].PlayTweenListClose();
		}
	}
}
