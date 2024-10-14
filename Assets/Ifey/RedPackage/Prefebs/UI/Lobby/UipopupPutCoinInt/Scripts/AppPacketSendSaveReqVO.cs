using System.Collections;
using UnityEngine;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.UipopupPutCoinInt.Scripts
{
    public class AppPacketSendSaveReqVO
    {
        public int redAmount { get; set; } // 发包总金额(元), 示例值(10)
        public int thunderNo { get; set; } // 雷号, 示例值(3)
        public string payPassword { get; set; } // 支付密码, 示例值(11233455)
        public long playId { get; set; } // 玩法id, 示例值(11233455)
        public long channelId { get; set; } // 游戏id, 示例值(11233455)

        public override string ToString()
        {
            return $"AppPacketSendSaveReqVO: [redAmount={redAmount}, thunderNo={thunderNo}, payPassword={payPassword}, playId={playId}, channelId={channelId}]";
        }
    }
}