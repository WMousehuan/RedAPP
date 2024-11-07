using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRechargeDetail : Popup
{
    public enum RechargeStateType
    {
        None=-1,
        All,
        Obligation,
        Pail,
        Fail,
        Cancle,
    }

    private string catchRechargeUrl = "/app-api/red/cash-recharge/page";//获取订单列表
    private string cancleRechargeUrl = "/app-api/red/order/cancelRecharge/{0}";//取消订单
    //private string catchRechargeUrl = "/app-api/red/cash-recharge/page";//查询订单
    public Dropdown rechargeStateGroup_Dropdown;
    public GridLoopScroll_Ctrl rechargeDetail_LoopScroll;

    public Image loading_Image;

    public int currentPage = 0;
    public bool isLoadingList = false;
    public HashSet<int> loadedPageIndex_HashSet = new HashSet<int>();
    public List<PurchaseOrderDataVO> rechargeDetail_List = new List<PurchaseOrderDataVO>();
    public ObjectGroup<RechargeStateType, string> rechargeStateContents;

    public RechargeStateType currentRechargeStateType = RechargeStateType.None;
    private IEnumerator catchRechargeIE;
    private void Awake()
    {
        rechargeStateGroup_Dropdown.options.Clear();
        foreach (string amountStateType in Enum.GetNames(typeof(RechargeStateType)))
        {
            if ((int)Enum.Parse<RechargeStateType>(amountStateType) >= 0)
            {
                rechargeStateGroup_Dropdown.options.Add(new Dropdown.OptionData((rechargeStateContents.ContainsKey(Enum.Parse<RechargeStateType>(amountStateType)) ? rechargeStateContents[Enum.Parse<RechargeStateType>(amountStateType)].ToString() : amountStateType.ToString())));
            }
        }
        rechargeStateGroup_Dropdown.onValueChanged.AddListener(Index => {
            OnRechargeStateGroup_DropdownValueChange((RechargeStateType)(Index));
        });
    }
    public override void OnEnable()
    {
        //UserManager.Instance.GetUserMainInfo();
        loading_Image.gameObject.SetActive(false);
        base.OnEnable();
        loadedPageIndex_HashSet.Clear();
        rechargeDetail_List.Clear();
        isLoadingList = false;
        if(currentRechargeStateType!= RechargeStateType.None)
        {
            GetDetialsByPage(0, 10);
        }
        rechargeDetail_LoopScroll.scrollOverBottomAction = () =>
        {
            int nextPage = currentPage;// (amountDetail_LoopScroll.currentRow + amountDetail_LoopScroll.itemCount) / 10;
            if (!isLoadingList && !loadedPageIndex_HashSet.Contains(nextPage))
            {
                GetDetialsByPage(nextPage, 10);
            }
        };
        rechargeDetail_LoopScroll.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) =>
        {
            if (realIndex >= 0 && realIndex < rechargeDetail_List.Count)
            {
                PurchaseOrderDataVO purchaseOrderDataVO = rechargeDetail_List[realIndex];
                double amount = purchaseOrderDataVO.optCash;
                RechargeStateType currentRechargeStateType = ((RechargeStateType)(purchaseOrderDataVO.rechargeStatus + 1));
         
                Button button = target.transform.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                Button cancle_Button = target.transform.GetChild<Button>("Cancle_Button");
                cancle_Button.onClick.RemoveAllListeners();
                button.image.rectTransform.offsetMax = new Vector2(currentRechargeStateType == RechargeStateType.Obligation ? -48:0, button.image.rectTransform.offsetMax.y);
                cancle_Button.onClick.AddListener(currentRechargeStateType == RechargeStateType.Obligation ? () =>
                {
                    UiAgreeCase uiAgreeCase = PopupManager.Instance.Open(PopupType.PopupAgreeCase).GetComponent<UiAgreeCase>();
                    uiAgreeCase.content_Text.text = "Is cancle this purchase?";
                    uiAgreeCase.Init(() => {
                        //rechargeDetail_LoopScroll.Init(0,0);
                        string cancleRechargeUrl = string.Format(this.cancleRechargeUrl, purchaseOrderDataVO.orderNo);
                        UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(cancleRechargeUrl, "", null, (resultData) =>
                        {
                            purchaseOrderDataVO.rechargeStatus = 3;
                            if (this.currentRechargeStateType!= RechargeStateType.All&& purchaseOrderDataVO.rechargeStatus != (int)this.currentRechargeStateType)
                            {
                                rechargeDetail_List.Remove(purchaseOrderDataVO);
                            }
                            rechargeDetail_LoopScroll.Refresh(rechargeDetail_List.Count);
                            //OnRechargeStateGroup_DropdownValueChange(currentRechargeStateType, true);
                            IEPool_Manager.instance.StopIE(catchRechargeIE);
                        });
                    });
                }
                : () => { });
                switch (currentRechargeStateType)
                {
                    case RechargeStateType.Pail:
                        button.image.color = Color.green * 0.8f;
                        button.interactable = false;
                        break;
                    case RechargeStateType.Obligation:
                        button.image.color = Color.white;
                        button.interactable = true;
                        button.onClick.AddListener(() => {
                            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
                            
                            UiPurchaseCase.instance.closeAction = () => {
                                IEPool_Manager.instance.StopIE(catchRechargeIE);
                                waitMask_Ui?.ShowResultCase("Cancle recharge", 0);
                            };
                            UiPurchaseCase.instance.timeOverAction = () => {
                                UiPurchaseCase.instance.Close();
                                IEPool_Manager.instance.StopIE(catchRechargeIE);
                                IEPool_Manager.instance.WaitTimeToDo("", 1, null, () => {
                                    OnRechargeStateGroup_DropdownValueChange(currentRechargeStateType, true);
                                });
                                waitMask_Ui?.ShowResultCase("Cancle recharge", 1);
                            };
                            UiPurchaseCase.instance.OpenUrl(purchaseOrderDataVO.payUrl);
                            long timeStamp = 0;
                            if (long.TryParse(purchaseOrderDataVO.expirationTime, out timeStamp))
                            {

                            }
                            System.Action loopAction = null;
                            int stateIndex = 0;
                            float catchTime = 20;
                            float limitTime = timeStamp - GeneralTool_Ctrl.GetTimeStamp();
                            UiPurchaseCase.instance.targetStampTime = timeStamp;
                            loopAction = () => {
                                if (limitTime <= 0)
                                {
                                    UiPurchaseCase.instance.Close();
                                    waitMask_Ui?.ShowResultCase("Recharge timeout", 1);
                                    return;
                                }
                                switch (stateIndex)
                                {
                                    case 0:
                                        catchTime = 20;
                                        break;
                                    case 1:
                                        catchTime = 15;
                                        break;
                                    case 2:
                                        catchTime = 10;
                                        break;
                                    default:
                                        catchTime = 5;
                                        break;
                                }
                                stateIndex++;
                                IEPool_Manager.instance.WaitTimeToDo("RechargeCatch"+ purchaseOrderDataVO.orderNo, catchTime, null,() => {
                                    string catchRechargeUrl = this.catchRechargeUrl + "?orderNo=" + purchaseOrderDataVO.orderNo;
                                    UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(catchRechargeUrl, null, (resultData) => {
                                        //查询订单状态 未完成
                                        ReturnData<PurchaseOrderDataVO> returnData = JsonConvert.DeserializeObject<ReturnData<PurchaseOrderDataVO>>(resultData);
                                        switch (returnData.data.rechargeStatus)
                                        {
                                            case 0://正在充值
                                                loopAction?.Invoke();
                                                break;
                                            case 1://充值成功
                                                UiPurchaseCase.instance.Close();
                                                waitMask_Ui?.ShowResultCase("Recharge successful", 1);
                                                UIManager.Instance.ShowGetCoinEffect(base.transform, new Vector2(0, 100), () => {
                                                    RedPackageAuthor.Instance.userBalance += returnData.data.optCash + returnData.data.awardCash;
                                                }, 10);
                                                break;
                                            case 2://充值失败
                                                UiPurchaseCase.instance.Close();
                                                waitMask_Ui?.ShowResultCase("Recharge Failed", 1);
                                                break;
                                            case 3://充值取消
                                                UiPurchaseCase.instance.Close();
                                                waitMask_Ui?.ShowResultCase("Cancle recharge", 1);
                                                break;
                                        }
                                    }, (code, msg) => {
                                        loopAction?.Invoke();
                                    });
                                });
                            };
                            loopAction?.Invoke();
                        });
                        break;
                    default:
                        button.image.color = Color.red * 0.16f;
                        button.interactable = false;
                        break;
                }

                target.transform.GetChild<Text>("DealAmount_Text").text = (currentRechargeStateType == RechargeStateType.Pail ? "+" : "") + purchaseOrderDataVO.optCash.ToString();
                target.transform.GetChild<TMP_Text>("DealID_Text").text = "OrderNo." + purchaseOrderDataVO.orderNo.ToString();
                target.transform.GetChild<TMP_Text>("DealState_Text").text = rechargeStateContents.ContainsKey(currentRechargeStateType) ? rechargeStateContents[currentRechargeStateType].ToString() : currentRechargeStateType.ToString();
                target.transform.GetChild<TMP_Text>("CreateTime_Text").text = GeneralTool_Ctrl.GetDateTimeByStamp(purchaseOrderDataVO.createTime).ToString();
            }
        };

        rechargeDetail_LoopScroll.Init(0, 0);
        rechargeDetail_List.Clear();

    }
    private void Update()
    {
        if (loading_Image && loading_Image.gameObject.activeSelf)
        {
            loading_Image.transform.eulerAngles += new Vector3(0, 0, -180 * Time.deltaTime);
        }
    }

    public void GetDetialsByPage(int page, int size = 20, bool isReal = false)
    {
        if (!isLoadingList || isReal)
        {
            isLoadingList = true;
            loading_Image.gameObject.SetActive(true);

            string targetUrl = catchRechargeUrl + string.Format("?pageNo={0}&pageSize={1}{2}", (page + 1), size, currentRechargeStateType == RechargeStateType.All ? "" : "&rechargeStatus=" + (int)(currentRechargeStateType - 1));
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(targetUrl, new GetRechargeDetailInterface(this,currentRechargeStateType), (resultData) =>
            {
                if (this != null)
                {
                    loadedPageIndex_HashSet.Add(page);
                    currentPage++;
                    isLoadingList = false;
                    loading_Image.gameObject.SetActive(false);
                }
            }, (code, msg) =>
            {
                if (this != null)
                {
                    isLoadingList = false;
                    loading_Image.gameObject.SetActive(false);
                }
            });
        }

    }
    public void OnRechargeStateGroup_DropdownValueChange(RechargeStateType value, bool isReal)
    {
        if (this == null)
        {
            return;
        }
        rechargeStateGroup_Dropdown.SetValueWithoutNotify((int)value);
        currentRechargeStateType = value;
        currentPage = 0;
        loadedPageIndex_HashSet.Clear();
        rechargeDetail_List.Clear();
        isLoadingList = false;
        rechargeDetail_LoopScroll.Init(0, 0);
        GetDetialsByPage(0, 10, isReal);
    }
    public void OnRechargeStateGroup_DropdownValueChange(RechargeStateType value)
    {
        OnRechargeStateGroup_DropdownValueChange(value, false);
    }



    public class GetRechargeDetailInterface : HttpInterface
    {
        UiRechargeDetail source_Ctrl;
        public RechargeStateType currentRechargeStateType;
        public GetRechargeDetailInterface(UiRechargeDetail source, RechargeStateType currentRechargeStateType)
        {
            source_Ctrl = source;
            this.currentRechargeStateType = currentRechargeStateType;
        }
        public void Success(string result)
        {
            if (source_Ctrl == null)
            {
                return;
            }
            ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>>>(result);

            if (responseData.data.list.Length > 0)
            {
                if (source_Ctrl.currentRechargeStateType == 0 || source_Ctrl.currentRechargeStateType == currentRechargeStateType)
                {
                    source_Ctrl?.rechargeDetail_List?.AddRange(responseData.data.list);
                    source_Ctrl?.rechargeDetail_LoopScroll?.Refresh(source_Ctrl.rechargeDetail_List.Count);
                }
            }
            //Debug.Log("Success GetUserAmountDetail!");
        }

        public void Fail(JObject json)
        {
            // 实现 Fail 方法的逻辑
            int code = json["code"].Value<int>();
            //not login
            if (code == 401)
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                Debug.Log("User notLogin Show Login UI!");
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
                //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
            }
            else if (code == 407)
            {
                //user name exits!
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
            }

            else if (code == 406 || code == 1004003000 || code == 400)
            {
                Debug.Log("Login name or psd error!");
                //user name exits!
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");
            }

        }

        public void UnknowError(string errorMsg)
        {
            Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
        }
    }
}
