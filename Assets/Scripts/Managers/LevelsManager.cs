﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelsManager : PersistentSingleton<LevelsManager>
{
    [SerializeField]
    protected string mainMenuSceneName = "Scenes/Menu/MainMenu";

    [SerializeField]
    protected string classicGameSceneName = "Scenes/Game/Classic";
    public GameInfo CurrentGameInfo { get; set; } = new GameInfo();
    override protected void Awake()
    {
        base.Awake();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OpenClassicGame()
    {
        SceneManager.LoadScene(classicGameSceneName);
    }

    public void StartGame()
    {
        if (CurrentGameInfo.GameMode == GameInfo.EGameMode.classic)
        {
            if (!Utils.Verify(CurrentGameInfo.PlayerCount > 0))
                return;

            SceneManager.LoadScene(classicGameSceneName);
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
}
