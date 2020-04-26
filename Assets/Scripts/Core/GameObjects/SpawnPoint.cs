using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnPointAnimatorController))]
public class SpawnPoint : MonoBehaviour
{
    SpawnPointAnimatorController animator;

    public delegate void SpawnRequest(Transform transform);
    Queue<SpawnRequest> spawnRequests = new Queue<SpawnRequest>();

    public enum PointType
    {
        Player,
        Enemy
    }
    public PointType pointType = PointType.Player;

    void Awake()
    {
        animator = GetComponent<SpawnPointAnimatorController>();
        animator.OnAnimationFinishedCallback = () => MakeSpawnRequests();
    }

    public void Spawn(SpawnRequest spawnRequest)
    {
        if (spawnRequest == null)
            return;
        spawnRequests.Enqueue(spawnRequest);
        animator.PlayAnimation();
    }

    void MakeSpawnRequests()
    {
        if (!Utils.Verify(spawnRequests.Count != 0))
            return;

        SpawnRequest spawnRequest = spawnRequests.Dequeue();
        if (spawnRequest == null)
            return;
        spawnRequest(gameObject.transform);

        if (spawnRequests.Count > 0)
            animator.PlayAnimation();
    }
}
