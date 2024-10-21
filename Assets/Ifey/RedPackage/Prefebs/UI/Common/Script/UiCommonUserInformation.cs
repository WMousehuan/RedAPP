using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UiSuperiorOwnLowerUserDetail;
public class UiCommonUserInformation : Popup
{
    public RawImage avatar_RawImage;
    public Texture2D defaultAvatar_Texture;
    //public InputField userName_InputField;
    public TMP_InputField userName_InputField;
    public Texture2D texture;
    public void RefreshUserInformation(LowerAgentUserDataVO lowerAgentUserDataVO)
    {
        if (UserManager.Instance.appMemberUserInfoRespVO == null)
        {
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            return;
        }
        userName_InputField.text = lowerAgentUserDataVO.nickname;
        avatar_RawImage.texture = defaultAvatar_Texture;
        print(lowerAgentUserDataVO.avatar);
        if (!string.IsNullOrEmpty(lowerAgentUserDataVO.avatar))
        {
            GeneralTool_Ctrl.DownloadImage(lowerAgentUserDataVO.avatar, (texture) =>
            {
                if (this.texture != null)
                {
                    Destroy(this.texture);
                }
                if (this == null)
                {
                    Destroy(texture);
                    return;
                }
                this.texture = texture;
                avatar_RawImage.texture = texture;
            });
        }
    }
    private void OnDestroy()
    {
        if (this.texture != null)
        {
            Destroy(this.texture);
        }
    }
}
