using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLobby : SceneClass
{
    public GuidePoint_Ctrl putRedpack_GuidePoint;
    private void Start()
    {
        if (UserManager.Instance.appMemberUserInfoRespVO != null)
        {
            putRedpack_GuidePoint.enabled = true;
        }
    }
}
