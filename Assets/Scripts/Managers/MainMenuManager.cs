using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public GameObject mainMenuCanvas;
    public GameObject settingCanvas;

    override protected void Awake()
    {
        base.Awake();
        Assert.IsNotNull(mainMenuCanvas);
        Assert.IsNotNull(settingCanvas);
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

    public void OpenSettings()
    {
        HideAllCanvases();
        settingCanvas.SetActive(true);
    }

    public void OpenMainMenu()
    {
        HideAllCanvases();
        mainMenuCanvas.SetActive(true);
    }

    private void HideAllCanvases()
    {
        settingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
    }
}
