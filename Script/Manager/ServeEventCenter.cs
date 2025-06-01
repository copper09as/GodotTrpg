using System;
using System.Collections.Generic;

public static class ServeEventCenter
{
    // 无参数事件回调结构
    private struct EventCallback
    {
        public Action Callback;
        public bool IsOneTime;
    }
    
    // 整型参数事件回调结构
    private struct EventCallbackInt
    {
        public Action<int> Callback;
        public bool IsOneTime;
    }

    // 事件字典
    private static readonly Dictionary<string, List<EventCallback>> _eventDictionary = new Dictionary<string, List<EventCallback>>();
    private static readonly Dictionary<string, List<EventCallbackInt>> _eventDictionaryInt = new Dictionary<string, List<EventCallbackInt>>();

    #region 无参数事件系统
    
    // 注册普通事件
    public static void RegisterEvent(string eventName, Action callback)
    {
        AddCallback(eventName, callback, false);
    }

    // 注册一次性事件
    public static void RegisterOneTimeEvent(string eventName, Action callback)
    {
        AddCallback(eventName, callback, true);
    }

    // 取消注册事件
    public static void UnregisterEvent(string eventName, Action callback)
    {
        RemoveCallback(eventName, callback);
    }

    // 触发事件
    public static void TriggerEvent(string eventName)
    {
        TriggerCallbacks(eventName);
    }
    
    #endregion

    #region 整型参数事件系统
    
    // 注册普通事件（带整型参数）
    public static void RegisterEvent(string eventName, Action<int> callback)
    {
        AddCallback(eventName, callback, false);
    }

    // 注册一次性事件（带整型参数）
    public static void RegisterOneTimeEvent(string eventName, Action<int> callback)
    {
        AddCallback(eventName, callback, true);
    }

    // 取消注册事件（带整型参数）
    public static void UnregisterEvent(string eventName, Action<int> callback)
    {
        RemoveCallback(eventName, callback);
    }

    // 触发事件（带整型参数）
    public static void TriggerEvent(string eventName, int arg)
    {
        TriggerCallbacks(eventName, arg);
    }
    
    #endregion

    #region 私有实现方法
    
    // 添加无参数回调
    private static void AddCallback(string eventName, Action callback, bool isOneTime)
    {
        if (!_eventDictionary.TryGetValue(eventName, out var callbacks))
        {
            callbacks = new List<EventCallback>();
            _eventDictionary[eventName] = callbacks;
        }
        callbacks.Add(new EventCallback { Callback = callback, IsOneTime = isOneTime });
    }
    
    // 添加整型参数回调
    private static void AddCallback(string eventName, Action<int> callback, bool isOneTime)
    {
        if (!_eventDictionaryInt.TryGetValue(eventName, out var callbacks))
        {
            callbacks = new List<EventCallbackInt>();
            _eventDictionaryInt[eventName] = callbacks;
        }
        callbacks.Add(new EventCallbackInt { Callback = callback, IsOneTime = isOneTime });
    }
    
    // 移除无参数回调
    private static void RemoveCallback(string eventName, Action callback)
    {
        if (_eventDictionary.TryGetValue(eventName, out var callbacks))
        {
            callbacks.RemoveAll(ec => ec.Callback == callback);
            if (callbacks.Count == 0) _eventDictionary.Remove(eventName);
        }
    }
    
    // 移除整型参数回调
    private static void RemoveCallback(string eventName, Action<int> callback)
    {
        if (_eventDictionaryInt.TryGetValue(eventName, out var callbacks))
        {
            callbacks.RemoveAll(ec => ec.Callback == callback);
            if (callbacks.Count == 0) _eventDictionaryInt.Remove(eventName);
        }
    }
    
    // 触发无参数回调
    private static void TriggerCallbacks(string eventName)
    {
        if (_eventDictionary.TryGetValue(eventName, out var callbacks))
        {
            // 创建执行副本
            var executeList = new List<EventCallback>(callbacks);
            
            // 执行所有回调
            foreach (var ec in executeList)
            {
                ec.Callback?.Invoke();
            }

            // 移除一次性事件
            callbacks.RemoveAll(ec => ec.IsOneTime);
            
            // 清理空事件
            if (callbacks.Count == 0) _eventDictionary.Remove(eventName);
        }
    }
    
    // 触发整型参数回调
    private static void TriggerCallbacks(string eventName, int arg)
    {
        if (_eventDictionaryInt.TryGetValue(eventName, out var callbacks))
        {
            // 创建执行副本
            var executeList = new List<EventCallbackInt>(callbacks);
            
            // 执行所有回调
            foreach (var ec in executeList)
            {
                ec.Callback?.Invoke(arg);
            }

            // 移除一次性事件
            callbacks.RemoveAll(ec => ec.IsOneTime);
            
            // 清理空事件
            if (callbacks.Count == 0) _eventDictionaryInt.Remove(eventName);
        }
    }
    
    #endregion
}