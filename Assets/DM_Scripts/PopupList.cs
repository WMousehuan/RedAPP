using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PopupList : MonoBehaviour
{
	//public Dictionary<PopupType, GameObject> dicPopupList = new Dictionary<PopupType, GameObject>();

	//public List<GameObject> listPopupObject = new List<GameObject>();

	//public List<PopupType> listPopupType = new List<PopupType>();

	public ObjectGroup<PopupType, GameObject> popupObjectGroup;


	public GameObject GetPopup(PopupType type)
	{
		if (popupObjectGroup.ContainsKey(type))
		{
			return popupObjectGroup[type];
		}
		return null;
	}
}
