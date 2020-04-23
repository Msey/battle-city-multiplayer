


public class EntityRelationGroup
{
    public enum GroupType
    {
        Undefined,
        Players,
        Enemies,
        Obstacles,
    }

    GroupType CurrentGroup;

    public EntityRelationGroup(object entity)
    {
        if (entity is PlayerTank)
        {
            CurrentGroup = GroupType.Players;
        }
        else if (entity is EnemyTank)
        {
            CurrentGroup = GroupType.Enemies;
        }
        else if (entity is Brick ||
            entity is Concrete ||
            entity is Brick)
        {
            CurrentGroup = GroupType.Obstacles;
        }
        else CurrentGroup = GroupType.Undefined;
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
        return a.CurrentGroup == b.CurrentGroup;
    }

    public static bool operator !=(EntityRelationGroup a, EntityRelationGroup b)
    {
        return a.CurrentGroup != b.CurrentGroup;
    }
}
