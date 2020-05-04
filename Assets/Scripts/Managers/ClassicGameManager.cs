using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;
using System.Collections;
using static GameConstants;

[Serializable]
public class ClassicGameLevelInfo
{
    public int basicTanksCount = 0;
    public int fastTanksCount = 0;
    public int powerTanksCount = 0;
    public int armorTanksCount = 0;
    public GameObject levelPrefab = null;
}

public class ClassicGameManager : Singleton<ClassicGameManager>
{
    List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();
    List<SpawnPoint> playerSpawnPoints = new List<SpawnPoint>();
    Queue<EnemyTank.EnemyTankType> enemiesQueue = new Queue<EnemyTank.EnemyTankType>();

    [SerializeField]
    int createdEnemyTanksCount;
    [SerializeField]
    int livedEnemyTanksCount;
    [SerializeField]
    int enemyTanksOnCreatingCount;
    [SerializeField]
    int levelEnemeyTanksCount;

    public int LevelEnemeyTanksCount { get => levelEnemeyTanksCount; }

    public int maxEnemyLivesTanksCount = 4;

    public GameObject enemyTankPrefab;
    public GameObject playerTankPrefab;

    [SerializeField]
    ClassicGameLevelInfo[] levels;

    bool[] playerTankCreating = new bool[MAX_PLAYERS] { false, false, false, false };
    bool[] playerTankLiving = new bool[MAX_PLAYERS] { false, false, false, false };

    [SerializeField]
    GameState gameState = GameState.NotStarted;

    public event EventHandler IsPausedChanged;
    bool isPaused;
    public bool IsPaused
    {
        get => isPaused;
        protected set
        {
            if (isPaused == value)
                return;

            isPaused = value;
            IsPausedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public GameState GameState
    {
        get => gameState;
        protected set
        {
            if (gameState == value)
                return;

            if (gameState != GameState.Started)
                IsPaused = false;

            gameState = value;
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool CanUserControlsTanks
    {
        get => !(isEagleDestroyed && !isAllEnemyTanksDestroyed) && !isPaused; 
    }

    private bool isAllEnemyTanksDestroyed;
    private bool isEagleDestroyed;
    public bool IsEagleDestroyed
    {
        get => isEagleDestroyed;
    }

    override protected void Awake()
    {
        Assert.IsNotNull(enemyTankPrefab);
        Assert.IsNotNull(playerTankPrefab);

        base.Awake();
    }
    private void Start()
    {
        if (!LevelsManager.s_Instance.CurrentGameInfo.IsFirstGame)
            LoadGame();
    }

    private int playerLives = 2;

    public void AddLife() => ++playerLives;
    public void TakeLife() => --playerLives;

    public int GetTotalLives()
    {
        return playerLives;
    }

    protected override void OnDestroy() => StopListeningEvents();

    public event EventHandler GameStateChanged;

    public void LoadGame()
    {
        if (!Utils.Verify(GameState == GameState.NotStarted))
            return;

        GameState = GameState.Loading;
        StartCoroutine(LoadLevelCoroutine());
    }

    private void StartGame()
    {
        if (!Utils.Verify(GameState == GameState.Loading))
            return;

        LoadLevel();
        GameState = GameState.Started;
    }

    void LoadLevel()
    {
        StartListeningEvents();
        LoadLevelObjects();
        CreateEnemyQueue();
        LoadSpawnPoints();
        GenerateEnemyTank();
        CreatePlayerTanks();
    }

    void CreatePlayerTanks()
    {
        int playersCount = LevelsManager.s_Instance.CurrentGameInfo.PlayersCount;
        for (int playerIndex = 0; playerIndex < playersCount; ++playerIndex)
            SpawnPlayerTank(playerIndex);
    }

    public bool PlayerTankIsLiving(int playerIndex)
    {
        if (!Utils.InRange(0, playerIndex, MAX_PLAYERS))
            return false;

        return playerTankLiving[playerIndex];
    }

    private void SpawnPlayerTank(int playerIndex)
    {
        if (!Utils.InRange(0, playerIndex, MAX_PLAYERS)
            || playerTankCreating[playerIndex]
            || playerTankLiving[playerIndex])
            return;


        playerTankCreating[playerIndex] = true;

        SpawnPoint spawnPoint = playerSpawnPoints[playerIndex];
        spawnPoint.Spawn((point) =>
        {
            playerTankCreating[playerIndex] = false;
            playerTankLiving[playerIndex] = true;
            PlayerTank tank = Instantiate(playerTankPrefab, point.position, Quaternion.identity).GetComponent<PlayerTank>();
            tank.PlayerIndex = playerIndex;
        });
    }

    public void RespawnPlayer(int playerIndex)
    {
        if (GetTotalLives() > 0)
        {
            TakeLife();
            SpawnPlayerTank(playerIndex);
        }
    }

    void LoadLevelObjects()
    {
        ClassicGameLevelInfo levelInfo = levels[LevelsManager.s_Instance.CurrentGameInfo.CurrentStage % levels.Length];
        var levelObjects = Instantiate(levelInfo.levelPrefab, Vector3.zero, Quaternion.identity);
        levelObjects.name = "Level";
    }

    void CreateEnemyQueue()
    {
        enemiesQueue.Clear();
        ClassicGameLevelInfo levelInfo = levels[LevelsManager.s_Instance.CurrentGameInfo.CurrentStage % levels.Length];

        for (int basicTank = 0; basicTank < levelInfo.basicTanksCount; ++basicTank)
            enemiesQueue.Enqueue(EnemyTank.EnemyTankType.Basic);

        for (int armorTank = 0; armorTank < levelInfo.armorTanksCount; ++armorTank)
            enemiesQueue.Enqueue(EnemyTank.EnemyTankType.Armor);

        for (int fastTank = 0; fastTank < levelInfo.fastTanksCount; ++fastTank)
            enemiesQueue.Enqueue(EnemyTank.EnemyTankType.Fast);

        for (int powerTank = 0; powerTank < levelInfo.powerTanksCount; ++powerTank)
            enemiesQueue.Enqueue(EnemyTank.EnemyTankType.Power);

        levelEnemeyTanksCount = enemiesQueue.Count;
    }

    void LoadSpawnPoints()
    {
        enemySpawnPoints.Clear();
        playerSpawnPoints.Clear();
        var allSpawnPoints = FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in allSpawnPoints)
        {
            if (spawnPoint.pointType == SpawnPoint.PointType.Enemy)
                enemySpawnPoints.Add(spawnPoint);
            else if (spawnPoint.pointType == SpawnPoint.PointType.Player)
                playerSpawnPoints.Add(spawnPoint);
        }

        Assert.AreEqual(playerSpawnPoints.Count, MAX_PLAYERS);
    }

    void GenerateEnemyTank()
    {
        if (enemiesQueue.Count - enemyTanksOnCreatingCount == 0)
            return;
        enemyTanksOnCreatingCount++;
        int spawnPointIndex = createdEnemyTanksCount % enemySpawnPoints.Count;
        SpawnPoint spawnPoint = enemySpawnPoints[spawnPointIndex];
        spawnPoint.Spawn(SpawnEnemyTank);
    }

    void SpawnEnemyTank(Transform spawnPoint)
    {
        if (spawnPoint == null)
            return;

        if (!Utils.Verify(enemiesQueue.Count != 0))
            return;

        EnemyTank tank = Instantiate(enemyTankPrefab, spawnPoint.position, Quaternion.identity).GetComponent<EnemyTank>();
        tank.Direction = Direction.Down;
        tank.TankType = enemiesQueue.Dequeue();
    }

    void StartListeningEvents()
    {
        EnemyTank.TankCreated += OnEnemyTankCreated;
        EnemyTank.TankDestroyed += OnEnemyTankDestroyed;
        PlayerTank.TankCreated += OnPlayerTankCreated;
        PlayerTank.TankDestroyed += OnPlayerTankDestroyed;
        Eagle.EagleDestroyed += OnEagleDestroyed;
    }

    void StopListeningEvents()
    {
        EnemyTank.TankCreated -= OnEnemyTankCreated;
        EnemyTank.TankDestroyed -= OnEnemyTankDestroyed;
        PlayerTank.TankCreated -= OnPlayerTankCreated;
        PlayerTank.TankDestroyed -= OnPlayerTankDestroyed;
        Eagle.EagleDestroyed -= OnEagleDestroyed;
    }

    public void PauseGame()
    {
        if (IsPaused)
        {
            Time.timeScale = 1;
            IsPaused = false;
        }
        else if (GameState == GameState.Started)
        {
            Time.timeScale = 0;
            IsPaused = true;
        }
    }

    void PreFinishGame()
    {
        GameState = GameState.PreFinished;
        StartCoroutine(PreFinishCoroutine());
    }

    void FinishGame()
    {
        LevelsManager.s_Instance.CurrentGameInfo.IsGameOver = isEagleDestroyed;
        GameState = GameState.Finished;
    }

    void OnEnemyTankCreated(object sender, EventArgs e)
    {
        createdEnemyTanksCount++;
        livedEnemyTanksCount++;
        enemyTanksOnCreatingCount--;

        if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < maxEnemyLivesTanksCount)
            GenerateEnemyTank();
    }

    void OnEnemyTankDestroyed(object sender, EventArgs e)
    {
        livedEnemyTanksCount--;
        if (enemiesQueue.Count == 0 && livedEnemyTanksCount == 0 && enemyTanksOnCreatingCount == 0)
        {
            PreFinishGame();
            return;
        }
        else if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < maxEnemyLivesTanksCount)
            GenerateEnemyTank();
    }

    void OnPlayerTankCreated(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        if (tank.PlayerIndex < 0 || tank.PlayerIndex >= MAX_PLAYERS)
            return;

        playerTankCreating[tank.PlayerIndex] = false;
        playerTankLiving[tank.PlayerIndex] = true;
    }

    void OnPlayerTankDestroyed(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        if (tank.PlayerIndex < 0 || tank.PlayerIndex >= MAX_PLAYERS)
            return;

        playerTankLiving[tank.PlayerIndex] = false;
    }

    void OnEagleDestroyed(object sender, EventArgs e)
    {
        isEagleDestroyed = true;
        if (gameState == GameState.Started)
        {
            PreFinishGame();
        }
    }

    private IEnumerator LoadLevelCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        StartGame();
    }

    private IEnumerator PreFinishCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        FinishGame();
    }
}
