using System.Collections;

public class XMLNode : Hashtable
{
	public XMLNodeList GetNodeList(string path)
	{
		return GetObject(path) as XMLNodeList;
	}

	public XMLNode GetNode(string path)
	{
		return GetObject(path) as XMLNode;
	}

	public string GetValue(string path)
	{
		string text = GetObject(path) as string;
		if (text == null)
		{
			return text;
		}
		return text.Trim();
	}

	private object GetObject(string path)
	{
		string[] array = path.Split('>');
		XMLNode xMLNode = this;
		XMLNodeList xMLNodeList = null;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			object obj;
			if (flag)
			{
				xMLNode = (XMLNode)xMLNodeList[int.Parse(array[i])];
				obj = xMLNode;
				flag = false;
				continue;
			}
			obj = xMLNode[array[i]];
			if (obj is ArrayList)
			{
				xMLNodeList = (XMLNodeList)(obj as ArrayList);
				flag = true;
				continue;
			}
			if (i != array.Length - 1)
			{
				string str = string.Empty;
				for (int j = 0; j <= i; j++)
				{
					str = str + ">" + array[j];
				}
			}
			return obj;
		}
		if (flag)
		{
			return xMLNodeList;
		}
		return xMLNode;
	}
}
