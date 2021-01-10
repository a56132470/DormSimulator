using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_EventDic = new Dictionary<EventType, Delegate>();

    private static void OnListenerAdding(EventType eventType, Delegate callback)
    {
        Delegate dlg;
        if (!m_EventDic.TryGetValue(eventType, out dlg))
        {
            m_EventDic.Add(eventType, null);
        }
        //取到后 判断一下类型是否一样
        if (dlg != null && dlg.GetType() != callback.GetType())
        {
            throw new Exception($"尝试为事件{eventType}添加不同类型的委托，当前事件所对应的委托是{dlg.GetType()}，要添加的委托类型为{callback.GetType()}");
        }

    }
    private static void OnListenerRemoving(EventType eventType, Delegate callback)
    {
        Delegate dlg;
        if (m_EventDic.TryGetValue(eventType, out dlg))
        {
            if (dlg == null)
            {
                throw new Exception($"移除监听错误：事件{eventType}没有对应的委托");
            }
            else if (dlg.GetType() != callback.GetType())
            {
                throw new Exception($"移除监听错误：尝试为事件{eventType}移除不同类型的委托，当前委托类型为{dlg.GetType()}，要移除的委托类型为{callback.GetType()}");
            }
        }
        else
        {
            throw new Exception($"移除监听错误：没有事件码{eventType}");
        }
    }
    private static void OnListenerRemoved(EventType eventType)
    {
        if (m_EventDic[eventType] == null)
        {
            m_EventDic.Remove(eventType);
        }
    }
    //no parameters
    public static void AddListener(EventType eventType, Callback callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback)m_EventDic[eventType] + callback;
    }
    //Single parameters
    public static void AddListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback<T>)m_EventDic[eventType] + callback;
    }
    //two parameters
    public static void AddListener<T, X>(EventType eventType, Callback<T, X> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X>)m_EventDic[eventType] + callback;
    }
    //three parameters
    public static void AddListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y>)m_EventDic[eventType] + callback;
    }
    //four parameters
    [UsedImplicitly]
    public static void AddListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y, Z>)m_EventDic[eventType] + callback;
    }
    //five parameters
    [UsedImplicitly]
    public static void AddListener<T, X, Y, Z, W>(EventType eventType, Callback<T, X, Y, Z, W> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y, Z, W>)m_EventDic[eventType] + callback;
    }

    //no parameters
    public static void RemoveListener(EventType eventType, Callback callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (m_EventDic[eventType] as Callback) - callback;
        OnListenerRemoved(eventType);
    }
    //single parameters
    public static void RemoveListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (m_EventDic[eventType] as Callback<T>) - callback;
        OnListenerRemoved(eventType);
    }
    //two parameters
    public static void RemoveListener<T, X>(EventType eventType, Callback<T, X> callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X>)m_EventDic[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //three parameters
    [UsedImplicitly]
    public static void RemoveListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y>)m_EventDic[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //four parameters
    [UsedImplicitly]
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y, Z>)m_EventDic[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //five parameters
    [UsedImplicitly]
    public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, Callback<T, X, Y, Z, W> callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventDic[eventType] = (Callback<T, X, Y, Z, W>)m_EventDic[eventType] - callback;
        OnListenerRemoved(eventType);
    }


    //no parameters
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback callback)
            {
                callback();
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
    //single parameters
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback<T> callback)
            {
                callback(arg);
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
    //two parameters
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback<T, X> callback)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
    //three parameters
    public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback<T, X, Y> callback)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
    //four parameters
    [UsedImplicitly]
    public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback<T, X, Y, Z> callback)
            {
                callback(arg1, arg2, arg3, arg4);
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
    //five parameters
    [UsedImplicitly]
    public static void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        Delegate d;
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            if (d is Callback<T, X, Y, Z, W> callback)
            {
                callback(arg1, arg2, arg3, arg4, arg5);
            }
            else
            {
                throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
            }
        }
    }
}