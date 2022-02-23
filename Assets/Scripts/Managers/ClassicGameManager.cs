using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;
using System.Collections;
using static GameConstants;

public class PlayerKillsStatistic
{
    public Dictionary<EnemyTankType, int> tanks = new Dictionary<EnemyTankType, int>();
}

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
    private List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();
    private List<SpawnPoint> playerSpawnPoints = new List<SpawnPoint>();
    private Queue<EnemyTankType> enemiesQueue = new Queue<EnemyTankType>();
    private Lazy<Transform> tilemap;
    private Lazy<GameObject[]> bonuses;
    private Lazy<Rect> levelBorderRect;

    [SerializeField]
    private int createdEnemyTanksCount;
    [SerializeField]
    private int livedEnemyTanksCount;
    [SerializeField]
    private int enemyTanksOnCreatingCount;
    [SerializeField]
    private int levelEnemeyTanksCount;


    public int LevelEnemeyTanksCount { get => levelEnemeyTanksCount; }

    public int MaxEnemyLivesTanksCount
    {
        get => 2 + LevelsManager.s_Instance.CurrentGameInfo.PlayersCount * 2;
    }

    public Transform Tilemap => tilemap.Value;

    [SerializeField]
    private ClassicGameLevelInfo[] levels;

    private bool[] playerTankCreating = new bool[MAX_PLAYERS] { false, false, false, false };
    private bool[] playerTankLiving = new bool[MAX_PLAYERS] { false, false, false, false };

    private PlayerKillsStatistic[] playersStatistic = new PlayerKillsStatistic[MAX_PLAYERS] { new PlayerKillsStatistic(), new PlayerKillsStatistic(), new PlayerKillsStatistic(), new PlayerKillsStatistic() };
    public PlayerKillsStatistic[] PlayersStatistic => playersStatistic;

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
            if (isPaused)
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.Pause);
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

    public float RespawnTime
    {
        get
        {
            GameInfo currentGameInfo = LevelsManager.s_Instance.CurrentGameInfo;
            float time = (190 - currentGameInfo.CurrentStage * 4 - (currentGameInfo.PlayersCount - 1) * 20) / 60;
            if (time < 0)
                return 0;
            return time;
        }
    }
    public HashSet<PlayerTank> ActivePlayerTanks { get; } = new HashSet<PlayerTank>();
    public HashSet<EnemyTank> ActiveEnemyTanks { get; } = new HashSet<EnemyTank>();

    public List<Eagle> Eagles { get; } = new List<Eagle>();

    EnemyTanksAISystem enemyTanksAISystem;
    public EnemyTanksAISystem EnemyTanksAISystem
    {
        get => enemyTanksAISystem;
    }

    private int playerLives = 2;

    public void AddLife()
    {
        ++playerLives;
        TotalLivesChanged?.Invoke(this, EventArgs.Empty);
    }
    public void TakeLife()
    {
        --playerLives;
        TotalLivesChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler TotalLivesChanged;
    public int GetTotalLives()
    {
        return playerLives;
    }

    override protected void Awake()
    {
        Assert.IsNotNull(ResourceManager.s_Instance.EnemyTankPrefab);
        Assert.IsNotNull(ResourceManager.s_Instance.PlayerTankPrefab);
        enemyTanksAISystem = new EnemyTanksAISystem(this);

        base.Awake();
    }
    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        if (GameState != GameState.NotStarted)
            UpdateTanksSoundBackground();
    }

    private void UpdateTanksSoundBackground()
    {
        if (isPaused || GameState == GameState.Finished)
        {
            AudioManager.s_Instance.PlayAmibientClip(AudioManager.AudioClipType.None, false);
            return;
        }

        foreach (PlayerTank playerTank in ClassicGameManager.s_Instance.ActivePlayerTanks)
        {
            if (!playerTank.TankMovement.Stopped)
            {
                AudioManager.s_Instance.PlayAmibientClip(AudioManager.AudioClipType.PlayerForceedEngine, true);
                return;
            }
        }

        if (!isAllEnemyTanksDestroyed && !(isEagleDestroyed || ActivePlayerTanks.Count == 0))
        {
            AudioManager.s_Instance.PlayAmibientClip(AudioManager.AudioClipType.PlayerStoppedEngine, true);
        }
        else if (GameState == GameState.PreFinished)
        {
            AudioManager.s_Instance.PlayAmibientClip(AudioManager.AudioClipType.None, false);
        }
    }

    protected override void OnDestroy() => StopListeningEvents();

    public event EventHandler GameStateChanged;

    public void LoadGame()
    {
        if (!Utils.Verify(GameState == GameState.NotStarted))
            return;

        playerLives = LevelsManager.s_Instance.CurrentGameInfo.LivesCount;
        GameState = GameState.Loading;
        AudioManager.s_Instance.PlayAmibientClip(AudioManager.AudioClipType.LevelStarted, false);
        StartCoroutine(LoadLevelCoroutine());
    }

    private void StartGame()
    {
        if (!Utils.Verify(GameState == GameState.Loading))
            return;

        LoadLevel();
        GameState = GameState.Started;
        enemyTanksAISystem.Start();

        tilemap = new Lazy<Transform>(() => GameObject.Find("Tilemap").transform);

        bonuses = new Lazy<GameObject[]>(() => new GameObject[]
        {
            ResourceManager.s_Instance.TankBonusPrefab,
            ResourceManager.s_Instance.StarBonusPrefab,
            ResourceManager.s_Instance.ShovelBonusPrefab,
            ResourceManager.s_Instance.HelmetBonusPrefab,
            ResourceManager.s_Instance.PistolBonusPrefab,
            ResourceManager.s_Instance.GrenadeBonusPrefab,
            ResourceManager.s_Instance.ClockBonusPrefab
        });

        levelBorderRect = new Lazy<Rect>(() => 
        {
            var levelBorderPositions = FindObjectsOfType<LevelBorder>()
                .ToGameObjects()
                .ToVectors();

            return levelBorderPositions.ComputeOverlapRect();
        });
    }

    void LoadLevel()
    {
        playerLives = LevelsManager.s_Instance.CurrentGameInfo.LivesCount;
        StartListeningEvents();
        LoadLevelObjects();
        CreateEnemyQueue();
        LoadSpawnPoints();
        StartCoroutine(GenerateEnemyTank());
        CreatePlayerTanks();
    }

    void CreatePlayerTanks()
    {
        int playersCount = LevelsManager.s_Instance.CurrentGameInfo.PlayersCount;
        for (int playerIndex = 0; playerIndex < playersCount; ++playerIndex)
        {
            if (LevelsManager.s_Instance.CurrentGameInfo.PrevLevelPlayerTankLiving[playerIndex])
                SpawnPlayerTank(playerIndex, LevelsManager.s_Instance.CurrentGameInfo.PrevLevelPlayerTankCharacteristic[playerIndex]);
        }
    }

    public bool PlayerTankIsLiving(int playerIndex)
    {
        if (!Utils.InRange(0, playerIndex, MAX_PLAYERS))
            return false;

        return playerTankLiving[playerIndex];
    }

    private void SpawnPlayerTank(int playerIndex, PlayerTankStaticCharacteristicSet characteristic = null)
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
            PlayerTank tank = Instantiate(ResourceManager.s_Instance.PlayerTankPrefab, point.position, Quaternion.identity).GetComponent<PlayerTank>();
            tank.PlayerIndex = playerIndex;
            tank.Direction = Direction.Up;
            tank.HelmetTimer = 3f;
            if (characteristic != null)
                tank.Characteristics.LoadFrom(characteristic);
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
        Eagles.AddRange(FindObjectsOfType<Eagle>());
    }

    void CreateEnemyQueue()
    {
        enemiesQueue.Clear();
        ClassicGameLevelInfo levelInfo = levels[LevelsManager.s_Instance.CurrentGameInfo.CurrentStage % levels.Length];

        for (int basicTank = 0; basicTank < levelInfo.basicTanksCount; ++basicTank)
            enemiesQueue.Enqueue(EnemyTankType.Basic);

        for (int armorTank = 0; armorTank < levelInfo.armorTanksCount; ++armorTank)
            enemiesQueue.Enqueue(EnemyTankType.Armor);

        for (int fastTank = 0; fastTank < levelInfo.fastTanksCount; ++fastTank)
            enemiesQueue.Enqueue(EnemyTankType.Fast);

        for (int powerTank = 0; powerTank < levelInfo.powerTanksCount; ++powerTank)
            enemiesQueue.Enqueue(EnemyTankType.Power);

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

       // Assert.AreEqual(playerSpawnPoints.Count, MAX_PLAYERS);
    }

    IEnumerator GenerateEnemyTank()
     {
        if (enemiesQueue.Count - enemyTanksOnCreatingCount != 0)
        {
            enemyTanksOnCreatingCount++;
            yield return new WaitForSeconds(createdEnemyTanksCount == 0 ? 0 : RespawnTime);
            int spawnPointIndex = createdEnemyTanksCount % enemySpawnPoints.Count;
            SpawnPoint spawnPoint = enemySpawnPoints[spawnPointIndex];
            spawnPoint.Spawn(SpawnEnemyTank);
        }
    }

    void SpawnEnemyTank(Transform spawnPoint)
    {
        if (spawnPoint == null)
            return;

        if (!Utils.Verify(enemiesQueue.Count != 0))
            return;

        EnemyTank tank = Instantiate(ResourceManager.s_Instance.EnemyTankPrefab, spawnPoint.position, Quaternion.identity).GetComponent<EnemyTank>();
        tank.Direction = Direction.Down;
        tank.TankIndex = createdEnemyTanksCount;
        tank.TankType = enemiesQueue.Dequeue();
        tank.IsBounusTank = IsBonusEnemyTank(tank.TankIndex);

        if (!tank.IsBounusTank)
            tank.ArmorLevel = GenerateEnemyTankArmorLevel(tank.TankType);
    }

    static bool IsBonusEnemyTank(int tankIndex)
    {
        if (tankIndex < 4)
            return false;
        tankIndex -= 4;
        return tankIndex % 7 == 0;
    }

    int GenerateEnemyTankArmorLevel(EnemyTankType tankType)
    {
        if (tankType == EnemyTankType.Armor)
            return EnemyTank.MaxArmorLevel;

        if (LevelsManager.s_Instance.CurrentGameInfo.CurrentStage < 5)
            return 0;

        return UnityEngine.Random.Range(0, EnemyTank.MaxArmorLevel + 1);
    }

    void StartListeningEvents()
    {
        EnemyTank.TankCreated += OnEnemyTankCreated;
        EnemyTank.TankDestroyed += OnEnemyTankDestroyed;
        EnemyTank.BonusTankHit += OnBonusTankHit;
        PlayerTank.TankCreated += OnPlayerTankCreated;
        PlayerTank.TankDestroyed += OnPlayerTankDestroyed;
        PlayerTank.EnemyTankDestroyed += OnPlayerEnemyTankDestroyed;
        Eagle.EagleDestroyed += OnEagleDestroyed;
    }

    void StopListeningEvents()
    {
        EnemyTank.TankCreated -= OnEnemyTankCreated;
        EnemyTank.TankDestroyed -= OnEnemyTankDestroyed;
        EnemyTank.BonusTankHit -= OnBonusTankHit;
        PlayerTank.TankCreated -= OnPlayerTankCreated;
        PlayerTank.TankDestroyed -= OnPlayerTankDestroyed;
        PlayerTank.EnemyTankDestroyed -= OnPlayerEnemyTankDestroyed;
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
        GameInfo currentGameInfo = LevelsManager.s_Instance.CurrentGameInfo;
        playerTankLiving.CopyTo(currentGameInfo.PrevLevelPlayerTankLiving, 0);
        currentGameInfo.PrevLevelPlayerTankCharacteristic = new PlayerTankStaticCharacteristicSet[MAX_PLAYERS];
        currentGameInfo.LivesCount = playerLives;

        foreach (PlayerTank playerTank in ClassicGameManager.s_Instance.ActivePlayerTanks)
            currentGameInfo.PrevLevelPlayerTankCharacteristic[playerTank.PlayerIndex] = playerTank.Characteristics.ExportStaticCharacteristic();

        currentGameInfo.IsGameOver = isEagleDestroyed || ActivePlayerTanks.Count == 0;
        GameState = GameState.Finished;
    }

    void OnEnemyTankCreated(object sender, EventArgs e)
    {
        EnemyTank tank = sender as EnemyTank;
        if (tank == null)
            return;

        ActiveEnemyTanks.Add(tank);
        createdEnemyTanksCount++;
        livedEnemyTanksCount++;
        enemyTanksOnCreatingCount--;

        if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < MaxEnemyLivesTanksCount)
            StartCoroutine(GenerateEnemyTank());
    }

    void OnEnemyTankDestroyed(object sender, EventArgs e)
    {
        EnemyTank tank = sender as EnemyTank;
        if (tank == null)
            return;

        ActiveEnemyTanks.Remove(tank);
        livedEnemyTanksCount--;
        if (enemiesQueue.Count == 0 && livedEnemyTanksCount == 0 && enemyTanksOnCreatingCount == 0)
        {
            isAllEnemyTanksDestroyed = true;
            PreFinishGame();
            return;
        }
        else if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < MaxEnemyLivesTanksCount)
        {
            StartCoroutine(GenerateEnemyTank());
        }
    }

    void OnPlayerTankCreated(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        if (tank.PlayerIndex < 0 || tank.PlayerIndex >= MAX_PLAYERS)
            return;

        ActivePlayerTanks.Add(tank);
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

        ActivePlayerTanks.Remove(tank);
        playerTankLiving[tank.PlayerIndex] = false;

        if (playerLives == 0)
            PreFinishGame();
    }

    void OnEagleDestroyed(object sender, EventArgs e)
    {
        isEagleDestroyed = true;
        if (gameState == GameState.Started)
        {
            PreFinishGame();
        }
    }

    void OnBonusTankHit(object sender, EventArgs e)
    {
        Vector2 randomPoint =
            GameUtils.RandomPointInRect(levelBorderRect.Value, CELL_SIZE, 2 * CELL_SIZE, 2 * CELL_SIZE);

        if (SpawnedBonus)
            Destroy(SpawnedBonus);

        SpawnedBonus = Instantiate(bonuses.Value[GameUtils.Rand(0, bonuses.Value.Length)], randomPoint, Quaternion.identity);
    }

    private GameObject SpawnedBonus;

    private void OnPlayerEnemyTankDestroyed(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        EnemyTankDestroyedEventArgs eventsArgs = e as EnemyTankDestroyedEventArgs;
        if (eventsArgs == null)
            return;

        if (eventsArgs.Tank == null)
            return;

        if (!Utils.InRange(0, tank.PlayerIndex, MAX_PLAYERS))
            return;

        EnemyTankType destroyedTankType = eventsArgs.Tank.TankType;
        PlayerKillsStatistic playerKillsStatistic = PlayersStatistic[tank.PlayerIndex];
        if (playerKillsStatistic.tanks.ContainsKey(destroyedTankType))
            playerKillsStatistic.tanks[destroyedTankType]++;
        else
            playerKillsStatistic.tanks.Add(destroyedTankType, 1);
    }

    private IEnumerator LoadLevelCoroutine()
    {
        AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.LevelStarted);
        yield return new WaitForSeconds(2.0f);
        StartGame();
    }

    private IEnumerator PreFinishCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        FinishGame();
    }

    private IEnumerator GenerateEnemyTankTimer()
    {
        yield return new WaitForSeconds(RespawnTime);
        int spawnPointIndex = createdEnemyTanksCount % enemySpawnPoints.Count;
        SpawnPoint spawnPoint = enemySpawnPoints[spawnPointIndex];
        spawnPoint.Spawn(SpawnEnemyTank);
    }
}
