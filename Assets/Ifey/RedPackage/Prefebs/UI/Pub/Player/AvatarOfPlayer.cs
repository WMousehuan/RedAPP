using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class AvatarOfPlayer : MonoBehaviour
{
    private static Dictionary<string, Sprite> _avatar_Dictionary;
    public static Dictionary<string, Sprite> avatar_Dictionary
    {
        get
        {
            if (_avatar_Dictionary == null)
            {
                _avatar_Dictionary = new Dictionary<string, Sprite>();
            }
            return _avatar_Dictionary;
        }
    }
    public Image rawImage;
    public Sprite oriSprite;

    public void StartToGetUrlImage(string imageUrl)
    {
        if (!avatar_Dictionary.ContainsKey(imageUrl))
        {
            StartCoroutine(DownloadImage(imageUrl));
            avatar_Dictionary.Add(imageUrl, null);
        }
        else
        {
            rawImage.sprite = avatar_Dictionary[imageUrl]==null? oriSprite: avatar_Dictionary[imageUrl];
        }
    }
    public void SetDefaultAvatar()
    {
        rawImage.sprite = oriSprite;
    }
    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            rawImage.sprite = oriSprite;
            avatar_Dictionary.Remove(imageUrl);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            if (avatar_Dictionary.ContainsKey(imageUrl))
            {
                avatar_Dictionary[imageUrl] = sprite;
            }
            rawImage.sprite = sprite;
        }
    }
}
