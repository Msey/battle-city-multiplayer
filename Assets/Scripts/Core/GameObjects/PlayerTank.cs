using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(TankMovement), typeof(PlayerTankAnimator))]
public class PlayerTank : MonoBehaviour, ITank
{
    public GameObject invulnerabilityPrefab;

    private GameObject tempInvulnerabilityPrefab = null;


    private TankMovement tankMovement;
    public TankMovement TankMovement
    {
        get => tankMovement;
    }

    private PlayerTankAnimator tankAnimator;

    public bool CanShoot
    {
        get
        {
            return (shootDelay <= 0
                && ammoLeft > 0);
        }
    }

    public bool CanMove => freezeTime <= 0;

    float shootDelay;
    private int ammoLeft;
    private float freezeTime;

    public Direction Direction
    {
        get => tankMovement.Direction;
        set
        {
            if (CanMove)
                tankMovement.Direction = value;
        }
    }
    public bool Stopped
    {
        get => tankMovement.Stopped;
        set => tankMovement.Stopped = value;
    }

    private int playerIndex;

    public int PlayerIndex
    {
        get => playerIndex;
        set
        {
            playerIndex = value;
            Characteristics.Animator.AnimationColorIndex = playerIndex % Characteristics.Animator.AnimationColorCount;
        }
    }

    public GroupType Group { get; set; }
    public TankCharacteristicSet Characteristics { get; set; }
    public float HelmetTimer;
    public bool Invulnerable => HelmetTimer > 0;
    public bool IsDestroyed { get; private set; }

    public static event EventHandler TankCreated;
    public static event EventHandler TankDestroyed;
    public static event EventHandler EnemyTankDestroyed;

    void Awake()
    {
        Assert.IsNotNull(ResourceManager.s_Instance.BulletPrefab);
        Assert.IsNotNull(ResourceManager.s_Instance.BigExplosionPrefab);
        Assert.IsNotNull(invulnerabilityPrefab);

        tankMovement = GetComponent<TankMovement>();

        Characteristics = new TankCharacteristicSet(GetComponent<PlayerTankAnimator>());

        Group = GroupType.Players;
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);

        tankMovement.Velocity = Characteristics.Velocity;

        ammoLeft = 1;

        Characteristics.UpdateAmmo = (appendix) => ammoLeft += appendix;
    }

    void Update()
    {
        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;

        if (freezeTime > 0)
        {           
            freezeTime -= Time.deltaTime;
        }
        else if (GetComponent<Blinking>())
        {
            tankMovement.Velocity = Characteristics.Velocity;
            GetComponent<SpriteRenderer>().enabled = true;
            Destroy(GetComponent<Blinking>());
        }

        if (Invulnerable)
        {
            HelmetTimer -= Time.deltaTime;
            if (!tempInvulnerabilityPrefab)
                tempInvulnerabilityPrefab = Instantiate(invulnerabilityPrefab, transform.position, Quaternion.identity);

            tempInvulnerabilityPrefab.transform.position = transform.position;
        }
        else if (tempInvulnerabilityPrefab)
        {
            Destroy(tempInvulnerabilityPrefab);
            tempInvulnerabilityPrefab = null;
        }
    }

    void LateUpdate()
    {
        if (IsDestroyed)
            Destroy(gameObject);
    }

    public void Shoot()
    {
        if (!CanShoot)
            return;

        ammoLeft--;
        shootDelay = Characteristics.ShootDelay;

        IBullet bulletComponent =
            Instantiate(ResourceManager.s_Instance.BulletPrefab, transform.position, transform.rotation)
            .GetComponent<IBullet>();

        if (bulletComponent != null)
        {
            bulletComponent.CanDestroyConcrete = Characteristics.CanDestroyConcrete;
            bulletComponent.CanDestroyForest = Characteristics.CanDestroyForest;
            bulletComponent.Direction = Direction;
            bulletComponent.Velocity = Characteristics.BulletVelocity;
            bulletComponent.Group = this.Group;
            bulletComponent.Owner = this;
        }

        AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.Shoot);
    }

    public void FreezeFor(float freezeTime)
    {
        gameObject.AddComponent<Blinking>();
        tankMovement.Velocity = 0;
        this.freezeTime = freezeTime;
    }

    public void Destroy()
    {
        if (IsDestroyed)
            return;

        AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.PlayerExplosion);
        Instantiate(ResourceManager.s_Instance.BigExplosionPrefab, transform.position, Quaternion.identity);

        if (tempInvulnerabilityPrefab)
            Destroy(tempInvulnerabilityPrefab);

        IsDestroyed = true;
    }

    void OnDestroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public bool OnHit(IBullet bullet)
    {
        if (bullet.Group == GroupType.Enemies && !Invulnerable)
        {
            if (Characteristics.HasGun)
            {
                Characteristics.HasGun = false;
                Characteristics.StarBonusLevel = 1;
                Characteristics.Recalculate();
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.TankHit);
            }
            else Destroy();
        }

        return true;
    }

    public void OnMyBulletHit(IBullet bullet, List<IBulletTarget> targets)
    {
        ammoLeft++;
        shootDelay = 0;

        if (targets == null)
            return;

        foreach (var target in targets)
        {
            if (target is EnemyTank)
            {
                EnemyTank enemyTank = target as EnemyTank;
                if (enemyTank.IsDestroyed)
                    EnemyTankDestroyed?.Invoke(this, new EnemyTankDestroyedEventArgs(enemyTank));
            }
        }
    }
}

public class EnemyTankDestroyedEventArgs : EventArgs
{
    public EnemyTankDestroyedEventArgs(EnemyTank enemyTank)
    {
        Tank = enemyTank;
    }

    public EnemyTank Tank { get; set; }
}