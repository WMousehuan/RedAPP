
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using PathologicalGames;
using System.Collections;
using UnityEngine;

public class SceneGame : SceneClass
{
	private GameObject ObjPoolGameCharacterEffectGretel;

	private GameObject ObjPoolGameCharacterEffectWitch;

	private GameObject ObjPoolGamePlaying;

	public GameObject PrefabGamePlayingPool;

	//public Camera cameraGame;




	public override IEnumerator OnSceneShow()
	{
		//Debug.Log("Chay vao DAYYYAAAAAA");
		Application.targetFrameRate = GlobalSetting.LOW_FPS;
		ObjPoolGamePlaying = Object.Instantiate(PrefabGamePlayingPool);
        if (MonoSingleton<SceneControlManager>.Instance.OldSceneType == SceneType.MapTool)
        {
            //Debug.Log("Chay vao DAYYYAAAAAA");
            GameMain.main.StartProto(MapData.main);
			tk2dCamera component = Camera.main.GetComponent<tk2dCamera>();
			//Debug.Log("Chay vao DAYYYAAAAAA:" + component.TargetResolution.ToString()) ;
			if ((bool)component)
			{
				Vector2 targetResolution = component.TargetResolution;
				//Debug.Log("Chay vao DAYYYAAAAAA: " + targetResolution.ToString());
				if (targetResolution.x == 1900)//1900
				{
					Vector2 targetResolution2 = component.TargetResolution;
					if (targetResolution2.y == 900)//900
					{
						//Camera.main.GetComponent<tk2dCamera>().CameraSettings.orthographicPixelsPerMeter = 1.0f;
						//cameraGame.orthographicSize = 700f;
					}
				}
			}
        }
        else
        {
           // cameraGame.orthographicSize = 600f;
            GameMain.main.StartProto(MapData.main.gid);
        }

        yield return null;
	}

	public override void OnSceneHideStart()
	{
		BoardManager.main.RemoveBoard();
		if (PoolManager.Pools != null && PoolManager.Pools.Count > 0)
		{
			if (PoolManager.IsEnablePoolGameEffect)
			{
				PoolManager.PoolGameEffect.DespawnAll();
			}
			if (PoolManager.IsEnablePoolGameBlocks)
			{
				PoolManager.PoolGameBlocks.DespawnAll();
			}
		}
	}

	public override void OnSceneHideEnd()
	{
		if ((bool)ObjPoolGamePlaying)
		{
			UnityEngine.Object.Destroy(ObjPoolGamePlaying);
		}
		if ((bool)ObjPoolGameCharacterEffectGretel)
		{
			UnityEngine.Object.Destroy(ObjPoolGameCharacterEffectGretel);
		}
		if ((bool)ObjPoolGameCharacterEffectWitch)
		{
			UnityEngine.Object.Destroy(ObjPoolGameCharacterEffectWitch);
		}
	}
}
