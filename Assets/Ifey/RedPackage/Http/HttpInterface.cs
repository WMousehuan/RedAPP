using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HttpInterface
{
    public void Success(string result);
    public void Fail(JObject json);

    public void UnknowError(string errorMsg);
}

public class FailPubDo
{
    public bool failPubdo(JObject json)
    {
        // 实现 Fail 方法的逻辑
        int code = json["code"].Value<int>();
        //not login
        if (code == 401)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            Debug.Log("User notLogin Show Login UI!");
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
            return true;
        }
        else if (code == 407)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
            return true;
        }

        else if (code == 406 || code == 1004003000 || code == 400)
        {
            Debug.Log("Login name or psd error!");
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");
            return true;
        }
        return false;
    }
}
