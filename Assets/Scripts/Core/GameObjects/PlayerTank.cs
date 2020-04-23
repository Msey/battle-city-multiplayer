using System;
using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(TankMovement),typeof(PlayerTankAnimator))]
public class PlayerTank : MonoBehaviour, ITank
{
    public GameObject bulletPrefab;

    private float shootDelay = 0.0f;
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
        Group = new EntityRelationGroup(this);
        TankCreated?.Invoke(this, EventArgs.Empty);
    }

    void Start()
    {
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<PlayerTankAnimator>();
    }

    void Update()
    {
        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;
    }

    public void Shoot()
    {
        const float SHOOT_DELAY_CONSTANT = 0.6f; // TODO: need to be replaced with Level_Upgrade_Constants (later probably)
        if (shootDelay <= 0)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            var bulletComponent = bullet.GetComponent<IBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Direction = tankMovement.Direction;
                bulletComponent.Owner = this;
                bulletComponent.Group = new EntityRelationGroup(this);
            }
            shootDelay = SHOOT_DELAY_CONSTANT;
        }
    }

    void ChangeTankLevel()
    {
        tankAnimator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(tankAnimator.LevelType) + 1;
    }

    private void Destroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void OnHit(IBullet bullet)
    {
        print(1);
    }
}
