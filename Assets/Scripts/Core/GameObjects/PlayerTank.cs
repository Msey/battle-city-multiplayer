using System;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(TankMovement),typeof(PlayerTankAnimator))]
public class PlayerTank : MonoBehaviour, ITank
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private bool canShoot = true;
    private TankMovement tankMovement;
    private PlayerTankAnimator tankAnimator;

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

    public int PlayerIndex { get; set; }
    public EntityRelationGroup Group { get; set; }

    public static event EventHandler TankCreated;
    public static event EventHandler TankDestroyed;

    void Awake()
    {
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(explosionPrefab);
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<PlayerTankAnimator>();
        Group = new EntityRelationGroup(this);
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);
    }

    public void Shoot()
    {
        if (canShoot)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            var bulletComponent = bullet.GetComponent<IBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Direction = tankMovement.Direction;
                bulletComponent.Owner = this;
                bulletComponent.Group = new EntityRelationGroup(this);
            }
            canShoot = false;
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
        print(1);
    }

    public void OnBulletHit(IBullet bullet)
    {
        canShoot = true;
    }
}
