using InControl;
using UnityEngine.Assertions;

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

    public PlayerAction PlayerAction(InputPlayerManager.ActionType action)
    {
        switch (action)
        {
            case InputPlayerManager.ActionType.FireA:
                return FireA;
            case InputPlayerManager.ActionType.Fire:
                return Fire;
            case InputPlayerManager.ActionType.Left:
                return Left;
            case InputPlayerManager.ActionType.Right:
                return Right;
            case InputPlayerManager.ActionType.Up:
                return Up;
            case InputPlayerManager.ActionType.Down:
                return Down;
            case InputPlayerManager.ActionType.Start:
                return Start;
        }

        Assert.IsTrue(false);
        return null;
    }

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
        actions.Start.AddDefaultBinding(Key.PadEnter);

        return actions;
    }

    public static InputPlayerActions CreateWithGamepadBindings()
    {
        var actions = GetDefaultBindingSettings();

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        actions.Fire.AddDefaultBinding(InputControlType.Action2);
        actions.FireA.AddDefaultBinding(InputControlType.Action1);
        //actions.Start.AddDefaultBinding(InputControlType.Start);
        actions.Start.AddDefaultBinding(Key.Space);

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
