using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField]
    private AudioSource amibientAudioSource;
    [SerializeField]
    private AudioSource fxAudioSource;

    [SerializeField]
    private AudioClip tankHitSound;
    [SerializeField]
    private AudioClip environmentHitSound;
    [SerializeField]
    private AudioClip targetSound;
    [SerializeField]
    private AudioClip iceSound;
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip enemyExplosionSound;
    [SerializeField]
    private AudioClip playerExplosionSound;
    [SerializeField]
    private AudioClip bonusAppearanceSound;
    [SerializeField]
    private AudioClip bonusTakenSound;
    [SerializeField]
    private AudioClip pauseSound;

    public enum AudioClipType
    {
        TankHit,
        EnvironmentHit,
        TargetHit,
        Ice,
        Shoot,
        EnemyExplosion,
        PlayerExplosion,
        BonusAppearance,
        BonusTaken,
        Pause,
    };

    public void PlayFxClip(AudioClipType type)
    {
        fxAudioSource.loop = false;
        fxAudioSource.clip = GetClipByType(type);
        fxAudioSource.Play();
    }

    protected override void Awake()
    {
        Assert.IsNotNull(amibientAudioSource);
        Assert.IsNotNull(fxAudioSource);

        Assert.IsNotNull(tankHitSound);
        Assert.IsNotNull(environmentHitSound);
        Assert.IsNotNull(targetSound);
        Assert.IsNotNull(iceSound);
        Assert.IsNotNull(shootSound);
        Assert.IsNotNull(enemyExplosionSound);
        Assert.IsNotNull(playerExplosionSound);
        Assert.IsNotNull(bonusAppearanceSound);
        Assert.IsNotNull(bonusTakenSound);
        Assert.IsNotNull(pauseSound);

        base.Awake();
    }

    private AudioClip GetClipByType(AudioClipType type)
    {
        switch (type)
        {
            case AudioClipType.TankHit:
                return tankHitSound;
            case AudioClipType.EnvironmentHit:
                return environmentHitSound;
            case AudioClipType.TargetHit:
                return targetSound;
            case AudioClipType.Ice:
                return iceSound;
            case AudioClipType.Shoot:
                return shootSound;
            case AudioClipType.EnemyExplosion:
                return enemyExplosionSound;
            case AudioClipType.PlayerExplosion:
                return playerExplosionSound;
            case AudioClipType.BonusAppearance:
                return bonusAppearanceSound;
            case AudioClipType.BonusTaken:
                return bonusTakenSound;
            case AudioClipType.Pause:
                return pauseSound;
        }

        Assert.IsTrue(false, "Empty audio clip");
        return null;
    }
}
