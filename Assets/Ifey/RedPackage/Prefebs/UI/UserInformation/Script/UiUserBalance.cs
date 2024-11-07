using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField]
    private Text title_Text;

    private float amount = 0;
    [SerializeField]
    private Text amount_Text;

    private string withdrawalUrl;

    public Popup uiWithdrawalCase_Prefab;

    public BalanceType balanceType;
    public override void Start()
    {
        base.Start();
        EventManager.Instance.Regist(GameEventType.CoinUpdate.ToString(), this.GetInstanceID(), (objects) => {
            //amount_Text.text = RedPackageAuthor.Instance.userBalance.ToString("F2");
            Init(balanceType);
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.CoinUpdate.ToString(), this.GetInstanceID());
    }
    public void Init(BalanceType type)
    {
        balanceType = type;
        switch (type)
        {
            case BalanceType.Default:
                title_Text.text = "Balance";
                amount = RedPackageAuthor.Instance.userBalance - RedPackageAuthor.Instance.userCommissionBalance;
                break;
            case BalanceType.Commission:
                title_Text.text = "Commission Balance";
                amount = UserManager.Instance.appMemberUserInfoRespVO.commission;
                break;
        }
        amount_Text.text = amount.ToString("F2");
    }

    public void OnEventWithdrawal()
    {
        var uiWithdrawalCase = PopupManager.Instance.Open(uiWithdrawalCase_Prefab).GetComponent<UiWithdrawalCase>();
        uiWithdrawalCase.Init(balanceType, amount);
        //UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        //string withdrawalUrl = this.withdrawalUrl;
        //UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(withdrawalUrl,null, null, (requestData) =>
        //{

        //}, (code, msg) =>
        //{
        //});
    }
}
