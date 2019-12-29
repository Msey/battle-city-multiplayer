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

    public void StartListening<EventType>(UnityAction<EventType> listener)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(EventType), out var currentEvent))
        {
            (currentEvent as GameEvent<EventType>).AddListener(listener);
        }
        else
        {
            var newEvent = new GameEvent<EventType>();
            newEvent.AddListener(listener);
            s_Instance.eventDictionary.Add(typeof(EventType), newEvent);
        }
    }

    public void StopListening<EventType>(UnityAction<EventType> listener)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(EventType), out var currentEvent))
           (currentEvent as GameEvent<EventType>).RemoveListener(listener);
    }

    public void TriggerEvent<EventType>(EventType e)
    {
        if (s_Instance.eventDictionary.TryGetValue(typeof(EventType), out var currentEvent))
           (currentEvent as GameEvent<EventType>).Invoke(e);
    }
}