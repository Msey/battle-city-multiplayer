using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : PersistentSingleton<EventManager>
{
    private Dictionary<System.Type, System.Object> eventDictionary;

    override protected void Awake()
    {
        base.Awake();
        if (eventDictionary == null)
            eventDictionary = new Dictionary<System.Type, System.Object>();
    }

    public class GameEvent<T> : UnityEvent<T>
    {
    }

    public void StartListening<T>(UnityAction<T> listener)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(T), out var currentEvent))
        {
            (currentEvent as GameEvent<T>).AddListener(listener);
        }
        else
        {
            var newEvent = new GameEvent<T>();
            newEvent.AddListener(listener);
            s_Instance.eventDictionary.Add(typeof(T), newEvent);
        }
    }

    public void StopListening<T>(UnityAction<T> listener)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(T), out var currentEvent))
           (currentEvent as GameEvent<T>).RemoveListener(listener);
    }

    public void TriggerEvent<T>(T e)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(T), out var currentEvent))
           (currentEvent as GameEvent<T>).Invoke(e);
    }
}