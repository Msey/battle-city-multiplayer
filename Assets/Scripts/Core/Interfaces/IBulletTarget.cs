
public interface IBulletTarget
{
    bool OnHit(IBullet bullet);

    EntityRelationGroup Group { get; set; }
}
