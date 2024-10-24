using UnityEngine;

public class UIOptionButton : MonoBehaviour
{
	public enum OptionMenuType
	{
		Lobby,
		Game
	}

	public OptionMenuType optionMenuType;

	public void OnPressMainButton()
	{
		if (GameMain.main != null && GameMain.main.CanIWait())
		{
			MonoSingleton<UIManager>.Instance.CancelBooster();
			PopupSetting popupSetting = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupSetting) as PopupSetting;
			popupSetting.SetPopup(optionMenuType);
		}
		else
		{
			PopupSetting popupSetting2 = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupSetting) as PopupSetting;
//			Debug.Log("NAME :" + popupSetting2.name);
			popupSetting2.SetPopup(optionMenuType);
		}
	}
}
