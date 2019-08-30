
public interface IDestructable
{
    void OnHit(UnityEngine.GameObject hitSource);

    void Die();

    bool isVulnerable { get; }

    bool isAlive { get; }

    int Health { get; }
}
