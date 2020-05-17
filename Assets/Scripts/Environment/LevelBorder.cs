using UnityEngine;
using static GameConstants;

public class LevelBorder : Environment
{
    public override bool OnHit(IBullet bullet)
    {
        if (bullet.Group == GroupType.Players)
            AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.EnvironmentHit);
        return true;
    }
}
