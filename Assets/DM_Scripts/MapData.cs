
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using dotmob.sr.maptool;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapData
{
	public static readonly int LEVEL_NORMAL_COUNT_PER_EPISODE = 45;

	public static MapData main;

	public static Dictionary<int, PacketGameLevelData> dicServerGameData = new Dictionary<int, PacketGameLevelData>();

	public static Dictionary<int, PacketGameMapData[]> dicArrayServerMapData = new Dictionary<int, PacketGameMapData[]>();

	public static int MaxEpisode = 1;

	public static readonly int MaxBlockColor = 6;

	public static readonly int MaxCollect = 4;

	public static readonly int MaxWidth = 9;

	public static readonly int MaxHeight = 9;

	public static readonly int MaxDropList = 7;

	public static readonly int MaxDropProbList = 27;

	public static readonly int MaxKindOfBringDown = 3;

	public readonly IBlockType[] bossThrowObsLists = new IBlockType[6]
	{
		IBlockType.ChocolateJail,
		IBlockType.Crunky_HP1,
		IBlockType.Crunky_HP2,
		IBlockType.Crunky_HP3,
		IBlockType.Crow,
		IBlockType.MagicalCrow
	};

	public IBlockType[,] blocksDig;

	public int bonusOpenConditionLevel;

	public int bonusOpenConditionStarCount;

	public int[] bossThrowObsProbs = new int[MapToolMainMenuMode.MaxBossThrowBlockCount];

	public int[,] chipsDig;

	public MapDataCollectBlock[] collectBlocks = new MapDataCollectBlock[MaxCollect];

	public int currentBoardDataIndex;

	public int currentLineDataIndex;

	public int diggingScrollDirection = 1;

	public int[] dropBringDownProb = new int[MaxKindOfBringDown];

	public DropDirection[,] dropDirectionDig;

	public bool[,] dropLockDig;

	public int[] dropRandomBringDown;

	public int gid = 1;

	public int heightDig;

	public bool isNewLevel;

	public int keepShowBringDownObjectCount = 3;

	public int levelMode;

	public List<MapBoardData> listBoardData = new List<MapBoardData>();

	public int moveCount = 30;

	public int orgMoveCount = 30;

	public int[,] param1Dig;

	public int[,] param2Dig;

	public int[,] powerUpsDig;

	public int receivedServerMapCount;

	public int reducingMoveCount;

	public int[,] rockCandyTileDig;

	public int[] scorePoint = new int[3];

	public GoalTarget target;

	public int[,] tunnelDig;

	public int widthDig;

	public MapBoardData CurrentMapBoardData
	{
		get
		{
			if (listBoardData.Count <= currentBoardDataIndex)
			{
				if (listBoardData == null || listBoardData.Count == 0)
				{
					MapBoardData mapBoardData = new MapBoardData();
					mapBoardData.isNewMap = true;
					listBoardData.Add(mapBoardData);
				}
				return listBoardData[0];
			}
			return listBoardData[currentBoardDataIndex];
		}
	}

	public static int LevelCount => dicServerGameData.Count;

	public int SubMapCount => listBoardData.Count;

	public MapData()
	{
		for (int i = 0; i < collectBlocks.Length; i++)
		{
			collectBlocks[i] = new MapDataCollectBlock(string.Empty, 0);
		}
		Reset();
	}

	public MapData(int newGid)
		: this()
	{
		if (LevelCount > 0)
		{
			if (!dicServerGameData.ContainsKey(newGid))
			{
				return;
			}
			PacketGameLevelData packetGameLevelData = dicServerGameData[newGid];
			receivedServerMapCount = packetGameLevelData.map_count;
			gid = packetGameLevelData.gid;
			target = (GoalTarget)packetGameLevelData.game_mode;
			moveCount = (orgMoveCount = packetGameLevelData.move_count);
			scorePoint[0] = packetGameLevelData.star_point_1;
			scorePoint[1] = packetGameLevelData.star_point_2;
			scorePoint[2] = packetGameLevelData.star_point_3;
			bonusOpenConditionStarCount = packetGameLevelData.res_star;
			if (!string.IsNullOrEmpty(packetGameLevelData.collect_data))
			{
				string[] array = packetGameLevelData.collect_data.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(':');
					if (array2.Length == 2)
					{
						collectBlocks[i].blockType = array2[0];
						int.TryParse(array2[1], out collectBlocks[i].count);
					}
				}
			}
			listBoardData.Clear();
			currentBoardDataIndex = 0;
			currentLineDataIndex = 0;
			if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.MapTool && !dicArrayServerMapData.ContainsKey(gid))
			{
				MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Sorry, Map board data is lost");
				MonoSingleton<MapToolManager>.Instance.MapDataReset();
				return;
			}
			for (int j = 0; j < dicArrayServerMapData[gid].Length; j++)
			{
				if (dicArrayServerMapData[gid][j] != null)
				{
					listBoardData.Add(new MapBoardData(dicArrayServerMapData[gid][j]));
				}
			}
			if (!string.IsNullOrEmpty(packetGameLevelData.map_mode))
			{
				string[] array3 = packetGameLevelData.map_mode.Split(',');
				for (int k = 0; k < array3.Length; k++)
				{
					if (array3[k].StartsWith("ReducingMoveCount"))
					{
						int.TryParse(array3[k].Substring(18), out reducingMoveCount);
					}
					switch (target)
					{
					case GoalTarget.RescueVS:
						if (array3[k].StartsWith("ThrowProb"))
						{
							string[] array4 = array3[k].Substring(10).Split('/');
							for (int l = 0; l < Mathf.Min(array4.Length, bossThrowObsProbs.Length); l++)
							{
								int.TryParse(array4[l], out bossThrowObsProbs[l]);
							}
						}
						break;
					case GoalTarget.BringDown:
					case GoalTarget.CollectCracker:
						if (array3[k].StartsWith("KeepShow"))
						{
							int.TryParse(array3[k].Substring(9), out keepShowBringDownObjectCount);
						}
						if (array3[k].StartsWith("Prob"))
						{
							string[] array5 = array3[k].Substring(5).Split('/');
							for (int m = 0; m < Mathf.Min(array5.Length, dropBringDownProb.Length); m++)
							{
								int.TryParse(array5[m], out dropBringDownProb[m]);
							}
						}
						break;
					case GoalTarget.Digging:
						if (array3[k].StartsWith("Direction"))
						{
							int.TryParse(array3[k].Substring(10), out diggingScrollDirection);
						}
						break;
					}
				}
			}
			isNewLevel = false;
		}
		else
		{
			isNewLevel = true;
			CurrentMapBoardData.isNewMap = true;
		}
	}

	public static int GetEpsidoeNoByLevel(int level)
	{
		return (level - 1) / LEVEL_NORMAL_COUNT_PER_EPISODE + 1;
	}

	private void SetWidthDigAndHeightDig()
	{
		switch (main.diggingScrollDirection)
		{
		case 1:
		case 2:
			widthDig = MaxWidth;
			heightDig = MaxHeight * (SubMapCount - 1);
			break;
		case 3:
		case 4:
			widthDig = MaxWidth * (SubMapCount - 1);
			heightDig = MaxHeight;
			break;
		}
	}

	public static GoalTarget GetGoalTarget(int level)
	{
		if (dicServerGameData.ContainsKey(level))
		{
			PacketGameLevelData packetGameLevelData = dicServerGameData[level];
			GoalTarget game_mode = (GoalTarget)packetGameLevelData.game_mode;
			packetGameLevelData = null;
			return game_mode;
		}
		return GoalTarget.Score;
	}

	public static bool IsCollectMakeSpecial(int level)
	{
		if (dicServerGameData.ContainsKey(level))
		{
			PacketGameLevelData packetGameLevelData = dicServerGameData[level];
			if (packetGameLevelData.game_mode == 0 && !string.IsNullOrEmpty(packetGameLevelData.collect_data))
			{
				string[] array = packetGameLevelData.collect_data.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(':');
					if (array2.Length == 2 && (array2[0] == "N0B" || array2[0] == "N0H" || array2[0] == "N0L" || array2[0] == "N0W"))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void InitForDiggingMode()
	{
		SetWidthDigAndHeightDig();
		blocksDig = new IBlockType[widthDig, heightDig];
		chipsDig = new int[widthDig, heightDig];
		tunnelDig = new int[widthDig, heightDig];
		dropDirectionDig = new DropDirection[widthDig, heightDig];
		dropLockDig = new bool[widthDig, heightDig];
		rockCandyTileDig = new int[widthDig, heightDig];
		powerUpsDig = new int[widthDig, heightDig];
		param1Dig = new int[widthDig, heightDig];
		param2Dig = new int[widthDig, heightDig];
		for (int i = 0; i < listBoardData.Count; i++)
		{
			if (i == 0)
			{
				continue;
			}
			MapBoardData mapBoardData = listBoardData[i];
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < MaxWidth; j++)
			{
				for (int k = 0; k < MaxHeight; k++)
				{
					if (main.diggingScrollDirection == 1)
					{
						num = j;
						num2 = k + (listBoardData.Count - 1 - i) * MaxHeight;
					}
					else if (main.diggingScrollDirection == 2)
					{
						num = j;
						num2 = k + (i - 1) * MaxHeight;
					}
					else if (main.diggingScrollDirection == 3)
					{
						num = j + (listBoardData.Count - 1 - i) * MaxWidth;
						num2 = k;
					}
					else if (main.diggingScrollDirection == 4)
					{
						num = j + (i - 1) * MaxWidth;
						num2 = k;
					}
					blocksDig[num, num2] = mapBoardData.blocks[j, k];
					chipsDig[num, num2] = mapBoardData.chips[j, k];
					tunnelDig[num, num2] = mapBoardData.tunnel[j, k];
					dropDirectionDig[num, num2] = mapBoardData.dropDirection[j, k];
					dropLockDig[num, num2] = mapBoardData.dropLock[j, k];
					rockCandyTileDig[num, num2] = mapBoardData.rockCandyTile[j, k];
					powerUpsDig[num, num2] = mapBoardData.powerUps[j, k];
					param1Dig[num, num2] = mapBoardData.param1[j, k];
					param2Dig[num, num2] = mapBoardData.param2[j, k];
					if (!mapBoardData.slots[j, k])
					{
						blocksDig[num, num2] = IBlockType.DiggingBlank;
					}
				}
			}
		}
	}

	public static bool IsHardLevel(int level)
	{
		if (dicServerGameData.ContainsKey(level))
		{
			PacketGameLevelData packetGameLevelData = dicServerGameData[level];
			if (packetGameLevelData.res_level == 1 || packetGameLevelData.res_level == 3)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void SetCurrentOneMap()
	{
		MapBoardData currentMapBoardData = CurrentMapBoardData;
		listBoardData.Clear();
		listBoardData.Add(currentMapBoardData);
	}

	public void RemoveSubMap(int subMapIndex)
	{
		if (SubMapCount != 1 && subMapIndex <= SubMapCount - 1)
		{
			listBoardData.RemoveAt(subMapIndex);
			currentBoardDataIndex = 0;
			currentLineDataIndex = 0;
		}
	}

	public void AddNewSubMap()
	{
		MapBoardData mapBoardData = new MapBoardData();
		mapBoardData.isNewMap = true;
		listBoardData.Add(mapBoardData);
	}

	public static void AddNewServerData(int newGid, MapData mapData)
	{
		dicServerGameData.Add(newGid, new PacketGameLevelData(mapData));
		dicArrayServerMapData.Add(newGid, new PacketGameMapData[1]);
		dicArrayServerMapData[newGid][0] = new PacketGameMapData(newGid, 1, mapData.target, mapData.listBoardData[0]);
	}

	public static void ReceiveServerGameData(PacketGameLevelData[] lists)
	{
		dicServerGameData.Clear();
		MaxEpisode = 1;
		int num = 0;
		for (int i = 0; i < lists.Length; i++)
		{
			int num2 = lists[i].gid;
			if (!dicServerGameData.ContainsKey(num2))
			{
				dicServerGameData.Add(num2, lists[i]);
			}
			if (num2 > num && num2 < 10000)
			{
				num = num2;
			}
		}
		MaxEpisode = GetEpsidoeNoByLevel(num);
	}

	public static void UpdateServerGameData(int gid, ResPacketLoadGameData res)
	{
		if (dicServerGameData.ContainsKey(gid))
		{
			dicServerGameData[gid] = res.game[0];
		}
	}

	public static void CopyAndPasteGameData(int toGid, int fromGid)
	{
		MapData mapData = new MapData(fromGid);
		PacketGameLevelData packetGameLevelData = new PacketGameLevelData(mapData);
		PacketGameLevelData packetGameLevelData2 = dicServerGameData[toGid];
		packetGameLevelData.gid = packetGameLevelData2.gid;
		dicServerGameData[toGid] = packetGameLevelData;
		PacketGameMapData[] value = dicArrayServerMapData[toGid];
		dicArrayServerMapData.Remove(toGid);
		dicArrayServerMapData.Add(toGid, new PacketGameMapData[dicServerGameData[fromGid].map_count]);
		for (int i = 0; i < dicServerGameData[fromGid].map_count; i++)
		{
			PacketGameMapData packetGameMapData = (PacketGameMapData)dicArrayServerMapData[fromGid][i].Clone();
			packetGameMapData.gid = toGid;
			dicArrayServerMapData[toGid][i] = packetGameMapData;
		}
		MonoSingleton<MapToolManager>.Instance.topMenu.ChangeLevel(main.gid);
		main.receivedServerMapCount = packetGameLevelData2.map_count;
		for (int j = packetGameLevelData2.map_count; j < dicServerGameData[fromGid].map_count; j++)
		{
			main.listBoardData[j].isNewMap = true;
		}
		dicServerGameData[toGid] = packetGameLevelData2;
		dicArrayServerMapData.Remove(toGid);
		dicArrayServerMapData.Add(toGid, value);
		mapData = null;
	}

	public static void ReceiveServerMapData(PacketGameMapData[] lists)
	{
		dicArrayServerMapData.Clear();
		if (dicServerGameData == null || dicServerGameData.Count == 0)
		{
			MonoSingleton<ServerDataTable>.Instance.LoadGameLevelDataFromLocalFile();
		}
		for (int i = 0; i < lists.Length; i++)
		{
			int key = lists[i].gid;
			if (dicServerGameData.ContainsKey(key))
			{
				if (!dicArrayServerMapData.ContainsKey(key))
				{
					dicArrayServerMapData.Add(key, new PacketGameMapData[dicServerGameData[key].map_count]);
				}
				try
				{
					if (lists[i].mid - 1 < dicServerGameData[key].map_count)
					{
						dicArrayServerMapData[key][lists[i].mid - 1] = lists[i];
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public static void UpdateServerMapData(int gid, ResPacketLoadMapData res)
	{
		if (dicArrayServerMapData.ContainsKey(gid))
		{
			dicArrayServerMapData.Remove(gid);
		}
		int num = Mathf.Min(dicServerGameData[gid].map_count, res.map.Length);
		dicArrayServerMapData.Add(gid, new PacketGameMapData[num]);
		for (int i = 0; i < num; i++)
		{
			dicArrayServerMapData[gid][res.map[i].mid - 1] = res.map[i];
		}
	}

	public int GetRescueGingerManCount()
	{
		for (int i = 0; i < collectBlocks.Length; i++)
		{
			if (!string.IsNullOrEmpty(collectBlocks[i].blockType) && collectBlocks[i].blockType == "RB")
			{
				return collectBlocks[i].count;
			}
		}
		return 0;
	}

	public int GetBringDownCount()
	{
		for (int i = 0; i < collectBlocks.Length; i++)
		{
			if (!string.IsNullOrEmpty(collectBlocks[i].blockType) && collectBlocks[i].blockType == "N0D")
			{
				return collectBlocks[i].count;
			}
		}
		return 0;
	}

	public int GetRescueBlockCount()
	{
		return 0;
	}

	public void Reset()
	{
		moveCount = (orgMoveCount = 30);
		reducingMoveCount = 0;
		target = GoalTarget.Score;
		scorePoint = new int[3];
		scorePoint[0] = 1000;
		scorePoint[1] = 3000;
		scorePoint[2] = 5000;
		dropBringDownProb[0] = (dropBringDownProb[1] = 33);
		dropBringDownProb[2] = 34;
		for (int i = 0; i < collectBlocks.Length; i++)
		{
			collectBlocks[i].blockType = string.Empty;
			collectBlocks[i].count = 0;
		}
		listBoardData.Clear();
		currentBoardDataIndex = 0;
		currentLineDataIndex = 0;
		listBoardData.Add(new MapBoardData());
	}

	public void MapToolReset()
	{
		collectBlocks[0].blockType = GetBlockJsonFormat(IBlockType.Crunky_HP1);
		collectBlocks[0].count = 555;
		for (int i = 0; i < MaxWidth; i++)
		{
			CurrentMapBoardData.AddGeneratorDropList(new BoardPosition(i, MaxHeight - 1));
		}
	}

	public static ChipType GetChipTypeFromString(string blockName)
	{
		switch (blockName)
		{
		case "N1":
		case "N2":
		case "N3":
		case "N4":
		case "N5":
		case "N6":
			return ChipType.SimpleChip;
		case "N1V":
		case "N2V":
		case "N3V":
		case "N4V":
		case "N5V":
		case "N6V":
			return ChipType.VBomb;
		case "N1H":
		case "N2H":
		case "N3H":
		case "N4H":
		case "N5H":
		case "N6H":
			return ChipType.HBomb;
		case "N1B":
		case "N2B":
		case "N3B":
		case "N4B":
		case "N5B":
		case "N6B":
			return ChipType.SimpleBomb;
		case "N0W":
			return ChipType.RainbowBomb;
		case "N0L":
			return ChipType.CandyChip;
		case "N0D":
			return ChipType.BringDown;
		case "N0R":
			return ChipType.ChamelonChip;
		case "N0O":
			return ChipType.OreoCracker;
		case "E":
			return ChipType.Empty;
		default:
			return ChipType.None;
		}
	}

	public static IBlockType GetBlockTypeFromString(string blockName)
	{
		string[] array = blockName.Split('.');
		switch (array[0])
		{
		case "O1":
			return IBlockType.ChocolateJail;
		case "O2":
			return (IBlockType)(9 + (int.Parse(array[1]) - 1));
		case "O3":
			return (IBlockType)(3 + (int.Parse(array[1]) - 1));
		case "O4":
			return IBlockType.Crow;
		case "O5":
			return IBlockType.RescueFriend;
		case "O6":
			return IBlockType.PastryBag;
		case "O7":
			return IBlockType.MagicalCrow;
		case "O8":
		{
			IBlockType blockType = IBlockType.Digging_HP1;
			int num = int.Parse(array[1].Substring(1)) - 1;
			if (array.Length == 3)
			{
				if (array[2] == "C1")
				{
					blockType = IBlockType.Digging_HP1_Collect;
				}
				else if (array[2] == "B1")
				{
					blockType = IBlockType.Digging_HP1_Bomb1;
				}
				else if (array[2] == "B2")
				{
					blockType = IBlockType.Digging_HP1_Bomb2;
				}
				else if (array[2] == "B3")
				{
					blockType = IBlockType.Digging_HP1_Bomb3;
				}
				else if (array[2].Substring(0, 2) == "T1")
				{
					blockType = ((array[1][0] == 'H' || array[1][0] == 'B') ? IBlockType.Digging_HP1_Treasure_G3 : ((array[1][0] == 'S') ? IBlockType.Digging_HP1_Treasure_G2 : ((array[1][0] != 'G') ? IBlockType.Digging_HP1_Treasure_G3 : IBlockType.Digging_HP1_Treasure_G1)));
				}
			}
			return blockType + num;
		}
		case "O9":
			return IBlockType.GreenSlime;
		case "O10":
			return IBlockType.MilkCarton;
		case "O11":
			return (IBlockType)(51 + (int.Parse(array[1]) - 1) * 3 + (int.Parse(array[2].Substring(1)) - 1));
		case "O12":
			return IBlockType.Pocket;
		case "O13":
			return (IBlockType)(69 + int.Parse(array[1]));
		default:
			return IBlockType.None;
		}
	}

	public static string GetBlockJsonFormat(IBlockType blockType)
	{
		switch (blockType)
		{
		case IBlockType.ChocolateJail:
			return "O1";
		case IBlockType.Crunky_HP1:
			return "O2.1";
		case IBlockType.Crunky_HP2:
			return "O2.2";
		case IBlockType.Crunky_HP3:
			return "O2.3";
		case IBlockType.CandyFactory_1:
			return "O3.1";
		case IBlockType.CandyFactory_2:
			return "O3.2";
		case IBlockType.CandyFactory_3:
			return "O3.3";
		case IBlockType.CandyFactory_4:
			return "O3.4";
		case IBlockType.CandyFactory_5:
			return "O3.5";
		case IBlockType.CandyFactory_6:
			return "O3.6";
		case IBlockType.Crow:
			return "O4";
		case IBlockType.MagicalCrow:
			return "O7";
		case IBlockType.RescueFriend:
			return "O5";
		case IBlockType.PastryBag:
			return "O6";
		case IBlockType.Digging_HP1:
			return "O8.H1";
		case IBlockType.Digging_HP2:
			return "O8.H2";
		case IBlockType.Digging_HP3:
			return "O8.H3";
		case IBlockType.Digging_HP1_Collect:
			return "O8.H1.C1";
		case IBlockType.Digging_HP2_Collect:
			return "O8.H2.C1";
		case IBlockType.Digging_HP3_Collect:
			return "O8.H3.C1";
		case IBlockType.Digging_HP1_Bomb1:
			return "O8.H1.B1";
		case IBlockType.Digging_HP2_Bomb1:
			return "O8.H2.B1";
		case IBlockType.Digging_HP3_Bomb1:
			return "O8.H3.B1";
		case IBlockType.Digging_HP1_Bomb2:
			return "O8.H1.B2";
		case IBlockType.Digging_HP2_Bomb2:
			return "O8.H2.B2";
		case IBlockType.Digging_HP3_Bomb2:
			return "O8.H3.B2";
		case IBlockType.Digging_HP1_Bomb3:
			return "O8.H1.B3";
		case IBlockType.Digging_HP2_Bomb3:
			return "O8.H2.B3";
		case IBlockType.Digging_HP3_Bomb3:
			return "O8.H3.B3";
		case IBlockType.Digging_HP1_Treasure_G1:
			return "O8.G1.T1";
		case IBlockType.Digging_HP2_Treasure_G1:
			return "O8.G2.T1";
		case IBlockType.Digging_HP3_Treasure_G1:
			return "O8.G3.T1";
		case IBlockType.Digging_HP1_Treasure_G2:
			return "O8.S1.T1";
		case IBlockType.Digging_HP2_Treasure_G2:
			return "O8.S2.T1";
		case IBlockType.Digging_HP3_Treasure_G2:
			return "O8.S3.T1";
		case IBlockType.Digging_HP1_Treasure_G3:
			return "O8.B1.T1";
		case IBlockType.Digging_HP2_Treasure_G3:
			return "O8.B2.T1";
		case IBlockType.Digging_HP3_Treasure_G3:
			return "O8.B3.T1";
		case IBlockType.GreenSlime:
			return "O9";
		case IBlockType.MilkCarton:
			return "O10";
		case IBlockType.SpriteDrink_1_HP1:
			return "O11.1.H1";
		case IBlockType.SpriteDrink_1_HP2:
			return "O11.1.H2";
		case IBlockType.SpriteDrink_1_HP3:
			return "O11.1.H3";
		case IBlockType.SpriteDrink_2_HP1:
			return "O11.2.H1";
		case IBlockType.SpriteDrink_2_HP2:
			return "O11.2.H2";
		case IBlockType.SpriteDrink_2_HP3:
			return "O11.2.H3";
		case IBlockType.SpriteDrink_3_HP1:
			return "O11.3.H1";
		case IBlockType.SpriteDrink_3_HP2:
			return "O11.3.H2";
		case IBlockType.SpriteDrink_3_HP3:
			return "O11.3.H3";
		case IBlockType.SpriteDrink_4_HP1:
			return "O11.4.H1";
		case IBlockType.SpriteDrink_4_HP2:
			return "O11.4.H2";
		case IBlockType.SpriteDrink_4_HP3:
			return "O11.4.H3";
		case IBlockType.SpriteDrink_5_HP1:
			return "O11.5.H1";
		case IBlockType.SpriteDrink_5_HP2:
			return "O11.5.H2";
		case IBlockType.SpriteDrink_5_HP3:
			return "O11.5.H3";
		case IBlockType.SpriteDrink_6_HP1:
			return "O11.6.H1";
		case IBlockType.SpriteDrink_6_HP2:
			return "O11.6.H2";
		case IBlockType.SpriteDrink_6_HP3:
			return "O11.6.H3";
		case IBlockType.Pocket:
			return "O12";
		case IBlockType.NumberChocolate:
			return "O13.0";
		case IBlockType.NumberChocolate_1:
			return "O13.1";
		case IBlockType.NumberChocolate_2:
			return "O13.2";
		case IBlockType.NumberChocolate_3:
			return "O13.3";
		case IBlockType.NumberChocolate_4:
			return "O13.4";
		case IBlockType.NumberChocolate_5:
			return "O13.5";
		case IBlockType.NumberChocolate_6:
			return "O13.6";
		default:
			return string.Empty;
		}
	}

	public static int GetBlockLayerNo(IBlockType blockType)
	{
		if (blockType == IBlockType.ChocolateJail || blockType == IBlockType.GreenSlimeChild)
		{
			return 2;
		}
		return 1;
	}

	public static bool IsBlockTypeIncludingChip(IBlockType blockType)
	{
		if (blockType == IBlockType.ChocolateJail || blockType == IBlockType.GreenSlimeChild)
		{
			return true;
		}
		return false;
	}

	public static bool IsNumberChocolate(IBlockType blockType)
	{
		if (blockType >= IBlockType.NumberChocolate && blockType <= IBlockType.NumberChocolate_6)
		{
			return true;
		}
		return false;
	}

	public static int GetChipColorFromString(string chipName)
	{
		if (chipName.Length >= 2 && chipName[0] == 'N')
		{
			int result = 0;
			if (int.TryParse(chipName[1].ToString(), out result))
			{
				return result;
			}
		}
		return 0;
	}

	public static ChipType GetChipTypeFromPower(Powerup powerUp)
	{
		switch (powerUp)
		{
		case Powerup.SimpleBomb:
			return ChipType.SimpleBomb;
		case Powerup.ColorHBomb:
			return ChipType.HBomb;
		case Powerup.ColorVBomb:
			return ChipType.VBomb;
		case Powerup.RainbowBomb:
			return ChipType.RainbowBomb;
		case Powerup.CandyChip:
			return ChipType.CandyChip;
		case Powerup.BringDownObject:
			return ChipType.BringDown;
		case Powerup.None:
			return ChipType.SimpleChip;
		case Powerup.Chameleon:
			return ChipType.ChamelonChip;
		case Powerup.OreoCracker:
			return ChipType.OreoCracker;
		default:
			return ChipType.None;
		}
	}

	public static string GetChipJsonFormat(ChipType chipType, int chipID)
	{
		switch (chipType)
		{
		case ChipType.None:
			return "0";
		case ChipType.Empty:
			return "E";
		default:
		{
			string arg = string.Empty;
			switch (chipType)
			{
			case ChipType.SimpleBomb:
				arg = "B";
				break;
			case ChipType.HBomb:
				arg = "H";
				break;
			case ChipType.VBomb:
				arg = "V";
				break;
			case ChipType.RainbowBomb:
				chipID = 0;
				arg = "W";
				break;
			case ChipType.CandyChip:
				chipID = 0;
				arg = "L";
				break;
			case ChipType.BringDown:
				chipID = 0;
				arg = "D";
				break;
			case ChipType.ChamelonChip:
				chipID = 0;
				arg = "R";
				break;
			case ChipType.OreoCracker:
				chipID = 0;
				arg = "O";
				break;
			}
			return "N" + chipID + arg;
		}
		}
	}

	public static Powerup ChipTypeToPowerup(ChipType chipType)
	{
		switch (chipType)
		{
		case ChipType.SimpleChip:
			return Powerup.None;
		case ChipType.SimpleBomb:
			return Powerup.SimpleBomb;
		case ChipType.HBomb:
			return Powerup.ColorHBomb;
		case ChipType.VBomb:
			return Powerup.ColorVBomb;
		case ChipType.RainbowBomb:
			return Powerup.RainbowBomb;
		case ChipType.CandyChip:
			return Powerup.CandyChip;
		case ChipType.BringDown:
			return Powerup.BringDownObject;
		case ChipType.ChamelonChip:
			return Powerup.Chameleon;
		case ChipType.OreoCracker:
			return Powerup.OreoCracker;
		default:
			return Powerup.None;
		}
	}

	public bool DigCheckLastLine(int x, int y)
	{
		bool result = false;
		switch (diggingScrollDirection)
		{
		case 1:
			if (y == 0)
			{
				result = true;
			}
			break;
		case 2:
			if (y == MaxHeight - 1)
			{
				result = true;
			}
			break;
		case 3:
			if (x == 0)
			{
				result = true;
			}
			break;
		case 4:
			if (x == MaxWidth - 1)
			{
				result = true;
			}
			break;
		}
		return result;
	}

	public int2 GetDiggingDirectionXYDelta()
	{
		int2 result = default(int2);
		result.x = 0;
		result.y = 0;
		switch (diggingScrollDirection)
		{
		case 1:
			result.x = 0;
			result.y = 1;
			break;
		case 2:
			result.x = 0;
			result.y = -1;
			break;
		case 3:
			result.x = 1;
			result.y = 0;
			break;
		case 4:
			result.x = -1;
			result.y = 0;
			break;
		}
		return result;
	}

	public int2 TransformXYConsiderCurrentLineDataIndex(int x, int y)
	{
		int2 result = default(int2);
		result.x = x;
		result.y = y;
		switch (diggingScrollDirection)
		{
		case 1:
			result.y = main.heightDig - main.currentLineDataIndex;
			break;
		case 2:
			result.y = main.currentLineDataIndex - 1;
			break;
		case 3:
			result.x = main.widthDig - main.currentLineDataIndex;
			break;
		case 4:
			result.x = main.currentLineDataIndex - 1;
			break;
		}
		return result;
	}

	public int2 TransformXYToBoardDataXY(int digX, int digY)
	{
		int2 result = default(int2);
		result.x = digX;
		result.y = digY;
		switch (diggingScrollDirection)
		{
		case 1:
		case 2:
			if (digY != 0)
			{
				result.y = digY % MaxHeight;
			}
			else
			{
				result.y = 0;
			}
			break;
		case 3:
		case 4:
			if (digX != 0)
			{
				result.x = digX % MaxWidth;
			}
			else
			{
				result.x = 0;
			}
			break;
		}
		return result;
	}

	public string GetDefaultBoardData()
	{
		return "R/N1/N12/N12:42/P";
	}

	public string GetCollectJsonFormat()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		for (int i = 0; i < collectBlocks.Length; i++)
		{
			if (!string.IsNullOrEmpty(collectBlocks[i].blockType) && collectBlocks[i].count > 0)
			{
				num++;
			}
		}
		for (int j = 0; j < collectBlocks.Length; j++)
		{
			string jsonFormat = collectBlocks[j].GetJsonFormat();
			if (!string.IsNullOrEmpty(jsonFormat))
			{
				stringBuilder.Append(jsonFormat);
				if (--num > 0)
				{
					stringBuilder.Append(",");
				}
			}
		}
		return stringBuilder.ToString();
	}

	public string GetParamJsonFormat()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (reducingMoveCount > 0)
		{
			stringBuilder.AppendFormat("ReducingMoveCount:{0},", reducingMoveCount);
		}
		switch (target)
		{
		case GoalTarget.RescueVS:
			stringBuilder.Append("ThrowProb:");
			for (int i = 0; i < bossThrowObsProbs.Length; i++)
			{
				stringBuilder.Append(bossThrowObsProbs[i]);
				if (i < bossThrowObsProbs.Length - 1)
				{
					stringBuilder.Append("/");
				}
			}
			break;
		case GoalTarget.BringDown:
		case GoalTarget.CollectCracker:
		{
			int[] array = dropBringDownProb;
			stringBuilder.AppendFormat("KeepShow:{0},Prob:{1}/{2}/{3}", keepShowBringDownObjectCount, array[0], array[1], array[2]);
			dropBringDownProb = array;
			break;
		}
		case GoalTarget.Digging:
			stringBuilder.AppendFormat("Direction:{0}", diggingScrollDirection);
			break;
		}
		return stringBuilder.ToString();
	}
}
