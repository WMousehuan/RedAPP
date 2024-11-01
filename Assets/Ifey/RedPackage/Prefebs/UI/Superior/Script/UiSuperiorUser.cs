using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UiSuperiorUser : Popup
{
    public enum UserType
    {
        Default,
        Robot,
        Superior,
    }
    public UserType userType=(UserType)2;
    public Text content_Text;
    public InputField superiorUrl_TextField;

    public Button agree_Button;
    public Button copy_Button;
    public Button detail_Button;

    private string setUserTypeUrl = "/app-api/member/user/update-userType";//修改userTypeUrl

    private string getPromotionUrl = "/app-api/red/promotion-setting/getActiveSetting";//获得推广链接Url

    private string promotionLink = "";//推广链接
    private string promotionDescribe = "";//推广简介

    public override void Start()
    {
        base.Start();

        WebRequestPromotionUrl();


    }
    public void WebRequestPromotionUrl()
    {
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getPromotionUrl, null, (resultData) =>
        {
            print(resultData);
            JObject json = JObject.Parse(resultData);
            string dataContent = json["data"].ToString();
            JObject data = JObject.Parse(dataContent);

            promotionLink = data["promotionLink"].Value<string>();
            promotionDescribe = data["promotionDescribe"].Value<string>();
            UpdateState();
            waitMask_Ui?.ShowResultCase("GetData", 1);

        }, (code, msg) =>
        {
            waitMask_Ui?.ShowResultCase("Fail", 1, () =>
            {
                PopupManager.Instance.Close();
            });
        });
    }
    public void UpdateState()
    {
        if (UserManager.Instance.appMemberUserInfoRespVO != null)
        {
            content_Text.text = UserManager.Instance.appMemberUserInfoRespVO.userType == (int)UserType.Superior ? "You are already the superior agent. Copy the link and send it to a friend" : "Ready to become a superior agent?";
            superiorUrl_TextField.text = UserManager.Instance.appMemberUserInfoRespVO.userType == (int)UserType.Superior ? promotionLink  : null;
            agree_Button.gameObject.SetActive(UserManager.Instance.appMemberUserInfoRespVO.userType != (int)UserType.Superior);
            copy_Button.gameObject.SetActive(UserManager.Instance.appMemberUserInfoRespVO.userType == (int)UserType.Superior);
            detail_Button.gameObject.SetActive(UserManager.Instance.appMemberUserInfoRespVO.userType == (int)UserType.Superior);
        }
    }
    public void OnEventBeASuperiorUser()
    {
        PopupManager.Instance.Open<UiAgreeCase>(PopupType.PopupAgreeCase).Init(() =>
        {
            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            string content = string.Format("?userType={0}", (int)userType);
            UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(setUserTypeUrl + content, "", null, (requestData) =>
            {
                UserManager.Instance.appMemberUserInfoRespVO.userType = (int)userType;
                waitMask_Ui?.ShowResultCase("Success", 1, () => {
                    WebRequestPromotionUrl();
                });
            }, (code, msg) =>
            {
                waitMask_Ui?.ShowResultCase("Fail", 1);
            });
        });
    }
    public void OnEventCopyUrl()
    {
        GUIUtility.systemCopyBuffer = promotionLink;
        WebMessage_Ctrl.SendMessageToWeb("clipbord^" + promotionLink);
        UiHintCase.instance.Show("Copied link");
    }
}
