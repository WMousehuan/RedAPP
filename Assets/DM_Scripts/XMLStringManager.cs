using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLStringManager : MonoSingleton<XMLStringManager>
{
	private Dictionary<string, Sprite> dicLocalizeAtlas;

	public Font[] FontListByLanguageCode;

	private bool isInit;

	[HideInInspector]
	public LanguageCode SelectedLanguageCode;

	private StringDataTable Table;

	public override void Awake()
	{
		base.Awake();
		LoadData(LanguageCode.en, firstLoad: true);
	}

	public void LoadData(LanguageCode newLangCode, bool firstLoad)
	{
		Table = null;
		Table = new StringDataTable();
		bool flag = false;
		if (newLangCode != SelectedLanguageCode)
		{
			flag = true;
		}
		SelectedLanguageCode = newLangCode;
		if (!isInit || flag)
		{
			Table.Load("Table/StrIndex", newLangCode);
		}
		isInit = true;
		if (flag)
		{
			StartCoroutine(RefreshAllLocalizingUI(newLangCode, firstLoad));
		}
	}

	private IEnumerator RefreshAllLocalizingUI(LanguageCode newLangCode, bool firstLoad)
	{
		if (!firstLoad)
		{
			MonoSingleton<UIManager>.Instance.ShowLoading();
			yield return null;
		}
		Sprite[] s = Resources.LoadAll<Sprite>("AtlasLocalizeImage/AtlasLocalize_" + newLangCode);
		if (s != null)
		{
			dicLocalizeAtlas = new Dictionary<string, Sprite>();
			for (int i = 0; i < s.Length; i++)
			{
				dicLocalizeAtlas.Add(s[i].name, s[i]);
			}
		}
		if (!firstLoad)
		{
			LocalizingText[] array = Object.FindObjectsOfType<LocalizingText>();
			LocalizingText[] array2 = array;
			foreach (LocalizingText localizingText in array2)
			{
				localizingText.Refresh();
			}
			LocalizingImage[] array3 = Object.FindObjectsOfType<LocalizingImage>();
			LocalizingImage[] array4 = array3;
			foreach (LocalizingImage localizingImage in array4)
			{
				localizingImage.Refresh();
			}
			MonoSingleton<UIManager>.Instance.HideLoading();
		}
	}

	public string GetString(string index)
	{
		if (!Table.m_Table.ContainsKey(index))
		{
			return string.Empty;
		}
		return Table.m_Table[index].m_strData;
	}

	public Sprite GetSprite(string imageName)
	{
		if (dicLocalizeAtlas.ContainsKey(imageName))
		{
			return dicLocalizeAtlas[imageName];
		}
		return null;
	}
}
