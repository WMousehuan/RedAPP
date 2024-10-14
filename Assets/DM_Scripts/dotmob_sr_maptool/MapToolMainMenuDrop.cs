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
	public class MapToolMainMenuDrop : MonoBehaviour
	{
		public CanvasGroup groupGraph;

		public Image[] ImageBlocks = new Image[MapData.MaxBlockColor];

		public Image[] ImageGraphs = new Image[MapData.MaxBlockColor];

		public InputField[] InputBlockProb = new InputField[MapData.MaxBlockColor];

		public MapToolMainMenu.MenuType menuType;

		public Text TextTotalProb;

		private void Start()
		{
			Refresh();
		}

		public void Reset()
		{
			Refresh();
		}

		public void Refresh()
		{
			for (int i = 0; i < MapData.MaxBlockColor; i++)
			{
				if (MapData.main.CurrentMapBoardData.dropBlockProb[i] == 0)
				{
					InputBlockProb[i].text = "0";
					ImageBlocks[i].color = new Color(1f, 1f, 1f, 0.2f);
				}
				else
				{
					InputBlockProb[i].text = MapData.main.CurrentMapBoardData.dropBlockProb[i].ToString();
					ImageBlocks[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			CalculateGraph();
		}

		private void CalculateGraph()
		{
			int num = 0;
			for (int i = 0; i < MapData.main.CurrentMapBoardData.dropBlockProb.Length; i++)
			{
				num += MapData.main.CurrentMapBoardData.dropBlockProb[i];
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
			for (int j = 0; j < MapData.main.CurrentMapBoardData.dropBlockProb.Length; j++)
			{
				if (MapData.main.CurrentMapBoardData.dropBlockProb[j] == 0)
				{
					ImageGraphs[j].enabled = false;
					continue;
				}
				ImageGraphs[j].enabled = true;
				ImageGraphs[j].fillAmount = (float)(num2 + MapData.main.CurrentMapBoardData.dropBlockProb[j]) / 100f;
				num2 += MapData.main.CurrentMapBoardData.dropBlockProb[j];
			}
		}

		public void OnChangedInputText(int index)
		{
			MapData.main.CurrentMapBoardData.dropBlockProb[index] = int.Parse(InputBlockProb[index].text);
			Refresh();
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
			MapData.main.CurrentMapBoardData.dropBlockProb[num] = Mathf.Min(100, Mathf.Max(0, MapData.main.CurrentMapBoardData.dropBlockProb[num] + num2));
			Refresh();
		}
	}
}
