using UnityEngine;
using UnityEngine.Assertions;
using InControl;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField]
    private GameObject _mainMenuCanvas;
    [SerializeField]
    private GameObject _settingCanvas;
    [SerializeField]
    private SelectLevelPanel _selectLevelPanel;

    override protected void Awake()
    {
        base.Awake();
        Assert.IsNotNull(_mainMenuCanvas);
        Assert.IsNotNull(_settingCanvas);
        Assert.IsNotNull(_selectLevelPanel);
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

    public void OnStartGameClicked()
    {
        OpenSelectLevelPanel();
    }

    public void OpenSettings()
    {
        HideAllCanvases();
        _settingCanvas.SetActive(true);
    }

    public void OpenMainMenu()
    {
        HideAllCanvases();
        _mainMenuCanvas.SetActive(true);
    }

    public void OpenSelectLevelPanel()
    {
        HideAllCanvases();
        _selectLevelPanel.Open();
    }

    private void HideAllCanvases()
    {
        _settingCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _selectLevelPanel.Close();
        TouchManager.Instance.controlsEnabled = false;
    }
}
