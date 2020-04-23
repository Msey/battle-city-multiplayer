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
    
    public void OnHit(IBullet bullet)
    {
        Die();
    }
}
