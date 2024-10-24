using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static UIServer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts
{
    public class RefreshChannelGameTypeTImer : MonoBehaviour
    {
            public GameObject packageItemParent; //pkgitem parent transfer
            public GameObject packageItemOri;    //One pkg item
            private System.Timers.Timer timer;
            private int interval = 60000; // 定时器间隔时间（单位：毫秒） fresh every 1 min
            string freshUrl = "/app-api/red/channel/page";
            bool ifNeedToRunRefresh = true;
            List<PubGameChannel> packetSendRespVOList = new List<PubGameChannel>(); // pkg item list
            public void addPacketSendRespVOList(PubGameChannel pubGameChannel)
            {
                packetSendRespVOList.Add(pubGameChannel);
            }
          
            void OnEnable()
            {
                timer = new System.Timers.Timer(interval);
                timer.Elapsed += OnTimerElapsed;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            public PubGameChannel ifExitsPkgReturn(long id)
            {
                foreach (var item in packetSendRespVOList)
                {
                    if (item.channelRespVO.Id == id)
                    {
                        return item;
                    }
                }
                return null;
            }

            private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                // 在这里编写定时执行的任务
                ifNeedToRunRefresh = true;
            }
            void OnDisable()
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                    ifNeedToRunRefresh = false;
                    Debug.Log("RefreshChannelGameTypeTImer On Disable");
                }
            }

            private void Update()
            {
                refreshFromRequest();
            }
            public void refreshFromRequest()
            {
                if (ifNeedToRunRefresh)
                {
                    Debug.Log("RefreshChannelGameTypeTImer定时器任务执行");
                    ifNeedToRunRefresh = false;
                    string memberId = "";
                    string pageNo = "1";
                    string pageSize = "50";
                    string paramsUrl = freshUrl + "?pageNo=1&pageSize=50";
                    UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(paramsUrl, new RefreshChannelGameTypeTImerRespond(this));
                }
            }
            public class RefreshChannelGameTypeTImerRespond : HttpInterface
            {
                public FailPubDo failPubDo = new FailPubDo();
                RefreshChannelGameTypeTImer refreshChannelTimer;
                // 构造方法
                public RefreshChannelGameTypeTImerRespond(RefreshChannelGameTypeTImer refreshChannelTimer)
                {
                    this.refreshChannelTimer = refreshChannelTimer;
                }
                public void Success(string result)
                {
                    //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                    ReturnData<PageResultPacketSendRespVO<ChannelRespVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<ChannelRespVO>>>(result);
                    // 实现 Success 方法的逻辑
                    Debug.Log("Success RefreshChannelGameTypeTImerRespond!And now create pkg item!count=" + responseData.data.list.Length);
                    if (responseData.data.list.Length > 0)
                    {
                        for (int i = responseData.data.list.Length - 1; i >= 0; i--)
                        {
                            ChannelRespVO pkgItem = responseData.data.list[i];
                            PubGameChannel oldPackageItem = this.refreshChannelTimer.ifExitsPkgReturn((long)pkgItem.Id);
                            //add new pkg
                            if (oldPackageItem == null)
                            {
                                GameObject createPkgItem = Instantiate(this.refreshChannelTimer.packageItemOri, this.refreshChannelTimer.packageItemParent.transform);
                                PubGameChannel packageItem = createPkgItem.GetComponent<PubGameChannel>();
                                packageItem.SetData(pkgItem);
                                this.refreshChannelTimer.addPacketSendRespVOList(packageItem);
                            }
                            //update old pkg
                            else
                            {
                                oldPackageItem.SetData(pkgItem);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("This RefreshChannelGameTypeTImerRespond did not have pkg item!");
                    }
                }

                public void Fail(JObject json)
                {
                    if (!failPubDo.failPubdo(json))
                    {
                        MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "RefreshChannelRespond Fail!");
                    }

                }

                public void UnknowError(string errorMsg)
                {
                    Debug.LogError("RefreshChannelRespond" + errorMsg);
                }
            }
        }
}