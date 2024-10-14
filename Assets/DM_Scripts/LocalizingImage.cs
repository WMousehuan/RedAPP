using UnityEngine;
using UnityEngine.UI;

public class LocalizingImage : MonoBehaviour
{
	private bool isInit;

	private LanguageCode setLangCode;

	private void Start()
	{
		if (MonoSingleton<XMLStringManager>.Instance.SelectedLanguageCode != 0)
		{
			Refresh();
		}
	}

	private void OnEnable()
	{
		if (isInit && MonoSingleton<XMLStringManager>.Instance.SelectedLanguageCode != setLangCode)
		{
			Refresh();
		}
	}

	public void Refresh()
	{
		Image component = base.gameObject.GetComponent<Image>();
		if ((bool)component)
		{
			component.sprite = MonoSingleton<XMLStringManager>.Instance.GetSprite(component.sprite.name);
		}
		setLangCode = MonoSingleton<XMLStringManager>.Instance.SelectedLanguageCode;
		isInit = true;
	}
}
