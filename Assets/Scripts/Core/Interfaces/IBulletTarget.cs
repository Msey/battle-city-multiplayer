
public interface IBulletTarget
{
    void OnHit(IBullet bullet);

    EntityRelationGroup Group { get; set; }
}
