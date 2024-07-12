using System;
using System.Collections.Generic;
using Singleton;

public class EventManager : MonoSingleton<EventManager>
{
    public delegate void EventDelegate(object[] args);
    private Dictionary<EventType, EventDelegate> _eventDictionary = new();

    public void PublishEvent(EventType type, params object[] args)
    {
        if (!_eventDictionary.ContainsKey(type) || _eventDictionary[type] == null)
        {
            return;
        }
        
        _eventDictionary[type].Invoke(args);
    }

    public void RegisterEvent(EventType type, EventDelegate action)
    {
        _eventDictionary.TryAdd(type, null);
        _eventDictionary[type] += action;
    }

    public void UnRegisterEvent(EventType type, EventDelegate action)
    {
        _eventDictionary[type] -= action;
    }
}