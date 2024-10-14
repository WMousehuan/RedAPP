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
	public class MapToolMainMenuCollect : MonoBehaviour
	{
		public Image[] ImageBlocks = new Image[MapData.MaxCollect];

		public InputField[] InputCollectCount = new InputField[MapData.MaxCollect];

		public MapToolMainMenu.MenuType menuType = MapToolMainMenu.MenuType.Collect;

		public GameObject ObjListSelectCollectBlock;

		public GameObject ObjListSelectRescueBear;

		private int selectedBlockIndex = -1;

		public Toggle[] ToggleCollectBlocks = new Toggle[MapData.MaxCollect];

		public MapToolTopMenu topMenu;

		private void Start()
		{
			Refresh();
			Button[] componentsInChildren = ObjListSelectCollectBlock.GetComponentsInChildren<Button>(includeInactive: true);
			foreach (Button btn in componentsInChildren)
			{
				SettingBlockListButton(btn);
			}
			ObjListSelectCollectBlock.SetActive(value: false);
			ObjListSelectRescueBear.SetActive(value: false);
		}

		public void Reset()
		{
			Refresh();
			ObjListSelectCollectBlock.SetActive(value: false);
			ObjListSelectRescueBear.SetActive(value: false);
			if (selectedBlockIndex != -1)
			{
				ToggleCollectBlocks[selectedBlockIndex].targetGraphic.color = Color.white;
			}
			selectedBlockIndex = -1;
		}

		public void Refresh()
		{
			for (int i = 0; i < MapData.MaxCollect; i++)
			{
				InputCollectCount[i].text = MapData.main.collectBlocks[i].count.ToString();
				if (string.IsNullOrEmpty(MapData.main.collectBlocks[i].blockType))
				{
					ImageBlocks[i].enabled = false;
					ImageBlocks[i].sprite = null;
				}
				else
				{
					ImageBlocks[i].enabled = true;
					ImageBlocks[i].sprite = MonoSingleton<MapToolManager>.Instance.GetBlockSprite(MapData.main.collectBlocks[i].blockType);
					ImageBlocks[i].color = ((MapData.main.collectBlocks[i].count != 0) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.2f));
				}
			}
			topMenu.RefreshCollect();
		}

		private void Update()
		{
		}

		private void SettingBlockListButton(Button btn)
		{
			btn.onClick.AddListener(delegate
			{
				OnPressSelectBlock(btn);
			});
		}

		public void OnChangedInputText(int index)
		{
			if (int.Parse(InputCollectCount[index].text) == 0 || string.IsNullOrEmpty(MapData.main.collectBlocks[index].blockType))
			{
				MapData.main.collectBlocks[index].count = 0;
				InputCollectCount[index].text = "0";
				topMenu.RefreshCollect();
				Refresh();
			}
			else
			{
				MapData.main.collectBlocks[index].count = int.Parse(InputCollectCount[index].text);
				Refresh();
			}
		}

		public void OnChangedToggleBlocks(int index)
		{
			if (ToggleCollectBlocks[index].isOn)
			{
				ToggleCollectBlocks[index].targetGraphic.color = Color.red;
				selectedBlockIndex = index;
				ObjListSelectCollectBlock.SetActive(value: true);
				if (MapData.main.collectBlocks[index].blockType == "RB")
				{
					ObjListSelectRescueBear.SetActive(value: true);
					MonoSingleton<MapToolManager>.Instance.ToggleShowRescueBearLayer(changed: true);
				}
			}
			else
			{
				selectedBlockIndex = -1;
				ObjListSelectCollectBlock.SetActive(value: false);
				ObjListSelectRescueBear.SetActive(value: false);
				ToggleCollectBlocks[index].targetGraphic.color = Color.white;
			}
		}

		public void OnPressControlCountButton(string buttonName)
		{
			int num = int.Parse(buttonName.Substring(0, 1));
			if (!string.IsNullOrEmpty(MapData.main.collectBlocks[num].blockType))
			{
				char c = buttonName[1];
				int num2 = 1;
				if (c == 'L')
				{
					num2 = -1;
				}
				MapData.main.collectBlocks[num].count = Mathf.Max(0, MapData.main.collectBlocks[num].count + num2);
				Refresh();
			}
		}

		public void OnPressSelectBlock(Button blockButton)
		{
			if (selectedBlockIndex == -1)
			{
				return;
			}
			ObjListSelectRescueBear.SetActive(value: false);
			if (blockButton.name == "X")
			{
				ImageBlocks[selectedBlockIndex].enabled = false;
				ImageBlocks[selectedBlockIndex].sprite = null;
				MapData.main.collectBlocks[selectedBlockIndex].blockType = string.Empty;
				MapData.main.collectBlocks[selectedBlockIndex].count = 0;
			}
			else if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
			{
				//TODO
			}
			else
			{
				ImageBlocks[selectedBlockIndex].enabled = true;
				ImageBlocks[selectedBlockIndex].sprite = blockButton.image.sprite;
				MapData.main.collectBlocks[selectedBlockIndex].blockType = blockButton.name;
				if (blockButton.name == "RB")
				{
					ObjListSelectRescueBear.SetActive(value: true);
					MonoSingleton<MapToolManager>.Instance.ToggleShowRescueBearLayer(changed: true);
				}
			}
			Refresh();
		}
	}
}
