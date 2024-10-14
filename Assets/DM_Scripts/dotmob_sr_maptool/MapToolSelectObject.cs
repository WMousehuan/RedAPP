/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolSelectObject : MonoBehaviour
	{
		public LayoutGroup layoutGroup;

		public MapToolMainMenu.MenuType menuType;

		public GameObject ObjSelectedObject;

		protected GameObject selectedItem;

		public virtual void Start()
		{
			if ((bool)layoutGroup)
			{
				EventTrigger[] componentsInChildren = layoutGroup.gameObject.GetComponentsInChildren<EventTrigger>();
				foreach (EventTrigger eventTrigger in componentsInChildren)
				{
					EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
					triggerEvent.AddListener(OnPointerClick);
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.callback = triggerEvent;
					entry.eventID = EventTriggerType.PointerClick;
					EventTrigger.Entry item = entry;
					eventTrigger.triggers.Add(item);
				}
			}
		}

		public virtual void OnEnable()
		{
			if (selectedItem == null || selectedItem.name != MonoSingleton<MapToolManager>.Instance.SelectedSpriteKey)
			{
				ObjSelectedObject.SetActive(value: false);
			}
		}

		public virtual void SelectItem(GameObject selectedItem)
		{
			MonoSingleton<MapToolManager>.Instance.OnSelectItem(menuType, selectedItem);
		}

		protected void OnPointerClick(BaseEventData eventData)
		{
			PointerEventData pointerEventData = eventData as PointerEventData;
			if (pointerEventData.pointerEnter != null)
			{
				selectedItem = pointerEventData.pointerEnter;
				if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
				{
					
					return;
				}
				if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
				{
					
					return;
				}
				ObjSelectedObject.SetActive(value: true);
				ObjSelectedObject.transform.position = pointerEventData.pointerEnter.transform.position;
				SelectItem(selectedItem);
			}
		}
	}
}
