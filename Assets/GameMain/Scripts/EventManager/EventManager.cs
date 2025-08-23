using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static Dictionary<string, List<Action<EventData>>> _eventHandlers = new();


    public static void Subscribe(string eventID, Action<EventData> listener)
    {
        if (!_eventHandlers.ContainsKey(eventID))
        {
            _eventHandlers[eventID] = new List<Action<EventData>>();
        }

        _eventHandlers[eventID].Add(listener);
        Debug.Log($"Subscribed to {eventID}. Total listeners: {_eventHandlers[eventID].Count}");
    }

    public static void Unsubscribe(string eventID, Action<EventData> listener)
    {
        if (_eventHandlers.ContainsKey(eventID))
        {
            _eventHandlers[eventID].Remove(listener);
            Debug.Log($"Unsubscribed from {eventID}. Remaining listeners: {_eventHandlers[eventID].Count}");
        }
    }

    public static void TriggerEvent(string eventID, EventData eventData = null)
    {
        if (_eventHandlers.ContainsKey(eventID))
        {
            Debug.Log($"Triggering {eventID} for {_eventHandlers[eventID].Count} listeners");

            // Create a copy of the list to avoid modification during iteration
            var listeners = new List<Action<EventData>>(_eventHandlers[eventID]);

            foreach (var listener in listeners)
            {
                try
                {
                    listener?.Invoke(eventData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error in event listener for {eventID}: {e.Message}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No listeners for event {eventID}");
        }
    }

    public static void TriggerEvent<T>(string eventID, T eventData)
    {
        EventData data = new();
        data.Add(eventData);
        TriggerEvent(eventID, data);
    }

    public static void ShowAllListeners()
    {
        foreach (var kvp in _eventHandlers)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value.Count} listeners");
        }
    }
}
