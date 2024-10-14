using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebMessage_Ctrl : Singleton_Base<WebMessage_Ctrl>
{
    public override bool isDontDestroy => true;
    [DllImport("__Internal")]
    public static extern void receiveMessageFromUnity(string content);
    //private System.Action<string> ReciveMessageEvent;
    private Dictionary<string, WebReviceMessage> targets_Dictionary=new Dictionary<string, WebReviceMessage>();
    private Dictionary<string, List<string>> waitMessages_Dictionary=new Dictionary<string, List<string>>();
    public void ReciveMessage(string msg)
    {
        string[] data = msg.Split('|');
        string targetName = data[0];
        string message = data[1];
        if (targets_Dictionary.ContainsKey(targetName))
        {
            targets_Dictionary[targetName].ReciveMessage(message);
        }
        else
        {
            if (!waitMessages_Dictionary.ContainsKey(targetName))
            {
                waitMessages_Dictionary.Add(targetName, new List<string>());
               
            }
            waitMessages_Dictionary[targetName].Add(message);
        }
    }
    public void Regist(GameObject target )
    {
        WebReviceMessage webReviceMessage = target.GetComponent<WebReviceMessage>();
        if (webReviceMessage == null)
        {
            return;
        }
        if (!targets_Dictionary.ContainsKey(target.name))
        {
            targets_Dictionary.Add(target.name, webReviceMessage);
        }
        if (waitMessages_Dictionary.ContainsKey(target.name))
        {
            for(int i= waitMessages_Dictionary[target.name].Count-1; i >= 0; i--)
            {
                webReviceMessage.ReciveMessage(waitMessages_Dictionary[target.name][i]);
                waitMessages_Dictionary[target.name].RemoveAt(i);
            }
            waitMessages_Dictionary.Remove(target.name);
        }
    }
    public void UnRegist(GameObject target)
    {
        if (targets_Dictionary.ContainsKey(target.name))
        {
            targets_Dictionary.Remove(target.name);
        }
    }
    public static void SendMessageToWeb(string content)
    {
        if (!instance)
        {
            //Debug.Log("Create");
        }
#if UNITY_EDITOR
#elif UNITY_WEBGL
        receiveMessageFromUnity(content);
#endif
    }
}
public interface WebReviceMessage
{
    public void ReciveMessage(string msg);
}