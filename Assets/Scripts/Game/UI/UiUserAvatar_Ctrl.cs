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
        avatar_RawImage.color = (avatar_RawImage.texture != null) ? Color.white : Color.clear;
        EventManager.Instance.Regist(GameEventType.GetUserAvatar.ToString(), this.GetInstanceID(), (objects) =>
        {
            avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
            avatar_RawImage.color = (avatar_RawImage.texture != null) ? Color.white : Color.clear;
        });
        EventManager.Instance.Regist(GameEventType.Logout.ToString(), this.GetInstanceID(), (objects) =>
        {
            avatar_RawImage.texture = UserManager.Instance.defaultTexture;
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.GetUserAvatar.ToString(), this.GetInstanceID());
        EventManager.Instance?.UnRegist(GameEventType.Logout.ToString(), this.GetInstanceID());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
