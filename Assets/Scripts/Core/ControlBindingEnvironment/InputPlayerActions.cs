﻿using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayerActions : PlayerActionSet
{

    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Fire;
    public PlayerAction FireA;
    public PlayerAction Start;
    public PlayerTwoAxisAction Direction;

    public InputPlayerActions()
    {
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Fire = CreatePlayerAction("Fire");
        FireA = CreatePlayerAction("FireA");
        Start = CreatePlayerAction("Start");
        Direction = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }

    public static InputPlayerActions CreateWithKeyboardBindings()
    {
        var actions = GetDefaultBindingSettings();

        actions.Up.AddDefaultBinding(Key.W);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Right.AddDefaultBinding(Key.D);

        actions.Fire.AddDefaultBinding(Key.Space);
        actions.FireA.AddDefaultBinding(Key.LeftControl);
        actions.Start.AddDefaultBinding(Key.Q);

        return actions;
    }

    public static InputPlayerActions CreateWithEmptyBindings()
    {
        var actions = GetDefaultBindingSettings();

        actions.Up.AddDefaultBinding(Key.None);
        actions.Down.AddDefaultBinding(Key.None);
        actions.Left.AddDefaultBinding(Key.None);
        actions.Right.AddDefaultBinding(Key.None);

        actions.Fire.AddDefaultBinding(Key.None);
        actions.FireA.AddDefaultBinding(Key.None);

        actions.Start.AddDefaultBinding(Key.None);

        return actions;
    }


    private static InputPlayerActions GetDefaultBindingSettings()
    {
        var actions = new InputPlayerActions();

        actions.ListenOptions.IncludeUnknownControllers = true;

        actions.ListenOptions.MaxAllowedBindings = 1;

        return actions;
    }
}
