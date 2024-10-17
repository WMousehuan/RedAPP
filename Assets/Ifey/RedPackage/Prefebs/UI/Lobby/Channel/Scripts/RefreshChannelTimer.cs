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
    /// <summary>
    /// Timer with channel package info
    /// </summary>
    public class RefreshChannelTimer : MonoBehaviour
    {
        public GridLoopScroll_Ctrl loopScroll;
        [SerializeField]
        public ScrollRect scrollRect; //scroll of the pkg
        public VerticalLayoutGroup verticalLayoutGroup;
        public ContentSizeFitter contentSizeFitter;

        public GameObject packageItemParent; //pkgitem parent transfer
        public GameObject packageItemOri;    //One pkg item
        private System.Timers.Timer timer;
        public int interval = 5000; // 定时器间隔时间（单位：毫秒）
        private string freshUrl = "/app-api/red/packet-send/page";
        bool ifNeedToRunRefresh = true;
      
        List<PackageItem> packetSendRespVOList = new List<PackageItem>(); // pkg item list

        List<PacketSendRespVO> PacketSendResp_List=new List<PacketSendRespVO>();
        private void Start()
        {
            EventManager.Instance.Regist(typeof(submitPutCoinInItHttpCallBack).ToString(), this.GetInstanceID(), (objects) => {

                string sign= (string)objects[0];
                switch (sign)
                {
                    case "Refresh":
                        refreshFromRequest();
                        break;
                }
            });

            loopScroll.scrollEnterEvent = (realIndex, rowIndex, cloumIndex, target) =>
            {
                if (realIndex >= 0 && realIndex < PacketSendResp_List.Count)
                {
                    PackageItem packageItem = target.GetComponent<PackageItem>();
                    packageItem.setPacketSendRespVOInfo(PacketSendResp_List[realIndex]);
                }
            };
            loopScroll.Init(0, 0);
        }
        private void OnDestroy()
        {
            EventManager.Instance?.UnRegist(typeof(submitPutCoinInItHttpCallBack).ToString(), this.GetInstanceID());
        }
        public void addPacketSendRespVOList(PackageItem packageItem)
        {
            packetSendRespVOList.Add(packageItem);
            ScrollToBottom();
        }
        // 添加新内容后调用此方法，让Scroll Rect自动滚动到最底部
        public void ScrollToBottom()
        {
            StartCoroutine(DelayedScrollToBottom());
        }

        private IEnumerator DelayedScrollToBottom()
        {
            yield return null; // 等待一帧，确保内容已经更新
            //LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup.GetComponent<RectTransform>());
            //LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.GetComponent<RectTransform>());
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f; // 滚动到底部
        }
        void OnEnable()
        {
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        public PackageItem ifExitsPkgReturn(long id)
        {
            foreach (var item in packetSendRespVOList)
            {
                if (item.packetSendRespVO.id == id)
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
                Debug.Log("RefreshChannelTimer On Disable");
            }
        }

        private void Update()
        {
            if (ifNeedToRunRefresh)
            {
                refreshFromRequest();
            }
        }
        public void refreshFromRequest()
        {
            Debug.Log("refreshFromRequest定时器任务执行");
            ifNeedToRunRefresh = false;
            string memberId = "";
            string pageNo = "1";
            string pageSize = "50";
            string paramsUrl = freshUrl + "?channelId=" + PlayerTreasureGameData.Instance.entranceChannelId +
                "&memberId=" + memberId +
                "&pageNo=1&pageSize=50";
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(paramsUrl, new RefreshChannelRespond(this));
            
        }
        public class RefreshChannelRespond : HttpInterface
        {
            public FailPubDo failPubDo = new FailPubDo();
            RefreshChannelTimer source_Ctrl;
            // 构造方法
            public RefreshChannelRespond(RefreshChannelTimer refreshChannelTimer)
            {
                this.source_Ctrl = refreshChannelTimer;
            }
            public void Success(string result)
            {
                //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                ReturnData<PageResultPacketSendRespVO<PacketSendRespVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<PacketSendRespVO>>>(result);
                source_Ctrl?.PacketSendResp_List?.Clear();
                if (responseData.data.list.Length > 0)
                {
                    source_Ctrl?.PacketSendResp_List?.AddRange(responseData.data.list);
                    source_Ctrl?.loopScroll?.Refresh(source_Ctrl.PacketSendResp_List.Count);
                }
                // 实现 Success 方法的逻辑
                Debug.Log("Success RefreshChannelRespond!And now create pkg item!count="+responseData.data.list.Length);
                //if (responseData.data.list.Length > 0) {
                //    for(int i= responseData.data.list.Length-1; i>=0;i--){
                //       PacketSendRespVO pkgItem = responseData.data.list[i];
                //        PackageItem oldPackageItem = this.refreshChannelTimer.ifExitsPkgReturn(pkgItem.id);
                //        //add new pkg
                //        if (oldPackageItem==null)
                //        {
                //            GameObject createPkgItem = Instantiate(this.refreshChannelTimer.packageItemOri, this.refreshChannelTimer.packageItemParent.transform);
                //            PackageItem packageItem = createPkgItem.GetComponent<PackageItem>();
                //            packageItem.setPacketSendRespVOInfo(pkgItem);
                //            this.refreshChannelTimer.addPacketSendRespVOList(packageItem);
                //        }
                //        //update old pkg
                //        else {
                //            oldPackageItem.setPacketSendRespVOInfo(pkgItem);
                //            oldPackageItem.setBackgroundWithPacketSendRespVO();
                //        }
                //    }
                //}
                //else
                //{
                //    Debug.Log("This channel did not have pkg item!");
                //}
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