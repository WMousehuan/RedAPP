using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if UNITY_EDITOR|| PLATFORM_ANDROID
using NativeFilePickerNamespace;
#endif
using System.IO;
using UnityEngine.Networking;
using System.Text;
using System.Runtime.InteropServices;
using System;
using System.Xml.Linq;
public class Ui_UserInformation : Popup
{
    public enum UserInfoType
    {
        nickname,
        avatar,
        sex,
    }

    public RawImage avatar_RawImage;
    public Texture2D defaultAvatar_Texture;
    public InputField userName_InputField;

    public Button setName_Button;
    public Transform setNameEidit_Case;

    private string uploadUserDataUrl = "/app-api/member/user/update";
    private string updateFileUrl = "/app-api/infra/file/upload";

    public string receiveWebFileBase64Data;
    public string receiveWebFileName;

    [DllImport("__Internal")]
    public static extern void clickSelectFileBtn(string name);
    public override void Start()
    {
      
        base.Start();
        
        RefreshUserInformation();
        OnEventSwitchSetNameState(false);
        EventManager.Instance.Regist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID(), (objects) =>
        {
            string sign = (string)objects[0];
            switch (sign)
            {
                case "GetAvatar":
                    RefreshUserInformation();
                    break;
                case "UpdateData":
                    RefreshUserInformation();
                    break;
            }
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID());
    }
    public void RefreshUserInformation()
    {
        if (UserManager.Instance.appMemberUserInfoRespVO==null)
        {
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            return;
        }
        userName_InputField.text = UserManager.Instance.appMemberUserInfoRespVO.nickname;
        if (UserManager.Instance.currentAvatar_Texture!=null)
        {
            avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
        }
        else
        {
            avatar_RawImage.texture = defaultAvatar_Texture;
        }

    }

    public void OnEventSetAvatar()
    {

#if UNITY_EDITOR||PLATFORM_ANDROID
        NativeFilePicker.PickFile((path) => {
            UploadAvatarByPath(path);
        }, "image/*");
#elif UNITY_WEBGL
        clickSelectFileBtn(this.gameObject.name);
#endif
    }

    public void OnEventUploadUserName()
    {
        if (userName_InputField.text == UserManager.Instance.appMemberUserInfoRespVO.nickname)
        {
            return;
        }
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(uploadUserDataUrl, GetUploadDataString((UserInfoType.nickname, userName_InputField.text)), new PostAvatarFileInterface(this), (requestData) =>
        {
            UserManager.Instance.appMemberUserInfoRespVO.nickname = userName_InputField.text;
            OnEventSwitchSetNameState(false);
            waitMask_Ui.ShowResultCase("Success", 1);
        }, () =>
        {
            OnEventSwitchSetNameState(false);
            waitMask_Ui.ShowResultCase("Fail", 1);
        });
    }
    /// <summary>
    /// �������ƿɱ༭״̬
    /// </summary>
    /// <param name="isSwitch"></param>
    public void OnEventSwitchSetNameState(bool isSwitch)
    {
        userName_InputField.interactable = isSwitch;
#if UNITY_EDITOR||PLATFORM_ANDROID
        if (isSwitch)
        {
            userName_InputField.ActivateInputField();
        }
#endif
        setName_Button.gameObject.SetActive(!isSwitch);
        setNameEidit_Case.gameObject.SetActive(isSwitch);
        if (UserManager.Instance.appMemberUserInfoRespVO != null)
        {
            userName_InputField.text = UserManager.Instance.appMemberUserInfoRespVO.nickname;
        }

    }

    /// <summary>
    /// �ϴ��û���Ϣ(�����Ĳ� ��Ϣ���ᱻ����)
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
    /// <summary>
    /// ͨ��·���ϴ�ͼƬ
    /// </summary>
    /// <param name="path"></param>
    public void UploadAvatarByPath(string path)
    {
        if (File.Exists(path))
        {
            print(Path.GetFileName(path));
            // ���ļ�·����ȡͼƬ����
            byte[] imageData = File.ReadAllBytes(path);


            PostAvatarByte(imageData, path);
        }
        else
        {
            Debug.LogWarning("Image file not found: " + path);
        }
    }
    /// <summary>
    /// �ϴ�ͼƬ
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="path"></param>
    public void PostAvatarByte(byte[] imageData, string path)
    {
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PostFileWithParamAuthorizationToken(updateFileUrl + string.Format("?path={0}", Path.GetFileName(path)), Path.GetFileName(path), imageData, new PostAvatarFileInterface(this), (requestData) =>
        {
            JObject json = JObject.Parse(requestData);
            string url = json["data"].Value<string>();
            print(url);
            UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(uploadUserDataUrl, GetUploadDataString((UserInfoType.avatar, url)), new PostAvatarFileInterface(this), (requestData) =>
            {
                // ����Texture2D������ͼƬ����
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ASTC_8x8, false);
                texture.name = path;
                texture.LoadImage(imageData);
                UserManager.Instance.currentAvatar_Texture = texture;

                avatar_RawImage.texture = texture;
                waitMask_Ui.ShowResultCase("Success", 1);
            }, () =>
            {
                waitMask_Ui.ShowResultCase("Fail", 1);
            });

        }, () =>
        {
            avatar_RawImage.texture = defaultAvatar_Texture;
        });
    }



#if UNITY_WEBGL
    public void WebSelectFileStage(string fileBase64DataStage)
    {

        if (fileBase64DataStage == "")//�������ݽ���������
        {
            string[] contents = receiveWebFileBase64Data.Split(',');
            PostAvatarByte(Convert.FromBase64String(contents[1]), contents[0]);
            receiveWebFileBase64Data = "";
        }
        else if ((fileBase64DataStage.Contains("!")))
        {
            string[] _data = fileBase64DataStage.Split("\\");
            //Tips_Ctrl.instance?.ShowTips(string.Format(LocalizationManager.instance.GetText("photoCantOutOfSize", "ͼƬ��С���ܳ���{0}��"), _data[1]));
        }
        else if (fileBase64DataStage.Contains("\\"))//��ȡͼƬ����Ƭ�κ������ݻ���
        {
            string[] _data = fileBase64DataStage.Split("\\");
            receiveWebFileBase64Data += _data[1];
        }
        else//��ȡid
        {
            receiveWebFileName = fileBase64DataStage;
            receiveWebFileBase64Data = "";
        }
    }


#endif
}

public class PutUserInformationInterface : HttpInterface
{
    Ui_UserInformation ui_UserInformation;
    public PutUserInformationInterface(Ui_UserInformation source)
    {
        ui_UserInformation = source;
    }
    public void Success(string result)
    {

    }
    public void Fail(JObject json)
    {
        // ʵ�� Fail �������߼�
        if (!(new FailPubDo()).failPubdo(json))
        {

        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
    }
}
public class PostAvatarFileInterface : HttpInterface
{
    Ui_UserInformation ui_UserInformation;
    public PostAvatarFileInterface(Ui_UserInformation source)
    {
        ui_UserInformation = source;
    }
    public void Success(string result)
    {

    }
    public void Fail(JObject json)
    {
        // ʵ�� Fail �������߼�
        int code = json["code"].Value<int>();
        //not login
        if (code == 401)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            Debug.Log("User notLogin Show Login UI!");
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
        }
        else if (code == 407)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
        }

        else if (code == 406 || code == 1004003000 || code == 400)
        {
            Debug.Log("Login name or psd error!");
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
    }
}