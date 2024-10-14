/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolOptionMenu : MonoBehaviour
	{
		public Button ButtonLevelPaste;

		private int clipBoardCopyLevel;

		public Text TextBuildNumber;

		public Toggle ToggleShowButton;

		public Transform ViewPanel;

		private void Start()
		{
			if ((float)Screen.height / (float)Screen.width == 0.75f || MonoSingleton<PlayerDataManager>.Instance.IsIPad)
			{
				ViewPanel.localPosition = new Vector3(34f, 0f, 0f);
				ToggleShowButton.isOn = false;
			}
			else
			{
				ToggleShowButton.isOn = true;
			}
//			TextBuildNumber.text = BuildNumber.GetSVNRevision();
			MonoSingleton<MapToolManager>.Instance.CurrentSelectEpisodeIndex = MonoSingleton<MapToolManager>.Instance.topMenu.DropDownEpisode.value;
			MonoSingleton<MapToolManager>.Instance.CurrentSelectLevelIndex = MonoSingleton<MapToolManager>.Instance.topMenu.DropDownLevel.value;
			MapData.MaxEpisode = 0;
			MonoSingleton<MapToolManager>.Instance.WaitLoadData();
		}

		private void Update()
		{
		}

		public void OnPressShowButton(bool changed)
		{
			if (changed)
			{
				ViewPanel.DOLocalMoveX(-159f, 0.5f);
			}
			else
			{
				ViewPanel.DOLocalMoveX(34f, 0.5f);
			}
		}

		public void OnPressLevelCopy()
		{
			clipBoardCopyLevel = MapData.main.gid;
			ButtonLevelPaste.interactable = true;
			MonoSingleton<MapToolManager>.Instance.SetMessageLog($"{MapData.main.gid} test");
		}

		public void OnPressLevelPaste()
		{
			MapData.CopyAndPasteGameData(MapData.main.gid, clipBoardCopyLevel);
			MonoSingleton<MapToolManager>.Instance.SetMessageLog($"{clipBoardCopyLevel} check");
		}
	}
}
