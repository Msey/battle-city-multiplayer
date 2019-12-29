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
        //print(tank1 != null ? "tank exists":"tank doesn't exist");

        EventManager.s_Instance.StartListening<KeyBoardEvent>(InputHandler);
        EventManager.s_Instance.StartListening<KeyPressEvent>(KeyHandler);
    }

    void Update()
    {        
        EventManager.s_Instance.TriggerEvent<KeyBoardEvent>(kbEvent);
    }

    void KeyHandler(KeyPressEvent e)
    {
        //print("2");
        Action<string> invokable = print;
        switch (e.code)
        {
            case KeyCode.W: tank1.MoveUp(); break;
            case KeyCode.A: tank1.MoveLeft(); break;
            case KeyCode.S: tank1.MoveDown(); break;
            case KeyCode.D: tank1.MoveRight(); break;
            case KeyCode.Space: tank1.Shoot(); break;
        }
    }

    void InputHandler(KeyBoardEvent e)
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKey(kcode))
                EventManager.s_Instance.TriggerEvent<KeyPressEvent>(new KeyPressEvent(kcode));
    }
}
