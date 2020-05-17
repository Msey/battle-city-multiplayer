using UnityEngine;
using static GameConstants;

public class Concrete : Environment
{
    public override bool OnHit(IBullet bullet)
    {
        if (bullet.CanDestroyConcrete)
        {
            AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.TankHit);
            Die();
        }
        else
        {
            if (bullet.Group == GroupType.Players)
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.EnvironmentHit);
        }

        return true;
    }
}
