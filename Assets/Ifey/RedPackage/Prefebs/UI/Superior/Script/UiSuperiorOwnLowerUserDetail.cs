using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Serialization;
using BestHTTP.JSON.LitJson;
public class UiSuperiorOwnLowerUserDetail : Popup
{

    private string url = "/app-api/member/user/lower-page";

    public GridLoopScroll_Ctrl loopScroll;

    public int currentPage = 0;
    public bool isLoadingList = false;
    public HashSet<int> loadedPageIndex_HashSet = new HashSet<int>();

    public List<LowerAgentUserDataVO> lowerAgentUserDetail_List = new List<LowerAgentUserDataVO>();
    public Image loading_Image;
    public Text totalBrokerage_Text;

    public override void Start()
    {
        base.Start();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        loading_Image?.gameObject.SetActive(false);
     
        loadedPageIndex_HashSet.Clear();
        lowerAgentUserDetail_List.Clear();
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
            if (realIndex >= 0 && realIndex < lowerAgentUserDetail_List.Count)
            {
                LowerAgentUserDataVO lowerAgentUserData = lowerAgentUserDetail_List[realIndex];
                //Button button = target.GetComponent<Button>();
                //button.onClick.RemoveAllListeners();
                //button.onClick.AddListener(() =>
                //{
                //    ((UiCommonUserInformation)PopupManager.Instance.Open(PopupType.CommonUserInformation)).RefreshUserInformation(lowerAgentUserData);
                //});
                target.GetChild<Text>("Amount_Text").text = lowerAgentUserData.totalBrokerage.ToString("F2");
                target.GetChild<Text>("NickName_Text").text = lowerAgentUserData.nickname;
                print(lowerAgentUserData.createTime);
                target.GetChild<TMPro.TextMeshProUGUI>("CreateTime_Text").text = dotmob.ConvertFormat.TimeStampToDataTime(long.Parse(lowerAgentUserData.createTime)).ToString();
            }
        };

        loopScroll.Init(0, 0);
        lowerAgentUserDetail_List.Clear();
    }
    private void Update()
    {
        if (loading_Image && loading_Image.gameObject.activeSelf)
        {
            loading_Image.transform.eulerAngles += new Vector3(0, 0, -180 * Time.deltaTime);
        }
    }
    // Update is called once per frame
    public void GetDataByPage(int page, int size = 20)
    {
        if (!isLoadingList)
        {
            isLoadingList = true;
            loading_Image?.gameObject.SetActive(true);

            string targetUrl = url + string.Format("?pageNo={0}&pageSize={1}", (page + 1), size);
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(targetUrl, new GetSuperiorOwnLowerUserDetailInterface(this), (resultData) =>
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
    public class LowerAgentUserGroupDataVO 
    {
        public PageResultPacketSendRespVO<LowerAgentUserDataVO> pageResult;
        public double totalBrokerage;
    }
    [System.Serializable]
    public class LowerAgentUserDataVO
    {
        public int id;//
        public string nickname;//昵称
		//public string avatar;//头像
  //      public string mobile;//手机号
  //      public int point;//积分
  //      //public AgentLevelDataVO level;
  //      public int userType;//用户类型
        public string createTime;//创建日期
        public double totalBrokerage;
    }
    //public class AgentLevelDataVO
    //{
    //    public int id;//等级编号
    //    public string name;//等级名称
    //    public int level;//等级
    //    public int icon;//等级图标
    //}

    public class GetSuperiorOwnLowerUserDetailInterface : HttpInterface
    {
        UiSuperiorOwnLowerUserDetail source_Ctrl;
        public GetSuperiorOwnLowerUserDetailInterface(UiSuperiorOwnLowerUserDetail source_Ctrl)
        {
            this.source_Ctrl = source_Ctrl;
        }
        public void Success(string result)
        {
            if (source_Ctrl==null)
            {
                return;
            }
            print(result);
            if (!string.IsNullOrEmpty(result))
            {
                JsonData jsonData= JsonMapper.ToObject(result);
                LowerAgentUserGroupDataVO responseData =JsonUtility.FromJson<LowerAgentUserGroupDataVO>(jsonData["data"].ToJson());
                source_Ctrl.totalBrokerage_Text.text = responseData.totalBrokerage.ToString("F2");
                if (responseData.pageResult.list.Length > 0)
                {
                    source_Ctrl?.lowerAgentUserDetail_List?.AddRange(responseData.pageResult.list);
                    source_Ctrl?.loopScroll?.Refresh(source_Ctrl.lowerAgentUserDetail_List.Count);
                }
                Debug.Log("Success GetUserAmountDetail!");
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