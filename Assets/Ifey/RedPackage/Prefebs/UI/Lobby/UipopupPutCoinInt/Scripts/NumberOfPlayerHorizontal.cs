using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// parent of the checkbox of the play list  number
/// </summary>
public class NumberOfPlayerHorizontal : MonoBehaviour
{
    public GameObject playersCheckBoxGameobject;
    public BombNumberHorizontal bombNumberHorizontal;
    List<PlayersCheckBox> playersCheckBoxList = new List<PlayersCheckBox>();
    // Start is called before the first frame update
    public void startInitPlayersNumberCHeckbox()
    {
        foreach (AppPlayRespVO appPlayResp in MonoSingleton<PlayerTreasureGameData>.Instance.playList)
        {
            if(appPlayResp.playStatus == 0 && appPlayResp.status==0) {
                GameObject newCHeckBox = Instantiate(playersCheckBoxGameobject,gameObject.transform);
                PlayersCheckBox playerCheckBox = newCHeckBox.GetComponent<PlayersCheckBox>();
                playerCheckBox.id = appPlayResp.id;
                playerCheckBox.redPacketNum = appPlayResp.redPacketNum;
                playerCheckBox.setNumberText(appPlayResp.redPacketNum.ToString());
                playerCheckBox.compensateRatio = appPlayResp.compensateRatio;
                playerCheckBox.setPlayersCheckBoxList(playersCheckBoxList);
                playerCheckBox.setBombNumberHorizontal(bombNumberHorizontal);
                playersCheckBoxList.Add(playerCheckBox);
                //The first checkBox is default
                if (playerCheckBox.redPacketNum == 5)
                {
                    //设置的时候需要把炸弹的赔率也设置了
                    playerCheckBox.SetToggleOn();
                }
                Debug.Log("id: " + appPlayResp.id);
                Debug.Log("redPacketNum: " + appPlayResp.redPacketNum);
                Debug.Log("compensateRatio: " + appPlayResp.compensateRatio);
                Debug.Log("status: " + appPlayResp.status);
                Debug.Log("createTime: " + appPlayResp.createTime);
                Debug.Log("playStatus: " + appPlayResp.playStatus);
            }
        }
    }

    public PlayersCheckBox getOnSelectPlayer()
    {
        if (playersCheckBoxList == null || playersCheckBoxList.Count == 0)
        {
            return null;
        }
        foreach (var player in playersCheckBoxList)
        {
            if (player.getToggleIsOn())
            {
                return player;
            }
        }
        return null;
    }
    
}
