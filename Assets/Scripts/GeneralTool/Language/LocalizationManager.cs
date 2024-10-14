using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : Singleton_Base<LocalizationManager>
{
    public override bool isDontDestroy => true;
    public enum LanguageType
    {
        ����,
        ����,
        English,
    }
    public const string setLanguage = "SetLanguage";
    public LanguageType languageType= LanguageType.����;//����
    public Dictionary<string, List<string>> text_Direction=new Dictionary<string, List<string>>();//�������ֵı��ػ����� �ֵ�
    public Dictionary<string, Sprite> sprite_Direction=new Dictionary<string, Sprite>();//����ͼ��ı��ػ����� �ֵ�
    public Dictionary<LanguageType, int> languageIndex_Dictionary = new Dictionary<LanguageType, int>();
    /// <summary>
    /// ����������� ��������ͷ
    /// </summary>
    /// <returns></returns>
    public string GetLanguageHeader()
    {
        switch (languageType)
        {
            case LanguageType.����:
                return "zh-CN";
            case LanguageType.����:
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
       
        //��������
        SetLanguage(this.languageType);
    }
    /// <summary>
    /// ��ȡ����CSV�ļ�����text����
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
    /// ��������
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
    /// ͨ��id �������
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index">������������</param>
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
    /// ͨ��id �������
    /// </summary>
    /// <param name="id"></param>
    /// <param name="initContent">��ʼ������</param>
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