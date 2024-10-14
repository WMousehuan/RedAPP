using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandle_Ctrl : Singleton_Base<MainHandle_Ctrl>
{
    public override bool isDontDestroy => false;
    public List<ImageHandle_Ctrl> imageHandle_Ctrls = new List<ImageHandle_Ctrl>();
    public bool isMouseButtonDown=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseButtonDown = false;
        }
    }
}
