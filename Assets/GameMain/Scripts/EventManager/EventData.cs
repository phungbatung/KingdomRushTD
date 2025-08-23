using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventData
{
    private Dictionary<Type, object> _data;

    public EventData()
    {
        _data = new Dictionary<Type, object>(1);
    }

    public EventData(int capacity)
    {
        _data = new Dictionary<Type, object>(capacity);
    }

    public void Add<T>(T service)
    {
        _data[typeof(T)] = service!;
    }

    public T Get<T>()
    {
        if (!_data.TryGetValue(typeof(T), out object data))
            Debug.LogError($"[EventManager] No object found for type '{typeof(T)}'");
        return (T)data;
    }

    public bool TryGet<T>(out T datum)
    {
        object datumObj;

        if (_data.TryGetValue(typeof(T), out datumObj))
        {
            datum = (T)datumObj;
            return true;
        }

        datum = default;
        return false;
    }
}
