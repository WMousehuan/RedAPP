using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUserLoginByPhoneNumber : Popup
{
    //public TMP_InputField AreaCode_InputField;
    public TMP_InputField phoneNumber_InputField;
    public TMP_Dropdown phoneArae_DropDown;
    public Text areaCode_Text;
    public TMP_InputField code_InputField;

    private string loginUrl = "/app-api/member/auth/sms-login";

    public override void Start()
    {
        base.Start();
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < UserManager.Instance.areaCodeData_Group.Count; i++)
        {
            ObjectPack<UserManager.AreaType, AreaCodeData> areaCodeDataPack = UserManager.Instance.areaCodeData_Group[i];
            optionDatas.Add(new TMP_Dropdown.OptionData(areaCodeDataPack.key.ToString(), areaCodeDataPack.target.areaFlag_Sprite));
        }
        phoneArae_DropDown.onValueChanged.AddListener((index) =>
        {
            UserManager.Instance.currentAreaTypeIndex = index;
            areaCode_Text.text = string.Format("({0})", UserManager.Instance.currentAreaCode);
        });
        phoneArae_DropDown.ClearOptions();
        phoneArae_DropDown.AddOptions(optionDatas);
        phoneArae_DropDown.value = UserManager.Instance.currentAreaTypeIndex;
    }

    public void OnEventSendCode()
    {
        if (string.IsNullOrEmpty(phoneNumber_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Phone number cannot be empty");
            return;
        }
        
        UserManager.Instance.PhoneNumberSendCode( phoneNumber_InputField.text, UserManager.VerifySceneType.Login);
    }
    public void OnEventUserLogin()
    {
        UserManager.Instance.UserLoginByPhoneNumber( phoneNumber_InputField.text, code_InputField.text);
    }
    public void openSignUp()
    {
        //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupSignup);
    }
    public void OnEventChangeSignUp()
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
    }
}
