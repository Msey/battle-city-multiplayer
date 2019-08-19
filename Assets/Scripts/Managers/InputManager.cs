using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerTank tank1;

    KeyBoardEvent kbEvent = new KeyBoardEvent();

    private void Start()
    {
        tank1 = FindObjectOfType<PlayerTank>();
        print(tank1 != null ? "tank exists":"tank doesn't exist");

        EventManager.StartListening(GameEventBase.EventType.KeyBoardEvent, InputHandler);
        EventManager.StartListening(GameEventBase.EventType.KeyPressEvent, KeyHandler);
    }

    void Update()
    {        
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
                case KeyCode.W: tank1.MoveUp(); break;
                case KeyCode.A: tank1.MoveLeft(); break;
                case KeyCode.S: tank1.MoveDown(); break;
                case KeyCode.D: tank1.MoveRight(); break;
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
