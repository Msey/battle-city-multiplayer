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
        Action<string> invokable = print;
        switch (e.code)
        {
           // case KeyCode.W: tank1.UpdateMovement(Ga Direction.Up); break;
           // case KeyCode.A: tank1.UpdateMovement(Direction.Left); break;
           // case KeyCode.S: tank1.UpdateMovement(Direction.Down); break;
           // case KeyCode.D: tank1.UpdateMovement(Direction.Right); break;
           // case KeyCode.Space: tank1.Shoot(); break;
        }
    }

    void InputHandler(KeyBoardEvent e)
    {
        foreach (KeyCode kcode in /*Enum.GetValues(typeof(KeyCode))*/ GameCodes)
            if (Input.GetKey(kcode))
                EventManager.s_Instance.TriggerEvent<KeyPressEvent>(new KeyPressEvent(kcode));
    }

    private IEnumerable<KeyCode> GameCodes
    {
        get { return Codes(); }
    }

    private IEnumerable<KeyCode> Codes()
    {
        yield return KeyCode.W;
        yield return KeyCode.A;
        yield return KeyCode.S;
        yield return KeyCode.D;
        yield return KeyCode.Space;
    }
}
