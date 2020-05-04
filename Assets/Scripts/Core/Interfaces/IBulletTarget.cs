
public interface IBulletTarget
{
    bool OnHit(IBullet bullet);

    GroupType Group { get; set; }
}

public enum GroupType
{
    Other,
    Enemies,
    Players
}
