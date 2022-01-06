using UnityEngine;
using UnityEngine.Assertions;
using InControl;

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

    private void Start()
    {
        OpenMainMenu();
    }

    public void OnePlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo = new GameInfo();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 1;
        LevelsManager.s_Instance.StartGame();
    }

    public void TwoPlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo = new GameInfo();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 2;
        LevelsManager.s_Instance.StartGame();
    }
    public void ThreePlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo = new GameInfo();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 3;
        LevelsManager.s_Instance.StartGame();
    }
    public void FourPlayerGameClicked()
    {
        LevelsManager.s_Instance.CurrentGameInfo = new GameInfo();
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
        TouchManager.Instance.controlsEnabled = false;
    }
}
