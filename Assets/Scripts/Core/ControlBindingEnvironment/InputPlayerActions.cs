using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayerActions : PlayerActionSet
{

    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Direction;
    public PlayerAction Fire;
    public PlayerAction FireA;

    public InputPlayerActions()
    {
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Fire = CreatePlayerAction("Fire");
        FireA = CreatePlayerAction("FireA");
        Direction = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }

    public static InputPlayerActions CreateWithKeyboardBindings()
    {
        var actions = GetDefaultBindingSettings();

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        actions.Up.AddDefaultBinding(Key.W);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Right.AddDefaultBinding(Key.D);

        actions.Fire.AddDefaultBinding(Key.Space);
        actions.FireA.AddDefaultBinding(Key.LeftControl);

        return actions;
    }

    public static InputPlayerActions CreateWithEmptyBindings()
    {
        var actions = GetDefaultBindingSettings();

        actions.Up.AddDefaultBinding(Key.None);
        actions.Down.AddDefaultBinding(Key.None);
        actions.Left.AddDefaultBinding(Key.None);
        actions.Right.AddDefaultBinding(Key.None);

        actions.Up.AddDefaultBinding(Key.None);
        actions.Down.AddDefaultBinding(Key.None);
        actions.Left.AddDefaultBinding(Key.None);
        actions.Right.AddDefaultBinding(Key.None);

        actions.Fire.AddDefaultBinding(Key.None);
        actions.FireA.AddDefaultBinding(Key.None);

        return actions;
    }


    private static InputPlayerActions GetDefaultBindingSettings()
    {
        var actions = new InputPlayerActions();

        actions.ListenOptions.IncludeUnknownControllers = true;

        actions.ListenOptions.MaxAllowedBindings = 1;

        actions.ListenOptions.OnBindingAdded += (action, binding) =>
           Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);

        return actions;
    }
}
