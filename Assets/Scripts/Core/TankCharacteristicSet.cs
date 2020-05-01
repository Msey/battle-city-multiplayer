using System.Collections.Generic;

public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }

    private HashSet<Upgrade> upgrades;

    public void AddUpgrade(Upgrade upgrade)
    {

        if(upgrades.Add(upgrade))
        {
            //upgrades.First(x => x == upgrade);
        }
    }

    public TankCharacteristicSet()
    {
        upgrades = new HashSet<Upgrade>();

        BulletStrength = 1;
        Velocity = 5.4f;
        BulletVelocity = 16f;
        AmmoLimit = 2;
        ShootDelay = 2f;
    }
}

