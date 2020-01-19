using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnPointAnimatorController))]
public class SpawnPoint : MonoBehaviour
{
    SpawnPointAnimatorController animator;

    public delegate void SpawnRequest(Transform transform);
    Queue<SpawnRequest> spawnRequests = new Queue<SpawnRequest>();

    void Start()
    {
        animator = GetComponent<SpawnPointAnimatorController>();
        animator.AnimationFinished += OnAnimationFinished;
    }

    public void Spawn(SpawnRequest spawnRequest)
    {
        if (spawnRequest == null)
            return;
        spawnRequests.Enqueue(spawnRequest);
        animator.PlayAnimation();
    }

    void OnAnimationFinished()
    {
        if (spawnRequests.Count == 0)
            return;

        SpawnRequest spawnRequest = spawnRequests.Dequeue();
        if (spawnRequest == null)
            return;
        spawnRequest(gameObject.transform);

        if (spawnRequests.Count > 0)
            animator.PlayAnimation();
    }
}
