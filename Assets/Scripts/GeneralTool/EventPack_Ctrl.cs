using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventPack_Ctrl : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPack<UnityEvent>> events;

    private Dictionary<string, int> eventDictionary=new Dictionary<string, int>();
    private List<string> eventNameList=new List<string>();

    private void Start()
    {
        for(int i=0;i< events.Count; i++)
        {
            if (eventDictionary.TryAdd(events[i].name,i))
            {
                eventNameList.Add(events[i].name);
            }
        }
    }

    public void TriggerEventByIndex(int index)
    {
        if(eventNameList.Count> index)
        {
            events[index].target.Invoke();
        }
    }
    public void TriggerEventByName(string name)
    {
        if (eventDictionary.ContainsKey(name))
            events[ eventDictionary[name]].target.Invoke();
    }

    public void AddEvent(string name, UnityEvent _event)
    {
        events.Add(new ObjectPack<UnityEvent>(name, _event));
        eventDictionary.Add(name, events.Count-1);
        eventNameList.Add(name);
    }
    public void SetEvent(string name, UnityEvent _event)
    {
        if (!eventDictionary.ContainsKey(name))
        {
            events.Add(new ObjectPack<UnityEvent>(name, _event));
            eventDictionary.Add(name, events.Count-1);
            eventNameList.Add(name);
        }
        events[ eventDictionary[name] ].target= _event;
    }
    public void SetEvent(string name, System.Action action)
    {
        UnityEvent unityEvent = new UnityEvent();
        if (!eventDictionary.ContainsKey(name))
        {
            events.Add(new ObjectPack<UnityEvent>(name, unityEvent));
            eventDictionary.Add(name, events.Count-1);
            eventNameList.Add(name);
        }
        else
        {
            unityEvent = events[eventDictionary[name]].target;
        }
        unityEvent.RemoveAllListeners();
        unityEvent.AddListener(()=> {
            action?.Invoke();
        });
    }
}
