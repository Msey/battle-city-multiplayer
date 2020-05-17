using UnityEngine;
using static GameConstants;

public class Forest : Environment
{
    public override bool OnHit(IBullet bullet)
    {
        if (bullet.CanDestroyForest)
            DieBy(bullet);

        return false;
    }
}
