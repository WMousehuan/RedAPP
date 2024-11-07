using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMyUser : Popup
{
    public RawImage avatar_RawImage;
    public Texture2D defaultAvatar_Texture;
    //public InputField userName_InputField;
    public TMP_InputField userName_InputField;

    public ObjectGroup<string, Popup> popup_Group;

    public override void Start()
    {

        base.Start();

        RefreshUserInformation();
        EventManager.Instance.Regist(GameEventType.GetUserData.ToString(), this.GetInstanceID(), (objects) =>
        {
            RefreshUserInformation();
        });
        EventManager.Instance.Regist(GameEventType.GetUserAvatar.ToString(), this.GetInstanceID(), (objects) =>
        {
            RefreshUserInformation();
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.GetUserData.ToString(), this.GetInstanceID());
        EventManager.Instance?.UnRegist(GameEventType.GetUserAvatar.ToString(), this.GetInstanceID());
    }
    public void RefreshUserInformation()
    {
        if (UserManager.Instance.appMemberUserInfoRespVO == null)
        {
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            return;
        }
        userName_InputField.text = UserManager.Instance.appMemberUserInfoRespVO.nickname;
        if (UserManager.Instance.currentAvatar_Texture != null)
        {
            avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
        }
        else
        {
            avatar_RawImage.texture = defaultAvatar_Texture;
        }

    }
    public void OnEventOpenDefaultBalancePopup()
    {
        UiUserBalance uiUserBalance= PopupManager.Instance.Open(popup_Group["Balance"]).GetComponent<UiUserBalance>();
        uiUserBalance.Init(BalanceType.Default);
    }
    public void OnEventOpenCommissionBalancePopup()
    {
        UiUserBalance uiUserBalance = PopupManager.Instance.Open(popup_Group["Balance"]).GetComponent<UiUserBalance>();
        uiUserBalance.Init(BalanceType.Commission);
    }

    public void OnEventOpenRechargePopup(int rechargeStateValue)
    {
        //UiRechargeDetail.currentRechargeStateType = (UiRechargeDetail.RechargeStateType)rechargeStateValue;
        UiRechargeDetail uiRechargeDetail = PopupManager.Instance.Open(popup_Group["RechargeDetail"]).GetComponent<UiRechargeDetail>();
        uiRechargeDetail.OnRechargeStateGroup_DropdownValueChange((UiRechargeDetail.RechargeStateType)rechargeStateValue);
    }
}
