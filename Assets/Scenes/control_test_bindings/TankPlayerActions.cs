using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerActions : PlayerActionSet
{

    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Direction;
    public PlayerAction Fire;

    public TankPlayerActions()
    {
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Fire = CreatePlayerAction("Fire");
        Direction = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }

    public static TankPlayerActions CreateWithKeyboardBindings()
    {
        var actions = new TankPlayerActions();

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        return actions;
    }


    public static TankPlayerActions CreateWithJoystickBindings()
    {
        var actions = new TankPlayerActions();

        actions.Up.AddDefaultBinding(InputControlType.Action1);
        actions.Down.AddDefaultBinding(InputControlType.Action2);
        actions.Left.AddDefaultBinding(InputControlType.Action3);
        actions.Right.AddDefaultBinding(InputControlType.Action4);
        actions.Fire.AddDefaultBinding(InputControlType.Action5);

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        actions.Fire.AddDefaultBinding(InputControlType.RightStickButton);

        actions.Up.AddDefaultBinding(InputControlType.DPadUp);
        actions.Down.AddDefaultBinding(InputControlType.DPadDown);
        actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        actions.Right.AddDefaultBinding(InputControlType.DPadRight);
        actions.Fire.AddDefaultBinding(InputControlType.DPadX);

        return actions;
    }
}
