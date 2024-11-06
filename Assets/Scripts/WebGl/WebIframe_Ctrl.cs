using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebIframe_Ctrl : MonoBehaviour
{
    public Camera sourceCamera;
    public Transform originPoint;
    public Transform limitPoint;

    public Vector3 viewPos;
    public Vector3 viewSize;
    private void Update()
    {
        Chack();
    }
    public void Chack()
    {
        viewPos = sourceCamera.WorldToViewportPoint(originPoint.transform.position);
        Vector3 limitPos = sourceCamera.WorldToViewportPoint(limitPoint.transform.position);
        Vector3 origenPos = sourceCamera.WorldToViewportPoint(originPoint.transform.position);
        viewSize = limitPos - origenPos;

        //WebMessage_Ctrl.SendMessageToWeb("meetingUpdate^" + (int)(viewPos.x * 10000) + "," + (int)((1 - viewPos.y) * 10000) + "^" + (int)(Mathf.Abs(screemSize.x)) + "," + (int)(Mathf.Abs(screemSize.y)));
        WebMessage_Ctrl.SendMessageToWeb(string.Format("updateIframe^{0},{1}^{2},{3}", (int)(viewPos.x * 10000), (int)((1 - viewPos.y) * 10000), (int)(Mathf.Abs(viewSize.x * 10000)), (int)(Mathf.Abs(viewSize.y * 10000))));
    }
#if UNITY_WEBGL

    private void OnEnable()
    {
        Chack();
        //print("Unity:"+(int)(viewPos.x) + "," + (int)(viewPos.y));
        //WebMessage_Ctrl.SendMessageToWeb(string.Format("updateIframe^{0},{1}^{2},{3}", (int)(viewPos.x), (int)(viewPos.y), size.x, size.y));
    }
    public void SetUrl(string url)
    {
        WebMessage_Ctrl.SendMessageToWeb("setIframeUrl^" + url);
    }
    private void OnDisable()
    {
        WebMessage_Ctrl.SendMessageToWeb("closeIframe");
    }
    private void OnDestroy()
    {
        WebMessage_Ctrl.SendMessageToWeb("closeIframe");
    }
#else
    public void SetUrl(string url)
    {
        WebMessage_Ctrl.SendMessageToWeb("setIframeUrl^" + url);
    }
#endif
}
