using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankControllerActions : PlayerActionSet
{
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;

    public PlayerAction Fire;

    public PlayerTankControllerActions()
    {
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");

        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");

        Fire = CreatePlayerAction("Fire");
    }

    public static PlayerTankControllerActions CreateWithDefaultBindings()
    {
        var playerActions = new PlayerTankControllerActions();


        playerActions.Up.AddDefaultBinding(Key.W);
        playerActions.Down.AddDefaultBinding(Key.S);

        playerActions.Left.AddDefaultBinding(Key.A);
        playerActions.Right.AddDefaultBinding(Key.D);

        playerActions.Fire.AddDefaultBinding(Key.Space);

        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 3;
        playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;

        playerActions.ListenOptions.OnBindingFound = (action, binding) => {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerActions.ListenOptions.OnBindingAdded += (action, binding) => {
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
        };

        return playerActions;
    }


    void BindListen()
    {
        var actionCount = this.Actions.Count;
        for (var i = 0; i < actionCount; i++)
        {
            var action = this.Actions[i];

            var name = action.Name;
            if (action.IsListeningForBinding)
            {
                name += " (Listening)";
            }
            //name += " via " + action.ActiveDevice.Name;
            //name += ", class: " + action.LastDeviceClass;
            //name += ", style: " + action.LastDeviceStyle;

            var bindingCount = action.Bindings.Count;
            for (var j = 0; j < bindingCount; j++)
            {
                var binding = action.Bindings[j];

                    action.ListenForBindingReplacing(binding);

            }
        }
    }


    private void BindInput(PlayerAction action)
    {
        var aCount = action.Bindings.Count;
        for (var i = 0; i < aCount; i++)
        {
            var binding = Up.Bindings[i];

            action.ListenForBindingReplacing(binding);
        }
    }

    public void BindInput(InputType type)
    {
        switch (type)
        {
            case InputType.Up: BindInput(this.Up); break; 
            case InputType.Down: BindInput(this.Down); break; 
            case InputType.Left: BindInput(this.Left); break; 
            case InputType.Right: BindInput(this.Right); break; 
            case InputType.Fire: BindInput(this.Fire); break;
        }
    }


}

public enum InputType
{
    Up,
    Down,
    Left,
    Right,
    Fire
}
