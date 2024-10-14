using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupButton : MonoBehaviour
{
    public void OnEventOpenOtherPopup(string popupType)
    {
        PopupManager.Instance.Open(Enum.Parse<PopupType>(popupType));
    }
}
