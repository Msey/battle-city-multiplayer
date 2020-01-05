using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TestTankController : MonoBehaviour, ICombat
{
    float shootDelay = 0.0f;
    int ammoLimit = 0;
    public GameObject bulletPrefab;
    TankMovement tankMovement;

    public int AMMO_LIMIT_CONSTANT => 2;
    const float SHOOT_DELAY_CONSTANT = 0.6f;

    public float ShootDelay => SHOOT_DELAY_CONSTANT;
    public int AmmoLimit => AMMO_LIMIT_CONSTANT;

    private bool isMoving;

    void Start()
    {
        tankMovement = GetComponent<TankMovement>();

        ammoLimit = AmmoLimit;
    }

    void Update()
    {
        UpdateMovement();

        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    public void UpdateMovement(Direction direction = Direction.Undefined, float verticalAxis = 0, float horizontalAxis = 0)
    {
        isMoving = true;
        if (verticalAxis + horizontalAxis == 0 && direction == Direction.Undefined)
        {
            verticalAxis = Input.GetAxis("Vertical");
            horizontalAxis = Input.GetAxis("Horizontal");

            tankMovement.Stoped = false;
            if (verticalAxis > 0.0f)
                tankMovement.Direction = Direction.Up;
            else if (verticalAxis < 0.0f)
                tankMovement.Direction = Direction.Down;
            else if (horizontalAxis < 0.0f)
                tankMovement.Direction = Direction.Left;
            else if (horizontalAxis > 0.0f)
                tankMovement.Direction = Direction.Right;
            else
                tankMovement.Stoped = true;
        }
        else
        {
            tankMovement.Stoped = false;
            tankMovement.Direction = direction;
        }
        isMoving = false;
    }

    public void Shoot()
    {
        if (shootDelay <= 0 && ammoLimit > 0)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            // TODO: bulletPrefab must be loaded from Object inspector due 
            // to active bullet script attached to the object on scene
            var bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.Owner = this;
            if (bulletComponent)
            {
                bulletComponent.Direction = tankMovement.Direction;
            }

            shootDelay = SHOOT_DELAY_CONSTANT;
        }
    }

    public void UpdateAmmo()
    {
        ammoLimit++;
        shootDelay = 0;
    }
}
