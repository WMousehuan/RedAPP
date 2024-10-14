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
	public class GeneratorDropList : MonoBehaviour
	{
		private int generatorNo;

		public Image[] ImageDropBlocks = new Image[7];

		public Text TextGeneratorIndex;

		public int GeneratorNo
		{
			get
			{
				return generatorNo;
			}
			set
			{
				TextGeneratorIndex.text = value.ToString();
				generatorNo = value;
			}
		}

		public void SetBlock(int index, Sprite sprite, string block)
		{
			if (block == "RA")
			{
				ImageDropBlocks[index].enabled = false;
				return;
			}
			ImageDropBlocks[index].enabled = true;
			ImageDropBlocks[index].sprite = sprite;
		}

		public void OnPressSelectDropBlockButton(int dropIndex)
		{
			MonoSingleton<MapToolManager>.Instance.OnSelectGeneratorDropListToHighlight(GeneratorNo);
			MonoSingleton<MapToolManager>.Instance.generatorMenu.OnPressSelectGeneratorDropList(this, dropIndex);
		}
	}
}
