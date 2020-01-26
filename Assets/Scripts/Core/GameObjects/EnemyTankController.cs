﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EnemyTankCreatedEvent
{
}

[RequireComponent(typeof(TankMovement),typeof(EnemyTankAnimator))]
public class EnemyTankController : MonoBehaviour
{
    float shootDelay = 0.0f;
    public GameObject bulletPrefab;
    TankMovement tankMovement;
    EnemyTankAnimator tankAnimator;

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

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if (Input.GetKeyDown(KeyCode.F2))
            ChangeTankLevel();
    }

    void UpdateMovement()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        tankMovement.Stoped = false;
        if (verticalAxis > 0.0f)
            tankMovement.Direction = GameConstants.Direction.Up;
        else if (verticalAxis < 0.0f)
            tankMovement.Direction = GameConstants.Direction.Down;
        else if (horizontalAxis < 0.0f)
            tankMovement.Direction = GameConstants.Direction.Left;
        else if (horizontalAxis > 0.0f)
            tankMovement.Direction = GameConstants.Direction.Right;
        else
            tankMovement.Stoped = true;
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
        //tankAnimator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(tankAnimator.LevelType) + 1;
    }
}