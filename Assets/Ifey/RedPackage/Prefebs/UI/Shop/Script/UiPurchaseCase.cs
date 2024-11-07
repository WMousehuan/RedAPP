using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL
#elif UNITY_EDITOR || PLATFORM_ANDROID
using Vuplex.WebView;
using System.Security.Policy;
#endif
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
    public Transform webgl_Case;
    public Transform mobile_Case;
#if UNITY_WEBGL
#elif UNITY_EDITOR || PLATFORM_ANDROID
    public CanvasWebViewPrefab webViewPrefab;
#endif
    public string targetUrl = "";
    private async void Start()
    {
       
#if UNITY_WEBGL
        webgl_Case.gameObject.SetActive(true);
        mobile_Case.gameObject.SetActive(false);
        await Task.Delay(0);
#elif UNITY_EDITOR || PLATFORM_ANDROID
        webgl_Case.gameObject.SetActive(false);
        mobile_Case.gameObject.SetActive(true);
        await webViewPrefab.WaitUntilInitialized();
        if(targetUrl!= webViewPrefab.WebView.Url)
        {
            webViewPrefab.WebView.LoadUrl(targetUrl);
        }
#endif
    }
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
    }
    public void OpenUrl(string url)
    {
        targetUrl = url;
        _case.gameObject.SetActive(true);
#if UNITY_WEBGL
        webIframe_Ctrl.SetUrl(url);
#elif UNITY_EDITOR || PLATFORM_ANDROID
        webViewPrefab?.WebView?.LoadUrl(url);
#endif
    }
    public void Close()
    {
        _case?.gameObject?.SetActive(false);

#if UNITY_WEBGL
#elif UNITY_EDITOR || PLATFORM_ANDROID
        webViewPrefab?.WebView?.LoadUrl("about:blank");
#endif

        closeAction?.Invoke();
        closeAction = null;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

#if UNITY_WEBGL
#elif UNITY_EDITOR || PLATFORM_ANDROID
        webViewPrefab?.WebView?.LoadUrl("about:blank");
        webViewPrefab?.WebView?.Dispose();  // Çå³ý»º´æ
#endif

        closeAction?.Invoke();
        closeAction = null;
    }
}
