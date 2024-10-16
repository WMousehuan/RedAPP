using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUserAvatar_Ctrl : MonoBehaviour
{
    public RawImage avatar_RawImage;
    // Start is called before the first frame update
    void Start()
    {
        avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
        avatar_RawImage.color=(avatar_RawImage.texture != null)?Color.white:Color.clear;
        EventManager.Instance.Regist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID(), (objects) =>
        {
            string sign = (string)objects[0];
            switch (sign)
            {
                case "GetAvatar":
                    avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
                    break;
                case "UpdateData":
                    avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
                    break;
            }
            avatar_RawImage.color = (avatar_RawImage.texture != null) ? Color.white : Color.clear;
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
