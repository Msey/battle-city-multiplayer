using System;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(TankMovement), typeof(PlayerTankAnimator))]
public class PlayerTank : MonoBehaviour, ITank
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;


    private TankMovement tankMovement;
    private PlayerTankAnimator tankAnimator;

    public bool CanShoot
    {
        get
        {
            return (!isDead && shootDelay <= 0 && ammoLeft > 0);
        }
    }

    private bool isDead;
    float shootDelay;
    private int ammoLeft;

    public Direction Direction
    {
        get => tankMovement.Direction;
        set => tankMovement.Direction = value;
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

    public static event EventHandler TankCreated;
    public static event EventHandler TankDestroyed;

    void Awake()
    {
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(explosionPrefab);
        tankMovement = GetComponent<TankMovement>();

        Characteristics = new TankCharacteristicSet(GetComponent<PlayerTankAnimator>());

        Group = GroupType.Players;
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);

        tankMovement.Velocity = Characteristics.Velocity;

        ammoLeft = Characteristics.AmmoLimit;
    }

    void Update()
    {
        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;
    }


    public void Shoot()
    {
        if (CanShoot)
        {
            ammoLeft--;
            shootDelay = Characteristics.ShootDelay;

            IBullet bulletComponent =
                Instantiate(bulletPrefab, transform.position, transform.rotation)
                .GetComponent<IBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Direction = Direction;
                bulletComponent.Velocity = Characteristics.BulletVelocity;
                bulletComponent.Group = this.Group;
                bulletComponent.Owner = this;
            }
        }
        else if (isDead)
            ClassicGameManager.s_Instance.RespawnPlayer(playerIndex);
    }    

    private void Destroy()
    {
        isDead = true;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public bool OnHit(IBullet bullet)
    {
        if (bullet.Owner.Group == GroupType.Enemies)
        {
            if (Characteristics.HasGun)
            {
                Characteristics.HasGun = false;
                Characteristics.StarBonusLevel = 1;
                Characteristics.Recalculate();
            }
            else Destroy();
        }

        return true;
    }

    public void OnMyBulletHit(IBullet bullet)
    {
        ammoLeft++;

        if (ammoLeft >= Characteristics.AmmoLimit)
        {
            shootDelay = 0;
            ammoLeft = Characteristics.AmmoLimit;
        }
    }
}
