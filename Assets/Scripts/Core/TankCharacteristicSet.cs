﻿public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }

    public TankCharacteristicSet()
    {
        BulletStrength = 1;
        Velocity = 5.4f;
        BulletVelocity = 16.0f;
        AmmoLimit = 2;
        ShootDelay = 2f;
    }
}

