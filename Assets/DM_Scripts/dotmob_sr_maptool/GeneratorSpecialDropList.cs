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
	public class GeneratorSpecialDropList : MonoBehaviour
	{
		private readonly Dictionary<string, GeneratorSpecialDropItems> dicItems = new Dictionary<string, GeneratorSpecialDropItems>();

		private int generatorNo;

		public Text TextGeneratorIndex;

		public Text TextTotalProb;

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

		private void Start()
		{
			CheckValidDicItems();
			RefreshTotalProb();
		}

		private void CheckValidDicItems()
		{
			if (dicItems.Count != 0)
			{
				return;
			}
			GeneratorSpecialDropItems[] componentsInChildren = base.transform.GetComponentsInChildren<GeneratorSpecialDropItems>();
			foreach (GeneratorSpecialDropItems generatorSpecialDropItems in componentsInChildren)
			{
				if (!dicItems.ContainsKey(generatorSpecialDropItems.name))
				{
					dicItems.Add(generatorSpecialDropItems.name, generatorSpecialDropItems);
				}
			}
		}

		public void SetBlock(string block, int prob)
		{
			CheckValidDicItems();
			if (!string.IsNullOrEmpty(block) && dicItems.ContainsKey(block))
			{
				dicItems[block].transform.GetComponentInChildren<InputField>().text = prob.ToString();
				RefreshTotalProb();
			}
		}

		private void RefreshTotalProb()
		{
			int num = 0;
			foreach (GeneratorSpecialDropItems value in dicItems.Values)
			{
				int result = 0;
				string text = value.GetComponentInChildren<InputField>().text;
				if (!string.IsNullOrEmpty(text))
				{
					int.TryParse(text, out result);
					num += result;
				}
			}
			TextTotalProb.text = num.ToString();
			if (num == 100)
			{
				TextTotalProb.color = Color.white;
			}
			else
			{
				TextTotalProb.color = Color.red;
			}
		}

		public void OnChangeItemsProb(string objName, string strProb)
		{
			CheckValidDicItems();
			int result = 0;
			int.TryParse(strProb, out result);
			MonoSingleton<MapToolManager>.Instance.generatorSpecialMenu.SetProb(GeneratorNo, objName, result);
			RefreshTotalProb();
		}
	}
}
