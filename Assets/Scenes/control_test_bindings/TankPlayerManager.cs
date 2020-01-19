using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TankPlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;

    List<TankPlayer> players = new List<TankPlayer>(maxPlayers);

    TankPlayerActions keyboardListener;
    TankPlayerActions joystickListener;


    void OnEnable()
    {
        InControl.InputManager.OnDeviceDetached += OnDeviceDetached;

        keyboardListener = TankPlayerActions.CreateWithKeyboardBindings();
        joystickListener = TankPlayerActions.CreateWithJoystickBindings();
    }


    void OnDisable()
    {
        InControl.InputManager.OnDeviceDetached -= OnDeviceDetached;
        joystickListener.Destroy();
        keyboardListener.Destroy();
    }























    void OnDeviceDetached(InputDevice inputDevice)
    {
        var player = FindPlayerUsingJoystick(inputDevice);
        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    TankPlayer FindPlayerUsingJoystick(InputDevice inputDevice)
    {
        var playerCount = players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions.Device == inputDevice)
            {
                return player;
            }
        }

        return null;
    }

    void RemovePlayer(TankPlayer player)
    {
        //playerPositions.Insert(0, player.transform.position);
        players.Remove(player);
        player.Actions = null;
        Destroy(player.gameObject);
    }

}
