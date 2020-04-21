using System;
using UnityEngine;
using static GameConstants;

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

    static public event EventHandler TankCreated;
    static public event EventHandler TankDestroyed;

    void Awake()
    {
        TankCreated?.Invoke(this, new EventArgs());
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
        TankDestroyed?.Invoke(this, new EventArgs());
        Destroy(gameObject);
    }
}
