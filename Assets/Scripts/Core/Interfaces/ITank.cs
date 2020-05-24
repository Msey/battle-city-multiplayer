using System.Collections.Generic;
using static GameConstants;

public interface ITank : IBulletTarget
{
    Direction Direction { get; set; }
    bool Stopped { get; set; }
    void Shoot();
    void OnMyBulletHit(IBullet bullet, List<IBulletTarget> targets);
    bool IsDestroyed { get; }
}