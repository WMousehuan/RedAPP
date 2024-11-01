using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiChangePhoneNumber : Popup
{
    public TMP_InputField phoneNumber_InputField;
    public TMP_Dropdown phoneArae_DropDown;
    public Text areaCode_Text;
    public TMP_InputField code_InputField;
    private string changePhoneNumberUrl = "/app-api/member/user/update-mobile";



    public System.Action<string> numberChangeAction;

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
        UserManager.Instance.PhoneNumberSendCode(phoneNumber_InputField.text, UserManager.VerifySceneType.ChangePhoneNumber);
    }
    public void OnEventChangePhone()
    {
        try
        {
            AppAuthPhoneNumberReqVO appAuthPhoneNumberLoginReqVO = new AppAuthPhoneNumberReqVO();
            appAuthPhoneNumberLoginReqVO.mobile = string.Format("({0})", UserManager.Instance.currentAreaCode)+ phoneNumber_InputField.text;
            appAuthPhoneNumberLoginReqVO.code = code_InputField.text;
            if (appAuthPhoneNumberLoginReqVO.mobile.Length <=0)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Phone number cannot be empty");
                return;
            }
            if (appAuthPhoneNumberLoginReqVO.code.Length <4)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Minimum 4 digits for Verify Code");
                return;
            }

            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PutObjectWithParamAuthorizationToken(changePhoneNumberUrl, appAuthPhoneNumberLoginReqVO, null, (resultData) => {
                waitMask_Ui?.ShowResultCase("Success", 0);
                foreach (UserManager.VerifySceneType type in Enum.GetValues(typeof(UserManager.VerifySceneType)))
                {
                    PlayerPrefs.DeleteKey("VerifyStampTime"+type.ToString());
                }
                UserManager.Instance.appMemberUserInfoRespVO.mobile = appAuthPhoneNumberLoginReqVO.mobile;
                numberChangeAction?.Invoke(appAuthPhoneNumberLoginReqVO.mobile);
                OnEventClose();

            }, (code, msg) => {
                waitMask_Ui?.ShowResultCase("fail", 0);
                OnEventClose();
                UiHintCase.instance.Show("Change Phone Fail.\r\ncode="+ code);
                Debug.Log(msg);
            });
        }
        catch (Exception e)
        {
            Debug.LogError("An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
}
