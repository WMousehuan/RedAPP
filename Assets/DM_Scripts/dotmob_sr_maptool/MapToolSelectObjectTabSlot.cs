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
	public class MapToolSelectObjectTabSlot : MapToolSelectObject
	{
		public enum SubMenuType
		{
			None,
			Rotation,
			Moving
		}

		public SubMenuType CurrentSubMenuType;

		private readonly List<GameObject> listSubMenuPanels = new List<GameObject>();

		public GameObject ObjSubMenuItemMoving;

		public GameObject ObjSubMenuItemRotation;

		public GameObject ObjSubMenuPanelMoving;

		public GameObject ObjSubMenuPanelRotation;

		public override void Start()
		{
			base.Start();
			listSubMenuPanels.Add(ObjSubMenuPanelRotation);
			listSubMenuPanels.Add(ObjSubMenuPanelMoving);
			foreach (GameObject listSubMenuPanel in listSubMenuPanels)
			{
				listSubMenuPanel.SetActive(value: false);
			}
		}

		public override void OnEnable()
		{
			base.OnEnable();
			if (CurrentSubMenuType == SubMenuType.Rotation)
			{
				MonoSingleton<MapToolManager>.Instance.ShowEnableRotationButton();
			}
		}

		public override void SelectItem(GameObject selectedItem)
		{
			base.SelectItem(selectedItem);
			if (selectedItem == ObjSubMenuItemRotation)
			{
				SetSubMenu(SubMenuType.Rotation);
			}
			else if (selectedItem == ObjSubMenuItemMoving)
			{
				SetSubMenu(SubMenuType.Moving);
			}
			else
			{
				SetSubMenu(SubMenuType.None);
			}
		}

		private void SetSubMenu(SubMenuType menuType)
		{
			if (CurrentSubMenuType != menuType)
			{
				foreach (GameObject listSubMenuPanel in listSubMenuPanels)
				{
					listSubMenuPanel.SetActive(value: false);
				}
				switch (menuType)
				{
				case SubMenuType.Rotation:
					ObjSubMenuPanelRotation.SetActive(value: true);
					break;
				case SubMenuType.Moving:
					ObjSubMenuPanelMoving.SetActive(value: true);
					break;
				}
				CurrentSubMenuType = menuType;
			}
		}
	}
}
