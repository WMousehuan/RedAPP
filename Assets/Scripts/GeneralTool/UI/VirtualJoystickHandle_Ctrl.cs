using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class VirtualJoystickHandle_Ctrl : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    public RectTransform _selfRectTransform;
    public RectTransform selfRectTransform
    {
        get
        {
            if (_selfRectTransform == null)
            {
                _selfRectTransform = this.transform.GetComponent<RectTransform>();
            }
            return _selfRectTransform;
        }
    }
    public float clampDistanceMultiple = 1;
    public float clampDistance
    {
        get
        {
            return selfRectTransform.sizeDelta.x * 0.5f * clampDistanceMultiple;
        }
    }

    public bool isDown;
    public RectTransform virtualPoint;

    public UnityEvent<Vector3> downEvent;
    public UnityEvent<Vector3> dragEvent;
    public UnityEvent<Vector3> upEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTransform, eventData.position, eventData.enterEventCamera, out Vector2 localPosition))
        {
            if (virtualPoint != null)
            {
                virtualPoint.anchoredPosition = Vector2.ClampMagnitude(localPosition, clampDistance);
            }
            downEvent?.Invoke(localPosition/ clampDistance);
        }
        isDown = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTransform, eventData.position, eventData.enterEventCamera, out Vector2 localPosition))
        {
            if (isDown)
            {
                if (virtualPoint != null)
                {
                    virtualPoint.anchoredPosition = Vector2.ClampMagnitude(localPosition, clampDistance);

                }
                dragEvent?.Invoke(localPosition / clampDistance);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTransform, eventData.position, eventData.enterEventCamera, out Vector2 localPosition))
        {
            if (virtualPoint != null)
            {
                virtualPoint.transform.localPosition = Vector3.zero;
            }
            upEvent?.Invoke(localPosition / clampDistance);
        }
        isDown = false;
    }
    private void OnDisable()
    {
        isDown = false;
        virtualPoint.transform.localPosition = Vector3.zero;
    }
}
