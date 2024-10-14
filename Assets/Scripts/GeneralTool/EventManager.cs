using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class EventManager
{

    /*--------声明单例类---------*/

    static EventManager evenManager = new EventManager();
    private EventManager() { }
    public static EventManager Instance
    {
        get
        {
            return evenManager;
        }
    }

    //执行事件，参数为对象列表
    public delegate void EventDelegate(params object[] args);
    //使用字典格式（因为会有多个执行事件）存储执行事件，并用字符串存储ID
    private Dictionary<string, Dictionary<int, EventDelegate>> eventListeners = new Dictionary<string, Dictionary<int, EventDelegate>>();
    private Dictionary<string,List<object[]>> readyEvents =new Dictionary<string,  List<object[]>>();


    /*-------事件生命周期---------*/



    public void Regist(string eventName,int id, EventDelegate handler,bool isRunReadyEvent =false)
    {
        //如果要存储执行事件为空，不处理
        if (handler == null)
        {
            return;
        }

        //如果事件字典中不包含该eventName这个键，则创建一个键值对（即一个新的触发事件）
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners.Add(eventName, new Dictionary<int, EventDelegate>());
        }
     
        //获取eventName键对应的值,也同样是一个字典类型
        var handlerDic = eventListeners[eventName];
        //获取输入的委托事件的哈希值(字典索引的一种方式)
        //如果输入事件对应的字典中包含执行事件，则从中移除
        if (handlerDic.ContainsKey(id))
        {
            handlerDic[id] = handler;
        }
        else
        {
            //将eventName键对应的值（输入的执行事件）的字典类型（用handler的哈希值作为键）加入到事件字典中
            handlerDic.Add(id, handler);
        }
        if (isRunReadyEvent  && readyEvents.ContainsKey(eventName))
        {
            for (int i = 0; i < readyEvents[eventName].Count; i++)
            {
                handler.Invoke(readyEvents[eventName][i]);
            }
        }
    }

    /// <summary>
    /// 注销事件：在触发事件对应的多个执行事件中删除不再需要的执行事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handLer"></param>
    public void UnRegist(string eventName, EventDelegate handLer)
    {
        //同样，如果执行事件为空，不进行后续处理
        if (handLer == null)
        {
            return;
        }
        Debug.Log(eventName);
        //如果事件字典中包含输eventName这个ID,则移除该键中对应的字典中的输入的执行事件
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(handLer.GetHashCode());
            //在移除后，如果eventName对应的字典为空，或者不存在，则删除该键值对 
            if (eventListeners[eventName] == null || eventListeners[eventName].Count == 0)
            {
                eventListeners.Remove(eventName);
            }
        }
        if (readyEvents.ContainsKey(eventName))
        {
            readyEvents.Remove(eventName);
        }
    }

    public void UnRegist(string eventName, int id)
    {
        //如果事件字典中包含输eventName这个ID,则移除该键中对应的字典中的输入的执行事件
        if (eventListeners.ContainsKey(eventName))
        {
            if (eventListeners[eventName].ContainsKey(id))
            {
                eventListeners[eventName].Remove(id);
                //在移除后，如果eventName对应的字典为空，或者不存在，则删除该键值对 
                if (eventListeners[eventName] == null || eventListeners[eventName].Count == 0)
                {
                    eventListeners.Remove(eventName);
                }
            }
            
        }
    }

    /// <summary>
    /// 触发事件：根据ID来找到触发事件对应的所有执行事件并运行
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="objs"></param>
    public void DispatchEvent(string eventName, params object[] objs)
    {
        // 如果包含eventName这个ID
        if (eventListeners.ContainsKey(eventName))
        {
            //获取eventName键对应的所有执行事件
            var handlerDic = eventListeners[eventName];
            if (handlerDic != null && handlerDic.Count > 0)
            {
                var dic = new Dictionary<int, EventDelegate>(handlerDic);
                //通过对eventName键对应的所有执行进行遍历，然后运行
                foreach (var f in dic.Values)
                {
                    try
                    {
                        //执行所有的委托事件，即EventDelegate(object[] args)
                        f(objs);
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
            }

        }
    }
    /// <summary>
    /// 触发事件：根据ID来找到触发事件对应的所有执行事件并运行
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="objs"></param>
    public void DispatchReadyEvent(string eventName, params object[] objs)
    {
        // 如果包含eventName这个ID
        if (eventListeners.ContainsKey(eventName))
        {
            //获取eventName键对应的所有执行事件
            var handlerDic = eventListeners[eventName];
            if (handlerDic != null && handlerDic.Count > 0)
            {
                var dic = new Dictionary<int, EventDelegate>(handlerDic);
                //通过对eventName键对应的所有执行进行遍历，然后运行
                foreach (var f in dic.Values)
                {
                    try
                    {
                        //执行所有的委托事件，即EventDelegate(object[] args)
                        f(objs);
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
            }

        }
        else
        {
            if (!readyEvents.ContainsKey(eventName))
            {
                readyEvents.Add(eventName, new List<object[]>());
            }
            readyEvents[eventName].Add(objs);
        }
    }

    /// <summary>
    /// 删除事件：与注销事件不同，删除事件直接删除触发事件所对应的所有执行事件，同时删除这个触发事件
    /// </summary>
    /// <param name="eventName"></param>
    public void ClearEvents(string eventName)
    {
        //如果事件字典中包含eventName中的键，则移除触发事件对应的所有执行事件
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners.Remove(eventName);
        }
    }

    /// <summary>
    /// 清空事件
    /// </summary>
    public void ClearAllEvents()
    {
        eventListeners.Clear();
    }

}


