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
	public class MapToolMovingButton : MonoBehaviour
	{
		public Image[] ImageDirection;

		public MapToolMainMenuSlotSubMenuMoving.MovingDirection movingDirection = MapToolMainMenuSlotSubMenuMoving.MovingDirection.None;

		private readonly Vector2[] orgDirectionSizeDelta = new Vector2[4];

		public int posX;

		public int posY;

		public RectTransform RectTSelectedGroup;

		public RectTransform SelectedBackPanel;

		public RectTransform RectTDirectionGroup;

		public int SizeHeight;

		public int SizeWidth;

		public bool IsSelected
		{
			get;
			private set;
		}

		public MapToolMovingButton()
		{
			IsSelected = false;
		}

		private void Start()
		{
			for (int i = 0; i < ImageDirection.Length; i++)
			{
				orgDirectionSizeDelta[i] = ImageDirection[i].GetComponent<RectTransform>().sizeDelta;
			}
		}

		private void Update()
		{
		}

		public void Select(MapToolMainMenuSlotSubMenuMoving.MovingDirection movingDirection, int sizeWidth, int sizeHeight, int moveCount)
		{
			IsSelected = true;
			RectTSelectedGroup.gameObject.SetActive(value: true);
			SizeWidth = sizeWidth;
			SizeHeight = sizeHeight;
			SelectedBackPanel.sizeDelta = new Vector2(50 * sizeWidth, 50 * sizeHeight);
			RectTDirectionGroup.localPosition = Vector3.zero;
			for (int i = 0; i < ImageDirection.Length; i++)
			{
				if (i == (int)movingDirection)
				{
					ImageDirection[i].gameObject.SetActive(value: true);
					ImageDirection[i].GetComponent<RectTransform>().sizeDelta = orgDirectionSizeDelta[i];
					Vector2 sizeDelta = orgDirectionSizeDelta[i];
					sizeDelta.x += 50 * (moveCount - 1);
					ImageDirection[i].GetComponent<RectTransform>().sizeDelta = sizeDelta;
					switch (movingDirection)
					{
					case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Right:
						RectTDirectionGroup.localPosition += new Vector3(50 * (sizeWidth - 1), 0f, 0f);
						break;
					case MapToolMainMenuSlotSubMenuMoving.MovingDirection.Down:
						RectTDirectionGroup.localPosition -= new Vector3(0f, 50 * (sizeHeight - 1), 0f);
						break;
					}
				}
				else
				{
					ImageDirection[i].gameObject.SetActive(value: false);
				}
			}
		}

		public void Deselect()
		{
			IsSelected = false;
			RectTSelectedGroup.gameObject.SetActive(value: false);
		}

		public void OnClickButton()
		{
			MonoSingleton<MapToolManager>.Instance.OnSelectMovingSlot(this);
		}
	}
}
