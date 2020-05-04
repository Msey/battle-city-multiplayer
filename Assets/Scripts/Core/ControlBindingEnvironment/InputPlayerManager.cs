using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using static GameConstants;

public class InputPlayerManager : PersistentSingleton<InputPlayerManager>
{
    public bool IsBindingListening;
    public Dictionary<string, Text>[] textComponents;

    public static event EventHandler OnKeyBindingAdded;

    private InputPlayer[] inputPlayers;

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
        for (int playerIndex = 0; playerIndex < inputPlayers.Length; playerIndex++)
        {
            var input = inputPlayers[playerIndex];
            if (input != null)
            {
                if (input.Tank != null)
                    input.Update(ClassicGameManager.s_Instance.CanUserControlsTanks);
                else if ((input.PlayerActionSet.Fire.WasPressed || input.PlayerActionSet.FireA.WasPressed) 
                    && ClassicGameManager.s_Instance.GameState == GameState.Started)
                    ClassicGameManager.s_Instance.RespawnPlayer(playerIndex);
            }
        }
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
        PlayerAction playerAction = player.PlayerActionSet.PlayerAction(actionType);
        if (playerAction == null)
            return;

        playerAction.ResetBindings();
        playerAction.ListenForBindingReplacing(playerAction.Bindings[0]);
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

    public bool AnyPlayerActionWasPressed(ActionType action)
    {
        for (int playerIndex = 0; playerIndex < inputPlayers.Length; playerIndex++)
        {
            var player = inputPlayers[playerIndex];
            PlayerAction playerAction = player.PlayerActionSet.PlayerAction(action);
            if (playerAction.WasPressed)
                return true;
        }
        return false;
    }

    public bool AnyPlayerActionIsPressed(ActionType action)
    {
        for (int playerIndex = 0; playerIndex < inputPlayers.Length; playerIndex++)
        {
            var player = inputPlayers[playerIndex];
            PlayerAction playerAction = player.PlayerActionSet.PlayerAction(action);
            if (playerAction.IsPressed)
                return true;
        }
        return false;
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

        if (Utils.InRange(0, lastCreatedTank.PlayerIndex, GameConstants.MAX_PLAYERS))
        {
            inputPlayers[lastCreatedTank.PlayerIndex].Tank = lastCreatedTank;
        }
    }

    void OnPlayerTankDestroyed(object sender, EventArgs e)
    {
        var lastDestroyedTank = (PlayerTank)sender;

        if (Utils.InRange(0, lastDestroyedTank.PlayerIndex, GameConstants.MAX_PLAYERS))
        {
            inputPlayers[lastDestroyedTank.PlayerIndex].Tank = null;
        }
    }
}
