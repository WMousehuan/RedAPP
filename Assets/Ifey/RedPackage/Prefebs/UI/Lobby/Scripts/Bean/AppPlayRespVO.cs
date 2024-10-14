using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppPlayRespVO
{
    public long id { get; set; }
    public int redPacketNum { get; set; }
    public double compensateRatio { get; set; }
    public int status { get; set; }
    public string createTime { get; set; }
    public int playStatus { get; set; }

    public override string ToString()
    {
        return $"id: {id}, redPacketNum: {redPacketNum}, compensateRatio: {compensateRatio}, status: {status}, createTime: {createTime}, playStatus: {playStatus}";
    }
}
