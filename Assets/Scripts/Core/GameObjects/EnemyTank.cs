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

    float shootDelay;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    TankMovement tankMovement;
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

    public EntityRelationGroup Group { get; set; }
    public TankCharacteristicSet Characteristics { get; set; }


    static public event EventHandler TankCreated;
    static public event EventHandler TankDestroyed;

    private void Awake()
    {
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(explosionPrefab);

        Group = new EntityRelationGroup(this);
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<EnemyTankAnimator>();
        Characteristics = new TankCharacteristicSet();
    }

    void Start()
    {
        TankCreated?.Invoke(this, EventArgs.Empty);
        tankMovement.Velocity = Characteristics.Velocity;
    }

    void Update()
    {
        UpdateMovement();

        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;
    }

    void UpdateMovement()
    {
        /*
         * TODO:
         *      1. code below must be removed
         *      2. here should be an AI controller
         *      3. ???
         */

        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        tankMovement.Stopped = false;
        if (verticalAxis > 0.0f)
            tankMovement.Direction = Direction.Up;
        else if (verticalAxis < 0.0f)
            tankMovement.Direction = Direction.Down;
        else if (horizontalAxis < 0.0f)
            tankMovement.Direction = Direction.Left;
        else if (horizontalAxis > 0.0f)
            tankMovement.Direction = Direction.Right;
        else
            tankMovement.Stopped = true;
    }

    void ChangeTankLevel()
    {
        //tankAnimator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(tankAnimator.LevelType) + 1;
    }

    public void Shoot()
    {
        const float SHOOT_DELAY_CONSTANT = 0.6f; // TODO: need to be replaced with Level_Upgrade_Constants (later probably)
        if (shootDelay <= 0)
        {
            IBullet bulletComponent =
                Instantiate(bulletPrefab, transform.position, transform.rotation)
                .GetComponent<IBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Direction = Direction;
                bulletComponent.Group = new EntityRelationGroup(this);
                bulletComponent.Owner = this;
            }

            shootDelay = SHOOT_DELAY_CONSTANT;
        }
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

    public bool OnHit(IBullet bullet)
    {
        Destroy();
        return true;
    }

    public void OnMyBulletHit(IBullet bullet)
    {

    }
}
