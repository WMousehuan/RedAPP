using UnityEngine;

public class AppstoreTestScene : MonoBehaviour
{
	public string m_appID_IOS = "";

	public string m_appID_Android = "";

	public void OnButtonClick(string buttonName)
	{
		if (buttonName == "ViewApp" && !Application.isEditor)
		{
			MonoSingleton<AppstoreHandler>.Instance.openAppInStore(m_appID_Android);
		}
	}
}
