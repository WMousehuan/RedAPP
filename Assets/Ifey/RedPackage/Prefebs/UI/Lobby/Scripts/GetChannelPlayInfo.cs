using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Channel play list   
/// </summary>
public class GetChannelPlayInfo : MonoSingleton<GetChannelPlayInfo>
{
    [HideInInspector]
    string playListUrl = "/app-api/red/play/list/{channelId}";

    public void getChannelPlayInfo(string channelId)
    {
        // Construct the complete URL with channelId
        string completeUrl = playListUrl.Replace("{channelId}", channelId);

        //when start the game,get the userInfo
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(completeUrl, new GetPlayListInterface());
    }
}



public class GetPlayListInterface : HttpInterface
{
    public FailPubDo failPubDo = new FailPubDo();
    public void Success(string result)
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        ReturnData<AppPlayRespVO[]> responseData = JsonConvert.DeserializeObject<ReturnData<AppPlayRespVO[]>>(result);
        // ʵ�� Success �������߼�
        // ����һ���µ� AppPlayRespVO[] ����
        MonoSingleton<PlayerTreasureGameData>.Instance.playList = new AppPlayRespVO[responseData.data.Length];

        // �� responseData.data �е����ݸ��Ƶ���������
        for (int i = 0; i < responseData.data.Length; i++)
        {
            MonoSingleton<PlayerTreasureGameData>.Instance.playList[i] = responseData.data[i];
        }
        //MonoSingleton<GetChannelPlayInfo>.Instance.playList = responseData.data;
        Debug.Log("Success GetPlayListInterface="+ responseData.data.ToString());
        // ���� data ���鲢��ӡÿ�� AppPlayRespVO �������Ϣ
        foreach (AppPlayRespVO appPlayResp in MonoSingleton<PlayerTreasureGameData>.Instance.playList)
        {
            //Debug.Log("id: " + appPlayResp.id);
            //Debug.Log("redPacketNum: " + appPlayResp.redPacketNum);
            //Debug.Log("compensateRatio: " + appPlayResp.compensateRatio);
            //Debug.Log("status: " + appPlayResp.status);
            //Debug.Log("createTime: " + appPlayResp.createTime);
            //Debug.Log("playStatus: " + appPlayResp.playStatus);
        }
    }

    public void Fail(JObject json)
    {
        failPubDo.failPubdo(json);
    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetPlayListInterface UnknowError=" + errorMsg);
    }
}
