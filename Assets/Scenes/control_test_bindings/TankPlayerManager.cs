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


    void Start()
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




    void Update()
    {
            var inputDevice = InControl.InputManager.ActiveDevice;
        print(InControl.InputManager.Devices.Count);
        for (int l = 0; l < InControl.InputManager.Devices.Count; l++)
            print(InControl.InputManager.Devices[l].Name);


        if (JoinButtonWasPressedOnListener(joystickListener))
        {

            if (ThereIsNoPlayerUsingJoystick(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }

        if (JoinButtonWasPressedOnListener(keyboardListener))
        {
            if (ThereIsNoPlayerUsingKeyboard())
            {
                CreatePlayer(null);
            }
        }
    }

    bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
    {
        return FindPlayerUsingJoystick(inputDevice) == null;
    }
    bool ThereIsNoPlayerUsingKeyboard()
    {
        return FindPlayerUsingKeyboard() == null;
    }

    bool JoinButtonWasPressedOnListener(TankPlayerActions actions)
    {
        return actions.Up.WasPressed || 
            actions.Down.WasPressed ||
            actions.Left.WasPressed || 
            actions.Right.WasPressed ||
            actions.Fire.WasPressed;
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
    TankPlayer FindPlayerUsingKeyboard()
    {
        var playerCount = players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions == keyboardListener)
            {
                return player;
            }
        }

        return null;
    }


    TankPlayer CreatePlayer(InputDevice inputDevice)
    {
        if (players.Count < maxPlayers)
        {
            var gameObject = new GameObject("player_0");
            var player = gameObject.UseComponent<TankPlayer>();

            if (inputDevice == null)
                player.Actions = keyboardListener;
            else
            {
                var actions = TankPlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;

                player.Actions = actions;
            }

            players.Add(player);

            return player;
        }

        return null;
    }

    void RemovePlayer(TankPlayer player)
    {
        players.Remove(player);
        player.Actions = null;
        Destroy(player.gameObject);
    }

}
