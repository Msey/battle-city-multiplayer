using System;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(TankMovement), typeof(EnemyTankAnimator))]
public class EnemyTank : MonoBehaviour, ITank
{
    public enum EnemyTankType
    {
        Basic,
        Fast,
        Power,
        Armor,
    }

    [SerializeField]
    EnemyTankType tankType = EnemyTankType.Basic;
    public EnemyTankType TankType
    {
        get => tankType;
        set
        {
            tankType = value;
            tankAnimator.tankIndex = (int)tankType;
        }
    }

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    private TankMovement tankMovement;
    public TankMovement TankMovement
    {
        get => tankMovement;
    }

    EnemyTankAnimator tankAnimator;

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

    public GroupType Group { get; set; }
    public int TankIndex { get; set; }

    bool canShoot = true;

    static public event EventHandler TankCreated;
    static public event EventHandler TankDestroyed;

    private void Awake()
    {
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(explosionPrefab);

        Group = GroupType.Enemies;
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<EnemyTankAnimator>();
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);
        tankMovement.Velocity = 5.4f;
    }

    public void Shoot()
    {
        if (canShoot)
        {
            IBullet bulletComponent =
                Instantiate(bulletPrefab, transform.position, transform.rotation)
                .GetComponent<IBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Direction = Direction;
                bulletComponent.Group = this.Group;
                bulletComponent.Owner = this;
                bulletComponent.Velocity = 16.0f;
            }

            canShoot = false;
        }
    }

    public bool Destroy()
    {
        try
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        catch
        {
            Debug.Log("Exception occured while destroying an enemy tank");
            return false;
        }

        return true;
    }

    void OnDestroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public bool OnHit(IBullet bullet)
    {
        return Destroy(); 
    }

    public void OnMyBulletHit(IBullet bullet)
    {
        canShoot = true;
    }
}
