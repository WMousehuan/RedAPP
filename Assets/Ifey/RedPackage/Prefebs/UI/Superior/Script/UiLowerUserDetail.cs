using BestHTTP.JSON.LitJson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiLowerUserDetail : Popup
{
    private string url = "/app-api/red/brokerage-detail/page"; public GridLoopScroll_Ctrl loopScroll;


    public TMPro.TMP_InputField name_InputField;

    public RawImage avatar_RawImage;

    public int currentPage = 0;
    public bool isLoadingList = false;
    public HashSet<int> loadedPageIndex_HashSet = new HashSet<int>();

    public List<LowerAgentAmountVO> lowerAgentAmountDetail_List = new List<LowerAgentAmountVO>();
    public Image loading_Image;
    //public Text totalBrokerage_Text;
    public static int currentUserId;

    public void Inits(string name,string avatarUrl)
    {
        name_InputField.text = name;
        UserManager.Instance.SetAvatarRawImageByUrl(avatar_RawImage, avatarUrl);
    }

    public override void Start()
    {
        base.Start();
        loading_Image?.gameObject.SetActive(false);

        loadedPageIndex_HashSet.Clear();
        lowerAgentAmountDetail_List.Clear();
        isLoadingList = false;
        GetDataByPage(0, 10);
        loopScroll.scrollOverBottomAction = () =>
        {
            int nextPage = currentPage;// (amountDetail_LoopScroll.currentRow + amountDetail_LoopScroll.itemCount) / 10;
            if (!isLoadingList && !loadedPageIndex_HashSet.Contains(nextPage))
            {
                GetDataByPage(nextPage, 10);
            }
        };
        loopScroll.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) =>
        {
            if (realIndex >= 0 && realIndex < lowerAgentAmountDetail_List.Count)
            {
                LowerAgentAmountVO lowerAgentAmountData = lowerAgentAmountDetail_List[realIndex];
                //Button button = target.GetComponent<Button>();
                //button.onClick.RemoveAllListeners();
                //button.onClick.AddListener(() =>
                //{
                //    ((UiCommonUserInformation)PopupManager.Instance.Open(PopupType.CommonUserInformation)).RefreshUserInformation(lowerAgentAmountData);
                //});
                target.GetChild<Text>("Amount_Text").text = lowerAgentAmountData.lowerPacketAmount.ToString("F2");
                target.GetChild<Text>("CreateTime_Text").text = dotmob.ConvertFormat.TimeStampToDataTime(lowerAgentAmountData.createTime).ToString();
                //target.GetChild<TMPro.TextMeshProUGUI>("CreateTime_Text").text = dotmob.ConvertFormat.TimeStampToDataTime(long.Parse(lowerAgentAmountData.createTime)).ToString();
            }
        };

        loopScroll.Init(0, 0);
        lowerAgentAmountDetail_List.Clear();
    }
    private void Update()
    {
        if (loading_Image && loading_Image.gameObject.activeSelf)
        {
            loading_Image.transform.eulerAngles += new Vector3(0, 0, -180 * Time.deltaTime);
        }
    }
    public void GetDataByPage(int page, int size = 20)
    {
        if (!isLoadingList)
        {
            isLoadingList = true;
            loading_Image?.gameObject.SetActive(true);

            string targetUrl = url + string.Format("?pageNo={0}&pageSize={1}&lowerMemberId={2}", (page + 1), size, currentUserId);
            print(targetUrl);
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(targetUrl, new GetLowerUserAmountDetailInterface(this), (resultData) =>
            {
                loadedPageIndex_HashSet.Add(page);
                currentPage++;
                isLoadingList = false;
                loading_Image?.gameObject.SetActive(false);
            }, () =>
            {
                isLoadingList = false;
                loading_Image?.gameObject.SetActive(false);
            });
        }

    }
    //public class LowerAgentUserGroupDataVO
    //{
    //    public PageResultPacketSendRespVO<LowerAgentAmountVO> pageResult;
    //    public double totalBrokerage;
    //}
    [System.Serializable]
    public class LowerAgentAmountVO
    {
        public int id;//
        public int lowerPacketAmount;
        public long createTime;
    }

    public class GetLowerUserAmountDetailInterface : HttpInterface
    {
        UiLowerUserDetail source_Ctrl;
        public GetLowerUserAmountDetailInterface(UiLowerUserDetail source_Ctrl)
        {
            this.source_Ctrl = source_Ctrl;
        }
        public void Success(string result)
        {
            if (source_Ctrl == null)
            {
                return;
            }
            print(result);
            if (!string.IsNullOrEmpty(result))
            {
                //JsonData jsonData = JsonMapper.ToObject(result);
                ReturnData<PageResultPacketSendRespVO<LowerAgentAmountVO>> responseData = JsonUtility.FromJson<ReturnData<PageResultPacketSendRespVO<LowerAgentAmountVO>>>(result);
                //source_Ctrl.totalBrokerage_Text.text = responseData.totalBrokerage.ToString("F2");
                if (responseData.data.list.Length > 0)
                {
                    source_Ctrl?.lowerAgentAmountDetail_List?.AddRange(responseData.data.list);
                    source_Ctrl?.loopScroll?.Refresh(source_Ctrl.lowerAgentAmountDetail_List.Count);
                }
            }
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
