using System.Collections;
using UnityEngine;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean
{
    using System;

    public class AppPacketReceiveRespVO
    {
        public long? Id { get; set; }
        public long? MemberId { get; set; }
        public long? RedPacketSendId { get; set; }
        public decimal? GetAmount { get; set; }
        public decimal? CompensateAmount { get; set; }
        public int? ThunderNo { get; set; }
        public long? ChannelId { get; set; }
        public long? CreateTime { get; set; }
        public DateTime? CreateTimeDateTime
        {
            get
            {
                if (CreateTime > 0)
                {
                    DateTime utcTime = DateTimeOffset.FromUnixTimeMilliseconds((long)CreateTime).UtcDateTime;
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);
                    return localTime;
                }
                return null;
            }
        }

        public string? NickName { get; set; }
        public string? Avatar { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, MemberId: {MemberId}, RedPacketSendId: {RedPacketSendId}, GetAmount: {GetAmount}, CompensateAmount: {CompensateAmount}, ThunderNo: {ThunderNo}, ChannelId: {ChannelId}, CreateTime: {CreateTime}, NickName: {NickName}, Avatar: {Avatar}";
        }
    }
}