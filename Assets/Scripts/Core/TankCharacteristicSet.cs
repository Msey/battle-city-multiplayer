public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }




    private static int playerLives;

    public void AddLife() => ++playerLives; 
    public void TakeLife() => --playerLives;

    public int GetTotalLives()
    {
        return playerLives;
    }


    public TankCharacteristicSet()
    {
        BulletStrength = 1;
        Velocity = 5.4f;
        BulletVelocity = 16.0f;
        AmmoLimit = 2;
        ShootDelay = 2f;
        playerLives = 2;
    }
}

