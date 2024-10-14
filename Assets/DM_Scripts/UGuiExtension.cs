using UnityEngine.UI;

public static class UGuiExtension
{
	public static void SetLangValue(this Text text, string key)
	{
		MonoSingleton<ServerDataTable>.Instance.SetLangValue(text, key);
	}
}
