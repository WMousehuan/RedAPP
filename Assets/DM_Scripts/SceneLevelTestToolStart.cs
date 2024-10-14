using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneLevelTestToolStart : SceneClass
{
	public static int TestStartLevel = 1;

	public static int TestEndLevel = 100;

	public static float TestTimeScale = 3f;

	public static GameObject PrefabDoingLevelTestText;

	public static int TestPlayCount = 1;

	public static float TestStartUnixTime;

	public InputField InputEndLevel;

	public InputField InputStartLevel;

	public InputField InputTimeScale;

	public GameObject ObjDoingLevelTestText;

	private void Start()
	{
		TestPlayCount = 1;
		PrefabDoingLevelTestText = ObjDoingLevelTestText;
	}

	private void Update()
	{
	}

	public void OnPressTestStart()
	{
		if (TestStartLevel > 0 && TestEndLevel >= TestStartLevel && !(TestTimeScale <= 0f))
		{
			TestStartUnixTime = Utils.ConvertToTimestampDouble(DateTime.Now);
			MonoSingleton<UIManager>.Instance.ShowLoading();
//			MonoSingleton<GameDataLoadManager>.Instance.StartLoadData();
		}
	}

	public void OnValueChangedInputStartLevel(string recvString)
	{
		int.TryParse(recvString, out TestStartLevel);
	}

	public void OnValueChangedInputEndLevel(string recvString)
	{
		int.TryParse(recvString, out TestEndLevel);
	}

	public void OnValueChangedInputTimeScale(string recvString)
	{
		float.TryParse(recvString, out TestTimeScale);
	}
}
