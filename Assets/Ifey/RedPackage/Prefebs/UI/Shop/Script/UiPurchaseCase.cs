using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiPurchaseCase : Singleton_Base<UiPurchaseCase>
{
    public override bool isDontDestroy => false;
    public GameObject _case;
    public WebIframe_Ctrl webIframe_Ctrl;
    public System.Action closeAction;
    public System.Action timeOverAction;
    public Image loading_Image;
    public Text countDown_Text;
    public long targetStampTime;
    private void Update()
    {
        if (loading_Image != null && loading_Image.gameObject.activeSelf)
        {
            loading_Image?.transform.Rotate(new Vector3(0, 0, -6f));
        }
        float coolDownTime = ((targetStampTime - GeneralTool_Ctrl.GetTimeStamp()) * 0.001f);
        if (countDown_Text != null && countDown_Text.gameObject.activeSelf)
        {
            countDown_Text.text = coolDownTime.ToString("F1") + "s";
        }
        if (coolDownTime <= 0)
        {
            timeOverAction?.Invoke();
            timeOverAction = null;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        _case.gameObject.SetActive(false);
    }
    public void OpenUrl(string url)
    {
        webIframe_Ctrl.SetUrl(url);
        _case.gameObject.SetActive(true);
    }
    public void Close()
    {
        _case?.gameObject?.SetActive(false);
        closeAction?.Invoke();
        closeAction = null;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        closeAction?.Invoke();
        closeAction = null;
    }
}
