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
            return (shootDelay <= 0 && ammoLeft > 0);
        }
    }

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
            tankAnimator.AnimationColorIndex = playerIndex % tankAnimator.AnimationColorCount;
        }
    }

    public EntityRelationGroup Group { get; set; }
    public TankCharacteristicSet Characteristics { get; set; }

    public static event EventHandler TankCreated;
    public static event EventHandler TankDestroyed;

    void Awake()
    {
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(explosionPrefab);
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<PlayerTankAnimator>();
        Group = new EntityRelationGroup(this);
        Characteristics = new TankCharacteristicSet();
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);

        tankMovement.Owner = this;

        ammoLeft = Characteristics.AmmoLimit;
    }

    void Update()
    {
        //UpdateMovement();

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
                bulletComponent.Group = new EntityRelationGroup(this);
                bulletComponent.Owner = this;
            }

            
        }
    }

    void ChangeTankLevel()
    {
        tankAnimator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(tankAnimator.LevelType) + 1;
    }

    private void Destroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public void OnHit(IBullet bullet)
    {

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
