using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PubGameChannel : MonoBehaviour
{
    public Text treasureChannelGameName;
    public Text minMaxAmountValueText;
    [HideInInspector]
    public ChannelRespVO channelRespVO;
    public void SetData(ChannelRespVO channelRespVO)
    {
        this.channelRespVO = channelRespVO;
        SetGameUiInfo();
    }
    public void SetGameUiInfo()
    {
        treasureChannelGameName.text = channelRespVO.ChannelName;
        minMaxAmountValueText.text = channelRespVO.RedMinMoney.ToString()+" -- " + channelRespVO.RedMaxMoney.ToString();
    }

    // Update is called once per frame
    public void OnPressTreasureClick()
    {
        MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Lobby, SceneChangeEffect.Color);
        PlayerTreasureGameData.Instance.entranceChannelId = this.channelRespVO.Id.ToString();
        //Get playList info »ñÈ¡Íæ·¨
        MonoSingleton<GetChannelPlayInfo>.Instance.getChannelPlayInfo(PlayerTreasureGameData.Instance.entranceChannelId);
    }
}
