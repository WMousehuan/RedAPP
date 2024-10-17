using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
[System.Serializable]
public class ChannelRespVO
{
    public long? Id { get; set; }
    public long? ChannelCateId { get; set; }
    public string ChannelType { get; set; }
    public string ChannelName { get; set; }
    public string? ChannelCategory { get; set; }
    public double? RedMaxMoney { get; set; }
    public int? RedExpirationTime { get; set; }
    public double? RedMinMoney { get; set; }
    public double? PlatBrokerage { get; set; }
    public double? PromBrokerage { get; set; }
    public string CreateTime { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, ChannelCateId: {ChannelCateId}, ChannelType: {ChannelType}, ChannelName: {ChannelName}, ChannelCategory: {ChannelCategory}, RedMaxMoney: {RedMaxMoney}, RedExpirationTime: {RedExpirationTime}, RedMinMoney: {RedMinMoney}, PlatBrokerage: {PlatBrokerage}, PromBrokerage: {PromBrokerage}, CreateTime: {CreateTime}";
    }
}
