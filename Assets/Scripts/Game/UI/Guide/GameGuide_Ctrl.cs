using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameGuide_Ctrl : Singleton_Base<GameGuide_Ctrl>
{
    public override bool isDontDestroy => false;
    public Camera mainCamera;
    public UIBeginnerGuideManager beginnerGuideManager;
    public ObjectGroup<string, GuideEvent_Ctrl> guide_Group;

    public void Start()
    {
        EventManager.Instance.Regist(GameEventType.GameGuide.ToString(), this.GetInstanceID(), (objects) =>
        {
            string guideName = (string)objects[0];
            if (guide_Group.ContainsKey(guideName))
            {
                guide_Group[guideName].Open();
            }
        });
    }
    public void ShowGuide(string guideName, GuidePoint_Ctrl target, System.Action action)
    {
        if (guide_Group.ContainsKey(guideName))
        {
            guide_Group[guideName].Open(target, action);
        }
    }
    public Vector3 viewPosToWorldPos(Vector3 viewPos)
    {
        return mainCamera.ViewportToWorldPoint(viewPos);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Instance?.UnRegist(GameEventType.GameGuide.ToString(), this.GetInstanceID());
    }
}
