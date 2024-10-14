using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugPanel_Ctrl : MonoBehaviour
{
    public static DebugPanel_Ctrl instance;
    public Text content_Text;
    private string content;
    private void Awake()
    {
        instance = this;
    }
    public static void Log(string content)
    {
        if (instance != null)
        {
            instance.content = content + "\r\n" + instance.content;
            if (instance.content_Text.gameObject.activeSelf)
            {
                instance.content_Text.text = instance.content;
            }
        }
        Debug.Log(content);
    }
}
