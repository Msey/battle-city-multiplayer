using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    Queue<EnemyTank.EnemyTankType> enemiesQueue = new Queue<EnemyTank.EnemyTankType>();
    int createdEnemyTanksCount = 0;
    int liveTanksCount = 0;
    int levelEnemeyTanksCount = 0;

    public int LevelEnemeyTanksCount { get => levelEnemeyTanksCount; }

    public int maxEnemyLivesTanksCount = 4;

    public GameObject enemyTank;

    [SerializeField]
    ClassicGameLevelInfo[] levels;

    override protected void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        LoadLevel();
    }

    private void OnDestroy()
    {
        StopListeningEvents();
    }

    void LoadLevel()
    {
        StartListeningEvents();
        LoadLevelObjects();
        CreateEnemyQueue();
        LoadSpawnPoints();
        EventManager.s_Instance.TriggerEvent(new LevelStartedEvent());
        GenerateEnemyTank();
    }

    void StartListeningEvents()
    {
        EventManager.s_Instance.StartListening<EnemyTankCreatedEvent>(OnEnemyTankCreated);
    }

    void StopListeningEvents()
    {
        EventManager.s_Instance.StopListening<EnemyTankCreatedEvent>(OnEnemyTankCreated);
    }

    void OnEnemyTankCreated(EnemyTankCreatedEvent e)
    {
        createdEnemyTanksCount++;
        liveTanksCount++;
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
        var allSpawnPoints = FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in allSpawnPoints)
        {
            if (spawnPoint.pointType == SpawnPoint.PointType.Enemy)
                enemySpawnPoints.Add(spawnPoint);
        }
    }

    void GenerateEnemyTank()
    {
        int spawnPointIndex = createdEnemyTanksCount % enemySpawnPoints.Count;
        SpawnPoint spawnPoint = enemySpawnPoints[spawnPointIndex];
        spawnPoint.Spawn(SpawnTank);
    }

    void SpawnTank(Transform spawnPoint)
    {
        if (spawnPoint == null)
            return;

        if (enemiesQueue.Count == 0)
            return;

        EnemyTank tank = Instantiate(enemyTank, spawnPoint.position, Quaternion.identity).GetComponent<EnemyTank>();
        tank.Direction = GameConstants.Direction.Down;
        tank.TankType = enemiesQueue.Dequeue();

        if (liveTanksCount < maxEnemyLivesTanksCount)
            GenerateEnemyTank();
    }
}
