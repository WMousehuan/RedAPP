using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationTextItem_Ctrl : LocalizationItem_Base
{
    private string initContent;
    private Text _text;

    private Text text
    {
        get
        {
            if (_text == null)
            {
                _text = this.GetComponent<Text>();
            }

            return _text;
        }
    }
    private TextMeshPro _textMeshPro;
    private TextMeshPro textMeshPro
    {
        get
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = this.GetComponent<TextMeshPro>();
            }
            if (_textMeshPro != null)
            {
                if (string.IsNullOrEmpty(initContent))
                {
                    initContent = _textMeshPro.text;
                }
            }
            return _textMeshPro;
        }
    }
    private TMP_Text _TMP_Text;
    private TMP_Text TMP_Text
    {
        get
        {
            if (_TMP_Text == null)
            {
                _TMP_Text = this.GetComponent<TMP_Text>();
            }
            if (_TMP_Text != null)
            {
                if (string.IsNullOrEmpty(initContent))
                {
                    initContent = _TMP_Text.text;
                }
            }
            return _TMP_Text;
        }
    }
    public override void Init()
    {
        if (text != null)
        {
            if (string.IsNullOrEmpty(initContent))
            {
                initContent = text.text;
            }
        }
        SetText(LocalizationManager.instance?.GetText(id, initContent) ?? initContent);
    }
    private void OnEnable()
    {
        Init();
    }
    public void SetText(string content)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = content;
        }
        if (text != null)
        {
            text.text = content;
        }
        if (TMP_Text != null)
        {
            TMP_Text.text = content;
        }
    }
}
