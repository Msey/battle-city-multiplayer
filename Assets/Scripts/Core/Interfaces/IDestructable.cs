
public interface IDestructable
{
    void TakeDamade(int amount);

    void Die();

    bool isVulnerable { get; }

    bool isAlive { get; }

    int Health { get; }
}
