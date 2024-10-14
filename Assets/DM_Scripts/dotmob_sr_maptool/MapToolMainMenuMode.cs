/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolMainMenuMode : MonoBehaviour
	{
		public static readonly int MaxBossThrowBlockCount = 6;

		[Header("JewelHunter Mode")]
		public Dropdown DropDownSubLevel;

		public Dropdown DropDownSubLevelDigging;

		public CanvasGroup groupGraph;

		public Image[] ImageBossThrowObsBlocks;

		public Image[] ImageBringDownObjs = new Image[MapData.MaxKindOfBringDown];

		public Image[] ImageGraphs = new Image[MapData.MaxKindOfBringDown];

		[Header("RescueVS Mode")]
		public InputField[] InputBossThrowObsProb;

		public InputField[] InputBringDownProb = new InputField[MapData.MaxKindOfBringDown];

		[Header("BringDown Mode")]
		public InputField InputFieldBringDownKeepShowObject;

		public Text TextBossThrowObsTotalProb;

		public Text TextTotalProb;

		[Header("Digging Mode")]
		public Toggle[] ToggleDiggingDirections;

		private void Start()
		{
			ModeBringDownRefresh();
			RefreshBossThrowObs();
			SetDiggingDirection(MapData.main.diggingScrollDirection);
		}

		public void Reset()
		{
			ModeBringDownRefresh();
			RefreshBossThrowObs();
		}

		public void RefreshBossThrowObs()
		{
			for (int i = 0; i < MaxBossThrowBlockCount; i++)
			{
				if (MapData.main.bossThrowObsProbs[i] == 0)
				{
					InputBossThrowObsProb[i].text = "0";
					ImageBossThrowObsBlocks[i].color = new Color(1f, 1f, 1f, 0.2f);
				}
				else
				{
					InputBossThrowObsProb[i].text = MapData.main.bossThrowObsProbs[i].ToString();
					ImageBossThrowObsBlocks[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
		}

		public void OnChangedBossThrowProbInputText(int index)
		{
			MapData.main.bossThrowObsProbs[index] = int.Parse(InputBossThrowObsProb[index].text);
			RefreshBossThrowObs();
		}

		public void ResetSubLevelList(int maxSize)
		{
			DropDownSubLevel.options.Clear();
			for (int i = 0; i < maxSize; i++)
			{
				DropDownSubLevel.options.Add(new Dropdown.OptionData($"{MapData.main.gid} - {i + 1}"));
			}
			MapData.main.currentBoardDataIndex = 0;
			DropDownSubLevel.captionText.text = DropDownSubLevel.options[0].text;
			DropDownSubLevel.value = 0;
		}

		public void OnSelectDropDownSubLevel(int index)
		{
			if (MapData.main.currentBoardDataIndex != index)
			{
				MapData.main.currentBoardDataIndex = index;
				MonoSingleton<MapToolManager>.Instance.ResetCurrentBoardData();
			}
		}

		public void OnPressButtonNewSubLevel()
		{
			ResetSubLevelList(DropDownSubLevel.options.Count + 1);
			MapData.main.AddNewSubMap();
			DropDownSubLevel.value = DropDownSubLevel.options.Count - 1;
		}

		public void OnPressButtonPrevSubLevel()
		{
			DropDownSubLevel.value--;
		}

		public void OnPressButtonNextSubLevel()
		{
			DropDownSubLevel.value++;
		}

		public void OnPressButtonRemoveSubLevel()
		{
			MapData.main.RemoveSubMap(MapData.main.currentBoardDataIndex);
			ResetSubLevelList(MapData.main.SubMapCount);
			MonoSingleton<MapToolManager>.Instance.ResetCurrentBoardData();
		}

		public void SetBringDownKeepShowObject(int keepShowObjectCount)
		{
			InputFieldBringDownKeepShowObject.text = keepShowObjectCount.ToString();
		}

		public void OnValueChangeBringDownKeepShowObject(string value)
		{
			int.TryParse(value, out MapData.main.keepShowBringDownObjectCount);
		}

		public void ModeBringDownRefresh()
		{
			for (int i = 0; i < MapData.MaxKindOfBringDown; i++)
			{
				if (MapData.main.dropBringDownProb[i] == 0)
				{
					InputBringDownProb[i].text = "0";
					ImageBringDownObjs[i].color = new Color(1f, 1f, 1f, 0.2f);
				}
				else
				{
					InputBringDownProb[i].text = MapData.main.dropBringDownProb[i].ToString();
					ImageBringDownObjs[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			CalculateBringDownGraph();
		}

		private void CalculateBringDownGraph()
		{
			int num = 0;
			for (int i = 0; i < MapData.main.dropBringDownProb.Length; i++)
			{
				num += MapData.main.dropBringDownProb[i];
			}
			TextTotalProb.text = $"{num}%";
			if (num != 100)
			{
				TextTotalProb.color = Color.red;
				groupGraph.alpha = 0.2f;
				return;
			}
			TextTotalProb.color = Color.white;
			groupGraph.alpha = 1f;
			int num2 = 0;
			for (int j = 0; j < MapData.main.dropBringDownProb.Length; j++)
			{
				if (MapData.main.dropBringDownProb[j] == 0)
				{
					ImageGraphs[j].enabled = false;
					continue;
				}
				ImageGraphs[j].enabled = true;
				ImageGraphs[j].fillAmount = (float)(num2 + MapData.main.dropBringDownProb[j]) / 100f;
				num2 += MapData.main.dropBringDownProb[j];
			}
		}

		public void OnChangedInputText(int index)
		{
			MapData.main.dropBringDownProb[index] = int.Parse(InputBringDownProb[index].text);
			ModeBringDownRefresh();
		}

		public void OnPressControlProbButton(string buttonName)
		{
			int num = int.Parse(buttonName.Substring(0, 1));
			char c = buttonName[1];
			int num2 = 1;
			if (c == 'L')
			{
				num2 = -1;
			}
			MapData.main.dropBringDownProb[num] = Mathf.Min(100, Mathf.Max(0, MapData.main.dropBringDownProb[num] + num2));
			ModeBringDownRefresh();
		}

		public void SetDiggingDirection(int direction)
		{
			for (int i = 0; i < ToggleDiggingDirections.Length; i++)
			{
				if (direction > 0 && direction <= 4)
				{
					ToggleDiggingDirections[direction - 1].isOn = true;
				}
			}
		}

		public void OnValueChangedDiggingDirection(int index)
		{
			if (ToggleDiggingDirections[index - 1].isOn)
			{
				MapData.main.diggingScrollDirection = index;
			}
		}

		public void ResetSubLevelListDiggingMode(int maxSize)
		{
			DropDownSubLevelDigging.options.Clear();
			for (int i = 0; i < maxSize; i++)
			{
				DropDownSubLevelDigging.options.Add(new Dropdown.OptionData($"{MapData.main.gid} - {i + 1}"));
			}
			MapData.main.currentBoardDataIndex = 0;
			DropDownSubLevelDigging.captionText.text = DropDownSubLevelDigging.options[0].text;
			DropDownSubLevelDigging.value = 0;
		}

		public void OnSelectDropDownSubLevelDiggingMode(int index)
		{
			if (MapData.main.currentBoardDataIndex != index)
			{
				MapData.main.currentBoardDataIndex = index;
				MonoSingleton<MapToolManager>.Instance.ResetCurrentBoardData();
			}
		}

		public void OnPressButtonNewSubLevelDiggingMode()
		{
			ResetSubLevelListDiggingMode(DropDownSubLevelDigging.options.Count + 1);
			MapData.main.AddNewSubMap();
			DropDownSubLevelDigging.value = DropDownSubLevelDigging.options.Count - 1;
		}

		public void OnPressButtonPrevSubLevelDiggingMode()
		{
			DropDownSubLevelDigging.value--;
		}

		public void OnPressButtonNextSubLevelDiggingMode()
		{
			DropDownSubLevelDigging.value++;
		}

		public void OnPressButtonRemoveSubLevelDiggingMode()
		{
			MapData.main.RemoveSubMap(MapData.main.currentBoardDataIndex);
			ResetSubLevelListDiggingMode(MapData.main.SubMapCount);
			MonoSingleton<MapToolManager>.Instance.ResetCurrentBoardData();
		}
	}
}
