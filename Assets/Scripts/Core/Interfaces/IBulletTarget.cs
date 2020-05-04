using static GameConstants;

public interface IBulletTarget
{
    bool OnHit(IBullet bullet);

    GroupType Group { get; set; }
}
