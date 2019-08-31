using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : TankBase
{

    public enum TankVariation
    {
        Simple,
        Fast,
        Universal,
        Heavy
    }

    public override void Die()
    {
    }

    public override void Shoot()
    {
    }


    public override void OnHit(GameUnit hitSource)
    {
        throw new System.NotImplementedException();
    }
}
