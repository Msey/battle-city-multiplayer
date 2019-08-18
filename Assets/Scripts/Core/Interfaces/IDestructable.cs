
public interface IDestructable
{
    void TakeDamade(int amount);

    void Die();

    bool isVulnerable { get; set; }

    bool isAlive { get; set; }
}
