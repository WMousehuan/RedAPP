using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts;
using EasyUI.Toast;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static RedPackageAuthor;

/// <summary>
/// 查看抢包明细
/// </summary>
public class UIPopupTreasureDetailResultMain : MonoBehaviour
{
    public GameObject listItemOfGrabDetails;
    public Transform transformParentOflistItemOfGrabDetails;
    public PackageItem myPackageItem;
    public Text userName;
    public Text redAmount;
    public ImageNumberTools thunderNo;
    public Text compensateRatio;
    public AvatarOfPlayer avatarOfPlayer;
    string getTreasureDetailResultHttpResultUrl = "/app-api/red/packet-receive/page";
    // Start is called before the first frame update
    void Start()
    {
        GetTreasureDetailResultHttpResult();
        Debug.Log("UIPopupTreasureDetailResultMain open start do http get detail start.....");
    }
    public void GetTreasureDetailResultHttpResult()
    {
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getTreasureDetailResultHttpResultUrl + "?pageNo=" + 1+ "&pageSize=100&redPacketSendId="+ myPackageItem.packetSendRespVO.id, new UIPopupTreasureDetailResultMainCallBack(this));
    }
    // Update is called once per frame

    public void setPacketInfor(PacketSendRespVO packetSendRespVO)
    {
        if (!string.IsNullOrEmpty(packetSendRespVO.Avatar))
        {
            avatarOfPlayer.StartToGetUrlImage(packetSendRespVO.Avatar);
        }
        string usernameToShow = string.IsNullOrEmpty(packetSendRespVO.nickName) ? "noname" : packetSendRespVO.nickName;
        //Debug.Log("usernameToShow" + usernameToShow);
        this.userName.text = usernameToShow + (packetSendRespVO.id == UserManager.Instance.appMemberUserInfoRespVO.id ? "(Me)" : "");
        this.redAmount.text = packetSendRespVO.redAmount.ToString();
        this.thunderNo.setNumber(packetSendRespVO.thunderNo);
        this.compensateRatio.text = packetSendRespVO.compensateRatio.ToString() + "X";
    }

    public class UIPopupTreasureDetailResultMainCallBack : HttpInterface
    {
        public FailPubDo failPubDo = new FailPubDo();
        UIPopupTreasureDetailResultMain uIPopupTreasureDetailResultMain;
        public UIPopupTreasureDetailResultMainCallBack(UIPopupTreasureDetailResultMain uIPopupTreasureDetailResultMain)
        {
            this.uIPopupTreasureDetailResultMain = uIPopupTreasureDetailResultMain;
        }
        public void Success(string result)
        {
            //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            ReturnData<PageResultPacketSendRespVO<AppPacketReceiveRespVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<AppPacketReceiveRespVO>>>(result);
            // 实现 Success 方法的逻辑
            Debug.Log("Success UIPopupTreasureDetailResultMainCallBack!And now show item!data=" + responseData.data.ToString());
            if (responseData.data.list.Length > 0)
            {
                for (int i = 0; i < responseData.data.list.Length ; i++)
                {
                    AppPacketReceiveRespVO pkgDetailItem = responseData.data.list[i];
                    //add new pkg
                    if (pkgDetailItem != null)
                    {
                        GameObject createPkgItem = Instantiate(this.uIPopupTreasureDetailResultMain.listItemOfGrabDetails, this.uIPopupTreasureDetailResultMain.transformParentOflistItemOfGrabDetails);
                        ListOfPkgResultItem listOfPkgResultItem = createPkgItem.GetComponent<ListOfPkgResultItem>();
                        listOfPkgResultItem.SetResultItemDetailValue(pkgDetailItem);
                        //packageItem.setPacketSendRespVOInfo(pkgItem);
                        //this.refreshChannelTimer.addPacketSendRespVOList(packageItem);
                    }
                    //update old pkg
                    else
                    {

                    }
                }
            }
            else
            {
                Debug.Log("This channel did not have pkg item!");
            }
        }

        public void Fail(JObject json)
        {
            if (!failPubDo.failPubdo(json))
            {
                int code = json["code"].Value<int>();
                if (code == 1004001004)
                {
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Not enough coin to sent!");
                    Debug.Log("Not enough coin to sent!!");
                    return;
                }
                else
                {
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Get Treasure grab detail fail!");
                }
            }
        }

        public void UnknowError(string errorMsg)
        {
            Debug.Log("UIPopupTreasureDetailResultMainCallBack UnknowError=" + errorMsg);
        }
    }


}





