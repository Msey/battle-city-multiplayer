using UnityEngine;

public class Forest : MonoBehaviour, IBulletTarget
{
    public EntityRelationGroup Group { get; set; } // TODO: Implement init here
    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnHit(IBullet bullet)
    {
        Die();
    }
}
