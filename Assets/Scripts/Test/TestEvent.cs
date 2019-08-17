using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(GameEventBase.EventType.GuiEvent, EventHandler);
    }



    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            GuiEvent guiEvent = new GuiEvent();
            EventManager.TriggerEvent(guiEvent);
        }
    }

    void EventHandler(GameEventBase e)
    {
        Debug.Log(("Some Function was called {0}!",e.GetType().ToString()));
    }
}
