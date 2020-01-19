using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public SpawnPoint spawnPoint;
    public GameObject tankPrefab;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            spawnPoint.Spawn(SpawnTank);
        }
    }

    void SpawnTank(Transform spawnPoint)
    {
        if (spawnPoint == null)
            return;
        Instantiate(tankPrefab, spawnPoint.position, Quaternion.identity);
    }
}
