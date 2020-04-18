using System.Collections;
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

    [SerializeField]
    protected string controlsMenuSceneName = "Scenes/Menu/ControlsMenu";

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

    public void OpenControlsMenu()
    {
        SceneManager.LoadScene(controlsMenuSceneName);
    }

    public void StartGame()
    {
        if (CurrentGameInfo.GameMode == GameInfo.EGameMode.Classic)
        {
            if (!Utils.Verify(CurrentGameInfo.PlayersCount > 0))
                return;

            SceneManager.LoadScene(classicGameSceneName);
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
}
