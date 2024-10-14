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
	public class MapToolSelectObjectObstacle : MapToolSelectObject
	{
		public InputField[] InputFieldParams;

		public MapToolSelectObjectSubMenuCement ListCement;

		public MapToolSelectObjectSubMenuNumberChocolate ListNumberChocolate;

		public MapToolSelectObjectSubMenuSpriteDrink ListSpriteDrink;

		public GameObject MapToolSubParamList;

		public Text[] TextTitleParams;

		public override void Start()
		{
			base.Start();
		}

		public override void OnEnable()
		{
			base.OnEnable();
			SetSubList(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey);
		}

		public override void SelectItem(GameObject selectedItem)
		{
			base.SelectItem(selectedItem);
			SetSubList(selectedItem.name);
		}

		private void SetSubList(string obsKey)
		{
			bool flag = obsKey == "O6" && !MapToolSubParamList.activeSelf;
			MapToolSubParamList.SetActive(flag);
			TextTitleParams[0].transform.parent.gameObject.SetActive(flag);
			TextTitleParams[1].transform.parent.gameObject.SetActive(flag);
			if (flag)
			{
				TextTitleParams[0].text = "Turn Count";
				TextTitleParams[1].text = "Num Count";
			}
			ListCement.gameObject.SetActive(obsKey == "O8" && !ListCement.gameObject.activeSelf);
			ListNumberChocolate.gameObject.SetActive(obsKey == "O13" && !ListNumberChocolate.gameObject.activeSelf);
			ListSpriteDrink.gameObject.SetActive(obsKey == "O11" && !ListSpriteDrink.gameObject.activeSelf);
		}
	}
}
