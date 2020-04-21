using UnityEngine;

public class Concrete : MonoBehaviour, IBulletTarget
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
