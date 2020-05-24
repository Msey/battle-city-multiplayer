using UnityEngine;
using static GameConstants;

public class Concrete : Environment
{
    public override bool OnHit(IBullet bullet)
    {
        if (bullet.CanDestroyConcrete)
        {
            DieBy(bullet);
        }
        else
        {
            if (bullet.Group == GroupType.Players)
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.EnvironmentHit);
        }

        return true;
    }
}
