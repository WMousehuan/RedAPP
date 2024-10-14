using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocalizationItem_Base : MonoBehaviour
{
    public static List<LocalizationItem_Base> items=new List<LocalizationItem_Base>();
    public string id;
    public abstract void Init();

    private void Awake()
    {
        items.Add(this);
    }
    private void Start()
    {
        Init();
    }
    private void OnDestroy()
    {
        items.Remove(this);
    }
}
