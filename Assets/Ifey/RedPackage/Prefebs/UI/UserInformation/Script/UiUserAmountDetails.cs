using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public enum AmountStateType
{
    All,
    Recharge,//充值
    HandOut,//发红包
    Grab,//抢红包
    Commission,//佣金
    Reward,//奖励
    Compensation,//赔付
    Refund,//退款
    Withdrawal,//提现
    Give,//赠送
    CompensateAward,//赔付奖励
}
public class UiUserAmountDetails : Popup
{
    
    private string url = "/app-api/member/account-statement/page";

    public Dropdown amountStateGroup_Dropdown;
    public GridLoopScroll_Ctrl amountDetail_LoopScroll;

    public Image loading_Image;

    public int currentPage = 0;
    public bool isLoadingList = false;
    public HashSet<int> loadedPageIndex_HashSet = new HashSet<int>();
    public List<AmountDetailVO> amountDetail_List = new List<AmountDetailVO>();

    public ObjectGroup<AmountStateType, string> amountStateContents;

    public AmountStateType currentAmountStateType = AmountStateType.All;
    private void Awake()
    {
        amountStateGroup_Dropdown.options.Clear();
        foreach (string amountStateType in Enum.GetNames(typeof(AmountStateType)))
        {
            amountStateGroup_Dropdown.options.Add(new Dropdown.OptionData((amountStateContents.ContainsKey(Enum.Parse<AmountStateType>(amountStateType)) ? amountStateContents[Enum.Parse<AmountStateType>(amountStateType)].ToString() : "")));
        }
    }
    public override void OnEnable()
    {
        loading_Image.gameObject.SetActive(false);
        base.OnEnable();
        loadedPageIndex_HashSet.Clear();
        amountDetail_List.Clear();
        isLoadingList = false;
        GetAmountDetialsByPage(0,10);
        amountDetail_LoopScroll.scrollOverBottomAction = () =>
        {
            int nextPage = currentPage;// (amountDetail_LoopScroll.currentRow + amountDetail_LoopScroll.itemCount) / 10;
            if (!isLoadingList && !loadedPageIndex_HashSet.Contains(nextPage))
            {
                GetAmountDetialsByPage(nextPage,10);
            }
        };
        amountDetail_LoopScroll.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) =>
        {
            if (realIndex >= 0 && realIndex < amountDetail_List.Count)
            {
                double amount = amountDetail_List[realIndex].Amount;
                target.transform.GetChild<Text>("DealAmount_Text").text = (amount > 0 ? "+" : "") + amountDetail_List[realIndex].Amount.ToString();
                target.transform.GetChild<TMP_Text>("DealID_Text").text = "No." + amountDetail_List[realIndex].Id.ToString();
                AmountStateType currentAmountStateType = ((AmountStateType)(amountDetail_List[realIndex].TradeType + 1));
                target.transform.GetChild<TMP_Text>("DealState_Text").text = amountStateContents.ContainsKey(currentAmountStateType) ? amountStateContents[currentAmountStateType].ToString():"";
            }
        };

        amountDetail_LoopScroll.Init(0, 0);
        amountDetail_List.Clear();

    }
    private void Update()
    {
        if(loading_Image&& loading_Image.gameObject.activeSelf)
        {
            loading_Image.transform.eulerAngles += new Vector3(0, 0, -180 * Time.deltaTime);
        }
    }

    public void GetAmountDetialsByPage(int page, int size = 20, bool isReal = false)
    {
        if (!isLoadingList||isReal)
        {
            isLoadingList = true;
            loading_Image.gameObject.SetActive(true);
        
            string targetUrl = url + string.Format("?pageNo={0}&pageSize={1}{2}", (page + 1), size, currentAmountStateType == AmountStateType.All ? "" : "&tradeType=" + (int)(currentAmountStateType - 1));
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(targetUrl, new GetUserAmountDetailInterface(this, currentAmountStateType), (resultData) =>
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
    public void OnAmountStateGroup_DropdownValueChange(int value, bool isReal)
    {
        amountStateGroup_Dropdown.SetValueWithoutNotify(value);
        currentAmountStateType = (AmountStateType)value;
        currentPage = 0;
        loadedPageIndex_HashSet.Clear();
        amountDetail_List.Clear();
        isLoadingList = false;
        amountDetail_LoopScroll.Init(0,0);
        GetAmountDetialsByPage(0, 10, isReal);
    }
    public void OnAmountStateGroup_DropdownValueChange(int value)
    {
        OnAmountStateGroup_DropdownValueChange(value,false);
    }
}
public class GetUserAmountDetailInterface : HttpInterface
{
    UiUserAmountDetails source_Ctrl;
    public AmountStateType currentAmountStateType;
    public GetUserAmountDetailInterface(UiUserAmountDetails source, AmountStateType currentAmountStateType)
    {
        source_Ctrl = source;
        this.currentAmountStateType = currentAmountStateType;
    }
    public void Success(string result)
    {
        if (source_Ctrl == null)
        {
            return;
        }
        ReturnData<PageResultPacketSendRespVO<AmountDetailVO>> responseData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<AmountDetailVO>>>(result);

        if (responseData.data.list.Length > 0)
        {
            if(source_Ctrl.currentAmountStateType==0|| source_Ctrl.currentAmountStateType== currentAmountStateType)
            {
                source_Ctrl?.amountDetail_List?.AddRange(responseData.data.list);
                source_Ctrl?.amountDetail_LoopScroll?.Refresh(source_Ctrl.amountDetail_List.Count);
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