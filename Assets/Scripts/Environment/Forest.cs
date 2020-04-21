using UnityEngine;

public class Forest : MonoBehaviour, IBulletTarget
{
    public void Die()
    {
        Destroy(gameObject);
    }
    public void OnHit(Bullet bullet)
    {
        Die();
    }
}
