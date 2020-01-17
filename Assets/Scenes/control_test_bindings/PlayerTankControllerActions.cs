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
        playerActions.ListenOptions.MaxAllowedBindings = 1;
        playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;

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
}
