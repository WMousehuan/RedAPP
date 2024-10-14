/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotmob.UnityEngine.UI.Ext
{
	public abstract class DynamicScrollRect : ScrollRect
	{
		private RectTransform _t;

		private int eachLineCount = 1;

		private IEventDynamicScrollRect eventObject;

		private int firstListCount;

		private bool init;

		private float ItemAnotherSize;

		private float ItemSize;

		private int listMaxCount;

		private List<GameObject> listObjects;

		private int listStartIndex;

		private RectTransform[] prefabItems;

		private float viewListSize;

		private float viewStartPosition;

		protected RectTransform t
		{
			get
			{
				if (_t == null)
				{
					_t = GetComponent<RectTransform>();
				}
				return _t;
			}
		}

		public float ItemAnotherOffset
		{
			get;
			set;
		}

		protected abstract float GetDimension(Vector2 vector);

		protected abstract Vector2 GetVector(float value);

		protected abstract Vector2 GetReverseVector(float value);

		protected abstract int OneOrMinusOne();

		protected abstract int ReverseOneOrMinusOne();

		protected abstract float GetViewSize();

		private new void Awake()
		{
			if (Application.isPlaying)
			{
			}
		}

		public void RemoveAllListObjects()
		{
			if (listObjects != null && listObjects.Count > 0)
			{
				foreach (GameObject listObject in listObjects)
				{
                    DestroyImmediate(listObject);
				}
				listObjects.Clear();
			}
		}

		public GameObject GetFirstListItem()
		{
			if (listObjects != null && listObjects.Count > 0 && listStartIndex < listObjects.Count)
			{
				return listObjects[listStartIndex];
			}
			return null;
		}

		public void Init(IEventDynamicScrollRect _eventObject, List<GameObject> _listFirstObjects, int maxListCount, float _itemSize, float _itemSpacing, int _eachLineCount = 1, float _itemAnotherSize = 0f, float _itemAnotherSpacing = 0f, float _itemAnotherOffset = 0f, float _addContentSize = 0f)
		{
			init = true;
			eventObject = _eventObject;
			listObjects = _listFirstObjects;
			ItemSize = _itemSize + _itemSpacing;
			listMaxCount = maxListCount;
			eachLineCount = _eachLineCount;
			ItemAnotherSize = _itemAnotherSize + _itemAnotherSpacing;
			ItemAnotherOffset = _itemAnotherOffset;
			base.content.sizeDelta = GetVector(ItemSize * (float)((listMaxCount - 1) / eachLineCount + 1) + _addContentSize);
			RectTransform content = base.content;
			Vector2 anchoredPosition2;
			if (OneOrMinusOne() == 1)
			{
				Vector2 anchoredPosition = base.content.anchoredPosition;
				anchoredPosition2 = new Vector2(anchoredPosition.x, 0f);
			}
			else
			{
				Vector2 anchoredPosition3 = base.content.anchoredPosition;
				anchoredPosition2 = new Vector2(0f, anchoredPosition3.y);
			}
			content.anchoredPosition = anchoredPosition2;
			float num = _itemSpacing * (float)OneOrMinusOne();
			for (int i = 0; i < _listFirstObjects.Count; i++)
			{
				if (i > 0 && i % eachLineCount == 0)
				{
					num += ItemSize * (float)OneOrMinusOne();
				}
				_listFirstObjects[i].transform.localPosition = GetVector(num);
				_listFirstObjects[i].transform.localPosition += (Vector3)GetReverseVector(ItemAnotherOffset + ItemAnotherSize * (float)(i % eachLineCount));
			}
			viewListSize = ItemSize * (float)((_listFirstObjects.Count - 1) / eachLineCount + 1);
			viewStartPosition = 0f;
			listStartIndex = 0;
			firstListCount = listObjects.Count;
		}

		private void Update()
		{
			if (Application.isPlaying && init && GetDimension(base.content.sizeDelta) != 0f)
			{
				if (GetDimension(base.content.localPosition) * (float)ReverseOneOrMinusOne() + GetViewSize() >= viewStartPosition + viewListSize)
				{
					OnListAtEnd();
				}
				else if (GetDimension(base.content.localPosition) * (float)ReverseOneOrMinusOne() <= viewStartPosition + ItemSize)
				{
					OnListAtStart();
				}
			}
		}

		private void OnListAtEnd()
		{
			GameObject gameObject = null;
			if (listObjects.Count == 0)
			{
				return;
			}
			do
			{
				int num = listStartIndex + firstListCount;
				do
				{
					if (num >= listMaxCount)
					{
						return;
					}
					gameObject = eventObject.GetReplaceObject(listObjects[listStartIndex], num);
					if ((bool)gameObject)
					{
						listObjects[listStartIndex] = null;
						if (num >= listObjects.Count)
						{
							listObjects.Add(gameObject);
						}
						else
						{
							listObjects[num] = gameObject;
						}
						gameObject.transform.SetAsLastSibling();
						gameObject.transform.localPosition = GetVector(viewStartPosition + viewListSize) * OneOrMinusOne();
						gameObject.transform.localPosition += (Vector3)GetReverseVector(ItemAnotherOffset + ItemAnotherSize * (float)(num % eachLineCount));
						listStartIndex++;
						num++;
					}
				}
				while (num % eachLineCount > 0);
				viewStartPosition += ItemSize;
			}
			while (GetDimension(base.content.localPosition) * (float)ReverseOneOrMinusOne() + GetViewSize() >= viewStartPosition + viewListSize);
		}

		private void OnListAtStart()
		{
			if (listObjects.Count == 0)
			{
				return;
			}
			while (listStartIndex != 0)
			{
				if (listStartIndex % eachLineCount == 0)
				{
					viewStartPosition -= ItemSize;
				}
				do
				{
					if (listStartIndex == 0)
					{
						return;
					}
					int num = listStartIndex + firstListCount - 1;
					if (num < 0 || num >= listObjects.Count)
					{
						return;
					}
					GameObject gameObject = null;
					gameObject = eventObject.GetReplaceObject(listObjects[num], listStartIndex - 1);
					if ((bool)gameObject)
					{
						listObjects[num] = null;
						listObjects[listStartIndex - 1] = gameObject;
						gameObject.transform.SetAsFirstSibling();
						gameObject.transform.localPosition = GetVector(viewStartPosition) * OneOrMinusOne();
						gameObject.transform.localPosition += (Vector3)GetReverseVector(ItemAnotherOffset + ItemAnotherSize * (float)((listStartIndex - 1) % eachLineCount));
						listStartIndex--;
						num--;
					}
				}
				while (listStartIndex % eachLineCount > 0);
				if (!(GetDimension(base.content.localPosition) * (float)ReverseOneOrMinusOne() <= viewStartPosition))
				{
					break;
				}
			}
		}
	}
}
