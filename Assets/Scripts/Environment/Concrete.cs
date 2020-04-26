using UnityEngine;

public class Concrete : MonoBehaviour, IBulletTarget
{
    public EntityRelationGroup Group { get; set; }

    void Awake()
    {
        Group = new EntityRelationGroup(this);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    public bool OnHit(IBullet bullet)
    {
        Die();
        return true;
    }
}
