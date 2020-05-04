using UnityEngine;
using static GameConstants;

public class Concrete : MonoBehaviour, IBulletTarget
{
    public GroupType Group { get; set; }
    void Awake()
    {
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
