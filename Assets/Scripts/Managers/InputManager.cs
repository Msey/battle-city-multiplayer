using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(GameEventBase.EventType.KeyBoardEvent, InputHandler);
        EventManager.StartListening(GameEventBase.EventType.KeyPressEvent, KeyHandler);
        // TODO: надо добавить функции обратного вызова в конструктор 
        // событий, чтобы Update() вызывался только в EventManager
    }

    void Update()
    {
        KeyBoardEvent kbEvent = new KeyBoardEvent();
        EventManager.TriggerEvent(kbEvent);
    }

    void KeyHandler(GameEventBase e)
    {
        //print("1");
        if (e is KeyPressEvent)
        {
            //print("2");
            Action<string> invokable = print;
            switch ((e as KeyPressEvent).code)
            {
                case KeyCode.W: invokable("w");break;
                case KeyCode.A: invokable("a"); break;
                case KeyCode.S: invokable("s"); break;
                case KeyCode.D: invokable("d"); break;
                case KeyCode.Space: invokable("space"); break;
            }


        }
    }



    void InputHandler(GameEventBase e)
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKey(kcode))
                EventManager.TriggerEvent(new KeyPressEvent(kcode));
    }
}
