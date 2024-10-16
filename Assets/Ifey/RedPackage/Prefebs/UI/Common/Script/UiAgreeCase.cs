using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAgreeCase : Popup
{
    public Text content_Text;

    public Text agreeButton_Text;
    public Text cancleButton_Text;
    public System.Action agreeAction;
    public void Init(System.Action agreeAction)
    {
        this.agreeAction = agreeAction;
    }
    public void Init(System.Action agreeAction,string content,string agreeText=null,string cancleText=null)
    {
        this.agreeAction = agreeAction;
        this.content_Text.text = content;
        if (agreeText != null)
        {
            agreeButton_Text.text = agreeText;
        }
        if (cancleText != null)
        {
            cancleButton_Text.text = cancleText;
        }
    }
    public void OnEventAgree()
    {
        OnEventClose();
        agreeAction?.Invoke();
    }
}
