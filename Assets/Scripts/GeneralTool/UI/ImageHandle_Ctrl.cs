using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ImageHandle_Ctrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IDragHandler
{
    private GraphicRaycaster graphicRaycaster;
    public static HashSet<int> down_HashSet=new HashSet<int>();
    public bool isDowned
    {
        get
        {
            return down_HashSet.Count > 0;
        }
    }
    public UnityEvent<PointerEventData> clickEvent;
    public UnityEvent<PointerEventData> downEvent;
    public UnityEvent<PointerEventData> moveEvent;
    public bool isOnExitTriggerUp;
    public List<ImageHandle_Ctrl> linkUpHandle_Ctrls;
    public UnityEvent upEvent;
    public UnityEvent<PointerEventData> enterEvent;
    public bool isOnExitTriggerExit;
    public UnityEvent exitEvent;

    public UnityEvent<PointerEventData> downOrEnterEvent;
    public bool isOnExitTriggerUpOrExit;
    public UnityEvent upOrExitEvent;
    public bool isOnEnableCheck;
    private bool isChecked = true;
    private void Start()
    {
        MainHandle_Ctrl.instance?.imageHandle_Ctrls.Add(this);
        graphicRaycaster = this.GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
    }
    private void OnEnable()
    {
        StartCoroutine(IEChecking(0.1f));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downEvent?.Invoke(eventData);
        downOrEnterEvent?.Invoke(eventData);
        down_HashSet.Add(this.gameObject.GetInstanceID()); 
    }
    public void OnDrag(PointerEventData eventData)
    {
        moveEvent?.Invoke(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        foreach(ImageHandle_Ctrl handle_Ctrl in linkUpHandle_Ctrls)
        {
            handle_Ctrl.OnPointerUp(eventData);
        }
        upOrExitEvent?.Invoke();
        upEvent?.Invoke();
        down_HashSet.Remove(this.gameObject.GetInstanceID());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDowned)
        {
            enterEvent?.Invoke(eventData);
            downOrEnterEvent?.Invoke(eventData);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        upOrExitEvent?.Invoke();
        exitEvent?.Invoke();
    }
    private void OnDisable()
    {
        if (isOnExitTriggerUp)
        {
            upEvent?.Invoke();
        }
        if (isOnExitTriggerExit)
        {
            exitEvent?.Invoke();
        }
        if (isOnExitTriggerUpOrExit)
        {
            upOrExitEvent?.Invoke();
        }
        down_HashSet.Remove(this.gameObject.GetInstanceID());
        isChecked = false;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            if (isOnExitTriggerUp)
            {
                upEvent?.Invoke();
            }
            if (isOnExitTriggerExit)
            {
                exitEvent?.Invoke();
            }
            if (isOnExitTriggerUpOrExit)
            {
                upOrExitEvent?.Invoke();
            }
            down_HashSet.Clear();
        }
    }
    private IEnumerator IEChecking(float time)
    {
        yield return new WaitForSeconds(time);

        if (isOnEnableCheck && !isChecked)
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current)
                    {
                        position = touch.position
                    };
                    // 存储射线检测结果的列表
                    List<RaycastResult> results = new List<RaycastResult>();
                    // 进行射线检测
                    graphicRaycaster.Raycast(pointerData, results);
                    // 检查射线检测结果中是否包含目标UI
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject == this.gameObject)
                        {
                            OnPointerDown(pointerData);
                        }
                    }
                }
            }
            else
            {
                if (MainHandle_Ctrl.instance && MainHandle_Ctrl.instance.isMouseButtonDown)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };
                    // 存储射线检测结果的列表
                    List<RaycastResult> results = new List<RaycastResult>();

                    // 进行射线检测
                    graphicRaycaster.Raycast(pointerData, results);
                    // 检查射线检测结果中是否包含目标UI
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject == this.gameObject)
                        {
                            OnPointerDown(pointerData);
                            break;
                        }
                    }
                }
            }
        }
    }
}
