using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

class EnemyTankCreatedEvent
{
}

class EnemyTankDestroyedEvent
{
}

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

    float shootDelay = 0.0f;
    public GameObject bulletPrefab;
    TankMovement tankMovement;
    EnemyTankAnimator tankAnimator;

    public Direction Direction {
        get => tankMovement.Direction;
        set => tankMovement.Direction = value;
    }
    public bool Stopped
    {
        get => tankMovement.Stopped;
        set => tankMovement.Stopped = value;
    }

    private void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<EnemyTankAnimator>();

        if (EventManager.s_Instance != null)
            EventManager.s_Instance.TriggerEvent<EnemyTankCreatedEvent>(new EnemyTankCreatedEvent());
    }

    void Update()
    {
        UpdateMovement();

        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;
    }

    void UpdateMovement()
    {
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
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            var bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent)
                bulletComponent.Direction = tankMovement.Direction;
            shootDelay = SHOOT_DELAY_CONSTANT;
        }
    }

    private void Destroy()
    {
        if (EventManager.s_Instance != null)
            EventManager.s_Instance.TriggerEvent<EnemyTankDestroyedEvent>(new EnemyTankDestroyedEvent());
        Destroy(gameObject);
    }
}
