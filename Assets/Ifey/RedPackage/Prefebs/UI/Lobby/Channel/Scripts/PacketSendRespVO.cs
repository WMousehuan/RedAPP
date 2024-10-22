using System.Collections;
using UnityEngine;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts
{
    [System.Serializable]
    public class PacketSendRespVO
    {
        public long id; // 主键  红包ID
        public long memberId { get; set; } // 用户id
        public string? username { get; set; } // 用户名称
        public double redAmount { get; set; } // 发包总金额
        public int redCount { get; set; } // 发包数量
        public int thunderNo { get; set; } // 雷号
        public double? compensateAmount { get; set; } // 赔付金额
        public double compensateRatio { get; set; } // 赔付比例
        public long channelId { get; set; } // 红包游戏ID
        public string? redPacketData { get; set; } // 发送红包数据，多个用，分割
        public string? redPassword { get; set; } // 红包密码
        public int? expiration { get; set; } // 红包过期时间（分）
        public string nickName { get; set; }//用户昵称
        public double? redQuota { get; set; } // 红包额度
        public string createTime { get; set; } // 创建时间
        public int redStatus { get; set; } // 红包状态 0-正常 1-已抢完 2-已过期
        public string? Avatar { get; set; }

        public string receiveMemberIds;//抢过此红包的用户

        public bool isGrabed
        {
            get
            {
                bool _isGrabed = false;
                if (!string.IsNullOrEmpty(receiveMemberIds))
                {
                    string[] receiveMemberIds = this.receiveMemberIds.Split(",");
                    for (int i = 0; i < receiveMemberIds.Length; i++)
                    {
                        if (UserManager.Instance != null&& UserManager.Instance.appMemberUserInfoRespVO!=null)
                        {
                            if (receiveMemberIds[i] == (UserManager.Instance.appMemberUserInfoRespVO.id.ToString()??""))
                            {
                                _isGrabed = true;
                                break;
                            }
                        }
                        
                    }
                }
                return _isGrabed;
            }
        }
        public override string ToString()
        {
            return $"PacketSendRespVO: [id={id}, memberId={memberId}, username={username}, redAmount={redAmount}, redCount={redCount}, thunderNo={thunderNo}, compensateAmount={compensateAmount}, compensateRatio={compensateRatio}, channelId={channelId}, redPacketData={redPacketData}, redPassword={redPassword}, expiration={expiration}, redQuota={redQuota}, createTime={createTime}, redStatus={redStatus}]";
        }
    }
}