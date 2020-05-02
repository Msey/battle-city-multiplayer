using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class HUDController : MonoBehaviour
{
    public GameObject notStartedCanvas;
    public GameObject inGameCanvas;
    public GameObject finishedCanvas;

    public Text currentStageHUDText;
    public Text stageStartText;
    public GameObject pauseText;
    public LayoutGroup tanksCountLayout;
    public GameObject tanksPrefab;
    public Curtain curtain;
    private List<GameObject> tanksIcons;
    private int enemyTanksCount = 0;

    public LayoutGroup livesPanelLayout;
    public GameObject livesPanelPrefab;
    private List<GameObject> livesPanels;

    protected void Awake()
    {
        Assert.IsNotNull(notStartedCanvas);
        Assert.IsNotNull(inGameCanvas);
        Assert.IsNotNull(finishedCanvas);

        Assert.IsNotNull(currentStageHUDText);
        Assert.IsNotNull(stageStartText);
        Assert.IsNotNull(pauseText);

        Assert.IsNotNull(curtain);

        tanksIcons = new List<GameObject>();
        Assert.IsNotNull(tanksCountLayout);
        Assert.IsNotNull(tanksPrefab);

        livesPanels = new List<GameObject>();
        Assert.IsNotNull(livesPanelLayout);
        Assert.IsNotNull(livesPanelPrefab);

        SetStage(LevelsManager.s_Instance.CurrentGameInfo.CurrentStage);
        ClassicGameManager.s_Instance.GameStateChanged += OnGameStateChanged;
        ClassicGameManager.s_Instance.IsPausedChanged += OnGameStateChanged;
        EnemyTank.TankCreated += OnEnemyTankCreated;
        UpdateUIElementsVisibility();
    }

    private void Update()
    {
        HandleInput();
    }

    private void OnDestroy()
    {
        ClassicGameManager.s_Instance.GameStateChanged -= OnGameStateChanged;
        ClassicGameManager.s_Instance.IsPausedChanged -= OnGameStateChanged;
        EnemyTank.TankCreated -= OnEnemyTankCreated;
    }

    public void OnBackToMenuClicked()
    {
        LevelsManager.s_Instance.OpenMainMenu();
    }

    public void OnGameStateChanged(object sender, EventArgs e)
    {
        GameConstants.GameState newGameState = ClassicGameManager.s_Instance.GameState;

        if (newGameState == GameConstants.GameState.Started)
        {
            curtain.Open();
            LoadGameInfo();
        }
        UpdateUIElementsVisibility();
    }

    private void LoadGameInfo()
    {
        if (ClassicGameManager.s_Instance != null)
            SetEnemyTanksCount(ClassicGameManager.s_Instance.LevelEnemeyTanksCount);
        else
            SetEnemyTanksCount(0);

        SetLivesPanelCount(LevelsManager.s_Instance.CurrentGameInfo.PlayersCount);
    }

    private void UpdateUIElementsVisibility()
    {
        GameConstants.GameState gameState = ClassicGameManager.s_Instance.GameState;

        notStartedCanvas.SetActive(gameState == GameConstants.GameState.NotStarted);
        inGameCanvas.SetActive(gameState == GameConstants.GameState.Started);
        finishedCanvas.SetActive(gameState == GameConstants.GameState.Finished);
        pauseText.SetActive(ClassicGameManager.s_Instance.IsPaused);
    }
    private void OnEnemyTankCreated(object sender, EventArgs e)
    {
        SetEnemyTanksCount(enemyTanksCount - 1);
    }

    public void SetStage(int stage)
    {
        currentStageHUDText.text = (stage + 1).ToString();
        stageStartText.text = String.Format("STAGE {0}", stage + 1);
    }

    public void SetEnemyTanksCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        enemyTanksCount = count;

        while (tanksIcons.Count < count)
        {
            GameObject tankIcon = Instantiate(tanksPrefab);
            tanksIcons.Add(tankIcon);
            tankIcon.transform.SetParent(tanksCountLayout.transform, false);
        }
        while (tanksIcons.Count > count)
        {
            GameObject tankIcon = tanksIcons[0];
            Destroy(tankIcon);
            tanksIcons.RemoveAt(0);
        }
    }

    public void SetLivesPanelCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        while (livesPanels.Count < count)
        {
            GameObject livesPanelObject = Instantiate(livesPanelPrefab);
            livesPanels.Add(livesPanelObject);
            livesPanelObject.transform.SetParent(livesPanelLayout.transform, false);

            var livesPanel = livesPanelObject.GetComponent<LivesPanel>();
            if (!Utils.Verify(livesPanel))
                continue;
            livesPanel.SetPlayerNumber(livesPanels.Count);
        }
        while (livesPanels.Count > count)
        {
            GameObject livesPanel = livesPanels[0];
            Destroy(livesPanel);
            livesPanels.RemoveAt(0);
        }
    }

    public void SetPlayerLives(int player, int lives)
    {
        if (!Utils.InRange(0, player, livesPanels.Count))
            return;

        GameObject livesPanelObject = livesPanels[player];
        if (!Utils.Verify(livesPanelObject))
            return;

        var livesPanel = livesPanelObject.GetComponent<LivesPanel>();
        if (!Utils.Verify(livesPanel))
            return;

        livesPanel.SetLives(lives);
    }

    void HandleInput()
    {
        GameInfo currentGameInfo = LevelsManager.s_Instance.CurrentGameInfo;

        switch (ClassicGameManager.s_Instance.GameState)
        {
            case GameConstants.GameState.NotStarted:
            {
                if (currentGameInfo.IsFirstGame)
                {
                    if (InputPlayerManager.s_Instance.AnyPlayerActionWasPressed(InputPlayerManager.ActionType.Down))
                    {
                        currentGameInfo.CurrentStage--;
                        SetStage(currentGameInfo.CurrentStage);
                    }
                    else if (InputPlayerManager.s_Instance.AnyPlayerActionWasPressed(InputPlayerManager.ActionType.Up))
                    {
                        currentGameInfo.CurrentStage++;
                        SetStage(currentGameInfo.CurrentStage);
                    }
                    else if (InputPlayerManager.s_Instance.AnyPlayerActionWasPressed(InputPlayerManager.ActionType.Start))
                    {
                        ClassicGameManager.s_Instance.StartGame();
                    }
                }
                break;
            }
        }
    }
}
