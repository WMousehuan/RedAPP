using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApplicationInitManager : MonoBehaviour, WebReviceMessage
{
    private string sharedChannelUrl = "/app-api/red/channel/shared";
    // Start is called before the first frame update
    private void OnEnable()
    {
        WebMessage_Ctrl.instance.Regist(this.gameObject);
    }
    private void OnDisable()
    {
        WebMessage_Ctrl.instance?.UnRegist(this.gameObject);
    }
    private void Start()
    {
        EventManager.Instance.Regist(typeof(UserManager).ToString(), this.GetInstanceID(), (objects) => {
            string sign = (string)objects[0];
            switch (sign)
            {
                case "LoginIn":
                    TurnToChannel();
                    break;
            }
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(typeof(UserManager).ToString(), this.GetInstanceID());
    }
    public void TurnToChannel()
    {
        if (!string.IsNullOrEmpty(SceneLobby.autoEnterChannelId))
        {
            if (UserManager.Instance && UserManager.Instance.appMemberUserInfoRespVO != null)
            {


               
                var dataObject = new
                {
                    channelId = SceneLobby.autoEnterChannelId,
                };
                UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(sharedChannelUrl, dataObject, new CommonHttpInterface(), (resultData) =>
                {
                    SceneLobby.autoEnterChannelId = SceneLobby.autoEnterChannelId.Remove(SceneLobby.autoEnterChannelId.Length - 3);
                    MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Lobby, SceneChangeEffect.Color);
                    PlayerTreasureGameData.Instance.entranceChannelId = SceneLobby.autoEnterChannelId;
                    //Get playList info »ñÈ¡Íæ·¨
                    MonoSingleton<GetChannelPlayInfo>.Instance.getChannelPlayInfo(SceneLobby.autoEnterChannelId);
                    SceneLobby.autoEnterChannelId = "";
                }, () => {
                    UiHintCase.instance.Show("Channel Non-existent");
                });
            }
        }
    }
    public void ReciveMessage(string msg)
    {
        string[] msgStages = msg.Split("^");
        switch (msgStages[0])
        {
            case "WebInitData":
                UserManager.encryptSuperiorId = msgStages[1];
                SceneLobby.autoEnterChannelId = msgStages[2];
                TurnToChannel();
                break;
        }
    }
}
