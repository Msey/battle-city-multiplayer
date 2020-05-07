﻿using System;
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

    public bool IsDestroyed { get; private set; }

    bool canShoot = true;

    int armorLevel;
    public int ArmorLevel
    {
        get => armorLevel;
        set
        {
            if (armorLevel < 0)
                armorLevel = 0;

            if (armorLevel == value)
                return;

            armorLevel = value;
            tankAnimator.ArmorColor = ArmorLevelToArmorColor(armorLevel);
        }
    }

    bool isBounusTank;
    public bool IsBounusTank
    {
        get => isBounusTank;
        set
        {
            if (isBounusTank == value)
                return;

            isBounusTank = value;
            tankAnimator.Blinking = isBounusTank;
        }
    }

    static public event EventHandler TankCreated;
    static public event EventHandler TankDestroyed;
    static public event EventHandler BonusTankHit;

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

    void LateUpdate()
    {
        if (IsDestroyed)
            Destroy(gameObject);
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

    public void Destroy()
    {
        if (IsDestroyed)
            return;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        IsDestroyed = true;
    }

    void OnDestroy()
    {
        TankDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public bool OnHit(IBullet bullet)
    {
        if (IsBounusTank)
        {
            IsBounusTank = false;
            BonusTankHit?.Invoke(this, EventArgs.Empty);
            return true;
        }

        if (ArmorLevel == 0)
            Destroy();
        else
            ArmorLevel--;

        return true;
    }

    public void OnMyBulletHit(IBullet bullet)
    {
        canShoot = true;
    }

    static EnemyTankAnimator.eArmorColor ArmorLevelToArmorColor(int level)
    {
        switch (level)
        {
            case 0: return EnemyTankAnimator.eArmorColor.Default;
            case 1: return EnemyTankAnimator.eArmorColor.Yellow;
            case 2: return EnemyTankAnimator.eArmorColor.Brown;
            case 3: return EnemyTankAnimator.eArmorColor.Green;
        }
        return EnemyTankAnimator.eArmorColor.Default;
    }
}
