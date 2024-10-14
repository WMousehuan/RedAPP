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
	public class MapToolGeneratorMenu : MonoBehaviour
	{
		private readonly List<GeneratorDropList> listGeneratorDropList = new List<GeneratorDropList>();

		public GameObject ObjGeneratorListContent;

		public GameObject ObjBlockListContent;

		public GameObject PrefabGeneratorList;

		public GeneratorDropList SelectedGeneratorDropList;

		public int SelectedGeneratorDropListIndex;

		private void Start()
		{
			Button[] componentsInChildren = ObjBlockListContent.GetComponentsInChildren<Button>();
			foreach (Button btn in componentsInChildren)
			{
				SettingBlockListButton(btn);
			}
		}

		private void OnEnable()
		{
			if ((bool)SelectedGeneratorDropList)
			{
				MonoSingleton<MapToolManager>.Instance.OnSelectGeneratorDropListToHighlight(SelectedGeneratorDropListIndex);
			}
		}

		private void OnDisable()
		{
			if ((bool)SelectedGeneratorDropList)
			{
				MonoSingleton<MapToolManager>.Instance.OnSelectGeneratorDropListToHighlight();
			}
		}

		private void SettingBlockListButton(Button btn)
		{
			btn.onClick.AddListener(delegate
			{
				OnPressSelectBlock(btn);
			});
		}

		public void Reset()
		{
			SelectedGeneratorDropList = null;
			SelectedGeneratorDropListIndex = 0;
			foreach (GeneratorDropList listGeneratorDrop in listGeneratorDropList)
			{
				UnityEngine.Object.Destroy(listGeneratorDrop.gameObject);
			}
			listGeneratorDropList.Clear();
		}

		public void RefreshFromMapData(int slotX, int slotY, int generatorIndex)
		{
			GeneratorDropList generatorDropList = listGeneratorDropList[generatorIndex];
			if (generatorDropList == null)
			{
				return;
			}
			BoardPosition key = new BoardPosition(slotX, slotY);
			if (!MapData.main.CurrentMapBoardData.dicGeneratorDropBlock.ContainsKey(key))
			{
				return;
			}
			MapDataGeneratorDrop mapDataGeneratorDrop = MapData.main.CurrentMapBoardData.dicGeneratorDropBlock[key];
			for (int i = 0; i < mapDataGeneratorDrop.dropBlocks.Length; i++)
			{
				if (mapDataGeneratorDrop.dropBlocks[i].chipType != ChipType.SimpleChip || mapDataGeneratorDrop.dropBlocks[i].chipColor != 0)
				{
					generatorDropList.SetBlock(i, MonoSingleton<MapToolManager>.Instance.GetBlockSprite(mapDataGeneratorDrop.dropBlocks[i].chipType, mapDataGeneratorDrop.dropBlocks[i].chipColor), string.Empty);
				}
			}
		}

		private void Update()
		{
			if ((bool)SelectedGeneratorDropList)
			{
				int num = -1;
				if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
				{
					num = 1;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
				{
					num = 2;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
				{
					num = 3;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
				{
					num = 4;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
				{
					num = 5;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
				{
					num = 6;
				}
				if (num != -1)
				{
					OnKeyPressSelectBlock(num);
				}
			}
		}

		public void RefreshGenerator()
		{
			int num = 1;
			foreach (GeneratorDropList listGeneratorDrop in listGeneratorDropList)
			{
				listGeneratorDrop.GeneratorNo = num++;
			}
		}

		public void RemoveGenerator(int generatorIndex)
		{
			UnityEngine.Object.Destroy(listGeneratorDropList[generatorIndex - 1].gameObject);
			listGeneratorDropList.RemoveAt(generatorIndex - 1);
		}

		public void AddGenerator()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(PrefabGeneratorList);
			gameObject.transform.SetParent(ObjGeneratorListContent.transform, worldPositionStays: false);
			GeneratorDropList component = gameObject.GetComponent<GeneratorDropList>();
			listGeneratorDropList.Add(component);
			component.GeneratorNo = listGeneratorDropList.Count;
		}

		public void OnPressSelectGeneratorDropList(GeneratorDropList dropList, int index)
		{
			Color color = dropList.ImageDropBlocks[index].transform.parent.GetComponent<Image>().color;
			float a = color.a;
			if ((bool)SelectedGeneratorDropList)
			{
				SelectedGeneratorDropList.ImageDropBlocks[SelectedGeneratorDropListIndex].transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, a);
			}
			SelectedGeneratorDropList = dropList;
			SelectedGeneratorDropListIndex = index;
			SelectedGeneratorDropList.ImageDropBlocks[SelectedGeneratorDropListIndex].transform.parent.GetComponent<Image>().color = new Color(1f, 0f, 0f, a);
		}

		public void OnPressSelectBlock(Button blockButton)
		{
			if ((bool)SelectedGeneratorDropList)
			{
				SelectedGeneratorDropList.SetBlock(SelectedGeneratorDropListIndex, blockButton.GetComponent<Image>().sprite, blockButton.name);
				MapToolSlot slotFromGeneratorNo = MonoSingleton<MapToolManager>.Instance.GetSlotFromGeneratorNo(SelectedGeneratorDropList.GeneratorNo);
				if (slotFromGeneratorNo != null)
				{
					MapData.main.CurrentMapBoardData.SetGeneratorDropBlock(slotFromGeneratorNo.boardPosition, SelectedGeneratorDropListIndex, blockButton.name);
				}
			}
		}

		public void OnKeyPressSelectBlock(int blockId)
		{
			if ((bool)SelectedGeneratorDropList)
			{
				SelectedGeneratorDropList.SetBlock(SelectedGeneratorDropListIndex, MonoSingleton<MapToolManager>.Instance.GetBlockSprite(ChipType.SimpleChip, blockId), "N" + blockId);
				MapToolSlot slotFromGeneratorNo = MonoSingleton<MapToolManager>.Instance.GetSlotFromGeneratorNo(SelectedGeneratorDropList.GeneratorNo);
				if (slotFromGeneratorNo != null)
				{
					MapData.main.CurrentMapBoardData.SetGeneratorDropBlock(slotFromGeneratorNo.boardPosition, SelectedGeneratorDropListIndex, "N" + blockId);
				}
			}
		}
	}
}
