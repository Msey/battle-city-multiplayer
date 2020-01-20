using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : Singleton<ClassicGameManager>
{
    List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();

    override protected void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForFixedUpdate();
        LoadSpawnPoints();
        EventManager.s_Instance.TriggerEvent(new LevelStartedEvent());
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

    }
}
