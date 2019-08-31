using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameUnit : MonoBehaviour, IDestructable
{
    [HideInInspector]
    public MovementSystem.Direction direction;

    public bool isVulnerable { get; }

    public bool isAlive { get; }

    public int Health { get; }

    public abstract void Die();

    public abstract void OnHit(GameUnit hitSource);
}
