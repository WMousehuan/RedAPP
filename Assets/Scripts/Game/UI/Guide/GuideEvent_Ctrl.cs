using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GuideEvent_Ctrl : MonoBehaviour
{
    private UIBeginnerGuideDataList _guideDataList;
    public UIBeginnerGuideDataList guideDataList
    {
        get
        {
            if (_guideDataList == null)
            {
                _guideDataList = this.gameObject.GetComponent<UIBeginnerGuideDataList>();
            }
            return _guideDataList;
        }
    }
    public RectTransform target;
    public string eventName;
    // Start is called before the first frame update
    public void Open(GuidePoint_Ctrl target = null, System.Action action = null)
    {
        GameGuide_Ctrl.instance.beginnerGuideManager.AddGuideList(guideDataList);
        if (target != null)
        {
            this.target.position = GameGuide_Ctrl.instance.viewPosToWorldPos(target.viewPos);
            this.target.sizeDelta = target.rectTransform.rect.size * 1.2f;
            //guideDataList.guideDataList[0].selectedObject = target.gameObject;
        }
        UIBeginnerGuideManager.Instance.ShowGuideList();
        //clickEvent.AddListener(() => { action?.Invoke(); });
        Button gameObject = guideDataList.transform.GetChild<Button>("GameObject");
        RectTransform gestureWidget = guideDataList.transform.GetChild<RectTransform>("GestureWidget");
        gameObject.onClick.RemoveAllListeners();
        gameObject.onClick.AddListener(() =>
        {
            EventManager.Instance.DispatchEvent(GameEventType.GameGuide.ToString(), eventName);
            action?.Invoke();
            UIBeginnerGuideManager.Instance.FinishGuide();
        });
        gestureWidget.transform.position = this.target.position;
    }
}
