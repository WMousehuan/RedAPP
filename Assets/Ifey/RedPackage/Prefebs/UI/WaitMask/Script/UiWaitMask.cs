using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiWaitMask : Popup
{
    public Text waitCaseDots_Text;
    public float dotsSpeed = 3.2f;
    private void Update()
    {
        waitCaseDots_Text.text = " ";
        for (int i = 0; i < (int)(Time.time * dotsSpeed) % 4; i++)
        {
            waitCaseDots_Text.text += ". ";
        }
    }
    public void Init(string content)
    {
        if (this == null)
        {
            return;
        }
        ShowCase("Wait_Case").GetChild<Text>().text = content;
    }
    public Transform ShowCase(string childName)
    {
        if (this == null)
        {
            return null;
        }
        Transform target = null;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.gameObject.SetActive(child.name == childName);
            if (child.name == childName)
            {
                target = child;
            }
        }
        return target;
    }
    public void ShowResultCase(string content,float closeTime,System.Action finishAction=null)
    {
        if (this == null)
        {
            return;
        }
        ShowCase("Result_Case").GetChild<Text>().text = content;
        IEPool_Manager.instance.WaitTimeToDo("", closeTime,null, () => {
            PopupManager.Instance.Close(this);
            finishAction?.Invoke();
        });
    }
}
