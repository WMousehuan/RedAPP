using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GeneralTool_Ctrl : Singleton_Base<GeneralTool_Ctrl>
{
    public delegate bool ReturnBoolDelegate();

    public override bool isDontDestroy => false;
    /// <summary>
    /// Base64转Texture2D
    /// </summary>
    /// <param name="str">Base64 Content</param>
    /// <returns></returns>
    public static Texture2D Base64ToTexture2D(string str)
    {
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32,false);
        byte[] arr = Convert.FromBase64String(str);
        tex.LoadImage(arr);
        return tex;
    }

    /// <summary>
    /// 获得text长度
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static float GetTextWidth(Text text)
    {
        TextGenerator tg = text.cachedTextGeneratorForLayout;
        TextGenerationSettings setting = text.GetGenerationSettings(Vector2.zero);
        float width = tg.GetPreferredWidth(text.text, setting) / text.pixelsPerUnit;
        return width;
    }
    /// <summary>
    /// 获得当前时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    public static DateTime GetDateTimeByStamp(string createTimeContent)
    {
        //long createTime = 0;
        if (long.TryParse(createTimeContent, out long createTime))
        {
            return GetDateTimeByStamp(createTime);
        }
        return default;
    }
    public static DateTime GetDateTimeByStamp(long createTime)
    {
        DateTime utcTime = DateTimeOffset.FromUnixTimeMilliseconds((long)createTime).UtcDateTime;
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);
        return localTime;
    }
    public static int CheckBit(int number,int index)
    {
        int i = number >> index;
        //if ((number >> index)|1 == 1)
        //{

        //}
        return 0;
    }

    public static void DownloadImage(string imageUrl, System.Action<Texture2D> successAction, System.Action failAction=null)
    {
        instance.StartCoroutine(IEDownloadImage(imageUrl, successAction, failAction));
    }
    private static IEnumerator IEDownloadImage(string imageUrl, System.Action<Texture2D> successAction, System.Action failAction)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            failAction?.Invoke();
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            successAction?.Invoke(texture);
        }
    }
}
public static class GeneralToolExtensionMethods//方法拓展
{
    /// <summary>
    /// 获取子物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform GetChild(this Transform parent,string name)
    {
        foreach(Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
    /// <summary>
    /// 获得transform在Inspector中的角度
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 InspectorEulers(this Transform target)
    {
        Vector3 angle = target.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        if (Vector3.Dot(target.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(target.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }

        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }

        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }
        return new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
    }
    /// <summary>
    /// 获得子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetChild<T>(this Transform parent, string name=null) where T: Component
    {
        if (name != null)
        {
            if (!name.Contains("/"))
            {
                foreach (T child in parent.GetComponentsInChildren<T>(true))
                {
                    if (child.name == name)
                    {
                        return child;
                    }
                }
            }
            else
            {
                string[] names = name.Split("/");
                Transform currentParent;
                foreach (T child in parent.GetComponentsInChildren<T>(true))
                {
                    currentParent = child.transform.parent;
                    bool isRight = true;
                    if (child.name == names[names.Length - 1])
                    {
                        for (int i = names.Length - 2; i >= 0; i--)
                        {
                            if (currentParent.parent != null && currentParent.name == names[i])
                            {
                                currentParent = currentParent.parent;
                            }
                            else
                            {
                                isRight = false;
                                break;
                            }
                        }
                        if (isRight)
                        {
                            return child;
                        }
                    }
                }
            }
        }
        else
        {
            return parent.GetComponentInChildren<T>();
        }
        return null;
    }

    //判断rendertexture当前是否为黑色 用于视频，有些浏览器视频有声音但没有画面
    public static bool IsRenderTextureBlack(this RenderTexture  renderTexture)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB48, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        Color[] pixels = tex.GetPixels();
        int count = (int)(pixels.Length * 0.1f);
        for (int i = 0; i < pixels.Length; i += count)
        {
            Color color = pixels[(int)Mathf.Clamp((int)pixels.Length * 0.5f, 0, pixels.Length - 1)];
            if (color.r != 0 || color.g != 0 || color.b != 0)
            {
                UnityEngine.Object.Destroy(tex);
                return false;
            }
        }
        UnityEngine.Object.Destroy(tex);
        return true;
    }
}
[System.Serializable]
public class ObjectGroup<V>
{
    public List<ObjectPack<V>> objects = new List<ObjectPack<V>>();
    public Dictionary<string, int> object_Dictionary;
    public int Count
    {
        get
        {
            return objects.Count;
        }
    }
    public V this[string name]
    {
        get
        {
            if(object_Dictionary==null)
            {
                object_Dictionary = new Dictionary<string, int>();
                int index = 0;
                foreach (ObjectPack<V> _object in objects)
                {
                    object_Dictionary.Add(_object.name, index);
                    index++;
                }
            }
            if (object_Dictionary.ContainsKey(name))
            {
                return objects[object_Dictionary[name]].target;
            }
            return default;
        }
    }
    public V this[int index]
    {
        get
        {
            return objects[index].target;
        }
    }
    public bool Contains(string name)
    {
        if (object_Dictionary == null)
        {
            object_Dictionary = new Dictionary<string, int>();
            int index = 0;
            foreach (ObjectPack<V> _object in objects)
            {
                object_Dictionary.Add(_object.name, index);
                index++;
            }
        }
        if (object_Dictionary != null)
        {
            return object_Dictionary.ContainsKey(name);
        }
        return false;
    }

}
[System.Serializable]
public class ObjectPack<V>
{
    public string name;
    public V target;
    public ObjectPack(string name,V target)
    {
        this.name = name;
        this.target = target;
    }
}
[System.Serializable]
public class ObjectGroup<K, V>
{
    public List<ObjectPack<K,V>> objects = new List<ObjectPack<K,V>>();
    public Dictionary<K, int> object_Dictionary;
    public int Count
    {
        get
        {
            return objects.Count;
        }
    }
    public V this[K key]
    {
        get
        {
            if (object_Dictionary == null)
            {
                object_Dictionary = new Dictionary<K, int>();
                int index = 0;
                foreach (ObjectPack<K,V> _object in objects)
                {
                    object_Dictionary.Add(_object.key, index);
                    index++;
                }
            }
            if (object_Dictionary.ContainsKey(key))
            {
                return objects[object_Dictionary[key]].target;
            }
            return default;
        }
    }
    public ObjectPack<K, V> this[int index]
    {
        get
        {
            return objects[index];
        }
    }
    public bool ContainsKey(K key)
    {
        if (object_Dictionary == null)
        {
            object_Dictionary = new Dictionary<K, int>();
            int index = 0;
            foreach (ObjectPack<K,V> _object in objects)
            {
                object_Dictionary.Add(_object.key, index);
                index++;
            }
        }
        if (object_Dictionary != null)
        {
            return object_Dictionary.ContainsKey(key);
        }
        return false;
    }
    public int GetIndexByKey(K key)
    {
        if (object_Dictionary == null)
        {
            object_Dictionary = new Dictionary<K, int>();
            int index = 0;
            foreach (ObjectPack<K, V> _object in objects)
            {
                object_Dictionary.Add(_object.key, index);
                index++;
            }
        }
        if (object_Dictionary.ContainsKey(key))
        {
            return object_Dictionary[key];
        }
        return -1;
    }
    public void Add(K key,V value)
    {
        objects.Add(new ObjectPack<K, V>(key, value));
    }
    public void Clear()
    {
        objects.Clear();

    }
}
[System.Serializable]
public class ObjectPack<K,V>
{
    public K key;
    public V target;
    public ObjectPack(K key, V target)
    {
        this.key = key;
        this.target = target;
    }
}