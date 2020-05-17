using UnityEngine;
using static GameConstants;

public class Environment : MonoBehaviour, IBulletTarget
{
    public GroupType Group { get; set; }

    protected void Die()
    {
        Destroy(gameObject);
    }

    protected void DieBy(IBullet bullet)
    {
        if (bullet != null)
        {
            if (bullet.CanDestroyConcrete || bullet.Group == GroupType.Players)
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.TargetHit);
        }
        Destroy(gameObject);
    }

    public virtual bool OnHit(IBullet bullet)
    {
        return false;
    }
}
