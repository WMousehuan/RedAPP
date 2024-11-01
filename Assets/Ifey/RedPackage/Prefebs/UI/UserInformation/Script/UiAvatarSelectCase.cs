using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Ui_UserInformation;
public class UiAvatarSelectCase : Popup
{
    public GridLoopScroll_Ctrl loopScroll_Ctrl;


    public int selectAvatarIndex;

    private string getAvatarsUrl = "/app-api/member/avatar/page";
    private string uploadUserDataUrl = "/app-api/member/user/update";

    public override void Start()
    {
        base.Start();
        loopScroll_Ctrl.Init(0, 0, (target, index) =>
        {
            EventManager.Instance.Regist(GameEventType.LoadedAvatarTexture.ToString(), target.GetInstanceID(), objects =>
            {
                Texture2D texture2d = (Texture2D)objects[0];
                int realIndex = (int)objects[1];
                if (target != null && realIndex == index)
                {
                    target.transform.GetChild<RawImage>("Avatar_RawImage").texture = texture2d;
                }
            });
        }, (target, index) =>
        {
            EventManager.Instance?.UnRegist(GameEventType.LoadedAvatarTexture.ToString(), target.GetInstanceID());
        });
        loopScroll_Ctrl.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) =>
        {
            if (UserManager.avatarData_Group != null && realIndex >= 0 && realIndex < UserManager.avatarData_Group.Count)
            {
                Button button = target.GetChild<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    selectAvatarIndex = realIndex;
                    loopScroll_Ctrl.Refresh(loopScroll_Ctrl.count);
                    target.transform.GetChild<Image>("Selected_Image").gameObject.SetActive(true);
                });
                target.transform.GetChild<Image>("Selected_Image").gameObject.SetActive(selectAvatarIndex == realIndex);
                if (UserManager.avatarData_Group[realIndex].target.Item2 == null)
                {
                    target.transform.GetChild<Image>("Loading_Image").gameObject.SetActive(true);
                }
                else
                {
                    if (this != null)
                    {
                        target.transform.GetChild<Image>("Loading_Image").gameObject.SetActive(false);
                        target.transform.GetChild<RawImage>("Avatar_RawImage").texture = UserManager.avatarData_Group[realIndex].target.Item2;
                    }
                }
            }
        };
      

        if (UserManager.avatarData_Group == null )
        {
            UserManager.Instance.GetAvatarDatas();
            PopupManager.Instance.Close(this);
            UiHintCase.instance.Show("Avatars Load Fail");
        }
        else
        {
            int currentAvatarIndex = UserManager.avatarData_Group.GetIndexByKey(UserManager.Instance.appMemberUserInfoRespVO.avatar);
            selectAvatarIndex = currentAvatarIndex < 0 ? 0 : currentAvatarIndex;
            loopScroll_Ctrl.Init(UserManager.avatarData_Group.Count, 0);
        }
    }
    public void Save()
    {
        if (UserManager.avatarData_Group[selectAvatarIndex].key == UserManager.Instance.appMemberUserInfoRespVO.avatar)
        {
            this.OnEventClose();
            return;
        }
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(uploadUserDataUrl, GetUploadDataString((UserInfoType.avatar, UserManager.avatarData_Group[selectAvatarIndex].key)), null, (requestData) =>
        {
            UserManager.Instance.appMemberUserInfoRespVO.avatar = UserManager.avatarData_Group[selectAvatarIndex].key;
            if (UserManager.avatarData_Group[selectAvatarIndex].target.Item2 == null)
            {

                if (UserManager.avatarData_Group[selectAvatarIndex].target.Item1)
                {
                    System.Action<Texture2D, int> _loadedAvatarAction = null;
                    _loadedAvatarAction= (texture2d, Index) =>
                    {
                        if (Index == selectAvatarIndex)
                        {
                            UserManager.Instance.currentAvatar_Texture = texture2d;
                            waitMask_Ui.ShowResultCase("Success", 0);
                            this.OnEventClose();
                            UserManager.Instance.loadedAvatarAction -= _loadedAvatarAction;
                        }
                    };
                    UserManager.Instance.loadedAvatarAction += _loadedAvatarAction;
                }
                else
                {
                    GeneralTool_Ctrl.DownloadImage(UserManager.avatarData_Group[selectAvatarIndex].key, (texture2d) =>
                    {
                        UserManager.avatarData_Group[selectAvatarIndex].target = (false, texture2d);
                        UserManager.Instance.currentAvatar_Texture = texture2d;
                        waitMask_Ui.ShowResultCase("Success", 0);
                        this.OnEventClose();

                    }, () =>
                    {
                        UserManager.avatarData_Group[selectAvatarIndex].target = (false, null);
                        waitMask_Ui.ShowResultCase("Fail", 1);
                    });
                }
            }
            else
            {
                UserManager.Instance.currentAvatar_Texture = UserManager.avatarData_Group[selectAvatarIndex].target.Item2;
                waitMask_Ui.ShowResultCase("Success", 0);
                this.OnEventClose();
            }
        }, (code, msg) =>
        {
            waitMask_Ui.ShowResultCase("Fail", 1);
        });
    }
    /// <summary>
    /// 上传用户信息(不传的参 信息不会被覆盖)
    /// </summary>
    /// <param name="keyValues"></param>
    public string GetUploadDataString(params (UserInfoType, string)[] keyValues)
    {
        string uploadData = "{";
        for (int i = 0; i < keyValues.Length; i++)
        {
            switch (keyValues[i].Item1)
            {
                case UserInfoType.avatar:
                case UserInfoType.nickname:
                    uploadData += string.Format("\"{0}\":\"{1}\"", keyValues[i].Item1.ToString(), keyValues[i].Item2);
                    break;
                case UserInfoType.sex:
                    uploadData += string.Format("\"{0}\":{1}", keyValues[i].Item1.ToString(), keyValues[i].Item2);
                    break;
            }
        }
        uploadData += "}";

        return uploadData;

    }
}
public class ReserveAvatarData
{
    public int id;
    public string avatarName;
    public string avatarUrl;
    public long createTime;
}