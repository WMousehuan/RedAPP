
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolManager : MonoSingleton<MapToolManager>
	{
		public static bool IsMapTool;

		private static readonly int BOARD_MAX_ROW = MapData.MaxHeight;

		private static readonly int BOARD_MAX_COL = MapData.MaxWidth;

		public Transform BoardMovingInfoInCell;

		public Transform BoardParent;

		public Transform BoardRotationInfoInCell;

		public Transform BoardRotationInfoInGrid;

		public int CurrentSelectEpisodeIndex;

		public int CurrentSelectLevelIndex;

		public Dictionary<BoardPosition, MapToolMovingButton> dicCellMovingButton = new Dictionary<BoardPosition, MapToolMovingButton>();

		public Dictionary<BoardPosition, MapToolRotationButton> dicCellRotationButton = new Dictionary<BoardPosition, MapToolRotationButton>();

		public Dictionary<BoardPosition, MapToolRotationButton> dicGridRotationButton = new Dictionary<BoardPosition, MapToolRotationButton>();

		public Dictionary<string, GameObject> dicPrefabRescueGingerMan = new Dictionary<string, GameObject>();

		protected Dictionary<string, Sprite> dicSpriteBlockList = new Dictionary<string, Sprite>();

		public MapToolGeneratorMenu generatorMenu;

		public MapToolGeneratorSpecialMenu generatorSpecialMenu;

		public Transform GroupMovingPoint;

		public Transform GroupRotationPoint;

		public Image ImageSelectedItemInfo;

		private bool isReadyNextRail;

		private readonly List<MapToolSlot> listGeneratorSlot = new List<MapToolSlot>();

		private readonly List<MapToolSlot> listGeneratorSpecialSlot = new List<MapToolSlot>();

		public Canvas MainCanvas;

		public MapToolMainMenuCollect mainMenuCollect;

		public MapToolMainMenuDrop mainMenuDrop;

		public MapToolMainMenuMode mainMenuMode;

		public MapToolMainMenuMode modeParamMenu;

		public GameObject ObjBlockList;

		private GameObject[] objBoardSlots;

		private GameObject objCurrentMainMenuTab;

		public GameObject ObjGroupLayerMenu;

		public GameObject ObjParentRescueGingerMan;

		public GameObject ObjSelectedItemInfo;

		public MapToolSelectObjectObstacle obsMenu;

		public MapToolOptionMenu optionMenu;

		public GameObject PrefabGeneratorIndex;

		public GameObject PrefabRailNextText;

		public GameObject[] PrefabRescueGingerManSize;

		private GameObject prevInputDownSlot;

		[HideInInspector]
		public MapToolMainMenu.MenuType SelectedMenuType;

		private BoardPosition selectedRailPosition;

		[HideInInspector]
		public string SelectedSpriteKey;

		public MapToolMainMenuSlotSubMenuMoving slotMovingMenu;

		public MapToolMainMenuSlotSubMenuRotation slotRotationMenu;

		public Sprite[] SpriteDropDirection;

		public Text TextMessageLog;

		public float throwFlyingTime = 1f;

		public MapToolTopMenu topMenu;

		private Color tweenMessageColor = Color.white;

		private float tweenMessageTime = -1f;

		public Camera UICamera;

		private int numberChocolateInOrder = 1;

		private int selectedDropListGeneratorNo;

		private int selectedDropListGeneratorSpecialNo;

		public MapToolSlot[] SlotsInfo
		{
			get;
			private set;
		}

		public Sprite GetBlockSprite(string spriteKey)
		{
			if (!dicSpriteBlockList.ContainsKey(spriteKey))
			{
				return null;
			}
			return dicSpriteBlockList[spriteKey];
		}

		public Sprite GetBlockSprite(ChipType chipType, int chipID)
		{
			return GetBlockSprite(MapData.GetChipJsonFormat(chipType, chipID));
		}

		public Sprite GetBlockSprite(IBlockType blockType)
		{
			return GetBlockSprite(MapData.GetBlockJsonFormat(blockType));
		}

		public override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
		}

		public void InitTestGamePlay()
		{
			for (int i = 0; i < MapData.main.SubMapCount; i++)
			{
				MapData.main.listBoardData[i].orgSlots = new bool[MapData.main.listBoardData[i].width, MapData.main.listBoardData[i].height];
				Array.Copy(MapData.main.listBoardData[i].slots, MapData.main.listBoardData[i].orgSlots, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				MapData.main.listBoardData[i].orgChips = new int[MapData.main.listBoardData[i].width, MapData.main.listBoardData[i].height];
				Array.Copy(MapData.main.listBoardData[i].chips, MapData.main.listBoardData[i].orgChips, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				MapData.main.listBoardData[i].orgBlocks = new IBlockType[MapData.main.listBoardData[i].width, MapData.main.listBoardData[i].height];
				Array.Copy(MapData.main.listBoardData[i].blocks, MapData.main.listBoardData[i].orgBlocks, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				MapData.main.listBoardData[i].orgTunnel = new int[MapData.main.listBoardData[i].width, MapData.main.listBoardData[i].height];
				Array.Copy(MapData.main.listBoardData[i].tunnel, MapData.main.listBoardData[i].orgTunnel, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				MapData.main.listBoardData[i].orgRescueGingerManSize = new RescueGingerManSize[MapData.main.listBoardData[i].width, MapData.main.listBoardData[i].height];
				Array.Copy(MapData.main.listBoardData[i].rescueGinerManSize, MapData.main.listBoardData[i].orgRescueGingerManSize, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
			}
		}

		public void EndTestGamePlay()
		{
			for (int i = 0; i < MapData.main.SubMapCount; i++)
			{
				Array.Copy(MapData.main.listBoardData[i].orgSlots, MapData.main.listBoardData[i].slots, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				Array.Copy(MapData.main.listBoardData[i].orgChips, MapData.main.listBoardData[i].chips, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				Array.Copy(MapData.main.listBoardData[i].orgBlocks, MapData.main.listBoardData[i].blocks, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				Array.Copy(MapData.main.listBoardData[i].orgTunnel, MapData.main.listBoardData[i].tunnel, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
				Array.Copy(MapData.main.listBoardData[i].orgRescueGingerManSize, MapData.main.listBoardData[i].rescueGinerManSize, MapData.main.listBoardData[i].width * MapData.main.listBoardData[i].height);
			}
		}

		private void RecvGameData(ResPacketLoadGameData res)
		{
			MapData.ReceiveServerGameData(res.game);
			NetRequestLoadMapData.Request(RecvMapData);
		}

		private void RecvMapData(ResPacketLoadMapData res)
		{
			MapData.ReceiveServerMapData(res.map);
			topMenu.ResetEpisodeList(MapData.MaxEpisode);
			if (CurrentSelectEpisodeIndex >= MapData.MaxEpisode)
			{
				MonoSingleton<UIManager>.Instance.HideLoading();
				MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonInfo, "Error", "해당 에피소드가 없어 1 에피소드로 이동합니다.");
				CurrentSelectEpisodeIndex = 0;
				CurrentSelectLevelIndex = 0;
				topMenu.DropDownEpisode.value = CurrentSelectEpisodeIndex;
			}
			topMenu.OnSelectDropDownEpisode(CurrentSelectEpisodeIndex);
			topMenu.DropDownLevel.value = CurrentSelectLevelIndex;
			MonoSingleton<UIManager>.Instance.HideLoading();
		}

		public void WaitLoadData()
		{
			MonoSingleton<UIManager>.Instance.ShowLoading();
			NetRequestLoadGameData.Request(RecvGameData);
		}

		public void ChangeLevel()
		{
			Reset();
		}

		public void ChangeSubMap()
		{
		}

		public void MapDataReset()
		{
			MapData.main.Reset();
			MapData.main.MapToolReset();
			Reset();
		}

		public void Reset()
		{
			ObjSelectedItemInfo.SetActive(value: false);
			SelectedSpriteKey = string.Empty;
			topMenu.Reset();
			mainMenuCollect.Reset();
			ResetCurrentBoardData();
		}

		public void ResetCurrentBoardData()
		{
			MapBoardData currentMapBoardData = MapData.main.CurrentMapBoardData;
			BoardPosition key = default(BoardPosition);
			modeParamMenu.SetBringDownKeepShowObject(MapData.main.keepShowBringDownObjectCount);
			MapToolSlot[] slotsInfo = SlotsInfo;
			foreach (MapToolSlot mapToolSlot in slotsInfo)
			{
				mapToolSlot.Reset();
			}
			foreach (MapToolRotationButton value in dicCellRotationButton.Values)
			{
				value.Deselect();
			}
			foreach (MapToolRotationButton value2 in dicGridRotationButton.Values)
			{
				value2.Deselect();
			}
			foreach (MapToolMovingButton value3 in dicCellMovingButton.Values)
			{
				value3.Deselect();
			}
			mainMenuDrop.Reset();
			selectedDropListGeneratorNo = 0;
			listGeneratorSlot.Clear();
			listGeneratorSpecialSlot.Clear();
			generatorMenu.Reset();
			generatorSpecialMenu.Reset();
			mainMenuMode.Reset();
			if (currentMapBoardData.gateEnterX != -1 && currentMapBoardData.gateEnterY != -1)
			{
				SlotsInfo[currentMapBoardData.gateEnterY * BOARD_MAX_COL + currentMapBoardData.gateEnterX].SetRoadGate(isGateEnter: true);
			}
			if (currentMapBoardData.gateExitX != -1 && currentMapBoardData.gateExitY != -1)
			{
				SlotsInfo[currentMapBoardData.gateExitY * BOARD_MAX_COL + currentMapBoardData.gateExitX].SetRoadGate(isGateEnter: false);
			}
			if (currentMapBoardData.tutorial1X != -1 && currentMapBoardData.tutorial1Y != -1)
			{
				SlotsInfo[currentMapBoardData.tutorial1Y * BOARD_MAX_COL + currentMapBoardData.tutorial1X].SetTutorial();
			}
			if (currentMapBoardData.tutorial2X != -1 && currentMapBoardData.tutorial2Y != -1)
			{
				SlotsInfo[currentMapBoardData.tutorial2Y * BOARD_MAX_COL + currentMapBoardData.tutorial2X].SetTutorial();
			}
			for (int j = 0; j < MapData.MaxWidth; j++)
			{
				for (int k = 0; k < MapData.MaxHeight; k++)
				{
					int num = k * MapData.MaxWidth + j;
					if (!currentMapBoardData.slots[j, k])
					{
						SlotsInfo[num].SetNull(newSetValue: true);
					}
					else
					{
						SlotsInfo[num].SetNull(newSetValue: false);
						SlotsInfo[num].SetChip(MapData.GetChipTypeFromPower((Powerup)currentMapBoardData.powerUps[j, k]), currentMapBoardData.chips[j, k]);
						SlotsInfo[num].SetDirection(currentMapBoardData.dropDirection[j, k], currentMapBoardData.dropLock[j, k]);
						if (currentMapBoardData.blocks[j, k] != 0)
						{
							SlotsInfo[num].NumberChocolateIndex = currentMapBoardData.inOrder[j, k];
							SlotsInfo[num].SetObs(currentMapBoardData.blocks[j, k]);
						}
						key.x = j;
						key.y = k;
						if (currentMapBoardData.dicGeneratorDropBlock.ContainsKey(key))
						{
							SlotsInfo[num].SetGenerator();
							generatorMenu.RefreshFromMapData(j, k, listGeneratorSlot.Count - 1);
						}
						else if (currentMapBoardData.dicGeneratorSpecialDropBlock.ContainsKey(key))
						{
							SlotsInfo[num].SetGeneratorSpecial();
							generatorSpecialMenu.RefreshFromMapData(j, k, listGeneratorSpecialSlot.Count - 1);
						}
						if (currentMapBoardData.rockCandyTile[j, k] > 0)
						{
							SlotsInfo[num].SetRockCandyTile(currentMapBoardData.rockCandyTile[j, k]);
						}
						else
						{
							SlotsInfo[num].RemoveRockCandyTile();
						}
						if (currentMapBoardData.jellyTile[j, k])
						{
							SlotsInfo[num].SetJellyTile();
						}
						else
						{
							SlotsInfo[num].RemoveJellyTile();
						}
						if (currentMapBoardData.milkTile[j, k])
						{
							SlotsInfo[num].SetMilkTile();
						}
						else
						{
							SlotsInfo[num].RemoveMilkTile();
						}
						if (currentMapBoardData.bringDownGenerator[j, k])
						{
							SlotsInfo[num].SetBringDown(isStart: true);
						}
						else if (currentMapBoardData.bringDownGoal[j, k])
						{
							SlotsInfo[num].SetBringDown(isStart: false);
						}
						if (currentMapBoardData.rail[j, k] && SlotsInfo[num].SetRailInfo())
						{
							BoardPosition key2 = new BoardPosition(j, k);
							MapToolSlot obj = SlotsInfo[num];
							BoardPosition boardPosition = currentMapBoardData.dicRailNextPosition[key2];
							int x = boardPosition.x;
							BoardPosition boardPosition2 = currentMapBoardData.dicRailNextPosition[key2];
							obj.SetNextRail(x, boardPosition2.y);
						}
						if (currentMapBoardData.railImage[j, k] > 0)
						{
							SlotsInfo[num].SetRailImage("RI" + currentMapBoardData.railImage[j, k]);
						}
						SlotsInfo[num].RemoveTunnel();
						if (currentMapBoardData.tunnel[j, k] > 0)
						{
							SlotsInfo[num].SetTunnel("TN" + currentMapBoardData.tunnel[j, k]);
						}
						SlotsInfo[num].RemoveRibbon();
						if (currentMapBoardData.ribbon[j, k] > 0)
						{
							SlotsInfo[num].SetRibbon($"RBN{currentMapBoardData.ribbon[j, k]:00}");
						}
						SlotsInfo[num].RemoveKnot();
						if (currentMapBoardData.knot[j, k] > 0)
						{
							SlotsInfo[num].SetKnot("KNOT." + currentMapBoardData.knot[j, k]);
						}
						SlotsInfo[num].RemoveYarn();
						if (currentMapBoardData.yarn[j, k])
						{
							SlotsInfo[num].SetYarn(string.Format("YARN", currentMapBoardData.yarn[j, k]));
						}
						SlotsInfo[num].RemoveClothButton();
						if (currentMapBoardData.clothButton[j, k] > 0)
						{
							SlotsInfo[num].SetClothButton("CBTN." + currentMapBoardData.clothButton[j, k]);
						}
						if (currentMapBoardData.safeObsSlot[j, k])
						{
							SlotsInfo[num].SetSafeObs();
						}
						if (currentMapBoardData.wallsH[j, k])
						{
							SlotsInfo[num].SetWall(isH: true);
						}
						if (currentMapBoardData.wallsV[j, k])
						{
							SlotsInfo[num].SetWall(isH: false);
						}
					}
					if (currentMapBoardData.rescueGinerManSize[j, k] != 0)
					{
						SlotsInfo[num].SetRescueGingerMan("RB" + Utils.GetRescueGingerManSizeWidth(currentMapBoardData.rescueGinerManSize[j, k]) + Utils.GetRescueGingerManSizeHeight(currentMapBoardData.rescueGinerManSize[j, k]));
					}
				}
			}
			foreach (MapDataMovingSlot item in currentMapBoardData.listMovingSlot)
			{
				MapToolMainMenuSlotSubMenuMoving.MovingDirection movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.None;
				int moveCount = 0;
				if (item.targetX < item.startX)
				{
					movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.Left;
					moveCount = item.startX - item.targetX;
				}
				else if (item.targetX > item.startX)
				{
					movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.Right;
					moveCount = item.targetX - item.startX;
				}
				else if (item.targetY < item.startY)
				{
					movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.Down;
					moveCount = item.startY - item.targetY;
				}
				else if (item.targetY > item.startY)
				{
					movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.Up;
					moveCount = item.targetY - item.startY;
				}
				if (movingDirection != MapToolMainMenuSlotSubMenuMoving.MovingDirection.None)
				{
					key.x = item.startX;
					key.y = item.startY;
					if (dicCellMovingButton.ContainsKey(key))
					{
						dicCellMovingButton[key].Select(movingDirection, item.width, item.height, moveCount);
					}
				}
			}
			foreach (MapDataRotationSlot item2 in currentMapBoardData.listRotationSlot)
			{
				key.x = item2.centerX;
				key.y = item2.centerY;
				if (item2.isGrid)
				{
					if (dicGridRotationButton.ContainsKey(key))
					{
						dicGridRotationButton[key].Select(item2.size, item2.isClockwork);
					}
				}
				else if (dicCellRotationButton.ContainsKey(key))
				{
					dicCellRotationButton[key].Select(item2.size, item2.isClockwork);
				}
			}
			Toggle[] componentsInChildren = ObjGroupLayerMenu.GetComponentsInChildren<Toggle>();
			foreach (Toggle toggle in componentsInChildren)
			{
				toggle.isOn = true;
			}
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				prevInputDownSlot = null;
			}
			if (tweenMessageTime >= 0f)
			{
				TextMessageLog.color = tweenMessageColor;
				tweenMessageColor.a = Mathf.InverseLerp(0f, 4f, tweenMessageTime);
				tweenMessageTime -= Time.deltaTime;
			}
		}

		private void OnSelectSlot(BaseEventData eventData)
		{
			PointerEventData pointerEventData = eventData as PointerEventData;
			if (!(pointerEventData.pointerEnter != null) || !(prevInputDownSlot != pointerEventData.pointerEnter))
			{
				return;
			}
			prevInputDownSlot = pointerEventData.pointerEnter;
			string[] array = pointerEventData.pointerEnter.name.Split('x');
			if (array.Length != 2)
			{
				return;
			}
			int result = -1;
			int result2 = -1;
			if (!int.TryParse(array[0], out result) || !int.TryParse(array[1], out result2) || result == -1 || result2 == -1)
			{
				return;
			}
			int num = result * BOARD_MAX_COL + result2;
			if (pointerEventData.button == PointerEventData.InputButton.Right)
			{
				SlotsInfo[num].RemoveRescueGingerMan();
				MapData.main.CurrentMapBoardData.SetChip(result2, result, ChipType.None, 0);
				SlotsInfo[num].RemoveObs();
				MapData.main.CurrentMapBoardData.RemoveRescueBear(result2, result);
				MapData.main.CurrentMapBoardData.tunnel[result2, result] = 0;
				MapData.main.CurrentMapBoardData.RemoveGeneratorDropList(new BoardPosition(result2, result));
				MapData.main.CurrentMapBoardData.RemoveGeneratorSpecialDropList(new BoardPosition(result2, result));
				MapData.main.CurrentMapBoardData.bringDownGenerator[result2, result] = (MapData.main.CurrentMapBoardData.bringDownGoal[result2, result] = false);
				MapData.main.CurrentMapBoardData.ribbon[result2, result] = 0;
				MapData.main.CurrentMapBoardData.knot[result2, result] = 0;
				MapData.main.CurrentMapBoardData.yarn[result2, result] = false;
				MapData.main.CurrentMapBoardData.clothButton[result2, result] = 0;
				MapData.main.CurrentMapBoardData.RemoveRail(result2, result);
				MapData.main.CurrentMapBoardData.tunnel[result2, result] = 0;
				MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] = 0;
				MapData.main.CurrentMapBoardData.jellyTile[result2, result] = false;
				MapData.main.CurrentMapBoardData.wallsH[result2, result] = false;
				MapData.main.CurrentMapBoardData.wallsV[result2, result] = false;
				SlotsInfo[num].Reset();
			}
			else
			{
				if (string.IsNullOrEmpty(SelectedSpriteKey))
				{
					return;
				}
				if (SelectedMenuType == MapToolMainMenu.MenuType.Block)
				{
					if (SelectedSpriteKey == "Eraser")
					{
						SlotsInfo[num].SetEraseChip();
						MapData.main.CurrentMapBoardData.SetChip(result2, result, ChipType.None, 0);
						return;
					}
					if (MapData.GetBlockLayerNo(MapData.main.CurrentMapBoardData.blocks[result2, result]) != 2)
					{
						SlotsInfo[num].RemoveObs();
					}
					ChipType chipTypeFromString = MapData.GetChipTypeFromString(SelectedSpriteKey);
					int chipColorFromString = MapData.GetChipColorFromString(SelectedSpriteKey);
					SlotsInfo[num].SetChip(chipTypeFromString, chipColorFromString);
					MapData.main.CurrentMapBoardData.SetChip(result2, result, chipTypeFromString, chipColorFromString);
				}
				else if (SelectedMenuType == MapToolMainMenu.MenuType.Direction)
				{
					if (SelectedSpriteKey == "Eraser")
					{
						SlotsInfo[num].RemoveYarn();
						SlotsInfo[num].RemoveClothButton();
						SlotsInfo[num].RemoveKnot();
						SlotsInfo[num].RemoveRibbon();
						MapData.main.CurrentMapBoardData.ribbon[result2, result] = 0;
						MapData.main.CurrentMapBoardData.knot[result2, result] = 0;
						MapData.main.CurrentMapBoardData.yarn[result2, result] = false;
						MapData.main.CurrentMapBoardData.clothButton[result2, result] = 0;
					}
					else if (SelectedSpriteKey == "G")
					{
						if (SlotsInfo[num].SetGenerator())
						{
							if (SlotsInfo[num].isGenerator)
							{
								MapData.main.CurrentMapBoardData.AddGeneratorDropList(new BoardPosition(result2, result));
							}
							else
							{
								MapData.main.CurrentMapBoardData.RemoveGeneratorDropList(new BoardPosition(result2, result));
							}
							MapData.main.CurrentMapBoardData.RemoveGeneratorSpecialDropList(new BoardPosition(result2, result));
						}
					}
					else if (SelectedSpriteKey == "GS")
					{
						if (SlotsInfo[num].SetGeneratorSpecial())
						{
							if (SlotsInfo[num].isGeneratorSpecial)
							{
								MapData.main.CurrentMapBoardData.AddGeneratorSpecialDropList(new BoardPosition(result2, result));
							}
							else
							{
								MapData.main.CurrentMapBoardData.RemoveGeneratorSpecialDropList(new BoardPosition(result2, result));
							}
							MapData.main.CurrentMapBoardData.RemoveGeneratorDropList(new BoardPosition(result2, result));
						}
					}
					else if (SelectedSpriteKey == "GateEnter")
					{
						if (result != MapData.main.CurrentMapBoardData.gateEnterY || result2 != MapData.main.CurrentMapBoardData.gateEnterX)
						{
							if (MapData.main.CurrentMapBoardData.gateEnterX != -1 && MapData.main.CurrentMapBoardData.gateEnterY != -1)
							{
								SlotsInfo[MapData.main.CurrentMapBoardData.gateEnterY * BOARD_MAX_COL + MapData.main.CurrentMapBoardData.gateEnterX].RemoveRoadGate();
							}
							SlotsInfo[num].SetRoadGate(isGateEnter: true);
							MapData.main.CurrentMapBoardData.gateEnterX = result2;
							MapData.main.CurrentMapBoardData.gateEnterY = result;
						}
					}
					else if (SelectedSpriteKey == "GateExit")
					{
						if (result != MapData.main.CurrentMapBoardData.gateExitY || result2 != MapData.main.CurrentMapBoardData.gateExitX)
						{
							if (MapData.main.CurrentMapBoardData.gateExitX != -1 && MapData.main.CurrentMapBoardData.gateExitY != -1)
							{
								SlotsInfo[MapData.main.CurrentMapBoardData.gateExitY * BOARD_MAX_COL + MapData.main.CurrentMapBoardData.gateExitX].RemoveRoadGate();
							}
							SlotsInfo[num].SetRoadGate(isGateEnter: false);
							MapData.main.CurrentMapBoardData.gateExitX = result2;
							MapData.main.CurrentMapBoardData.gateExitY = result;
						}
					}
					else if (SelectedSpriteKey == "DD" || SelectedSpriteKey == "DR" || SelectedSpriteKey == "DU" || SelectedSpriteKey == "DL" || SelectedSpriteKey == "DDL" || SelectedSpriteKey == "DRL" || SelectedSpriteKey == "DUL" || SelectedSpriteKey == "DLL")
					{
						bool flag = (SelectedSpriteKey.Length == 3 && SelectedSpriteKey[2] == 'L') ? true : false;
						DropDirection dropDirection = DropDirection.Down;
						switch (SelectedSpriteKey.Substring(0, 2))
						{
						case "DD":
							dropDirection = DropDirection.Down;
							break;
						case "DR":
							dropDirection = DropDirection.Right;
							break;
						case "DL":
							dropDirection = DropDirection.Left;
							break;
						case "DU":
							dropDirection = DropDirection.Up;
							break;
						}
						SlotsInfo[num].SetDirection(dropDirection, flag);
						MapData.main.CurrentMapBoardData.dropDirection[result2, result] = dropDirection;
						MapData.main.CurrentMapBoardData.dropLock[result2, result] = flag;
					}
					else if (SelectedSpriteKey == "BDS")
					{
						if (SlotsInfo[num].SetBringDown(isStart: true))
						{
							MapData.main.CurrentMapBoardData.bringDownGenerator[result2, result] = SlotsInfo[num].isBringDownStart;
						}
					}
					else if (SelectedSpriteKey == "BDE")
					{
						if (SlotsInfo[num].SetBringDown(isStart: false))
						{
							MapData.main.CurrentMapBoardData.bringDownGoal[result2, result] = SlotsInfo[num].isBringDownEnd;
						}
					}
					else if (SelectedSpriteKey.Length >= 3 && SelectedSpriteKey.Substring(0, 3) == "RBN")
					{
						if (SlotsInfo[num].SetRibbon(SelectedSpriteKey))
						{
							int result3 = 0;
							if (SelectedSpriteKey.Length == 5)
							{
								int.TryParse(SelectedSpriteKey.Substring(3, 2), out result3);
							}
							MapData.main.CurrentMapBoardData.ribbon[result2, result] = result3;
							SlotsInfo[num].RemoveObs();
						}
						else
						{
							MapData.main.CurrentMapBoardData.ribbon[result2, result] = 0;
						}
					}
					else if (SelectedSpriteKey.Length >= 4 && SelectedSpriteKey.Substring(0, 4) == "KNOT")
					{
						if (SlotsInfo[num].SetKnot(SelectedSpriteKey))
						{
							int result4 = 0;
							if (SelectedSpriteKey.Length == 6)
							{
								int.TryParse(SelectedSpriteKey.Substring(5, 1), out result4);
							}
							MapData.main.CurrentMapBoardData.knot[result2, result] = result4;
						}
						else
						{
							MapData.main.CurrentMapBoardData.knot[result2, result] = 0;
						}
					}
					else if (SelectedSpriteKey == "YARN")
					{
						if (SlotsInfo[num].SetYarn(SelectedSpriteKey))
						{
							MapData.main.CurrentMapBoardData.yarn[result2, result] = true;
							SlotsInfo[num].RemoveObs();
						}
						else
						{
							MapData.main.CurrentMapBoardData.yarn[result2, result] = false;
						}
					}
					else if (SelectedSpriteKey.Length >= 4 && SelectedSpriteKey.Substring(0, 4) == "CBTN")
					{
						if (SlotsInfo[num].SetClothButton(SelectedSpriteKey))
						{
							int result5 = 0;
							if (SelectedSpriteKey.Length == 6)
							{
								int.TryParse(SelectedSpriteKey.Substring(5, 1), out result5);
							}
							MapData.main.CurrentMapBoardData.clothButton[result2, result] = result5;
						}
						else
						{
							MapData.main.CurrentMapBoardData.clothButton[result2, result] = 0;
						}
					}
					else if (SelectedSpriteKey.Length >= 3 && SelectedSpriteKey.Substring(0, 2) == "RI")
					{
						if (SlotsInfo[num].SetRailImage(SelectedSpriteKey))
						{
							int result6 = 0;
							if (SelectedSpriteKey.Length == 3)
							{
								int.TryParse(SelectedSpriteKey.Substring(2, 1), out result6);
							}
							else
							{
								int.TryParse(SelectedSpriteKey.Substring(2, 2), out result6);
							}
							MapData.main.CurrentMapBoardData.railImage[result2, result] = result6;
						}
						else
						{
							MapData.main.CurrentMapBoardData.railImage[result2, result] = 0;
						}
					}
					else if (SelectedSpriteKey == "Rail")
					{
						if (isReadyNextRail && result == selectedRailPosition.y && result2 == selectedRailPosition.x)
						{
							isReadyNextRail = false;
							ImageSelectedItemInfo.color = Color.white;
						}
						if (isReadyNextRail)
						{
							if (selectedRailPosition.x != -1 && selectedRailPosition.y != -1)
							{
								MapData.main.CurrentMapBoardData.SetRailNextPosition(selectedRailPosition.x, selectedRailPosition.y, result2, result);
								SlotsInfo[selectedRailPosition.y * BOARD_MAX_COL + selectedRailPosition.x].SetNextRail(result2, result);
								isReadyNextRail = false;
								ImageSelectedItemInfo.color = Color.white;
							}
						}
						else if (SlotsInfo[num].SetRailInfo())
						{
							ImageSelectedItemInfo.color = Color.red;
							isReadyNextRail = true;
							selectedRailPosition.x = SlotsInfo[num].boardPosition.x;
							selectedRailPosition.y = SlotsInfo[num].boardPosition.y;
						}
						else
						{
							MapData.main.CurrentMapBoardData.RemoveRail(result2, result);
						}
					}
					else if (SelectedSpriteKey.Length >= 3 && SelectedSpriteKey.Substring(0, 2) == "TN")
					{
						if (SlotsInfo[num].SetTunnel(SelectedSpriteKey))
						{
							int result7 = 0;
							if (SelectedSpriteKey.Length == 3)
							{
								int.TryParse(SelectedSpriteKey.Substring(2, 1), out result7);
							}
							MapData.main.CurrentMapBoardData.tunnel[result2, result] = result7;
						}
						else
						{
							MapData.main.CurrentMapBoardData.tunnel[result2, result] = 0;
						}
					}
					else if (SelectedSpriteKey == "Tutorial")
					{
						if ((MapData.main.CurrentMapBoardData.tutorial1X != -1 || MapData.main.CurrentMapBoardData.tutorial1Y != -1) && (MapData.main.CurrentMapBoardData.tutorial2X != -1 || MapData.main.CurrentMapBoardData.tutorial2Y != -1) && (MapData.main.CurrentMapBoardData.tutorial1X != result2 || MapData.main.CurrentMapBoardData.tutorial1Y != result) && (MapData.main.CurrentMapBoardData.tutorial2X != result2 || MapData.main.CurrentMapBoardData.tutorial2Y != result))
						{
							return;
						}
						if (SlotsInfo[num].SetTutorial())
						{
							if (MapData.main.CurrentMapBoardData.tutorial1X == -1 && MapData.main.CurrentMapBoardData.tutorial1Y == -1)
							{
								MapData.main.CurrentMapBoardData.tutorial1X = result2;
								MapData.main.CurrentMapBoardData.tutorial1Y = result;
							}
							else
							{
								MapData.main.CurrentMapBoardData.tutorial2X = result2;
								MapData.main.CurrentMapBoardData.tutorial2Y = result;
							}
						}
						else if (MapData.main.CurrentMapBoardData.tutorial1X == result2 && MapData.main.CurrentMapBoardData.tutorial1Y == result)
						{
							MapData.main.CurrentMapBoardData.tutorial1X = -1;
							MapData.main.CurrentMapBoardData.tutorial1Y = -1;
						}
						else
						{
							MapData.main.CurrentMapBoardData.tutorial2X = -1;
							MapData.main.CurrentMapBoardData.tutorial2Y = -1;
						}
					}
					else if (SelectedSpriteKey == "Safe")
					{
						MapData.main.CurrentMapBoardData.safeObsSlot[result2, result] = SlotsInfo[num].SetSafeObs();
					}
				}
				else if (SelectedMenuType == MapToolMainMenu.MenuType.Slot)
				{
					if (SelectedSpriteKey == "SLOT")
					{
						SlotsInfo[num].ChangeNull();
						if (SlotsInfo[num].IsNull)
						{
							BoardPosition key = new BoardPosition(result2, result);
							if (dicCellMovingButton.ContainsKey(key))
							{
								dicCellMovingButton[key].Deselect();
								MapData.main.CurrentMapBoardData.RemoveMovingSlot(key.x, key.y);
							}
							if (dicCellRotationButton.ContainsKey(key))
							{
								dicCellRotationButton[key].Deselect();
								MapData.main.CurrentMapBoardData.RemoveRotationSlot(key.x, key.y);
							}
							if (dicGridRotationButton.ContainsKey(key))
							{
								dicGridRotationButton[key].Deselect();
								MapData.main.CurrentMapBoardData.RemoveRotationSlot(key.x, key.y);
							}
							MapData.main.CurrentMapBoardData.rockCandyTile[key.x, key.y] = 0;
						}
						MapData.main.CurrentMapBoardData.slots[result2, result] = !SlotsInfo[num].IsNull;
					}
					else if (SelectedSpriteKey == "B1")
					{
						if (MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] == 1)
						{
							SlotsInfo[num].RemoveRockCandyTile();
							MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] = 0;
						}
						else
						{
							SlotsInfo[num].SetRockCandyTile(1);
							MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] = 1;
						}
					}
					else if (SelectedSpriteKey == "B2")
					{
						if (MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] == 2)
						{
							SlotsInfo[num].RemoveRockCandyTile();
							MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] = 0;
						}
						else
						{
							SlotsInfo[num].SetRockCandyTile(2);
							MapData.main.CurrentMapBoardData.rockCandyTile[result2, result] = 2;
						}
					}
					else if (SelectedSpriteKey == "J")
					{
						if (MapData.main.CurrentMapBoardData.jellyTile[result2, result])
						{
							MapData.main.CurrentMapBoardData.jellyTile[result2, result] = false;
							SlotsInfo[num].RemoveJellyTile();
						}
						else
						{
							MapData.main.CurrentMapBoardData.jellyTile[result2, result] = true;
							SlotsInfo[num].SetJellyTile();
						}
					}
					else if (SelectedSpriteKey == "MK")
					{
						if (MapData.main.CurrentMapBoardData.milkTile[result2, result])
						{
							MapData.main.CurrentMapBoardData.milkTile[result2, result] = false;
							SlotsInfo[num].RemoveMilkTile();
						}
						else
						{
							MapData.main.CurrentMapBoardData.milkTile[result2, result] = true;
							SlotsInfo[num].SetMilkTile();
						}
					}
				}
				else if (SelectedMenuType == MapToolMainMenu.MenuType.Obstacle)
				{
					SlotsInfo[num].RemoveYarn();
					SlotsInfo[num].RemoveClothButton();
					SlotsInfo[num].RemoveKnot();
					SlotsInfo[num].RemoveRibbon();
					MapData.main.CurrentMapBoardData.ribbon[result2, result] = 0;
					MapData.main.CurrentMapBoardData.knot[result2, result] = 0;
					MapData.main.CurrentMapBoardData.yarn[result2, result] = false;
					MapData.main.CurrentMapBoardData.clothButton[result2, result] = 0;
					if (SelectedSpriteKey == "Eraser")
					{
						SlotsInfo[num].RemoveObs();
					}
					else if (SelectedSpriteKey == "WH")
					{
						SlotsInfo[num].SetWall(isH: true);
						MapData.main.CurrentMapBoardData.wallsH[result2, result] = true;
					}
					else if (SelectedSpriteKey == "WV")
					{
						SlotsInfo[num].SetWall(isH: false);
						MapData.main.CurrentMapBoardData.wallsV[result2, result] = true;
					}
					else
					{
						IBlockType blockTypeFromString = MapData.GetBlockTypeFromString(SelectedSpriteKey);
						MapData.main.CurrentMapBoardData.SetBlock(result2, result, blockTypeFromString);
						SlotsInfo[num].SetObs(blockTypeFromString);
					}
				}
				else if (SelectedMenuType == MapToolMainMenu.MenuType.Collect)
				{
					if (SelectedSpriteKey == "Eraser")
					{
						SlotsInfo[num].RemoveRescueGingerMan();
						MapData.main.CurrentMapBoardData.RemoveRescueBear(result2, result);
					}
					else if (SelectedSpriteKey.Substring(0, 2) == "RB")
					{
						SlotsInfo[num].SetRescueGingerMan(SelectedSpriteKey);
						int sizeW = int.Parse(SelectedSpriteKey[2].ToString());
						int sizeH = int.Parse(SelectedSpriteKey[3].ToString());
						MapData.main.CurrentMapBoardData.SetRescueBear(result2, result, sizeW, sizeH);
					}
				}
			}
		}

		public void OnSelectItem(MapToolMainMenu.MenuType selectMenu, string spriteKeyName)
		{
			SelectedMenuType = selectMenu;
			if (!dicSpriteBlockList.ContainsKey(spriteKeyName))
			{
				SelectedSpriteKey = string.Empty;
				ObjSelectedItemInfo.SetActive(value: false);
				return;
			}
			SelectedSpriteKey = spriteKeyName;
			Sprite sprite = dicSpriteBlockList[SelectedSpriteKey];
			ObjSelectedItemInfo.SetActive(value: true);
			ImageSelectedItemInfo.sprite = sprite;
			ImageSelectedItemInfo.color = Color.white;
		}

		public void OnSelectItem(MapToolMainMenu.MenuType selectMenu, GameObject selectObject)
		{
			OnSelectItem(selectMenu, selectObject.name);
			if (selectMenu != MapToolMainMenu.MenuType.Direction || !selectObject.transform.parent.name.StartsWith("GroupDirection"))
			{
				return;
			}
			Toggle componentInChildren = selectObject.transform.parent.GetComponentInChildren<Toggle>();
			if ((bool)componentInChildren)
			{
				if (componentInChildren.isOn)
				{
					ImageSelectedItemInfo.color = Color.red;
					SelectedSpriteKey += "L";
				}
				else
				{
					ImageSelectedItemInfo.color = Color.white;
				}
			}
		}

		public void OnSelectMainMenuTab(GameObject objPanel)
		{
			if (!(objPanel == objCurrentMainMenuTab))
			{
				if (objCurrentMainMenuTab != null)
				{
					objCurrentMainMenuTab.SetActive(value: false);
				}
				objPanel.SetActive(value: true);
				objCurrentMainMenuTab = objPanel;
			}
		}

		public void OnPressOptionAllBackTile()
		{
			int num = 0;
			for (int i = 0; i < MapData.MaxWidth; i++)
			{
				for (int j = 0; j < MapData.MaxHeight; j++)
				{
					if (MapData.main.CurrentMapBoardData.slots[i, j])
					{
						num = j * MapData.MaxWidth + i;
						SlotsInfo[num].SetRockCandyTile(2);
						MapData.main.CurrentMapBoardData.rockCandyTile[i, j] = 2;
					}
				}
			}
		}

		public void ToggleShowDirectionLayer(bool changed)
		{
			MapToolSlot[] slotsInfo = SlotsInfo;
			foreach (MapToolSlot mapToolSlot in slotsInfo)
			{
				mapToolSlot.SetActiveDirectionLayer(changed);
			}
		}

		public void ToggleShowMovingLayer(bool changed)
		{
			GroupMovingPoint.gameObject.SetActive(changed);
		}

		public void ToggleShowRotationLayer(bool changed)
		{
			GroupRotationPoint.gameObject.SetActive(changed);
		}

		public void ToggleShowRescueBearLayer(bool changed)
		{
			ObjParentRescueGingerMan.SetActive(changed);
		}

		public void SetMessageLog(string text)
		{
			tweenMessageTime = 4f;
			TextMessageLog.text = text;
			TextMessageLog.color = Color.white;
		}

		private void InitRotationButton()
		{
			int num = BOARD_MAX_COL + 1;
			MapToolRotationButton[] componentsInChildren = BoardRotationInfoInCell.GetComponentsInChildren<MapToolRotationButton>();
			foreach (MapToolRotationButton mapToolRotationButton in componentsInChildren)
			{
				BoardPosition key = new BoardPosition(num % BOARD_MAX_COL, num / BOARD_MAX_COL);
				mapToolRotationButton.posY = key.y;
				mapToolRotationButton.posX = key.x;
				dicCellRotationButton.Add(key, mapToolRotationButton);
				num++;
				if ((num + 1) % BOARD_MAX_COL == 0)
				{
					num += 2;
				}
			}
			num = BOARD_MAX_COL + 1;
			MapToolRotationButton[] componentsInChildren2 = BoardRotationInfoInGrid.GetComponentsInChildren<MapToolRotationButton>();
			foreach (MapToolRotationButton mapToolRotationButton2 in componentsInChildren2)
			{
				BoardPosition key2 = new BoardPosition(num % BOARD_MAX_COL, num / BOARD_MAX_COL);
				mapToolRotationButton2.posY = key2.y;
				mapToolRotationButton2.posX = key2.x;
				dicGridRotationButton.Add(key2, mapToolRotationButton2);
				num++;
				if (num % BOARD_MAX_COL == 0)
				{
					num++;
				}
			}
			HideRotationButton();
		}

		public void HideRotationButton()
		{
			foreach (MapToolRotationButton value in dicCellRotationButton.Values)
			{
				Image component = value.GetComponent<Image>();
				if ((bool)component)
				{
					component.enabled = false;
				}
			}
			foreach (MapToolRotationButton value2 in dicGridRotationButton.Values)
			{
				Image component = value2.GetComponent<Image>();
				if ((bool)component)
				{
					component.enabled = false;
				}
			}
		}

		public void ShowEnableRotationButton()
		{
			ToggleShowRotationLayer(changed: true);
			int size = slotRotationMenu.size;
			int size2 = slotRotationMenu.size;
			bool[] array = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
			bool[] array2 = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = true;
				array2[i] = true;
			}
			foreach (MapToolRotationButton value in dicCellRotationButton.Values)
			{
				if (value.IsSelected)
				{
					for (int j = value.posX - value.SizeWidth / 2; j <= value.posX + value.SizeWidth / 2; j++)
					{
						for (int k = value.posY - value.SizeHeight / 2; k <= value.posY + value.SizeHeight / 2; k++)
						{
							if (j >= 0 && j < BOARD_MAX_COL && k >= 0 && k < BOARD_MAX_ROW)
							{
								array[k * BOARD_MAX_COL + j] = false;
							}
						}
					}
					for (int l = value.posX - value.SizeWidth / 2; l <= value.posX + value.SizeWidth / 2 + 1; l++)
					{
						for (int m = value.posY - value.SizeHeight / 2; m <= value.posY + value.SizeHeight / 2 + 1; m++)
						{
							if (l >= 0 && l < BOARD_MAX_COL && m >= 0 && m < BOARD_MAX_ROW)
							{
								array2[m * BOARD_MAX_COL + l] = false;
							}
						}
					}
				}
			}
			foreach (MapToolRotationButton value2 in dicGridRotationButton.Values)
			{
				if (value2.IsSelected)
				{
					for (int n = value2.posX - value2.SizeWidth / 2; n < value2.posX + value2.SizeWidth / 2; n++)
					{
						for (int num = value2.posY - value2.SizeHeight / 2; num < value2.posY + value2.SizeHeight / 2; num++)
						{
							if (n >= 0 && n < BOARD_MAX_COL && num >= 0 && num < BOARD_MAX_ROW)
							{
								array[num * BOARD_MAX_COL + n] = false;
							}
						}
					}
					for (int num2 = value2.posX - value2.SizeWidth / 2; num2 <= value2.posX + value2.SizeWidth / 2; num2++)
					{
						for (int num3 = value2.posY - value2.SizeHeight / 2; num3 <= value2.posY + value2.SizeHeight / 2; num3++)
						{
							if (num2 >= 0 && num2 < BOARD_MAX_COL && num3 >= 0 && num3 < BOARD_MAX_ROW)
							{
								array2[num3 * BOARD_MAX_COL + num2] = false;
							}
						}
					}
				}
			}
			if (size % 2 != 0 && size2 % 2 != 0)
			{
				for (int num4 = 0; num4 < array2.Length; num4++)
				{
					array2[num4] = false;
				}
				bool[] array3 = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
				for (int num5 = 0; num5 < array3.Length; num5++)
				{
					array3[num5] = true;
				}
				foreach (MapToolRotationButton value3 in dicCellRotationButton.Values)
				{
					if (!value3.IsSelected && array[value3.posY * BOARD_MAX_COL + value3.posX])
					{
						bool flag = true;
						int num6 = value3.posX - size / 2;
						while (flag && num6 <= value3.posX + size / 2)
						{
							int num7 = value3.posY - size2 / 2;
							while (flag && num7 <= value3.posY + size2 / 2)
							{
								if (num6 < 0 || num6 >= BOARD_MAX_COL || num7 < 0 || num7 >= BOARD_MAX_ROW)
								{
									flag = false;
									break;
								}
								if (!array[num7 * BOARD_MAX_COL + num6])
								{
									flag = false;
									break;
								}
								num7++;
							}
							num6++;
						}
						array3[value3.posY * BOARD_MAX_COL + value3.posX] = flag;
					}
				}
				for (int num8 = 0; num8 < array3.Length; num8++)
				{
					if (!array3[num8])
					{
						array[num8] = false;
					}
				}
			}
			else if (size % 2 == 0 && size2 % 2 == 0)
			{
				for (int num9 = 0; num9 < array.Length; num9++)
				{
					array[num9] = false;
				}
				bool[] array4 = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
				for (int num10 = 0; num10 < array4.Length; num10++)
				{
					array4[num10] = true;
				}
				foreach (MapToolRotationButton value4 in dicGridRotationButton.Values)
				{
					if (!value4.IsSelected && array2[value4.posY * BOARD_MAX_COL + value4.posX])
					{
						bool flag2 = true;
						int num11 = value4.posX - size / 2 + 1;
						while (flag2 && num11 < value4.posX + size / 2)
						{
							int num12 = value4.posY - size2 / 2 + 1;
							while (flag2 && num12 < value4.posY + size2 / 2)
							{
								if (num11 - 1 < 0 || num11 >= BOARD_MAX_COL || num12 - 1 < 0 || num12 >= BOARD_MAX_ROW)
								{
									flag2 = false;
									break;
								}
								if (num12 * BOARD_MAX_COL + num11 < array2.Length && !array2[num12 * BOARD_MAX_COL + num11])
								{
									flag2 = false;
									break;
								}
								num12++;
							}
							num11++;
						}
						array4[value4.posY * BOARD_MAX_COL + value4.posX] = flag2;
					}
				}
				for (int num13 = 0; num13 < array4.Length; num13++)
				{
					if (!array4[num13])
					{
						array2[num13] = false;
					}
				}
			}
			else
			{
				for (int num14 = 0; num14 < array2.Length; num14++)
				{
					array2[num14] = false;
				}
				for (int num15 = 0; num15 < array.Length; num15++)
				{
					array[num15] = false;
				}
			}
			foreach (MapToolRotationButton value5 in dicCellRotationButton.Values)
			{
				Image component = value5.GetComponent<Image>();
				if ((bool)component)
				{
					if (value5.IsSelected)
					{
						component.enabled = true;
					}
					else
					{
						component.enabled = array[value5.posY * BOARD_MAX_COL + value5.posX];
					}
				}
			}
			foreach (MapToolRotationButton value6 in dicGridRotationButton.Values)
			{
				Image component = value6.GetComponent<Image>();
				if ((bool)component)
				{
					if (value6.IsSelected)
					{
						component.enabled = true;
					}
					else
					{
						component.enabled = array2[value6.posY * BOARD_MAX_COL + value6.posX];
					}
				}
			}
		}

		public void OnSelectRotationSlot(MapToolRotationButton btn)
		{
			if (btn.IsSelected)
			{
				btn.Deselect();
				MapData.main.CurrentMapBoardData.RemoveRotationSlot(btn.posX, btn.posY);
			}
			else
			{
				btn.Select(slotRotationMenu.size, slotRotationMenu.isClockwise);
				MapData.main.CurrentMapBoardData.AddRotationSlot(btn.positionType == MapToolRotationButton.ButtonPositionType.Grid, btn.posX, btn.posY, slotRotationMenu.size, slotRotationMenu.isClockwise);
			}
			ShowEnableRotationButton();
		}

		private void InitMovingButton()
		{
			int num = 0;
			MapToolMovingButton[] componentsInChildren = BoardMovingInfoInCell.GetComponentsInChildren<MapToolMovingButton>();
			foreach (MapToolMovingButton mapToolMovingButton in componentsInChildren)
			{
				BoardPosition key = new BoardPosition(num % BOARD_MAX_COL, num / BOARD_MAX_COL);
				mapToolMovingButton.posY = key.y;
				mapToolMovingButton.posX = key.x;
				dicCellMovingButton.Add(key, mapToolMovingButton);
				num++;
			}
			HideMovingButton();
		}

		public void HideMovingButton()
		{
			foreach (MapToolMovingButton value in dicCellMovingButton.Values)
			{
				Image component = value.GetComponent<Image>();
				if ((bool)component)
				{
					component.enabled = false;
				}
			}
		}

		public void ShowEnableMovingButton()
		{
			ToggleShowMovingLayer(changed: true);
			int sizeWidth = slotMovingMenu.sizeWidth;
			int sizeHeight = slotMovingMenu.sizeHeight;
			bool[] array = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = true;
			}
			foreach (MapToolMovingButton value in dicCellMovingButton.Values)
			{
				if (value.IsSelected)
				{
					for (int j = value.posX; j < value.posX + value.SizeWidth; j++)
					{
						for (int num = value.posY; num > value.posY - value.SizeHeight; num--)
						{
							if (j >= 0 && j < BOARD_MAX_COL && num >= 0 && num < BOARD_MAX_ROW)
							{
								array[num * BOARD_MAX_COL + j] = false;
							}
						}
					}
				}
			}
			bool[] array2 = new bool[BOARD_MAX_COL * BOARD_MAX_ROW];
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = true;
			}
			int num2 = 0;
			int num3 = 0;
			switch (slotMovingMenu.direction)
			{
			case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Right:
				num2 = slotMovingMenu.movingCount;
				break;
			case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Left:
				num2 = -slotMovingMenu.movingCount;
				break;
			case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Up:
				num3 = slotMovingMenu.movingCount;
				break;
			case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Down:
				num3 = -slotMovingMenu.movingCount;
				break;
			}
			foreach (MapToolMovingButton value2 in dicCellMovingButton.Values)
			{
				if (!value2.IsSelected && array[value2.posY * BOARD_MAX_COL + value2.posX])
				{
					bool flag = true;
					int num4 = value2.posX;
					while (flag && num4 < value2.posX + sizeWidth)
					{
						int num5 = value2.posY;
						while (flag && num5 > value2.posY - sizeHeight)
						{
							if (num4 >= 0 && num4 < BOARD_MAX_COL && num5 >= 0 && num5 < BOARD_MAX_ROW)
							{
								if (num4 + num2 < 0 || num4 + num2 >= BOARD_MAX_COL || num5 + num3 < 0 || num5 + num3 >= BOARD_MAX_ROW)
								{
									flag = false;
									break;
								}
								if (!array[num5 * BOARD_MAX_COL + num4])
								{
									flag = false;
									break;
								}
							}
							num5--;
						}
						num4++;
					}
					array2[value2.posY * BOARD_MAX_COL + value2.posX] = flag;
				}
			}
			for (int l = 0; l < array2.Length; l++)
			{
				if (!array2[l])
				{
					array[l] = false;
				}
			}
			foreach (MapToolMovingButton value3 in dicCellMovingButton.Values)
			{
				Image component = value3.GetComponent<Image>();
				if ((bool)component)
				{
					if (value3.IsSelected)
					{
						component.enabled = true;
					}
					else
					{
						component.enabled = array[value3.posY * BOARD_MAX_COL + value3.posX];
					}
				}
			}
		}

		public void OnSelectMovingSlot(MapToolMovingButton btn)
		{
			if (btn.IsSelected)
			{
				btn.Deselect();
				MapData.main.CurrentMapBoardData.RemoveMovingSlot(btn.posX, btn.posY);
			}
			else
			{
				btn.Select(slotMovingMenu.direction, slotMovingMenu.sizeWidth, slotMovingMenu.sizeHeight, slotMovingMenu.movingCount);
				MapData.main.CurrentMapBoardData.AddMovingSlot(btn.posX, btn.posY, slotMovingMenu.direction, slotMovingMenu.sizeWidth, slotMovingMenu.sizeHeight, slotMovingMenu.movingCount);
			}
			ShowEnableMovingButton();
		}

		public void RefreshGeneratorList()
		{
			int num = 1;
			foreach (MapToolSlot item in listGeneratorSlot)
			{
				item.GeneratorIndex = num++;
			}
			generatorMenu.RefreshGenerator();
		}

		public void RemoveGeneratorList(int generatorNo)
		{
			int index = generatorNo - 1;
			if (selectedDropListGeneratorNo != 0)
			{
				if (generatorNo == selectedDropListGeneratorNo)
				{
					selectedDropListGeneratorNo = 0;
				}
				else if (generatorNo < selectedDropListGeneratorNo)
				{
					selectedDropListGeneratorNo--;
				}
			}
			if (listGeneratorSlot[index] != null)
			{
				listGeneratorSlot.RemoveAt(index);
				generatorMenu.RemoveGenerator(generatorNo);
				RefreshGeneratorList();
			}
		}

		public int AddGeneratorList(MapToolSlot slot)
		{
			listGeneratorSlot.Add(slot);
			generatorMenu.AddGenerator();
			return listGeneratorSlot.Count;
		}

		public void OnSelectGeneratorDropListToHighlight(int generatorNo = 0)
		{
			if (selectedDropListGeneratorNo != 0)
			{
				listGeneratorSlot[selectedDropListGeneratorNo - 1].TextGeneratorIndex.color = new Color(84f / 85f, 62f / 85f, 0.3529412f, 1f);
			}
			if (generatorNo != 0)
			{
				listGeneratorSlot[generatorNo - 1].TextGeneratorIndex.color = Color.red;
			}
			selectedDropListGeneratorNo = generatorNo;
		}

		public MapToolSlot GetSlotFromGeneratorNo(int generatorNo)
		{
			if (generatorNo > listGeneratorSlot.Count)
			{
				return null;
			}
			return listGeneratorSlot[generatorNo - 1];
		}

		public void RefreshGeneratorSpecialList()
		{
			int num = 1;
			foreach (MapToolSlot item in listGeneratorSpecialSlot)
			{
				item.GeneratorSpecialIndex = num++;
			}
			generatorSpecialMenu.RefreshGenerator();
		}

		public void RemoveGeneratorSpecialList(int generatorNo)
		{
			int index = generatorNo - 1;
			if (selectedDropListGeneratorSpecialNo != 0)
			{
				if (generatorNo == selectedDropListGeneratorSpecialNo)
				{
					selectedDropListGeneratorSpecialNo = 0;
				}
				else if (generatorNo < selectedDropListGeneratorSpecialNo)
				{
					selectedDropListGeneratorSpecialNo--;
				}
			}
			if (listGeneratorSpecialSlot[index] != null)
			{
				listGeneratorSpecialSlot.RemoveAt(index);
				generatorSpecialMenu.RemoveGenerator(generatorNo);
				RefreshGeneratorSpecialList();
			}
		}

		public int AddGeneratorSpecialList(MapToolSlot slot)
		{
			listGeneratorSpecialSlot.Add(slot);
			generatorSpecialMenu.AddGenerator();
			return listGeneratorSpecialSlot.Count;
		}

		public void OnSelectGeneratorSpecialDropListToHighlight(int generatorNo = 0)
		{
			if (selectedDropListGeneratorSpecialNo != 0)
			{
				listGeneratorSpecialSlot[selectedDropListGeneratorSpecialNo - 1].TextGeneratorIndex.color = new Color(84f / 85f, 62f / 85f, 0.3529412f, 1f);
			}
			if (generatorNo != 0)
			{
				listGeneratorSpecialSlot[generatorNo - 1].TextGeneratorIndex.color = Color.red;
			}
			selectedDropListGeneratorSpecialNo = generatorNo;
		}

		public MapToolSlot GetSlotFromGeneratorSpecialNo(int generatorNo)
		{
			if (generatorNo > listGeneratorSpecialSlot.Count)
			{
				return null;
			}
			return listGeneratorSpecialSlot[generatorNo - 1];
		}

		public int FindMaxInOrder()
		{
			int num = 0;
			MapBoardData currentMapBoardData = MapData.main.CurrentMapBoardData;
			for (int i = 0; i < MapData.MaxWidth; i++)
			{
				for (int j = 0; j < MapData.MaxHeight; j++)
				{
					if (num < currentMapBoardData.inOrder[i, j])
					{
						num = currentMapBoardData.inOrder[i, j];
					}
				}
			}
			return num;
		}

		public int AddNumberChocolateInOrder(MapToolSlot slot)
		{
			if (slot.NumberChocolateIndex == 0)
			{
				int num = FindMaxInOrder();
				MapBoardData currentMapBoardData = MapData.main.CurrentMapBoardData;
				return currentMapBoardData.inOrder[slot.boardPosition.x, slot.boardPosition.y] = num + 1;
			}
			return slot.NumberChocolateIndex;
		}

		public void DeleteNumberChocolateInOrder(MapToolSlot slot)
		{
			MapBoardData currentMapBoardData = MapData.main.CurrentMapBoardData;
			int deletedInOrder = currentMapBoardData.inOrder[slot.boardPosition.x, slot.boardPosition.y];
			currentMapBoardData.inOrder[slot.boardPosition.x, slot.boardPosition.y] = 0;
			slot.NumberChocolateIndex = 0;
			RefreshNumberChocolateInOrder(deletedInOrder);
		}

		public void RefreshNumberChocolateInOrder(int deletedInOrder)
		{
			MapBoardData currentMapBoardData = MapData.main.CurrentMapBoardData;
			for (int i = 0; i < MapData.MaxWidth; i++)
			{
				for (int j = 0; j < MapData.MaxHeight; j++)
				{
					if (currentMapBoardData.inOrder[i, j] > deletedInOrder)
					{
						SlotsInfo[j * MapData.MaxWidth + i].NumberChocolateIndex = --currentMapBoardData.inOrder[i, j];
					}
				}
			}
		}
	}
}
