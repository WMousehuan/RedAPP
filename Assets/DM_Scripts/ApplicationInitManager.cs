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
        EventManager.Instance.Regist(GameEventType.Login.ToString(), this.GetInstanceID(), (objects) => {
            TurnToChannel();
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.Login.ToString(), this.GetInstanceID());
    }
    public void TurnToChannel()
    {
        if (!string.IsNullOrEmpty(SceneMenu.autoEnterChannelId))
        {
            if (UserManager.Instance && UserManager.Instance.appMemberUserInfoRespVO != null)
            {


               
                var dataObject = new
                {
                    channelId = SceneMenu.autoEnterChannelId,
                };
                UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(sharedChannelUrl, dataObject, new CommonHttpInterface(), (resultData) =>
                {
                    SceneMenu.autoEnterChannelId = SceneMenu.autoEnterChannelId.Remove(SceneMenu.autoEnterChannelId.Length - 3);
                    MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Lobby, SceneChangeEffect.Color);
                    PlayerTreasureGameData.Instance.entranceChannelId = SceneMenu.autoEnterChannelId;
                    //Get playList info »ñÈ¡Íæ·¨
                    MonoSingleton<GetChannelPlayInfo>.Instance.getChannelPlayInfo(SceneMenu.autoEnterChannelId);
                    SceneMenu.autoEnterChannelId = "";
                }, (code,msg) => {
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
                SceneMenu.autoEnterChannelId = msgStages[2];
                TurnToChannel();
                break;
        }
    }
}
