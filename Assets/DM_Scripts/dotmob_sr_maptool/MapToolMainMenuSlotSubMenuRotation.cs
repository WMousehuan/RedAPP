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
	public class MapToolMainMenuSlotSubMenuRotation : MonoBehaviour
	{
		public Dropdown DropdownSizeHeight;

		public Dropdown DropdownSizeWidth;

		public bool isClockwise = true;

		public int size;

		public Slider SliderSizeHeight;

		public Slider SliderSizeWidth;

		public Toggle ToggleClockwise;

		private void Start()
		{
			size = 2;
			DropdownSizeWidth.value = size - 2;
			SliderSizeWidth.value = size;
		}

		private void Update()
		{
		}

		private void OnEnable()
		{
			Refresh();
		}

		private void OnDisable()
		{
			MonoSingleton<MapToolManager>.Instance.HideRotationButton();
		}

		private void Refresh()
		{
			MonoSingleton<MapToolManager>.Instance.ShowEnableRotationButton();
		}

		public void OnChangedDirection(bool changed)
		{
			isClockwise = ToggleClockwise.isOn;
		}

		public void OnChangedDropdownSizeWidth(int index)
		{
			SliderSizeWidth.value = (size = index + 2);
			Refresh();
		}

		public void OnSliderSizeWidth(float value)
		{
			DropdownSizeWidth.value = (int)value - 2;
		}
	}
}
