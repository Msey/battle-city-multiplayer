﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dummy : MonoBehaviour, IDestructable
{
    public MovementSystem.Direction direction;

    public abstract bool isVulnerable { get; set; }

    public abstract bool isAlive { get; set; }

    public abstract void Die();

    public abstract void TakeDamade(int amount);
}
