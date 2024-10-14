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
	public class MapToolRotationButton : MonoBehaviour
	{
		public enum ButtonPositionType
		{
			Cell,
			Grid
		}

		public Image ImageRotation;

		public ButtonPositionType positionType;

		public int posX;

		public int posY;

		public RectTransform RectTSelectedGroup;

		public int SizeHeight;

		public int SizeWidth;

		public Sprite[] SpriteRotation;

		public bool IsSelected
		{
			get;
			private set;
		}

		public MapToolRotationButton()
		{
			IsSelected = false;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void Select(int size, bool isRotationClockwise)
		{
			IsSelected = true;
			RectTSelectedGroup.gameObject.SetActive(value: true);
			ImageRotation.sprite = SpriteRotation[(!isRotationClockwise) ? 1 : 0];
			SizeWidth = size;
			SizeHeight = size;
			RectTSelectedGroup.sizeDelta = new Vector2(50 * size, 50 * size);
		}

		public void Deselect()
		{
			IsSelected = false;
			RectTSelectedGroup.gameObject.SetActive(value: false);
		}

		public void OnClickButton()
		{
			MonoSingleton<MapToolManager>.Instance.OnSelectRotationSlot(this);
		}
	}
}
