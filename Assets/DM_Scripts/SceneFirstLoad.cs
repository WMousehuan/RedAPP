/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using OneSignalSDK;
public class SceneFirstLoad : SceneClass
{
	private void Start()
	{
		// Replace 'YOUR_ONESIGNAL_APP_ID' with your OneSignal App ID from app.onesignal.com
		OneSignal.Initialize("YOUR_ONESIGNAL_APP_ID");
		Application.targetFrameRate = GlobalSetting.FPS;
		MonoSingleton<SceneControlManager>.Instance.CurrentScene = this;
		MonoSingleton<SceneControlManager>.Instance.CurrentSceneType = SceneType.FirstLoad;
		MonoSingleton<UIManager>.Instance.HideCoinCurrentMenuLayer();
		MonoSingleton<GameDataLoadManager>.Instance.MoveToMenuScene();
	}
}
