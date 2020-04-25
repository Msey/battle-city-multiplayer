using static GameConstants;

public interface ITank : IBulletTarget
{
    TankCharacteristicSet Characteristics { get; set; }
    Direction Direction { get; set; }
    bool Stopped { get; set; }
    void Shoot();
    void OnMyBulletHit(IBullet bullet);
}