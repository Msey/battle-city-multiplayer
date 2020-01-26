using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TankPlayerManager : MonoBehaviour
{

    public int maxPlayers = 4;
    // const int ControlInputCount = (int)InputControlType.Count;

    List<TankPlayer> players;//= new List<TankPlayer>(maxPlayers);


    void Start()
    {
        players = new List<TankPlayer>(maxPlayers);

        for (int i = 0; i < maxPlayers; i++)
        {
            var player = new TankPlayer();
            if (i == 0) { player.PlayerActionSet = TankPlayerActions.CreateWithKeyboardBindings(); print("created player " + i); }
            players.Add(player);
        }
    }

    bool setkey = false;

    void Update()
    {
        //    var inputDevice = InControl.InputManager.ActiveDevice;
        //print(InControl.InputManager.Devices.Count);
        //for (int l = 0; l < InControl.InputManager.Devices.Count; l++)
        //    if(InControl.InputManager.Devices[l].AnyButtonWasPressed)
        //        print("d: "+InControl.InputManager.Devices[l].GetFirstPressedButton());
        if (Input.GetKeyDown(KeyCode.Z)) setkey = true;

        //for (int i = 0; i < maxPlayers; i++)
        //{
        //    print(i);
        print(setkey);

        if (players[0] != null && players[0].PlayerActionSet != null)
        {
            print("player(" + 0 + ")X: " + players[0].PlayerActionSet.Direction.X + "| Y: " + players[0].PlayerActionSet.Direction.Y + "| Fire: " + (players[0].PlayerActionSet.Fire.IsPressed ? "Y" : "N"));

            var actionCount = players[0].PlayerActionSet.Actions.Count;
           // print("actioncount:" + actionCount);
            for (var j = 0; j < actionCount; j++)
            {
                var action = players[0].PlayerActionSet.Actions[j];

                var name = action.Name;
                if (action.IsListeningForBinding)
                {
                    name += " (Listening)";
                }
                //else print(name);
                if (setkey)
                {
                    action.ResetBindings();
                    action.ListenForBindingReplacing(action.Bindings[0]);
                    setkey = false;
                }
            }

        }
        //}
    }


    //bool JoinButtonWasPressedOnListener(TankPlayerActions actions)
    //{
    //    return actions.Up.WasPressed || 
    //        actions.Down.WasPressed ||
    //        actions.Left.WasPressed || 
    //        actions.Right.WasPressed ||
    //        actions.Fire.WasPressed;
    //}

    void RemovePlayer(TankPlayer player)
    {
        players.Remove(player);
        player.PlayerActionSet = null;
      //  Destroy(player.gameObject);
    }

    public void SetPlayers(int amount)
    {
        for (int i = amount; i < maxPlayers; i++)
        {
          //  players[i].enabled = true; // TODO: добавить механизм отключения игроков
        }
    }


    

}
