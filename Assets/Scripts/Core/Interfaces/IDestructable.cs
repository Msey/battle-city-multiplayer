
public interface IDestructable
{
    void OnHit(GameUnit hitSource);

    void Die();

    bool isVulnerable { get; }

    bool isAlive { get; }

    int Health { get; }
}
