using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : TankBase
{
    public override bool isVulnerable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override bool isAlive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Die()
    {
    }

    public override void Shoot()
    {
    }

    public override void TakeDamade(int amount)
    {
    }
}
