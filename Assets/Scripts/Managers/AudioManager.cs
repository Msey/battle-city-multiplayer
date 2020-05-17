using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField]
    private AudioSource audioSource;

    #region amibientClips
    [SerializeField]
    private AudioClip levelStartedSound;
    [SerializeField]
    private AudioClip playerForceedEngineSound;
    [SerializeField]
    private AudioClip playerStoppedEngineSound;
    #endregion

    #region fxClips
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
    #endregion

    public enum AudioClipType
    {
        None,
        LevelStarted,
        PlayerForceedEngine,
        PlayerStoppedEngine,
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

    public void PlayAmibientClip(AudioClipType type, bool loop)
    {
        if (type == AudioClipType.None)
        {
            audioSource.clip = null;
            audioSource.Play();
            return;
        }

        if (!audioSource.loop && audioSource.isPlaying)
            return;

        AudioClip newClip = GetClipByType(type);
        if (newClip == audioSource.clip)
            return;

        audioSource.clip = newClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayFxClip(AudioClipType type)
    {
        AudioClip newClip = GetClipByType(type);
        audioSource.PlayOneShot(newClip);
    }

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(levelStartedSound);
        Assert.IsNotNull(playerForceedEngineSound);
        Assert.IsNotNull(playerStoppedEngineSound);

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

    private void PlayerClip(AudioSource audioSource, AudioClipType type)
    {
        if (audioSource == null)
            return;

        AudioClip newClip = GetClipByType(type);
        if (newClip == audioSource.clip)
            return;

        audioSource.loop = false;
        audioSource.clip = newClip;
        audioSource.Play();
    }

    private AudioClip GetClipByType(AudioClipType type)
    {
        switch (type)
        {
            case AudioClipType.LevelStarted:
                return levelStartedSound;
            case AudioClipType.PlayerForceedEngine:
                return playerForceedEngineSound;
            case AudioClipType.PlayerStoppedEngine:
                return playerStoppedEngineSound;
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
