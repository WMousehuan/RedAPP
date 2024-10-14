using System;
using System.Collections.Generic;
using UnityEngine;

public class UIServer : MonoBehaviour
{
	[Serializable]
	public struct Page
	{
		public string name;

		public string[] panels;
	}

	public static UIServer main;

	private string currentPage;

	public string defaultPage;

	private readonly Dictionary<string, Dictionary<string, bool>> dPages = new Dictionary<string, Dictionary<string, bool>>();

	private readonly Dictionary<string, CPanel> dPanels = new Dictionary<string, CPanel>();

	public Page[] pages;

	private string previousPage;

	private void Start()
	{
		ArraysConvertation();
		if (!string.IsNullOrEmpty(defaultPage))
		{
			ShowPage(defaultPage);
		}
	}

	private void Awake()
	{
		main = this;
	}

	private void ArraysConvertation()
	{
		CPanel[] componentsInChildren = GetComponentsInChildren<CPanel>(includeInactive: true);
		foreach (CPanel cPanel in componentsInChildren)
		{
			dPanels.Add(cPanel.name, cPanel);
		}
		Page[] array = pages;
		for (int j = 0; j < array.Length; j++)
		{
			Page page = array[j];
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			for (int k = 0; k < page.panels.Length; k++)
			{
				dictionary.Add(page.panels[k], value: true);
			}
			dPages.Add(page.name, dictionary);
		}
	}

	public void ShowPage(string p)
	{
		if (CPanel.uiAnimation <= 0 && !(currentPage == p))
		{
			previousPage = currentPage;
			currentPage = p;
			foreach (string key in dPanels.Keys)
			{
				if (!dPages[p].ContainsKey("?" + key))
				{
					dPanels[key].SetActive(dPages[p].ContainsKey(key));
				}
			}
		}
	}

	public void HideAll()
	{
		foreach (string key in dPanels.Keys)
		{
			dPanels[key].SetActive(a: false);
		}
	}

	public void ShowPreviousPage()
	{
		ShowPage(previousPage);
	}

	public void SetPause(bool p)
	{
		Time.timeScale = ((!p) ? 1 : 0);
	}
}
