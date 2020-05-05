using static GameConstants;

public interface IBullet : IBulletTarget
{
    Direction Direction { get; set; }
    ITank Owner { get; set; }
    float Velocity { get; set; }

    bool CanDestroyConcrete { get; set; }
    bool CanDestroyForest { get; set; }
}
 