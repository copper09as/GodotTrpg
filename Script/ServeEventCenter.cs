using System;
using System.Collections.Generic;

public static class ServeEventCenter
{
    private static readonly Dictionary<string, Action> _eventDictionary = new Dictionary<string, Action>();

    // 注册事件
    public static void RegisterEvent(string eventName, Action callback)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = null;
        }
        _eventDictionary[eventName] += callback;
    }

    // 取消注册事件
    public static void UnregisterEvent(string eventName, Action callback)
    {
        if (_eventDictionary.TryGetValue(eventName, out Action eventDelegate))
        {
            eventDelegate -= callback;
            _eventDictionary[eventName] = eventDelegate;
        }
    }

    // 触发事件
    public static void TriggerEvent(string eventName, object data = null)
    {
        if (_eventDictionary.TryGetValue(eventName, out Action eventDelegate))
        {
            eventDelegate?.Invoke();
        }
    }
}