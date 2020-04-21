﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TankPlayerManager : PersistentSingleton<TankPlayerManager>
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

        LoadBindings();

        WireTankEvents();
    }


    void Update()
    {
        //if (players[0] != null && players[0].PlayerActionSet != null)
        //{
        //    print("player(" + 0 + ")X: " + players[0].PlayerActionSet.Direction.X + "| Y: " +
        //        players[0].PlayerActionSet.Direction.Y + "| Fire: " +
        //        (players[0].PlayerActionSet.Fire.IsPressed ? "Y" : "N"));
        //}
    }

    public void SetPlayers(int amount)
    {
        for (int i = amount; i < maxPlayers; i++)
        {
            players[i].EnabledController = true;
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
        EventManager.s_Instance.StartListening<PlayerTankCreatedEvent>(OnPlayerTankCreated);
        EventManager.s_Instance.StartListening<PlayerTankDestroyedEvent>(OnPlayerTankDestroyed);
    }

    void OnPlayerTankCreated(PlayerTankCreatedEvent e)
    {
        if (e == null && e.Tank == null &&
            (e.Tank.PlayerIndex < 0 || e.Tank.PlayerIndex >= GameConstants.playerTanksCount))
            return;

        print(e.Tank.PlayerIndex);
    }

    void OnPlayerTankDestroyed(PlayerTankDestroyedEvent e)
    {
        if (e == null && e.Tank == null &&
            (e.Tank.PlayerIndex < 0 || e.Tank.PlayerIndex >= GameConstants.playerTanksCount))
            return;

        print(e.Tank.PlayerIndex);
    }

    //void StartListeningEvents()
    //{
    //    if (EventManager.s_Instance == null)
    //        return;

    //    EventManager.s_Instance.StartListening<EnemyTankCreatedEvent>(OnEnemyTankCreated);
    //    EventManager.s_Instance.StartListening<EnemyTankDestroyedEvent>(OnEnemyTankDestroyed);
    //    EventManager.s_Instance.StartListening<PlayerTankCreatedEvent>(OnPlayerTankCreated);
    //    EventManager.s_Instance.StartListening<PlayerTankDestroyedEvent>(OnPlayerTankDestroyed);
    //}

    //void StopListeningEvents()
    //{
    //    if (EventManager.s_Instance == null)
    //        return;

    //    EventManager.s_Instance.StopListening<EnemyTankCreatedEvent>(OnEnemyTankCreated);
    //    EventManager.s_Instance.StopListening<EnemyTankDestroyedEvent>(OnEnemyTankDestroyed);
    //    EventManager.s_Instance.StopListening<PlayerTankCreatedEvent>(OnPlayerTankCreated);
    //    EventManager.s_Instance.StopListening<PlayerTankDestroyedEvent>(OnPlayerTankDestroyed);
    //}

    //void OnEnemyTankCreated(EnemyTankCreatedEvent e)
    //{
    //    createdEnemyTanksCount++;
    //    livedEnemyTanksCount++;
    //    enemyTanksOnCreatingCount--;

    //    if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < maxEnemyLivesTanksCount)
    //        GenerateEnemyTank();
    //}

    //void OnEnemyTankDestroyed(EnemyTankDestroyedEvent e)
    //{
    //    livedEnemyTanksCount--;
    //    if (enemiesQueue.Count == 0)
    //        return; //level ended
    //    else if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < maxEnemyLivesTanksCount)
    //        GenerateEnemyTank();
    //}



    //void OnPlayerTankDestroyed(PlayerTankDestroyedEvent e)
    //{
    //    if (e == null)
    //        return;

    //    if (e.Tank == null)
    //        return;

    //    if (e.Tank.PlayerIndex < 0 || e.Tank.PlayerIndex >= GameConstants.playerTanksCount)
    //        return;

    //    playerTankLiving[e.Tank.PlayerIndex] = false;
    //}
}
