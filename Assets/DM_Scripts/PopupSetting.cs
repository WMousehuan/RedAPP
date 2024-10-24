
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : Popup
{
	public GameObject buttonHome;

	public GameObject buttonQuit;


	public GameObject ObjGroupOption;

	public Toggle ToggleSoundBGMButton;

	public Toggle ToggleSoundEffectButton;



	public override void Start()
	{
		base.Start();
	}

	public void SetPopup(UIOptionButton.OptionMenuType type)
	{
	
        if (type == UIOptionButton.OptionMenuType.Lobby)
		{
			
			buttonHome.SetActive(value: false);
			buttonQuit.SetActive(value: true);
			
			ObjGroupOption.transform.localPosition = Vector3.zero;
		}
		else
		{
			buttonHome.SetActive(value: true);
			buttonQuit.SetActive(value: false);
			
			ObjGroupOption.transform.localPosition = Vector3.zero;
        }
      
	}

	public void Update()
	{
	}

	public override void OnEnable()
	{
		base.OnEnable();
		ToggleSoundBGMButton.isOn = !MonoSingleton<PlayerDataManager>.Instance.IsOnSoundBGM;
		ToggleSoundEffectButton.isOn = !MonoSingleton<PlayerDataManager>.Instance.IsOnSoundEffect;
	}

	public void OnToggleSoundBGMButton(bool changed)
	{
		SoundSFX.Play(SFXIndex.ButtonClick);
		MonoSingleton<PlayerDataManager>.Instance.IsOnSoundBGM = !changed;
		if (MonoSingleton<PlayerDataManager>.Instance.IsOnSoundBGM)
		{
			SoundManager.SetVolumeMusic(1f);
		}
		else
		{
			SoundManager.SetVolumeMusic(0f);
		}
		MonoSingleton<PlayerDataManager>.Instance.SaveOptionSound();
	}

	public void OnToggleSoundEffectButton(bool changed)
	{
		MonoSingleton<PlayerDataManager>.Instance.IsOnSoundEffect = !changed;
		SoundManager.Instance.offTheSFX = changed;
		if (!MonoSingleton<PlayerDataManager>.Instance.IsOnSoundEffect)
		{
			SoundManager.StopSFX();
		}
		MonoSingleton<PlayerDataManager>.Instance.SaveOptionSound();
		SoundSFX.Play(SFXIndex.ButtonClick);
	}


	public void OnPressTitle()
	{
		//Debug.Log("SCENE :" + MonoSingleton<SceneControlManager>.Instance.CurrentSceneType);
		SoundSFX.Play(SFXIndex.ButtonClick);
		//Application.Quit();
		if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Title)
		{
			//Debug.Log("QUIT");
			Application.Quit();
        }
        else 
		MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Title, SceneChangeEffect.Color);
	}

	public void OnPressQuit()
	{
		//Debug.Log("QUIT");
		SoundSFX.Play(SFXIndex.ButtonClick);
		Application.Quit();
		//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Title, SceneChangeEffect.Color);
	}

	public void OnPressGameQuit()
	{
		
		SoundSFX.Play(SFXIndex.ButtonClick);
		if (!GameMain.main.isFirstBoardSetting && MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Game)
		{
			GameMain.main.OnPressButtonExit();
        }
        
	}

	
}
