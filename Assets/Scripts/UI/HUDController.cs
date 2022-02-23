using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;
using InControl;
using TMPro;
using static GameConstants;

public class HUDController : MonoBehaviour
{
    public GameObject notStartedCanvas;
    public GameObject inGameCanvas;
    public GameObject finishedCanvas;

    [SerializeField]
    private TextMeshProUGUI[] currentStageHUDTexts;
    [SerializeField]
    private TextMeshProUGUI[] enemyTanksCountTexts;
    public TextMeshProUGUI stageStartText;
    public GameObject gameOverText;
    public GameObject pauseText;
    public GameObject tanksPrefab;
    public Curtain curtain;
    private List<GameObject> tanksIcons;
    private int enemyTanksCount = 0;
    public GameObject livesPanelPrefab;
    [SerializeField]
    private GameObject[] livesPanels;
    public FinishedCanvasController finishedCanvasController;

    protected void Awake()
    {
        Assert.IsNotNull(notStartedCanvas);
        Assert.IsNotNull(inGameCanvas);
        Assert.IsNotNull(finishedCanvas);

        Assert.IsNotNull(currentStageHUDTexts);
        Assert.IsNotNull(enemyTanksCountTexts);
        Assert.IsNotNull(stageStartText);
        Assert.IsNotNull(gameOverText);
        Assert.IsNotNull(pauseText);

        Assert.IsNotNull(curtain);

        tanksIcons = new List<GameObject>();
        Assert.IsNotNull(tanksPrefab);
        Assert.IsNotNull(livesPanels);
        Assert.IsNotNull(finishedCanvasController);

        SetStage(LevelsManager.s_Instance.CurrentGameInfo.CurrentStage);
        ClassicGameManager.s_Instance.GameStateChanged += OnGameStateChanged;
        ClassicGameManager.s_Instance.IsPausedChanged += OnPauseChanged;
        ClassicGameManager.s_Instance.TotalLivesChanged += OnPlayerLivesChanged;
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
        ClassicGameManager.s_Instance.TotalLivesChanged -= OnPlayerLivesChanged;
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

    public void OnPlayerLivesChanged(object sender, EventArgs e)
    {
        SetPlayerLives(0, (ClassicGameManager.s_Instance.GetTotalLives()));
    }

    private void LoadGameInfo()
    {
        if (ClassicGameManager.s_Instance != null)
            SetEnemyTanksCount(ClassicGameManager.s_Instance.LevelEnemeyTanksCount);
        else
            SetEnemyTanksCount(0);
        SetPlayerLives(0, (ClassicGameManager.s_Instance.GetTotalLives()));
    }

    private void UpdateUIElementsVisibility()
    {
        GameState gameState = ClassicGameManager.s_Instance.GameState;

        notStartedCanvas.SetActive(gameState == GameState.NotStarted || gameState == GameState.Loading);
        inGameCanvas.SetActive(gameState == GameState.Started || gameState == GameState.PreFinished);
        TouchManager.Instance.controlsEnabled = inGameCanvas.activeSelf;
        finishedCanvas.SetActive(gameState == GameState.Finished);
        pauseText.SetActive(ClassicGameManager.s_Instance.IsPaused);
    }
    private void OnEnemyTankCreated(object sender, EventArgs e)
    {
        SetEnemyTanksCount(enemyTanksCount - 1);
    }

    public void SetStage(int stage)
    {
        foreach (var currentStageHUDText in currentStageHUDTexts)
        {
            currentStageHUDText.text = String.Format("STAGE {0}/{1}", stage + 1, LEVELS_COUNT);
        }
        stageStartText.text = String.Format("STAGE {0}", stage + 1);
        finishedCanvasController.SetStage(stage + 1);
    }

    public void SetEnemyTanksCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        enemyTanksCount = count;

        foreach (var enemyTanksCountText in enemyTanksCountTexts)
        {
            enemyTanksCountText.text = String.Format("ENEMIES {0}/{1}", count, ClassicGameManager.s_Instance.LevelEnemeyTanksCount);
        }
    }

    public void SetPlayerLives(int player, int lives)
    {
        if (player != 0)
            return;

        foreach (GameObject livesPanelObject in livesPanels)
        {
            var livesPanel = livesPanelObject.GetComponent<LivesPanel>();
            if (!Utils.Verify(livesPanel))
                return;

            livesPanel.SetLives(lives);
        }
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
