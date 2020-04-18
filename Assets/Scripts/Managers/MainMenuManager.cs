﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    override protected void Awake()
    {
        base.Awake();
    }

    public void OnePlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo.Reset();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 1;
        LevelsManager.s_Instance.StartGame();
    }

    public void TwoPlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo.Reset();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 2;
        LevelsManager.s_Instance.StartGame();
    }
    public void ThreePlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo.Reset();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 3;
        LevelsManager.s_Instance.StartGame();
    }
    public void FourPlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo.Reset();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 4;
        LevelsManager.s_Instance.StartGame();
    }

    public void SettingsMenuClicked()
    {
        LevelsManager.s_Instance.OpenControlsMenu();
    }
}
