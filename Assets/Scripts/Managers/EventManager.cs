using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameEvent : UnityEvent<GameEventBase>
{
}

public class EventManager : PersistentSingleton<EventManager>
{
    private Dictionary<GameEventBase.EventType, GameEvent> eventDictionary;

    override protected void Awake()
    {
        base.Awake();
        if (eventDictionary == null)
            eventDictionary = new Dictionary<GameEventBase.EventType, GameEvent>();
    }

    public static void StartListening(GameEventBase.EventType eventType, UnityAction<GameEventBase> listener)
    {
        GameEvent thisEvent = null;
        if (s_Instance.eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new GameEvent();
            thisEvent.AddListener(listener);
            s_Instance.eventDictionary.Add(eventType, thisEvent);
        }
    }

    public static void StopListening(GameEventBase.EventType eventType, UnityAction<GameEventBase> listener)
    {
        if (s_Instance == null)
            return;
        GameEvent thisEvent = null;
        if (s_Instance.eventDictionary.TryGetValue(eventType, out thisEvent))
            thisEvent.RemoveListener(listener);
    }

    public static void TriggerEvent(GameEventBase e)
    {
        if (!Utils.Verify(e))
            return;

        GameEvent thisEvent = null;
        if (s_Instance.eventDictionary.TryGetValue(e.Type(), out thisEvent))
            thisEvent.Invoke(e);
    }
}