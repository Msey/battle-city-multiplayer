public class EntityRelationGroup
{
    public enum GroupType
    {
        Undefined,
        Players,
        Enemies,
        Obstacles,
    }

    public GroupType Current;

    public EntityRelationGroup(object entity)
    {
        if (entity is PlayerTank)
        {
            Current = GroupType.Players;
        }
        else if (entity is EnemyTank)
        {
            Current = GroupType.Enemies;
        }
        else if (entity is Concrete ||
            entity is LevelBorder ||
            entity is Brick)
        {
            Current = GroupType.Obstacles;
        }
        else Current = GroupType.Undefined;
    }

    public override bool Equals(object obj)
    {
        return this == (EntityRelationGroup)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(EntityRelationGroup a, EntityRelationGroup b)
    {        
        return a.Current == b.Current;
    }

    public static bool operator !=(EntityRelationGroup a, EntityRelationGroup b)
    {
        return a.Current != b.Current;
    }
}
