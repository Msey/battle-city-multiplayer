using static GameConstants;

public interface IBullet : IBulletTarget
{
    Direction Direction { get; set; }
    ITank Owner { get; set; }
}
 