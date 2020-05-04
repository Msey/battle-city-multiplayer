using UnityEngine;

public class Forest : MonoBehaviour, IBulletTarget
{
    public GroupType Group { get; set; }
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
