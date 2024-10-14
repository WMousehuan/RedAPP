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
	public class MapToolSelectObjectTabDirection : MapToolSelectObject
	{
		public MapToolGeneratorMenu generatorMenu;

		public MapToolGeneratorSpecialMenu generatorSpecialMenu;

		public Image[] ImageDropIcons;

		public MapToolSelectObjectSubMenuRail ObjListRail;

		public MapToolSelectObjectSubMenuRibbon ObjListRibbon;

		public MapToolSelectObjectSubMenuTunnel ObjListTunnel;

		public MapToolSelectObjectSubMenuRibbon ObjListYarn;

		public override void Start()
		{
			base.Start();
		}

		public override void OnEnable()
		{
			base.OnEnable();
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				generatorMenu.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "G");
			}
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				generatorSpecialMenu.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "GS");
			}
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				ObjListRail.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "ImgRail");
			}
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				ObjListTunnel.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "ImgTunnel");
			}
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				ObjListRibbon.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "ImgRibbon");
			}
			if (!string.IsNullOrEmpty(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey))
			{
				ObjListYarn.gameObject.SetActive(MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey == "ImgYarn");
			}
		}

		public void OnValueChangedDropLock(int dropLockIndex)
		{
			if (selectedItem == ImageDropIcons[dropLockIndex].gameObject)
			{
				SelectItem(ImageDropIcons[dropLockIndex].gameObject);
			}
		}

		public override void SelectItem(GameObject selectedItem)
		{
			base.SelectItem(selectedItem);
			generatorMenu.gameObject.SetActive(selectedItem.name == "G" && !generatorMenu.gameObject.activeSelf);
			generatorSpecialMenu.gameObject.SetActive(selectedItem.name == "GS" && !generatorSpecialMenu.gameObject.activeSelf);
			ObjListRail.gameObject.SetActive(selectedItem.name == "ImgRail" && !ObjListRail.gameObject.activeSelf);
			ObjListTunnel.gameObject.SetActive(selectedItem.name == "ImgTunnel" && !ObjListTunnel.gameObject.activeSelf);
			ObjListRibbon.gameObject.SetActive(selectedItem.name == "ImgRibbon" && !ObjListRibbon.gameObject.activeSelf);
			ObjListYarn.gameObject.SetActive(selectedItem.name == "ImgYarn" && !ObjListYarn.gameObject.activeSelf);
		}
	}
}
