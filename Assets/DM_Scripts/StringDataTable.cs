using System;
using System.Collections;
using System.Collections.Generic;

public class StringDataTable
{
	public Dictionary<string, StringData> m_Table = new Dictionary<string, StringData>();

	public bool Load(string strFileName, LanguageCode lang)
	{
		return LoadXML(Utils.GetResourcesStringFile(strFileName), lang);
	}

	public bool LoadXML(string text, LanguageCode lang)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(text);
		XMLNodeList nodeList = xMLNode.GetNodeList("table>0>StrIndex");
		IEnumerator enumerator = nodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				XMLNode xMLNode2 = current as XMLNode;
				StringData stringData = new StringData();
				string value = xMLNode2.GetValue("Index>0>_text");
				if (value != null)
				{
					stringData.m_strIndex = value;
				}
				value = xMLNode2.GetValue(lang + ">0>_text");
				if (value != null)
				{
					stringData.m_strData = value;
				}
				if (m_Table.ContainsKey(stringData.m_strIndex))
				{
				}
				m_Table.Add(stringData.m_strIndex, stringData);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}

	public bool LookupData(string strIndex, ref StringData rData)
	{
		if (!m_Table.ContainsKey(strIndex))
		{
			return false;
		}
		rData = m_Table[strIndex];
		return true;
	}
}
