using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;

public class InputPlayerManager : PersistentSingleton<InputPlayerManager>
{

    public const int maxPlayers = 4;

    public bool IsBindingListening { get; }

    InputPlayer[] players;

    void Start()
    {
        players = new InputPlayer[maxPlayers];

        for (int i = 0; i < players.Length; i++)
        {
            var player = new InputPlayer();

            player.PlayerActionSet = (i == 0)
                ? InputPlayerActions.CreateWithKeyboardBindings()
                : InputPlayerActions.CreateWithEmptyBindings();

            players[i] = player;
        }

        LoadBindings();

        WireTankEvents();
    }

    public Dictionary<string, Text>[] components;
    public void AssignButtonTextComponents(Dictionary<string, Text>[] components)
    {
        this.components = components;
        for (int i = 0; i < players.Length; i++)
        {
            int cached_index = i;
            players[i].EnabledController = true;

            players[i].PlayerActionSet.ListenOptions.OnBindingAdded += (action, binding) =>
            {
                var aName = action.Name;
                var component = this.components[cached_index][aName];
                component.text = $"{aName}: {binding.Name}";
                //print("Binding added... " + binding.DeviceName + ": " + binding.Name);

                OnBindingAdded?.Invoke(cached_index, EventArgs.Empty);
            };
        }
    }

    public static event EventHandler OnBindingAdded;

    bool pauseDelayed;
    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].Tank != null)
            {
                players[i].Update();

                if (players[i].PlayerActionSet.Start.IsPressed && !pauseDelayed)
                {
                    pauseDelayed = true;
                    ClassicGameManager.s_Instance.PauseGame();
                    Debug.Log($"paused = {ClassicGameManager.s_Instance.IsPaused}");
                    StartCoroutine(ReturnPauseAvaliability());
                }
            }
        }
    }
    private IEnumerator ReturnPauseAvaliability()
    {
        yield return new WaitForSecondsRealtime(1f);
        pauseDelayed = false;
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

            case ActionType.Start:
                return player.PlayerActionSet.Start.Bindings[0].Name;
        }

        return null;
    }

    public void BindPlayerKeyCode(int playerIndex, ActionType actionType)
    {
        var player = players[playerIndex];

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
            case ActionType.Start:
                player.PlayerActionSet.Start.ResetBindings();
                player.PlayerActionSet.Start.ListenForBindingReplacing(player.PlayerActionSet.Start.Bindings[0]);
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
        Down,
        Start
    }

    private void OnApplicationQuit()
    {
        SaveBindings();
    }

    string saveData = string.Empty;

    void SaveBindings()
    {
        for (int i = 0; i < players.Length; i++)
        {
            saveData = players[i].PlayerActionSet.Save();
            PlayerPrefs.SetString("binding_control_" + i, saveData);
        }
    }


    void LoadBindings()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (PlayerPrefs.HasKey("binding_control_" + i))
            {
                saveData = PlayerPrefs.GetString("binding_control_" + i);
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
        var lastDestroyedTank = (PlayerTank)sender;

        if (Utils.InRange(0, lastDestroyedTank.PlayerIndex, GameConstants.PlayerTanksCount))
        {
            players[lastDestroyedTank.PlayerIndex].Tank = null;
        }
    }
}
