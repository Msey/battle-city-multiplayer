using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelsManager : PersistentSingleton<LevelsManager>
{
    [SerializeField]
    protected string mainMenuSceneName = "Scenes/Menu/MainMenu";

    [SerializeField]
    protected string classicGameSceneName = "Scenes/Game/Classic";

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
}
