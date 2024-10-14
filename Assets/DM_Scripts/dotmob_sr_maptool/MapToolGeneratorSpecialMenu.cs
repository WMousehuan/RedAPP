/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;

namespace dotmob.sr.maptool
{
	public class MapToolGeneratorSpecialMenu : MonoBehaviour
	{
		private static readonly string[] blockListNames = new string[27]
		{
			"N1",
			"N2",
			"N3",
			"N4",
			"N5",
			"N6",
			"N1H",
			"N2H",
			"N3H",
			"N4H",
			"N5H",
			"N6H",
			"N1V",
			"N2V",
			"N3V",
			"N4V",
			"N5V",
			"N6V",
			"N1B",
			"N2B",
			"N3B",
			"N4B",
			"N5B",
			"N6B",
			"N0W",
			"N0L",
			"N0R"
		};

		private readonly List<GeneratorSpecialDropList> listGeneratorSpecialDropList = new List<GeneratorSpecialDropList>();

		public GameObject ObjGeneratorListContent;

		public GameObject PrefabGeneratorList;

		public static int GetGeneratorSpecialBlockIndex(string strBlockName)
		{
			for (int i = 0; i < blockListNames.Length; i++)
			{
				if (blockListNames[i] == strBlockName)
				{
					return i;
				}
			}
			return -1;
		}

		public void Reset()
		{
			foreach (GeneratorSpecialDropList listGeneratorSpecialDrop in listGeneratorSpecialDropList)
			{
				UnityEngine.Object.Destroy(listGeneratorSpecialDrop.gameObject);
			}
			listGeneratorSpecialDropList.Clear();
		}

		public void RefreshFromMapData(int slotX, int slotY, int generatorIndex)
		{
			GeneratorSpecialDropList generatorSpecialDropList = listGeneratorSpecialDropList[generatorIndex];
			if (generatorSpecialDropList == null)
			{
				return;
			}
			BoardPosition key = new BoardPosition(slotX, slotY);
			if (MapData.main.CurrentMapBoardData.dicGeneratorSpecialDropBlock.ContainsKey(key))
			{
				MapDataGeneratorSpecialDrop mapDataGeneratorSpecialDrop = MapData.main.CurrentMapBoardData.dicGeneratorSpecialDropBlock[key];
				for (int i = 0; i < mapDataGeneratorSpecialDrop.dropBlocks.Length; i++)
				{
					generatorSpecialDropList.SetBlock(blockListNames[i], mapDataGeneratorSpecialDrop.dropBlocks[i].prob);
				}
			}
		}

		public void RefreshGenerator()
		{
			int num = 1;
			foreach (GeneratorSpecialDropList listGeneratorSpecialDrop in listGeneratorSpecialDropList)
			{
				listGeneratorSpecialDrop.GeneratorNo = num++;
			}
		}

		public void RemoveGenerator(int generatorIndex)
		{
			UnityEngine.Object.Destroy(listGeneratorSpecialDropList[generatorIndex - 1].gameObject);
			listGeneratorSpecialDropList.RemoveAt(generatorIndex - 1);
		}

		public void AddGenerator()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(PrefabGeneratorList);
			gameObject.transform.SetParent(ObjGeneratorListContent.transform, worldPositionStays: false);
			GeneratorSpecialDropList component = gameObject.GetComponent<GeneratorSpecialDropList>();
			listGeneratorSpecialDropList.Add(component);
			component.GeneratorNo = listGeneratorSpecialDropList.Count;
		}

		public void SetProb(int generatorIndex, string blockName, int prob)
		{
			int num = 0;
			while (true)
			{
				if (num < blockListNames.Length)
				{
					if (blockName == blockListNames[num])
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			MapToolSlot slotFromGeneratorSpecialNo = MonoSingleton<MapToolManager>.Instance.GetSlotFromGeneratorSpecialNo(generatorIndex);
			MapData.main.CurrentMapBoardData.SetGeneratorSpecialDropBlock(slotFromGeneratorSpecialNo.boardPosition, num, blockName, prob);
		}
	}
}
