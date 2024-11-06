using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameGuide_Ctrl : MonoBehaviour
{
    public UIBeginnerGuideManager beginnerGuideManager;
    public ObjectGroup<string, UIBeginnerGuideDataList> guide_Group;

    public void Start()
    {
        EventManager.Instance.Regist(GameEventType.GameGuide.ToString(), this.GetInstanceID(), (objects) =>
        {
            string guideName = (string)objects[0];
            if (guide_Group.ContainsKey(guideName))
            {
                beginnerGuideManager.AddGuideList(guide_Group[guideName]);
                UIBeginnerGuideManager.Instance.ShowGuideList();
            }
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(GameEventType.GameGuide.ToString(), this.GetInstanceID());
    }
}
