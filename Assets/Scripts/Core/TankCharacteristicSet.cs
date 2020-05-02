using System.Collections.Generic;
using UnityEngine;
using static PickUp;

public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }

    private HashSet<PickUpType> upgrades;
    private int starUpgradeLevel = 0;

    public void AddUpgrade(PickUpType pickUp)
    {
        if (!upgrades.Add(pickUp))
        {
            if(pickUp == PickUpType.Star && starUpgradeLevel < 2)
                starUpgradeLevel++;
        }

        Recalculate();
    }

    public void Recalculate()
    {
        // TODO: recalculate characteristics
    }

    public TankCharacteristicSet()
    {
        upgrades = new HashSet<PickUpType>();

        BulletStrength = 1;
        Velocity = 5.4f;
        BulletVelocity = 16f;
        AmmoLimit = 2;
        ShootDelay = 2f;
    }
}

