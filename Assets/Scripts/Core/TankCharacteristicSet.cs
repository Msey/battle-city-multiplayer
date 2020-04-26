public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletSpeed { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }


    public TankCharacteristicSet()
    {
        BulletStrength = 1;
        Velocity = 5.4f;
        BulletSpeed = 7;
        AmmoLimit = 2;
        ShootDelay = 2f;
    }
}

