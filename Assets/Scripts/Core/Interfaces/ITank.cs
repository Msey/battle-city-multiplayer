using static GameConstants;

public interface ITank
{
    Direction Direction { get; set; }

    bool Stopped { get; set; }

    void Shoot();
}