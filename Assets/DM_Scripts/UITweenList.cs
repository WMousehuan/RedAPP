using System.Collections.Generic;
using UnityEngine;

public class UITweenList : MonoBehaviour
{
	[SerializeField]
	private List<UITweener> m_listTweenClose = new List<UITweener>();

	[SerializeField]
	private List<UITweener> m_listTweenOpen = new List<UITweener>();

	public List<UITweener> listTweenOpen => m_listTweenOpen;

	private void Awake()
	{
		OnSortStartDelay();
		ResetTweenerList();
	}

	private void ResetTweenerList()
	{
		UITweener uITweener = null;
		for (int i = 0; i < m_listTweenClose.Count; i++)
		{
			uITweener = m_listTweenClose[i];
			uITweener.enabled = false;
			uITweener.ResetToBeginning();
		}
		for (int j = 0; j < m_listTweenOpen.Count; j++)
		{
			uITweener = m_listTweenOpen[j];
			uITweener.enabled = false;
			uITweener.ResetToBeginning();
		}
	}

	public void PlayTweenListOpen()
	{
		UITweener uITweener = null;
		for (int i = 0; i < m_listTweenOpen.Count; i++)
		{
			uITweener = m_listTweenOpen[i];
			uITweener.ResetToBeginning();
			uITweener.enabled = true;
		}
	}

	public void PlayTweenListClose()
	{
		UITweener uITweener = null;
		for (int i = 0; i < m_listTweenClose.Count; i++)
		{
			uITweener = m_listTweenClose[i];
			uITweener.ResetToBeginning();
			uITweener.enabled = true;
		}
	}

	public void OnSortStartDelay()
	{
		m_listTweenOpen.Sort((UITweener x, UITweener y) => y.delay.CompareTo(x.delay));
		m_listTweenClose.Sort((UITweener x, UITweener y) => y.delay.CompareTo(x.delay));
	}
}
