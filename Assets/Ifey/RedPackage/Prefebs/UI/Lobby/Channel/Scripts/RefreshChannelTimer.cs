﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static UIServer;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts
{
    /// <summary>
    /// Timer with channel package info
    /// </summary>
    public class RefreshChannelTimer : MonoBehaviour
    {
        public GridLoopScroll_Ctrl loopScroll;
        public VerticalLayoutGroup verticalLayoutGroup;
        public ContentSizeFitter contentSizeFitter;

        public GameObject packageItemParent; //pkgitem parent transfer
        public GameObject packageItemOri;    //One pkg item
        //private System.Timers.Timer timer;

        private string freshUrl = "/app-api/red/packet-send/page";

        public float refreshIntervalTime = 2;
        float refreshTime = 0;
      
        List<PackageItem> packetSendRespVOList = new List<PackageItem>(); // pkg item list

        public PacketSendRespVO[] PacketSendResp_List = null;
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
                if (realIndex >= 0 && PacketSendResp_List != null && realIndex < PacketSendResp_List.Length)
                {
                    PackageItem packageItem = target.GetComponent<PackageItem>();
                    packageItem.SetPacketSendRespVOInfo(PacketSendResp_List[realIndex]);
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
        }
        void OnEnable()
        {
            //timer = new System.Timers.Timer(interval);
            //timer.Elapsed += OnTimerElapsed;
            //timer.AutoReset = true;
            //timer.Enabled = true;
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

        void OnDisable()
        {
            //if (timer != null)
            //{
            //    timer.Stop();
            //    timer.Dispose();
            //    Debug.Log("RefreshChannelTimer On Disable");
            //}
        }

        private void Update()
        {
            refreshTime -= Time.deltaTime;
            if (refreshTime <= 0)
            {
                refreshFromRequest();
            }
        }
        public void refreshFromRequest()
        {
            refreshTime = refreshIntervalTime;
            Debug.Log("refreshFromRequest定时器任务执行");
            //string memberId = "";
            int pageNo = Mathf.FloorToInt((loopScroll.currentRow ) / loopScroll.itemCount);

            int pageSize = loopScroll.itemCount;
            ObjectPack<int> valuaPack = new ObjectPack<int>("", 0);
            if (pageNo >= 0)
            {
                string paramsUrl_0 = freshUrl + string.Format("?channelId={0}&pageNo={1}&pageSize={2}", PlayerTreasureGameData.Instance.entranceChannelId, pageNo + 1, pageSize);
                UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(paramsUrl_0, new RefreshChannelRespond(this, pageNo, pageSize, valuaPack));
            }
            int pageNo_1 = pageNo + 1;
            if (pageNo_1 >= 0)
            {
                string paramsUrl_1 = freshUrl + string.Format("?channelId={0}&pageNo={1}&pageSize={2}", PlayerTreasureGameData.Instance.entranceChannelId, pageNo_1 + 1, pageSize);
                UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(paramsUrl_1, new RefreshChannelRespond(this, pageNo_1, pageSize, valuaPack));
            }

        }
        public class RefreshChannelRespond : HttpInterface
        {
            public FailPubDo failPubDo = new FailPubDo();
            RefreshChannelTimer source_Ctrl;
            int currentPage = 0;
            int pageSize = 10;
            ObjectPack<int> rankObject;
            // 构造方法
            public RefreshChannelRespond(RefreshChannelTimer refreshChannelTimer,int currentPage,int pageSize, ObjectPack<int> rankObject)
            {
                this.source_Ctrl = refreshChannelTimer;
                this.currentPage = currentPage;
                this.pageSize = pageSize;
                this.rankObject = rankObject;
            }
            public void Success(string result)
            {
                if (source_Ctrl == null)
                {
                    return;
                }

                ReturnData<PageResultPacketSendRespVO<PacketSendRespVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<PacketSendRespVO>>>(result);
                if (source_Ctrl != null)
                {
                    bool isLengthChange = false;
                    if (rankObject.target == 0 && (source_Ctrl.PacketSendResp_List == null || source_Ctrl?.PacketSendResp_List.Length != responseData.data.total))
                    {
                        isLengthChange = true;
                        rankObject.target++;
                        source_Ctrl.PacketSendResp_List = new PacketSendRespVO[responseData.data.total];
                    }
                    if (responseData.data.list.Length > 0)
                    {
                        List<PacketSendRespVO> packetSendResp_List = new List<PacketSendRespVO>();
                        packetSendResp_List.AddRange(responseData.data.list);
                        packetSendResp_List.Sort((item_0, item_1) =>
                        {
                            if (item_0.redStatus == 0 && !item_0.isGrabed && item_1.isGrabed)
                            {
                                return -1;
                            }
                            return 0;
                        });

                        for (int i = 0; i < packetSendResp_List.Count; i++)
                        {
                            if (((currentPage) * pageSize + i) < source_Ctrl.PacketSendResp_List.Length)
                            {
                                source_Ctrl.PacketSendResp_List[((currentPage) * pageSize + i)] = packetSendResp_List[i];
                            }
                        }
                        if (isLengthChange && false)
                        {
                            source_Ctrl?.loopScroll?.SetPosByIndex(0);
                        }
                        else
                        {
                            source_Ctrl?.loopScroll?.Refresh(responseData.data.total);
                        }
                    }
                }
                // 实现 Success 方法的逻辑
                Debug.Log("Success RefreshChannelRespond!And now create pkg item!count=" + responseData.data.list.Length);
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