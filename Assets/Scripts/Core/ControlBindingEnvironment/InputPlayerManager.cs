using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;

public class InputPlayerManager : PersistentSingleton<InputPlayerManager>
{
    private const int MAX_PLAYERS = 4;
    private const int PAUSE_AVALIABILITY_TIME = 1;

    public bool IsBindingListening;
    public Dictionary<string, Text>[] textComponents;

    public static event EventHandler OnKeyBindingAdded;

    private InputPlayer[] inputPlayers;
    private bool pauseDelayed;

    void Start()
    {
        inputPlayers = new InputPlayer[MAX_PLAYERS];

        for (int i = 0; i < inputPlayers.Length; i++)
        {
            var inputPlayer = new InputPlayer();

            inputPlayer.PlayerActionSet = (i == 0)
                ? InputPlayerActions.CreateWithKeyboardBindings()
                : InputPlayerActions.CreateWithEmptyBindings();

            inputPlayers[i] = inputPlayer;
        }

        LoadBindings();
        WireTankEvents();
    }

    public void AssignButtonTextComponents(Dictionary<string, Text>[] textComponents)
    {
        this.textComponents = textComponents;
        for (int i = 0; i < inputPlayers.Length; i++)
        {
            int cached_index = i;
            inputPlayers[i].EnabledController = true;

            inputPlayers[i].PlayerActionSet.ListenOptions.OnBindingAdded += (action, binding) =>
            {
                var aName = action.Name;
                var component = this.textComponents[cached_index][aName];
                component.text = $"{aName}: {binding.Name}";
                OnKeyBindingAdded?.Invoke(cached_index, EventArgs.Empty);
            };
        }
    }

    private void Update()
    {
        for (int i = 0; i < inputPlayers.Length; i++)
        {
            if (inputPlayers[i] != null && inputPlayers[i].Tank != null)
            {
                if (inputPlayers[i].PlayerActionSet.Start.IsPressed && !pauseDelayed)
                {
                    pauseDelayed = true;
                    ClassicGameManager.s_Instance.PauseGame();
                    StartCoroutine(ReturnPauseAvaliability());
                }

                if (!ClassicGameManager.s_Instance.IsPaused)
                    inputPlayers[i].Update();
            }
        }
    }
    private IEnumerator ReturnPauseAvaliability()
    {
        yield return new WaitForSecondsRealtime(PAUSE_AVALIABILITY_TIME);
        pauseDelayed = false;
    }


    public string GetPlayerActionCode(int playerNumber, ActionType actionType)
    {
        var player = inputPlayers[playerNumber];

        switch (actionType)
        {
            case ActionType.FireA: return player.PlayerActionSet.FireA.Bindings[0].Name;
            case ActionType.Fire: return player.PlayerActionSet.Fire.Bindings[0].Name;
            case ActionType.Left: return player.PlayerActionSet.Left.Bindings[0].Name;
            case ActionType.Right: return player.PlayerActionSet.Right.Bindings[0].Name;
            case ActionType.Up: return player.PlayerActionSet.Up.Bindings[0].Name;
            case ActionType.Down: return player.PlayerActionSet.Down.Bindings[0].Name;
            case ActionType.Start: return player.PlayerActionSet.Start.Bindings[0].Name;
        }

        return null;
    }

    public void BindPlayerKeyCode(int playerIndex, ActionType actionType)
    {
        var player = inputPlayers[playerIndex];

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
        for (int i = 0; i < inputPlayers.Length; i++)
        {
            saveData = inputPlayers[i].PlayerActionSet.Save();
            PlayerPrefs.SetString("binding_control_" + i, saveData);
        }
    }


    void LoadBindings()
    {
        for (int i = 0; i < inputPlayers.Length; i++)
        {
            if (PlayerPrefs.HasKey("binding_control_" + i))
            {
                saveData = PlayerPrefs.GetString("binding_control_" + i);
                inputPlayers[i].PlayerActionSet.Load(saveData);
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
            inputPlayers[lastCreatedTank.PlayerIndex].Tank = lastCreatedTank;
        }
    }

    void OnPlayerTankDestroyed(object sender, EventArgs e)
    {
        var lastDestroyedTank = (PlayerTank)sender;

        if (Utils.InRange(0, lastDestroyedTank.PlayerIndex, GameConstants.PlayerTanksCount))
        {
            inputPlayers[lastDestroyedTank.PlayerIndex].Tank = null;
        }
    }
}
