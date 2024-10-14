
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoSingleton<LobbyManager>
{
	private bool isFirstLoad = false;

	private bool isInit;

	public static bool afterFailed;

	//public Text currentLevel;

	public override void Awake()
	{
        isFirstLoad = false;
        base.Awake();
	}

	private void Start()
	{
		//currentLevel.text = "Level " +MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo.ToString();
		//if (!PlayerPrefs.HasKey("FirstInstalledVersion"))
		//{
		//	PlayerPrefs.SetString("FirstInstalledVersion", GlobalSetting.ConfigData.AppVersion);
		//}
	}

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine(OnEnableCoroutine());
	}

	public void OnEventRecvLevelData()
	{
	}

	private void OnDestroy()
	{
	}

	private IEnumerator OnEnableCoroutine()
	{
		isInit = true;
		isFirstLoad = false;
		yield return null;
	}

	public void OnPressStartLevel()
	{
		//SoundSFX.Play(SFXIndex.ButtonClick);
		//MonoSingleton<PlayerDataManager>.Instance.lastPlayedLevel = MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo;
		//MapData.main = new MapData(MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo);
		//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Game, SceneChangeEffect.Color);
		//GameMain.CompleteGameStart();
	}

	private void Update()
	{
		if (isInit)
		{
		}
	}

	private IEnumerator processStageClearEffect()
	{
		yield return null;
		yield return null;
	}
}
