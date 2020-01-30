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
        var actions = CreateWithEmptyBindings();

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        actions.Up.AddDefaultBinding(Key.W);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Right.AddDefaultBinding(Key.D);

        actions.Fire.AddDefaultBinding(Key.Space);

        return actions;
    }

    public static TankPlayerActions CreateWithEmptyBindings()
    {
        var actions = new TankPlayerActions();

        actions.ListenOptions.IncludeUnknownControllers = true;

        actions.ListenOptions.MaxAllowedBindings = 1;

        actions.ListenOptions.OnBindingAdded += (action, binding) =>
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);

        return actions;

    }
}
