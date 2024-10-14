using UnityEngine;

public class UIEffectScaler : MonoBehaviour
{
	private void OnEnable()
	{
		float num = Screen.height;
		float num2 = Screen.width;
		if (Screen.width > Screen.height)
		{
			num = Screen.width;
			num2 = Screen.height;
		}
		float num3 = 960f * num2 / (640f * num);
		ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].transform.localScale = new Vector3(num3, num3, num3);
		}
	}
}
