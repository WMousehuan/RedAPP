using UnityEngine;
using UnityEngine.UI;

public class SupportNotchScreenCanvasScaler : MonoBehaviour
{
	public bool SupportOrientation;

	public float matchWidthOrHeight;

	private float orgMathWidthOrHeight;

	private bool isNotchScreen;

	private bool isPortraitView;

	private void Start()
	{
		orgMathWidthOrHeight = GetComponent<CanvasScaler>().matchWidthOrHeight;
		if (UIManager.IsNotchScreen())
		{
			SetValue();
			isNotchScreen = true;
			isPortraitView = true;
		}
	}

	private void SetValue()
	{
		GetComponent<CanvasScaler>().matchWidthOrHeight = matchWidthOrHeight;
	}
}
