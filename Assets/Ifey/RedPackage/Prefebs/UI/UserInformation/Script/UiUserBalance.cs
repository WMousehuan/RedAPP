using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum BalanceType
{
    Default,
    Commission,
}
public class UiUserBalance : Popup
{

    public GridLoopScroll_Ctrl withdrawalDetail_LoopScroll;

    public Image loading_Image;

    public int currentPage = 0;
    public bool isLoadingList = false;
    private HashSet<int> loadedPageIndex_HashSet = new HashSet<int>();
    private List<PurchaseOrderDataVO> withdrawalDetail_List = new List<PurchaseOrderDataVO>();
    private string catchWithdrawalUrl = "/app-api/red/cash-withdraw/page";

    [SerializeField]
    private Text title_Text;

    private float amount = 0;
    private float currentAmount = 0;
    [SerializeField]
    private Text amount_Text;
    [SerializeField]
    private Text withdrawal_Text;



    public Popup uiWithdrawalCase_Prefab;

    public BalanceType balanceType;
    public override void Start()
    {
        base.Start();
        EventManager.Instance.Regist(GameEventType.CoinUpdate.ToString(), this.GetInstanceID(), (objects) => {
            Init(balanceType);
        });
    }
    public override void OnEnable()
    {
        base.OnEnable();

        loading_Image.gameObject.SetActive(false);
        loadedPageIndex_HashSet.Clear();
        withdrawalDetail_List.Clear();
        isLoadingList = false;
        GetDetialsByPage(0, 10);
        withdrawalDetail_LoopScroll.scrollOverBottomAction = () =>
        {
            int nextPage = currentPage;// (amountDetail_LoopScroll.currentRow + amountDetail_LoopScroll.itemCount) / 10;
            if (!isLoadingList && !loadedPageIndex_HashSet.Contains(nextPage))
            {
                GetDetialsByPage(nextPage, 10);
            }
        };
        withdrawalDetail_LoopScroll.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) =>
        {
            if (realIndex >= 0 && realIndex < withdrawalDetail_List.Count)
            {
                PurchaseOrderDataVO purchaseOrderDataVO = withdrawalDetail_List[realIndex];
                double amount = purchaseOrderDataVO.optCash;
                Button button= target.GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GeneralTool_Ctrl.CopyToClipbord(purchaseOrderDataVO.orderNo, "Copied order number");
                });
                target.transform.GetChild<Text>("DealAmount_Text").text = purchaseOrderDataVO.optCash.ToString();
                target.transform.GetChild<TMP_Text>("DealID_Text").text = "OrderNo." + purchaseOrderDataVO.orderNo.ToString();
                target.transform.GetChild<TMP_Text>("DealState_Text").text = "Withdrawing";
                target.transform.GetChild<TMP_Text>("CreateTime_Text").text = GeneralTool_Ctrl.GetDateTimeByStamp(purchaseOrderDataVO.createTime).ToString();
            }
        };

        withdrawalDetail_LoopScroll.Init(0, 0);
        withdrawalDetail_List.Clear();
    }

    private void Update()
    {
        if (loading_Image && loading_Image.gameObject.activeSelf)
        {
            loading_Image.transform.eulerAngles += new Vector3(0, 0, -180 * Time.deltaTime);
        }
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.CoinUpdate.ToString(), this.GetInstanceID());
    }
    public void Init(BalanceType type)
    {
        balanceType = type;
        float withdrawalAmount = 0;
        switch (type)
        {
            case BalanceType.Default:
                title_Text.text = "Balance";
                amount = RedPackageAuthor.Instance.realUserBalance - RedPackageAuthor.Instance.currentUserCommissionBalance;
                withdrawalAmount = RedPackageAuthor.Instance.withdrawalBalanceAmount;
                currentAmount = RedPackageAuthor.Instance.currentUserBalanceWithoutCommission;
                break;
            case BalanceType.Commission:
                title_Text.text = "Commission Balance";
                amount = RedPackageAuthor.Instance.realUserCommissionBalance;
                withdrawalAmount = RedPackageAuthor.Instance.withdrawalCommissionBalanceAmount;
                currentAmount = RedPackageAuthor.Instance.currentUserCommissionBalance;
                break;
        }
        amount_Text.text = amount.ToString("F2") + (withdrawalAmount > 0 ? "\r\n" : "");
        withdrawal_Text.gameObject.SetActive(withdrawalAmount > 0);
        withdrawal_Text.text = string.Format("-{0}", withdrawalAmount.ToString("F2"));
    }

    public void OnEventWithdrawal()
    {
        var uiWithdrawalCase = PopupManager.Instance.Open(uiWithdrawalCase_Prefab).GetComponent<UiWithdrawalCase>();
        uiWithdrawalCase.Init(this, currentAmount);
    }


    public void GetDetialsByPage(int page, int size = 20, bool isReal = false)
    {
        if (!isLoadingList || isReal)
        {
            isLoadingList = true;
            loading_Image.gameObject.SetActive(true);

            string targetUrl = catchWithdrawalUrl + string.Format("?pageNo={0}&pageSize={1}&optType={2}&optCashStatus={3}", (page + 1), size, (int)balanceType,0);
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(targetUrl, new GetWithdrawalDetailInterface(this, balanceType), (resultData) =>
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
    public class GetWithdrawalDetailInterface : HttpInterface
    {
        UiUserBalance source_Ctrl;
        public BalanceType currentBalanceType;
        public GetWithdrawalDetailInterface(UiUserBalance source, BalanceType currentBalanceType)
        {
            source_Ctrl = source;
            this.currentBalanceType = currentBalanceType;
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
                if (source_Ctrl.balanceType == currentBalanceType)
                {
                    source_Ctrl?.withdrawalDetail_List?.AddRange(responseData.data.list);
                    source_Ctrl?.withdrawalDetail_LoopScroll?.Refresh(source_Ctrl.withdrawalDetail_List.Count);
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
            print(code);
        }

        public void UnknowError(string errorMsg)
        {
            Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
        }
    }
}
