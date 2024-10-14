/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolTopMenu : MonoBehaviour
	{
		public Button ButtonSaveMobile;

		public Button ButtonPlay;

		private bool clearAllMap;

		private readonly Dictionary<GoalTarget, GameObject> dicModeSubMenu = new Dictionary<GoalTarget, GameObject>();

		public Dropdown DropDownEpisode;

		public Dropdown DropDownGameMode;

		public Dropdown DropDownLevel;

		public Dropdown DropDownMoveCount;

		public Image[] ImageCollectBlock = new Image[MapData.MaxCollect];

		public InputField[] InputTextScore = new InputField[3];

		public GameObject ModeSubMenuBonus;

		public GameObject ModeSubMenuBringDown;

		public GameObject ModeSubMenuDigging;

		public GameObject ModeSubMenuRescueVS;

		public GameObject ModeSubMenuSweetRoad;

		private int newEpisodeSubLevelIndex;

		private int newEpisodeSubMapIndex;

		public GameObject[] ObjCollectInfos = new GameObject[MapData.MaxCollect];

		public GameObject ObjScorePanel;

		private int reqSaveMapIndex;

		public Text[] TextCollectCount = new Text[MapData.MaxCollect];

		public Text TextEditorLogPrev;

		public Text TextEditorLogLast;

		private void InitUI()
		{
			DropDownLevel.options.Clear();
			for (int i = 0; i < 50; i++)
			{
				DropDownLevel.options.Add(new Dropdown.OptionData("LEVEL " + (i + 1)));
			}
			DropDownGameMode.options.Clear();
			for (int j = -1; j < 11; j++)
			{
				List<Dropdown.OptionData> options = DropDownGameMode.options;
				GoalTarget goalTarget = (GoalTarget)j;
				options.Add(new Dropdown.OptionData(goalTarget.ToString()));
			}
			DropDownMoveCount.options.Clear();
			for (int k = 0; k < 50; k++)
			{
				DropDownMoveCount.options.Add(new Dropdown.OptionData(k + 1 + " MOVE"));
			}
			for (int l = 0; l < ObjCollectInfos.Length; l++)
			{
				ObjCollectInfos[l].SetActive(value: false);
			}
			Text textEditorLogLast = TextEditorLogLast;
			string empty = string.Empty;
			TextEditorLogPrev.text = empty;
			textEditorLogLast.text = empty;
		}

		public void ResetEpisodeList(int maxSize)
		{
			DropDownEpisode.options.Clear();
			for (int i = 0; i < maxSize; i++)
			{
				DropDownEpisode.options.Add(new Dropdown.OptionData($"{i + 1} ({i * 45 + 1}~{(i + 1) * 45})"));
			}
			ResetLevelList(1, 1);
		}

		public void ResetLevelList(int episode, int startLevel)
		{
			DropDownLevel.options.Clear();
			int num = 0;
			for (int i = 0; i < MapData.LEVEL_NORMAL_COUNT_PER_EPISODE; i++)
			{
				string text = (i + 1).ToString();
				text = "LEVEL " + (startLevel + i);
				DropDownLevel.options.Add(new Dropdown.OptionData(text));
				num = startLevel + i;
				if (MapData.dicServerGameData.ContainsKey(num) && MapData.dicServerGameData[num].game_mode == -1)
				{
					DropDownLevel.options[i].text += " X";
				}
			}
			MapData.main.gid = 0;
			RefreshSaveAndPlayButtonInteractive();
		}

		private void Start()
		{
			dicModeSubMenu.Add(GoalTarget.RescueVS, ModeSubMenuRescueVS);
			dicModeSubMenu.Add(GoalTarget.SweetRoad, ModeSubMenuSweetRoad);
			dicModeSubMenu.Add(GoalTarget.BringDown, ModeSubMenuBringDown);
			dicModeSubMenu.Add(GoalTarget.BonusRoulette, ModeSubMenuBonus);
			dicModeSubMenu.Add(GoalTarget.BonusScore, ModeSubMenuBonus);
			dicModeSubMenu.Add(GoalTarget.Digging, ModeSubMenuDigging);
			dicModeSubMenu.Add(GoalTarget.CollectCracker, ModeSubMenuBringDown);
			InitUI();
		}

		public void Reset()
		{
			DropDownGameMode.value = (int)(MapData.main.target + 1);
			DropDownMoveCount.value = MapData.main.moveCount - 1;
			for (int i = 0; i < 3; i++)
			{
				InputTextScore[i].text = MapData.main.scorePoint[i].ToString();
			}
		}

		public void RefreshCollect()
		{
			for (int i = 0; i < MapData.MaxCollect; i++)
			{
				if (MapData.main.collectBlocks[i].count == 0)
				{
					ObjCollectInfos[i].SetActive(value: false);
					continue;
				}
				ObjCollectInfos[i].SetActive(value: true);
				TextCollectCount[i].text = MapData.main.collectBlocks[i].count.ToString();
				if (MonoSingleton<MapToolManager>.Instance.GetBlockSprite(MapData.main.collectBlocks[i].blockType) != null)
				{
					ImageCollectBlock[i].sprite = MonoSingleton<MapToolManager>.Instance.GetBlockSprite(MapData.main.collectBlocks[i].blockType);
				}
				else
				{
					ObjCollectInfos[i].SetActive(value: false);
				}
			}
		}

		public void ToggleScoreDetailPanel(bool changed)
		{
			ObjScorePanel.SetActive(changed);
		}

		public void OnValueChangedInputScore(int index)
		{
			if (!string.IsNullOrEmpty(InputTextScore[index].text))
			{
				int.TryParse(InputTextScore[index].text, out MapData.main.scorePoint[index]);
			}
		}

		public void OnPressButtonSaveMobile()
		{
			MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonYesNo, "Save", "[Mobile] Save ?", null, OnPopupSaveYes);
		}

		private void OnPopupSaveYes()
		{
			OnPressButtonSave();
		}

		private void OnPressButtonSave()
		{
			MonoSingleton<UIManager>.Instance.ShowLoading();
			if (MapData.main.SubMapCount > 1 && MapData.main.target != GoalTarget.SweetRoad && MapData.main.target != GoalTarget.Digging)
			{
				MapData.main.SetCurrentOneMap();
			}
			clearAllMap = false;
			if (MapData.main.SubMapCount < MapData.main.receivedServerMapCount)
			{
				clearAllMap = true;
				for (int i = 0; i < MapData.main.SubMapCount; i++)
				{
					MapData.main.listBoardData[i].isNewMap = true;
				}
			}
			ReqPacketSaveGameData reqPacketSaveGameData = new ReqPacketSaveGameData(MapData.main);
			reqSaveMapIndex = 0;
			reqPacketSaveGameData.type = ((!MapData.main.isNewLevel) ? "UP" : "IN");
			NetRequestSaveGameData.Request(reqPacketSaveGameData, RecvSaveGameData);
		}

		private void RecvSaveGameData(ResPacketSaveGameData res)
		{
			if (clearAllMap)
			{
				ReqPacketClearAllMapData req = new ReqPacketClearAllMapData(MapData.main.gid);
				NetRequestSaveMapData.RequestClearAllMap(req, RecvClearAllMapData);
			}
			else
			{
				RequestSaveMapData(reqSaveMapIndex);
			}
		}

		private void RequestSaveMapData(int subMapIndex)
		{
			ReqPacketSaveMapData req = new ReqPacketSaveMapData(MapData.main.gid, (!MapData.main.listBoardData[subMapIndex].isNewMap) ? (subMapIndex + 1) : 0, MapData.main.target, MapData.main.listBoardData[subMapIndex]);
			MapData.main.listBoardData[subMapIndex].isNewMap = false;
			NetRequestSaveMapData.Request(req, RecvSaveMapData);
		}

		private void RecvClearAllMapData(ResPacketSaveMapData res)
		{
			RequestSaveMapData(reqSaveMapIndex);
		}

		private void RecvSaveMapData(ResPacketSaveMapData res)
		{
			if (++reqSaveMapIndex < MapData.main.SubMapCount)
			{
				RequestSaveMapData(reqSaveMapIndex);
			}
			else
			{
				NetRequestLoadGameData.Request(MapData.main.gid, RecvReloadCurrentGameData);
			}
		}

		private void RecvReloadCurrentGameData(ResPacketLoadGameData res)
		{
			MapData.main.isNewLevel = false;
			if (res == null || res.game == null || res.game.Length == 0)
			{
				MonoSingleton<UIManager>.Instance.HideLoading();
				MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Reload game data error (Network)");
			}
			else
			{
				MapData.UpdateServerGameData(MapData.main.gid, res);
				MapData.main.receivedServerMapCount = MapData.dicServerGameData[MapData.main.gid].map_count;
				NetRequestLoadMapData.Request(MapData.main.gid, RecvReloadCurrentMapData);
			}
		}

		private void RecvReloadCurrentMapData(ResPacketLoadMapData res)
		{
			if (res == null || res.map == null || res.map.Length == 0)
			{
				MonoSingleton<UIManager>.Instance.HideLoading();
				MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Reload map data error (Network)");
				return;
			}
			MapData.UpdateServerMapData(MapData.main.gid, res);
			MonoSingleton<UIManager>.Instance.HideLoading();
			if (DropDownLevel.options[DropDownLevel.value].text[DropDownLevel.options[DropDownLevel.value].text.Length - 1] == 'X')
			{
				Text captionText = DropDownLevel.captionText;
				string text = DropDownLevel.options[DropDownLevel.value].text.Substring(0, DropDownLevel.options[DropDownLevel.value].text.Length - 2);
				DropDownLevel.options[DropDownLevel.value].text = text;
				captionText.text = text;
			}
			MonoSingleton<MapToolManager>.Instance.SetMessageLog("MapData Saved");
		}

		public void OnPressButtonGamePlay()
		{
			SoundSFX.Play(SFXIndex.ButtonClick);
			if (CheckTestPlay())
			{
				//MonoSingleton<MapToolManager>.Instance.InitTestGamePlay();
				//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Game, SceneChangeEffect.Color);
			}
		}

		public bool CheckTestPlay()
		{
			int num = 0;
			GoalTarget target = MapData.main.target;
			if (target == GoalTarget.SweetRoad)
			{
				bool flag = false;
				for (num = 0; num < MapData.main.SubMapCount; num++)
				{
					for (int i = 0; i < MapData.main.listBoardData[num].width; i++)
					{
						if (flag)
						{
							break;
						}
						for (int j = 0; j < MapData.main.listBoardData[num].height; j++)
						{
							if (flag)
							{
								break;
							}
							if (MapData.main.listBoardData[num].rockCandyTile[i, j] > 0)
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "0 BackTile");
						return false;
					}
				}
			}
			return true;
		}

		public void RefreshSaveAndPlayButtonInteractive()
		{
			ButtonSaveMobile.interactable = (MapData.main.target != GoalTarget.NotAssign);
			ButtonPlay.interactable = (MapData.main.target != GoalTarget.NotAssign);
		}

		public void OnSelectDropDownLevel(int index)
		{
			int value = DropDownEpisode.value;
			int gid = value * MapData.LEVEL_NORMAL_COUNT_PER_EPISODE + index + 1;
			ChangeLevel(gid);
		}

		public void ChangeLevel(int gid)
		{
			MapData.main = new MapData(gid);
			MonoSingleton<MapToolManager>.Instance.ChangeLevel();
			if (MapData.main.target == GoalTarget.SweetRoad)
			{
				MonoSingleton<MapToolManager>.Instance.modeParamMenu.ResetSubLevelList(MapData.main.SubMapCount);
			}
			else if (MapData.main.target == GoalTarget.Digging)
			{
				MonoSingleton<MapToolManager>.Instance.modeParamMenu.ResetSubLevelListDiggingMode(MapData.main.SubMapCount);
				MonoSingleton<MapToolManager>.Instance.modeParamMenu.SetDiggingDirection(MapData.main.diggingScrollDirection);
			}
			Reset();
			RefreshCollect();
			RefreshSaveAndPlayButtonInteractive();
		}

		public void OnSelectDropDownEpisode(int index)
		{
			ResetLevelList(index + 1, index * MapData.LEVEL_NORMAL_COUNT_PER_EPISODE + 1);
			OnSelectDropDownLevel(0);
			if (DropDownLevel.value != 0)
			{
				DropDownLevel.value = 0;
			}
			DropDownLevel.RefreshShownValue();
		}

		public void OnPressButtonNewEpisode()
		{
			MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonYesNo, "Really?", "Makes 45Level. Proces ?", null, OnPressNewEpisodeYes);
		}

		public void OnPressNewEpisodeYes()
		{
			MonoSingleton<UIManager>.Instance.ShowLoading();
			newEpisodeSubLevelIndex = (newEpisodeSubMapIndex = 0);
			MonoSingleton<MapToolManager>.Instance.MapDataReset();
			SendNewEpisode();
		}

		private void SendNewEpisode()
		{
			if (newEpisodeSubLevelIndex < MapData.LEVEL_NORMAL_COUNT_PER_EPISODE)
			{
				int num = MapData.MaxEpisode + 1;
				int num2 = 0;
				MapData mapData = new MapData();
				num2 = (mapData.gid = 1 + MapData.MaxEpisode * MapData.LEVEL_NORMAL_COUNT_PER_EPISODE + newEpisodeSubLevelIndex);
				mapData.target = GoalTarget.NotAssign;
				ReqPacketSaveGameData reqPacketSaveGameData = new ReqPacketSaveGameData(mapData);
				reqPacketSaveGameData.type = "IN";
				NetRequestSaveGameData.Request(reqPacketSaveGameData, WaitNetworkNewEpisode);
			}
			else if (newEpisodeSubMapIndex < MapData.LEVEL_NORMAL_COUNT_PER_EPISODE)
			{
				int num3 = 0;
				num3 = 1 + MapData.MaxEpisode * MapData.LEVEL_NORMAL_COUNT_PER_EPISODE + newEpisodeSubMapIndex;
				ReqPacketSaveMapData req = new ReqPacketSaveMapData(num3, 0, GoalTarget.NotAssign, MapData.main.listBoardData[0]);
				NetRequestSaveMapData.Request(req, WaitNetworkNewEpisodeSendMap);
			}
			else
			{
				MonoSingleton<UIManager>.Instance.HideLoading();
				MonoSingleton<MapToolManager>.Instance.WaitLoadData();
			}
		}

		private void WaitNetworkNewEpisode(ResPacketSaveGameData res)
		{
			newEpisodeSubLevelIndex++;
			SendNewEpisode();
		}

		private void WaitNetworkNewEpisodeSendMap(ResPacketSaveMapData res)
		{
			newEpisodeSubMapIndex++;
			SendNewEpisode();
		}

		public void OnPressButtonPrevLevel()
		{
			DropDownLevel.value--;
		}

		public void OnPressButtonNextLevel()
		{
			DropDownLevel.value++;
		}

		public void OnSelectDropDownGameMode(int index)
		{
			GoalTarget key = (GoalTarget)(index - 1);
			MapData.main.target = (GoalTarget)(index - 1);
			foreach (GoalTarget key2 in dicModeSubMenu.Keys)
			{
				if (dicModeSubMenu[key2] != null)
				{
					dicModeSubMenu[key2].SetActive(value: false);
				}
			}
			if (dicModeSubMenu.ContainsKey(key))
			{
				dicModeSubMenu[key].SetActive(value: true);
			}
			RefreshSaveAndPlayButtonInteractive();
		}

		public void OnSelectDropDownMoveCount(int index)
		{
			MapData.main.moveCount = (MapData.main.orgMoveCount = index + 1);
		}
	}
}
