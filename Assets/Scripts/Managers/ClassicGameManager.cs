using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

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
    int createdEnemyTanksCount = 0;
    int livedEnemyTanksCount = 0;
    int enemyTanksOnCreatingCount = 0;
    int levelEnemeyTanksCount = 0;

    public int LevelEnemeyTanksCount { get => levelEnemeyTanksCount; }

    public int maxEnemyLivesTanksCount = 4;

    public GameObject enemyTankPrefab;
    public GameObject playerTankPrefab;

    [SerializeField]
    ClassicGameLevelInfo[] levels;

    bool[] playerTankCreating = new bool[GameConstants.PlayerTanksCount] { false, false, false, false };
    bool[] playerTankLiving = new bool[GameConstants.PlayerTanksCount] { false, false, false, false };

    override protected void Awake()
    {
        Assert.IsNotNull(enemyTankPrefab);
        Assert.IsNotNull(playerTankPrefab);

        base.Awake();
    }
    private void Start() => LoadLevel();
    protected override void OnDestroy() => StopListeningEvents();

    static public event EventHandler GameStarted; //TODO, move to other place
    static public event EventHandler GameEnded; //TODO, move to other place

    void LoadLevel()
    {
        StartListeningEvents();
        LoadLevelObjects();
        CreateEnemyQueue();
        LoadSpawnPoints();
        GameStarted?.Invoke(this, EventArgs.Empty);
        GenerateEnemyTank();
        CreatePlayerTanks();
    }

    void CreatePlayerTanks()
    {
        SpawnPlayerTank(0);
    }

    public void SpawnPlayerTank(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= GameConstants.PlayerTanksCount)
            return;

        if (playerTankCreating[playerIndex])
            return;

        if (playerTankLiving[playerIndex])
            return;

        playerTankCreating[playerIndex] = true;

        SpawnPoint spawnPoint = playerSpawnPoints[playerIndex];
        spawnPoint.Spawn((point) => {
            PlayerTank tank = Instantiate(playerTankPrefab, point.position, Quaternion.identity).GetComponent<PlayerTank>();
            //tank.Direction = GameConstants.Direction.Down;
            //tank.TankType = enemiesQueue.Dequeue();
            playerTankCreating[playerIndex] = false;
            playerTankLiving[playerIndex] = true;
        });
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

        Assert.AreEqual(playerSpawnPoints.Count, GameConstants.PlayerTanksCount);
    }

    void GenerateEnemyTank()
    {
        if (enemiesQueue.Count == 0)
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

        if (enemiesQueue.Count == 0)
            return;

        EnemyTank tank = Instantiate(enemyTankPrefab, spawnPoint.position, Quaternion.identity).GetComponent<EnemyTank>();
        tank.Direction = GameConstants.Direction.Down;
        tank.TankType = enemiesQueue.Dequeue();
    }



    void StartListeningEvents()
    {
        EnemyTank.TankCreated += OnEnemyTankCreated;
        EnemyTank.TankDestroyed += OnEnemyTankDestroyed;
        PlayerTank.TankCreated += OnPlayerTankCreated;
        PlayerTank.TankDestroyed += OnPlayerTankDestroyed;
    }

    void StopListeningEvents()
    {
        EnemyTank.TankCreated -= OnEnemyTankCreated;
        EnemyTank.TankDestroyed -= OnEnemyTankDestroyed;
        PlayerTank.TankCreated -= OnPlayerTankCreated;
        PlayerTank.TankDestroyed -= OnPlayerTankDestroyed;
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
        if (enemiesQueue.Count == 0)
            return; //level ended
        else if ((livedEnemyTanksCount + enemyTanksOnCreatingCount) < maxEnemyLivesTanksCount)
            GenerateEnemyTank();
    }

    void OnPlayerTankCreated(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        if (tank.PlayerIndex < 0 || tank.PlayerIndex >= GameConstants.PlayerTanksCount)
            return;

        playerTankCreating[tank.PlayerIndex] = false;
        playerTankLiving[tank.PlayerIndex] = true;
    }

    void OnPlayerTankDestroyed(object sender, EventArgs e)
    {
        PlayerTank tank = sender as PlayerTank;
        if (tank == null)
            return;

        if (tank.PlayerIndex < 0 || tank.PlayerIndex >= GameConstants.PlayerTanksCount)
            return;

        playerTankLiving[tank.PlayerIndex] = false;
    }
}
