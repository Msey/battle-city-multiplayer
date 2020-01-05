using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TestTankController : MonoBehaviour
{
    float shootDelay = 0.0f;
    public GameObject bulletPrefab;
    TankMovement tankMovement;
    void Start()
    {
        tankMovement = GetComponent<TankMovement>();
    }

    void Update()
    {
        UpdateMovement();

        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void UpdateMovement()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        tankMovement.Stoped = false;
        if (verticalAxis > 0.0f)
            tankMovement.Direction = eDirection.Up;
        else if (verticalAxis < 0.0f)
            tankMovement.Direction = eDirection.Down;
        else if (horizontalAxis < 0.0f)
            tankMovement.Direction = eDirection.Left;
        else if (horizontalAxis > 0.0f)
            tankMovement.Direction = eDirection.Right;
        else
            tankMovement.Stoped = true;
    }

    public void Shoot()
    {

        const float SHOOT_DELAY_CONSTANT = 0.6f; // TODO: need to be replaced with Level_Upgrade_Constants (later probably)
        if (shootDelay <= 0)
        { 
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            // TODO: bulletPrefab must be loaded from Object inspector due 
            // to active bullet script attached to the object on scene
            var bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent)
                bulletComponent.Direction = tankMovement.Direction;
            shootDelay = SHOOT_DELAY_CONSTANT;
        }
    }
}
