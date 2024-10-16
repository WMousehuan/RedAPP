using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static UIServer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts
{
    public class RefreshChannelGameTypeTImer : MonoBehaviour
    {
        public enum ChannelType
        {
            Public,
            Private,
            Hot,
        }
        public GridLoopScroll_Ctrl loopScroll;
        public ChannelType channelType;
        public GameObject packageItemParent; //pkgitem parent transfer
        public GameObject packageItemOri;    //One pkg item
        public Text title_Text;
        private System.Timers.Timer timer;
        private int interval = 60000; // 定时器间隔时间（单位：毫秒） fresh every 1 min
        private string freshUrl = "/app-api/red/channel/page";
        bool ifNeedToRunRefresh = true;
        List<PubGameChannel> packetSendRespVOList = new List<PubGameChannel>(); // pkg item list
        List<ChannelRespVO> channelRespVOList = new List<ChannelRespVO>();
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
            title_Text.text = channelType.ToString();

            loopScroll.scrollEnterEvent= (realIndex, rowIndex, columnIndex, target) => 
            {
                PubGameChannel packageItem = target.GetComponent<PubGameChannel>();
                packageItem.SetData(channelRespVOList[realIndex]);
            };
            loopScroll.Init(0, 0);
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
                //string memberId = "";
                string pageNo = "1";
                string pageSize = "50";
                string paramsUrl = freshUrl + string.Format("?pageNo={0}&pageSize={1}&channelType={2}", pageNo, pageSize, (int)channelType) ;
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
                    refreshChannelTimer.channelRespVOList.Clear();
                    for (int i = responseData.data.list.Length - 1; i >= 0; i--)
                    {
                        ChannelRespVO pkgItem = responseData.data.list[i];
                        //PubGameChannel oldPackageItem = this.refreshChannelTimer.ifExitsPkgReturn((long)pkgItem.Id);
                        ////add new pkg
                        //if (oldPackageItem == null)
                        //{
                        //    GameObject createPkgItem = Instantiate(this.refreshChannelTimer.packageItemOri, this.refreshChannelTimer.packageItemParent.transform);
                        //    PubGameChannel packageItem = createPkgItem.GetComponent<PubGameChannel>();
                        //    packageItem.SetData(pkgItem);
                        //    this.refreshChannelTimer.addPacketSendRespVOList(packageItem);
                        //}
                        ////update old pkg
                        //else
                        //{
                        //    oldPackageItem.SetData(pkgItem);
                        //}
                        //this.refreshChannelTimer.addPacketSendRespVOList(packageItem);
                        refreshChannelTimer.channelRespVOList.Add(pkgItem);
                    }
                    refreshChannelTimer.loopScroll.Refresh(refreshChannelTimer.channelRespVOList.Count);
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