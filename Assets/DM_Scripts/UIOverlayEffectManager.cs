using System.Collections;
using UnityEngine;

public class UIOverlayEffectManager : MonoSingleton<UIOverlayEffectManager>
{
	public Transform posParent;

	private int showObjCount;

	public Camera uiCamera;

	public GameObject PrefabEffectRibbonFireworks;

	private void Start()
	{
		Reset();
	}

	public void Reset()
	{
		StopAllCoroutines();
		showObjCount = 0;
		base.gameObject.SetActive(value: false);
	}

	private void StartEffect()
	{
		showObjCount++;
		base.gameObject.SetActive(value: true);
	}

	private void EndEffect()
	{
		if (--showObjCount == 0)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void ShowEffectRibbonFireworks()
	{
		StartEffect();
		StartCoroutine(waitShowEffectRibbonFireworks());
	}

	private IEnumerator waitShowEffectRibbonFireworks()
	{
		GameObject obj = Object.Instantiate(PrefabEffectRibbonFireworks);
		obj.transform.SetParent(posParent, worldPositionStays: false);
		obj.transform.localPosition = Vector3.zero;
		yield return new WaitForSeconds(3.2f);
		UnityEngine.Object.Destroy(obj);
		EndEffect();
	}
}
