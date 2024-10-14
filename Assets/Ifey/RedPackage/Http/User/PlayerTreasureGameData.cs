using System.Collections;
using UnityEngine;
    public class PlayerTreasureGameData : MonoSingleton<PlayerTreasureGameData>
    {
    [HideInInspector]
    public AppPlayRespVO[] playList; //根据游戏channelid获取红包规则 红包规则表
    //what channel id I click In
    //[HideInInspector]
    //public string entranceChannelIdKey = "entranceChannelIdKey";
    public string entranceChannelId
    {
        get;
        //{
        //return PlayerPrefs.GetString(entranceChannelIdKey, "0").ToString();
        //}
        set;
        //{
            //PlayerPrefs.SetString(entranceChannelIdKey, value);
        //}
    }
}
 