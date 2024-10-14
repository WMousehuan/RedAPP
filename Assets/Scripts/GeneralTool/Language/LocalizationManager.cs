using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : Singleton_Base<LocalizationManager>
{
    public override bool isDontDestroy => true;
    public enum LanguageType
    {
        简中,
        繁中,
        English,
    }
    public const string setLanguage = "SetLanguage";
    public LanguageType languageType= LanguageType.简中;//语言
    public Dictionary<string, List<string>> text_Direction=new Dictionary<string, List<string>>();//用于文字的本地化内容 字典
    public Dictionary<string, Sprite> sprite_Direction=new Dictionary<string, Sprite>();//用于图像的本地化内容 字典
    public Dictionary<LanguageType, int> languageIndex_Dictionary = new Dictionary<LanguageType, int>();
    /// <summary>
    /// 获得语言类型 用于请求头
    /// </summary>
    /// <returns></returns>
    public string GetLanguageHeader()
    {
        switch (languageType)
        {
            case LanguageType.简中:
                return "zh-CN";
            case LanguageType.繁中:
                return "zh-HK";
            default:
                return "en";
        }
    }
    protected override void Awake()
    {

        //print((int)languageType+ "||"+languageType);
        base.Awake();
        if (System.Enum.TryParse(PlayerPrefs.GetString("Language"), out LanguageType languageType))
        {
            this.languageType = languageType;
        }
        LoadAllLanguageText(Resources.Load<TextAsset>("Txt/Localization").text);
    }

    private void Start()
    {
       
        //设置语言
        SetLanguage(this.languageType);
    }
    /// <summary>
    /// 获取本地CSV文件文字text内容
    /// </summary>
    /// <param name="csvData"></param>
    public void LoadAllLanguageText(string csvData)
    {
        string[] line = csvData.Split('\r', '\n');
        if (line.Length > 1)
        {
            string[] firstLineStages = line[0].Split(',');
            for (int i = 1; i < firstLineStages.Length; i++)
            {
                if (System.Enum.TryParse(firstLineStages[i], out LanguageType languageType))
                {
                    languageIndex_Dictionary.Add(languageType, i-1);
                }
            }
            for (int i = 1; i < line.Length; i++)
            {
                string[] stages = line[i].Split(',');
                if (stages.Length > 1)
                {
                    string trimId = stages[0].Trim();
                    text_Direction.Add(trimId, new List<string>());
                    for (int k = 1; k < stages.Length; k++)
                    {
                        text_Direction[trimId].Add(stages[k]);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 设置语言
    /// </summary>
    /// <param name="languageType"></param>
    public void SetLanguage(LanguageType languageType)
    {
        this.languageType = languageType;
        for (int i = 0; i < LocalizationItem_Base.items.Count; i++)
        {
            LocalizationItem_Base.items[i].Init();
        }
        print(languageType.ToString());
        EventManager.Instance.DispatchEvent(setLanguage, languageType);
    }

    /// <summary>
    /// 通过id 获得内容
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index">语言类型索引</param>
    /// <returns></returns>
    public string GetText(string id,LanguageType  languageType)
    {
        string trimId = id.Trim();
        if (text_Direction.ContainsKey(trimId) )
        {
            return text_Direction[trimId][languageIndex_Dictionary[languageType]];
        }
        return null;
    }
    /// <summary>
    /// 通过id 获得内容
    /// </summary>
    /// <param name="id"></param>
    /// <param name="initContent">初始化内容</param>
    /// <returns></returns>
    public string GetText(string id,string initContent)
    {
        string content = GetText(id,languageType);
        if (string.IsNullOrEmpty(content))
        {
            content = initContent;
        }
        if (content != null)
        {
            return content.Replace("\\r\\n", "\r\n");
        }
        return "";
    }
}