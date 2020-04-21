using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class PlayerTankCreatedEvent
{
    public PlayerTankCreatedEvent(PlayerTank tank)
    {
        Tank = tank;
    }

    public readonly PlayerTank Tank;
}

class PlayerTankDestroyedEvent
{
    public PlayerTankDestroyedEvent(PlayerTank tank)
    {
        Tank = tank;
    }

    public readonly PlayerTank Tank;
}

[RequireComponent(typeof(TankMovement),typeof(PlayerTankAnimator))]
public class PlayerTank : MonoBehaviour, ITank
{
    float shootDelay = 0.0f;
    public GameObject bulletPrefab;
    TankMovement tankMovement;
    PlayerTankAnimator tankAnimator;

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

    public int PlayerIndex { get; set; } = 0;

    void Awake()
    {
        if (EventManager.s_Instance != null)
            EventManager.s_Instance.TriggerEvent<PlayerTankCreatedEvent>(new PlayerTankCreatedEvent(this));
    }

    void Start()
    {
        tankMovement = GetComponent<TankMovement>();
        tankAnimator = GetComponent<PlayerTankAnimator>();
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

    void ChangeTankLevel()
    {
        tankAnimator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(tankAnimator.LevelType) + 1;
    }

    private void Destroy()
    {
        if (EventManager.s_Instance != null)
            EventManager.s_Instance.TriggerEvent<PlayerTankDestroyedEvent>(new PlayerTankDestroyedEvent(this));
        Destroy(gameObject);
    }
}
