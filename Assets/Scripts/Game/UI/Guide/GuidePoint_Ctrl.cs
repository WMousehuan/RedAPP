using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GuidePoint_Ctrl : MonoBehaviour
{
    private Camera _sourceCamera;
    private Camera sourceCamera
    {
        get
        {
            if (_sourceCamera == null)
            {
                _sourceCamera = this.GetComponentInParent<Canvas>().worldCamera;
            }
            return _sourceCamera;
        }
    }
    public Vector3 viewPos
    {
        get
        {
            return sourceCamera.WorldToViewportPoint(this.transform.position);
        }
    }
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = this.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    public int sourceValue;
    public string sign;
    public UnityEvent clickEvent;

    [ContextMenu("RemoveSign")]
    public void RemoveSign()
    {
        PlayerPrefs.DeleteKey(sign);
    }
    public void Start()
    {
        int currentValue = PlayerPrefs.GetInt(sign);
        if (GameGuide_Ctrl.instance != null && currentValue == sourceValue)
        {
            GameGuide_Ctrl.instance?.ShowGuide("", this, () =>
            {
                clickEvent?.Invoke();
                PlayerPrefs.SetInt(sign, currentValue + 1);
            });

        }
    }
}
