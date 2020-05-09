using System;
using UnityEngine;
using static GameConstants;

public class PickUp : MonoBehaviour
{
    public PickUpType Type;

    private Vector2 collisionSize = new Vector2(CELL_SIZE, CELL_SIZE);
    int tankMask;

    private void Start()
    {
        tankMask = LayerMask.GetMask("Tank");
    }
    void Update()
    {
        var tank = Physics2D.OverlapBox(transform.position, collisionSize, 0, tankMask);

        if (!tank) return;

        PlayerTank playerTank = tank.GetComponent<PlayerTank>();
        if (playerTank != null)
            HandlePlayerTank(playerTank);
        else
        {
            EnemyTank enemyTank = tank.GetComponent<EnemyTank>();
            HandleEnemyTank(enemyTank);
        }

        Destroy(gameObject);
    }

    void HandlePlayerTank(PlayerTank tank)
    {
        if (tank == null)
            return;

        switch (Type)
        {
            case PickUpType.Tank:
                ClassicGameManager.s_Instance.AddLife();
                print(ClassicGameManager.s_Instance.GetTotalLives());
                break;
            case PickUpType.Star:
                if (tank.Characteristics.StarBonusLevel < 2)
                    tank.Characteristics.StarBonusLevel = tank.Characteristics.StarBonusLevel + 1;
                break;
            case PickUpType.Pistol:
                tank.Characteristics.HasGun = true;
                break;
            case PickUpType.Grenade:
                foreach (var enemy in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                    enemy.Destroy();
                break;
            case PickUpType.Helmet:
                tank.HelmetTimer = 10f;
                break;
            case PickUpType.Clock:
                ClassicGameManager.s_Instance.EnemyTanksAISystem.SleepFor(10f);
                break;
            case PickUpType.Shovel:
                foreach (var eagle in ClassicGameManager.s_Instance.Eagles)
                    eagle.ActivateShowelEffect(true);
                break;
        }
        tank.Characteristics.Recalculate();
    }

    void HandleEnemyTank(EnemyTank tank)
    {
        if (tank == null)
            return;

        switch (Type)
        {
            case PickUpType.Tank:
            case PickUpType.Helmet:
                foreach (EnemyTank enemy in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                {
                    if (enemy.IsBounusTank)
                    {
                        enemy.IsBounusTank = false;
                        enemy.ArmorLevel = EnemyTank.MaxArmorLevel;
                    }
                    else
                    {
                        enemy.IsBounusTank = true;
                    }
                }
                break;
            case PickUpType.Star:
                foreach (EnemyTank enemy in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                    enemy.CanDestroyConcrete = true;
                break;
            case PickUpType.Pistol:
                tank.ArmorLevel = EnemyTank.MaxArmorLevel;
                tank.CanDestroyConcrete = true;
                break;
            case PickUpType.Grenade:
                foreach (PlayerTank player in ClassicGameManager.s_Instance.ActivePlayerTanks)
                    player.Destroy();
                break;
            case PickUpType.Clock:
                print("Enemy take clock pickup");
                break;
            case PickUpType.Shovel:
                foreach (var eagle in ClassicGameManager.s_Instance.Eagles)
                    eagle.ActivateShowelEffect(false);
                break;
        }
    }
}
