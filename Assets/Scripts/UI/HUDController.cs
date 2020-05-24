using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;
using static GameConstants;

public class HUDController : MonoBehaviour
{
    public GameObject notStartedCanvas;
    public GameObject inGameCanvas;
    public GameObject finishedCanvas;

    public Text currentStageHUDText;
    public Text stageStartText;
    public GameObject gameOverText;
    public GameObject pauseText;
    public LayoutGroup tanksCountLayout;
    public GameObject tanksPrefab;
    public Curtain curtain;
    private List<GameObject> tanksIcons;
    private int enemyTanksCount = 0;

    public LayoutGroup livesPanelLayout;
    public GameObject livesPanelPrefab;
    private List<GameObject> livesPanels;
    public FinishedCanvasController finishedCanvasController;

    protected void Awake()
    {
        Assert.IsNotNull(notStartedCanvas);
        Assert.IsNotNull(inGameCanvas);
        Assert.IsNotNull(finishedCanvas);

        Assert.IsNotNull(currentStageHUDText);
        Assert.IsNotNull(stageStartText);
        Assert.IsNotNull(gameOverText);
        Assert.IsNotNull(pauseText);

        Assert.IsNotNull(curtain);

        tanksIcons = new List<GameObject>();
        Assert.IsNotNull(tanksCountLayout);
        Assert.IsNotNull(tanksPrefab);

        livesPanels = new List<GameObject>();
        Assert.IsNotNull(livesPanelLayout);
        Assert.IsNotNull(livesPanelPrefab);

        Assert.IsNotNull(finishedCanvasController);

        SetStage(LevelsManager.s_Instance.CurrentGameInfo.CurrentStage);
        ClassicGameManager.s_Instance.GameStateChanged += OnGameStateChanged;
        ClassicGameManager.s_Instance.IsPausedChanged += OnPauseChanged;
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
        ClassicGameManager.s_Instance.IsPausedChanged -= OnPauseChanged;
        EnemyTank.TankCreated -= OnEnemyTankCreated;
    }

    public void OnBackToMenuClicked()
    {
        LevelsManager.s_Instance.OpenMainMenu();
    }

    public void OnGameStateChanged(object sender, EventArgs e)
    {
        GameState newGameState = ClassicGameManager.s_Instance.GameState;

        switch (newGameState)
        {
            case GameState.Started:
            {
                curtain.Open();
                LoadGameInfo();
                break;
            }
            case GameState.PreFinished:
            {
                if (ClassicGameManager.s_Instance.IsEagleDestroyed)
                    ShowGameOverLabel();
                break;
            }
            case GameState.Finished:
            {
                StartCoroutine(FinishGameCoroutine());
                break;
            }
        }
        UpdateUIElementsVisibility();
    }

    public void OnPauseChanged(object sender, EventArgs e)
    {
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
        GameState gameState = ClassicGameManager.s_Instance.GameState;

        notStartedCanvas.SetActive(gameState == GameState.NotStarted || gameState == GameState.Loading);
        inGameCanvas.SetActive(gameState == GameState.Started || gameState == GameState.PreFinished);
        finishedCanvas.SetActive(gameState == GameState.Finished);
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
        finishedCanvasController.SetStage(stage + 1);
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
            case GameState.NotStarted:
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
                        ClassicGameManager.s_Instance.LoadGame();
                    }
                }
                break;
            }
            case GameState.Started:
            {
                if (InputPlayerManager.s_Instance.AnyPlayerActionWasPressed(InputPlayerManager.ActionType.Start))
                {
                    ClassicGameManager.s_Instance.PauseGame();
                }
                break;
            }
        }
    }

    private void ShowGameOverLabel()
    {
        gameOverText.SetActive(true);
        Animator gameOverAnimator = gameOverText.GetComponent<Animator>();
        if (gameOverAnimator)
            gameOverAnimator.SetTrigger("Show");
    }

    private IEnumerator FinishGameCoroutine()
    {
        GameInfo currentGameInfo = LevelsManager.s_Instance.CurrentGameInfo;
        finishedCanvasController.SetPlayerCount(currentGameInfo.PlayersCount);
        PlayerKillsStatistic[] playersStatistic = ClassicGameManager.s_Instance.PlayersStatistic;

        foreach (EnemyTankType tankType in Enum.GetValues(typeof(EnemyTankType)))
        {
            int tankKilledCount = 0;
            while (true)
            {
                bool anyTankUpScore = false;
                for (int playerIndex = 0; playerIndex < MAX_PLAYERS; ++playerIndex)
                {
                    PlayerKillsStatistic playerKillsStatistic = playersStatistic[playerIndex];
                    int killedCount = 0;
                    playerKillsStatistic.tanks.TryGetValue(tankType, out killedCount);
                    if (killedCount > tankKilledCount)
                    {
                        anyTankUpScore = true;
                        finishedCanvasController.ScoreColumn(playerIndex).SetTankCount(tankType, tankKilledCount + 1);
                    }
                }
                if (!anyTankUpScore)
                    break;

                tankKilledCount++;
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.ScoreUp);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(3.0f);
        if (currentGameInfo.IsGameOver)
        {
            LevelsManager.s_Instance.OpenMainMenu();
        }
        else
        {
            currentGameInfo.IsFirstGame = false;
            currentGameInfo.CurrentStage++;
            LevelsManager.s_Instance.OpenClassicGame();
        }
    }
}
