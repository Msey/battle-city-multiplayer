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

    private enum CanvasType
    { 
        MainMenu,
        SelectLevel,
    };
    CanvasType _currentCanvas;

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

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnBackButtonClicked();
        }
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

    public void OnBackButtonClicked()
    {
        switch (_currentCanvas)
        {
            case CanvasType.MainMenu:
                Application.Quit();
                break;
            case CanvasType.SelectLevel:
                OpenMainMenu();
                break;
        }
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
        _currentCanvas = CanvasType.MainMenu;
    }

    public void OpenSelectLevelPanel()
    {
        HideAllCanvases();
        _selectLevelPanel.Open();
        _currentCanvas = CanvasType.SelectLevel;
    }

    private void HideAllCanvases()
    {
        _settingCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _selectLevelPanel.Close();
        TouchManager.Instance.controlsEnabled = false;
    }
}
