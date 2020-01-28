using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TankPlayerManager : MonoBehaviour
{

    public int maxPlayers = 4;

    public bool IsBindingListening { get; }

    List<TankPlayer> players;


    void Start()
    {
        players = new List<TankPlayer>(maxPlayers);

        for (int i = 0; i < maxPlayers; i++)
        {
            var player = new TankPlayer();

            player.PlayerActionSet = (i == 0)
                ? TankPlayerActions.CreateWithKeyboardBindings()
                : TankPlayerActions.CreateWithEmptyBindings();

            players.Add(player);
        }
    }

    bool setkey = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) setkey = true;

        print(setkey);

        if (players[0] != null && players[0].PlayerActionSet != null)
        {
            print("player(" + 0 + ")X: " + players[0].PlayerActionSet.Direction.X + "| Y: " + 
                players[0].PlayerActionSet.Direction.Y + "| Fire: " + 
                (players[0].PlayerActionSet.Fire.IsPressed ? "Y" : "N"));
        }
    }

    public void SetPlayers(int amount)
    {
        for (int i = amount; i < maxPlayers; i++)
        {
            players[i].EnabledController = true;
        }
    }

    public void BindPlayerKeyCode(int playerNumber, ActionType actionType)
    {
        var player = players[playerNumber - 1];

        switch (actionType)
        {
            case ActionType.Fire:
                player.PlayerActionSet.Fire.ResetBindings();
                player.PlayerActionSet.Fire.ListenForBindingReplacing(player.PlayerActionSet.Fire.Bindings[0]);
                break;
            case ActionType.Left:
                player.PlayerActionSet.Left.ResetBindings();
                player.PlayerActionSet.Left.ListenForBindingReplacing(player.PlayerActionSet.Left.Bindings[0]);
                break;
            case ActionType.Right:
                player.PlayerActionSet.Right.ResetBindings();
                player.PlayerActionSet.Right.ListenForBindingReplacing(player.PlayerActionSet.Right.Bindings[0]);
                break;
            case ActionType.Up:
                player.PlayerActionSet.Up.ResetBindings();
                player.PlayerActionSet.Up.ListenForBindingReplacing(player.PlayerActionSet.Up.Bindings[0]);
                break;
            case ActionType.Down:
                player.PlayerActionSet.Down.ResetBindings();
                player.PlayerActionSet.Down.ListenForBindingReplacing(player.PlayerActionSet.Down.Bindings[0]);
                break;
        }
    }

    public enum ActionType
    {
        Fire,
        Left,
        Right,
        Up,
        Down
    }
}
