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
	public class MapToolMainMenuSlotSubMenuMoving : MonoBehaviour
	{
		public enum MovingDirection
		{
			None = -1,
			Right,
			Up,
			Left,
			Down
		}

		public MovingDirection direction;

		public Dropdown DropdownSizeHeight;

		public Dropdown DropdownSizeWidth;

		public Dropdown DropdownTurnCount;

		[HideInInspector]
		public int movingCount = 2;

		public int sizeHeight;

		public int sizeWidth;

		public Slider SliderMovingCount;

		public Slider SliderSizeHeight;

		public Slider SliderSizeWidth;

		public Text TextMovingCount;

		public Toggle[] ToggleDirection;

		[HideInInspector]
		public int turnCount = 3;

		private void Start()
		{
			TextMovingCount.text = movingCount.ToString();
			SliderMovingCount.value = movingCount;
			DropdownTurnCount.value = turnCount - 1;
			DropdownSizeWidth.value = sizeWidth - 1;
			DropdownSizeHeight.value = sizeHeight - 1;
			SliderSizeWidth.value = sizeWidth;
			SliderSizeHeight.value = sizeHeight;
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
			MonoSingleton<MapToolManager>.Instance.HideMovingButton();
		}

		private void Refresh()
		{
			MonoSingleton<MapToolManager>.Instance.ShowEnableMovingButton();
		}

		public void OnChangedDirection(int index)
		{
			if (ToggleDirection[index].isOn)
			{
				direction = (MovingDirection)index;
			}
			Refresh();
		}

		public void OnChangedDropdownTurnCount(int index)
		{
			turnCount = index + 1;
		}

		public void OnSliderMovingCount(float value)
		{
			movingCount = (int)value;
			TextMovingCount.text = movingCount.ToString();
			Refresh();
		}

		public void OnChangedDropdownSizeWidth(int index)
		{
			SliderSizeWidth.value = (sizeWidth = index + 1);
			Refresh();
		}

		public void OnSliderSizeWidth(float value)
		{
			DropdownSizeWidth.value = (int)value - 1;
		}

		public void OnChangedDropdownSizeHeight(int index)
		{
			SliderSizeHeight.value = (sizeHeight = index + 1);
			Refresh();
		}

		public void OnSliderSizeHeight(float value)
		{
			DropdownSizeHeight.value = (int)value - 1;
		}
	}
}
