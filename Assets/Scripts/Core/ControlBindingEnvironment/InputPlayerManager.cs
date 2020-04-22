﻿using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputPlayerManager : PersistentSingleton<InputPlayerManager>
{

    public int maxPlayers = 4;

    public bool IsBindingListening { get; }

    List<InputPlayer> players;


    void Start()
    {
        players = new List<InputPlayer>(maxPlayers);

        for (int i = 0; i < maxPlayers; i++)
        {
            var player = new InputPlayer();

            player.PlayerActionSet = (i == 0)
                ? InputPlayerActions.CreateWithKeyboardBindings()
                : InputPlayerActions.CreateWithEmptyBindings();

            players.Add(player);
        }

        LoadBindings();

        WireTankEvents();
    }

    public void SetPlayers(int amount)
    {
        for (int i = amount; i < maxPlayers; i++)
        {
            players[i].EnabledController = true;
        }
    }

    private void Update()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null && players[i].Tank != null)
            {
                players[i].Update();
                //print("i = "+i+" "+(int)players[i].GetStandartDirection());
            }
        }
    }


    public string GetPlayerActionCode(int playerNumber, ActionType actionType)
    {

        var player = players[playerNumber];

        switch (actionType)
        {
            case ActionType.FireA:
                return player.PlayerActionSet.FireA.Bindings[0].Name;

            case ActionType.Fire:
                return player.PlayerActionSet.Fire.Bindings[0].Name;

            case ActionType.Left:
                return player.PlayerActionSet.Left.Bindings[0].Name;

            case ActionType.Right:
                return player.PlayerActionSet.Right.Bindings[0].Name;

            case ActionType.Up:
                return player.PlayerActionSet.Up.Bindings[0].Name;

            case ActionType.Down:
                return player.PlayerActionSet.Down.Bindings[0].Name;
        }

        return null;
    }

    public void BindPlayerKeyCode(int playerNumber, ActionType actionType)
    {
        var player = players[playerNumber - 1];

        switch (actionType)
        {
            case ActionType.FireA:
                player.PlayerActionSet.FireA.ResetBindings();
                player.PlayerActionSet.FireA.ListenForBindingReplacing(player.PlayerActionSet.FireA.Bindings[0]);
                break;
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
        FireA,
        Fire,
        Left,
        Right,
        Up,
        Down
    }

    private void OnApplicationQuit()
    {
        SaveBindings();
    }

    string saveData = string.Empty;

    void SaveBindings()
    {
        for (int i = 0; i < players.Count; i++)
        {
            saveData = players[i].PlayerActionSet.Save();
            PlayerPrefs.SetString("BindingsPlayer_" + i, saveData);
        }
    }


    void LoadBindings()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (PlayerPrefs.HasKey("BindingsPlayer_" + i))
            {
                saveData = PlayerPrefs.GetString("BindingsPlayer_" + i);
                players[i].PlayerActionSet.Load(saveData);
            }
        }
    }

    private void WireTankEvents()
    {
        PlayerTank.TankCreated += OnPlayerTankCreated;
        PlayerTank.TankDestroyed += OnPlayerTankDestroyed;
    }

    void OnPlayerTankCreated(object sender, EventArgs e)
    {
        var lastCreatedTank = (PlayerTank)sender;

        if (Utils.InRange(0, lastCreatedTank.PlayerIndex, GameConstants.PlayerTanksCount))
        {
            players[lastCreatedTank.PlayerIndex].Tank = lastCreatedTank;
        }
    }

    void OnPlayerTankDestroyed(object sender, EventArgs e)
    {
        var lastCreatedTank = (PlayerTank)sender;

        if (Utils.InRange(0, lastCreatedTank.PlayerIndex, GameConstants.PlayerTanksCount))
        {
            players[lastCreatedTank.PlayerIndex].Tank = null;
        }
    }
}