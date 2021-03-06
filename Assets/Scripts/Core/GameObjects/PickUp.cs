﻿using System;
using UnityEngine;
using static GameConstants;

public class PickUp : MonoBehaviour
{
    public PickUpType Type;

    private int tankMask;
    private Vector2 collisionSize
        = new Vector2(CELL_SIZE, CELL_SIZE);

    private void Start()
    {
        tankMask = LayerMask.GetMask("Tank");
        AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.BonusAppearance);
    }
    private void Update()
    {
        var tank = Physics2D.OverlapBox(transform.position, collisionSize, 0, tankMask);

        if (!tank) return;

        PlayerTank playerTank = tank.GetComponent<PlayerTank>();
        EnemyTank enemyTank = tank.GetComponent<EnemyTank>();

        HandleAnyTankPickup(playerTank, enemyTank);
        if (Type == PickUpType.Tank)
            AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.LiveTaken);
        else
            AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.BonusTaken);
        Destroy(gameObject);
    }

    private void HandleAnyTankPickup(PlayerTank player, EnemyTank enemy)
    {
    RESWITCH:
        switch (Type)
        {
            case PickUpType.Tank:
                {
                    if (player)
                        ClassicGameManager.s_Instance.AddLife();
                    else
                    {
                        Type = PickUpType.Helmet;
                        goto RESWITCH;
                    }
                    break;
                }

            case PickUpType.Star:
                {
                    if (player)
                    {
                        if (player.Characteristics.StarBonusLevel < 2)
                            player.Characteristics.StarBonusLevel = player.Characteristics.StarBonusLevel + 1;
                    }
                    else
                    {
                        foreach (EnemyTank enemyTank in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                            enemyTank.CanDestroyConcrete = true;
                    }
                    break;
                }

            case PickUpType.Pistol:
                {
                    if (player)
                        player.Characteristics.HasGun = true;
                    else
                    {
                        enemy.ArmorLevel = EnemyTank.MaxArmorLevel;
                        enemy.CanDestroyConcrete = true;
                        enemy.CanDestroyForest = true;
                    }

                    break;
                }

            case PickUpType.Grenade:
                {
                    if (player)
                        foreach (var enemyTank in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                            enemyTank.Destroy();
                    else
                        foreach (PlayerTank playerTank in ClassicGameManager.s_Instance.ActivePlayerTanks)
                            playerTank.Destroy();

                    break;
                }

            case PickUpType.Helmet:
                {
                    if (enemy)
                        foreach (EnemyTank enemyTank in ClassicGameManager.s_Instance.ActiveEnemyTanks)
                        {
                            if (enemyTank.IsBounusTank)
                            {
                                enemyTank.IsBounusTank = false;
                                enemyTank.ArmorLevel = EnemyTank.MaxArmorLevel;
                            }
                            else
                                enemyTank.IsBounusTank = true;
                        }
                    else
                        player.HelmetTimer = 10f;
                    break;
                }

            case PickUpType.Clock:
                {
                    if (player)
                        ClassicGameManager.s_Instance.EnemyTanksAISystem.FreezeFor(10f);
                    else
                        foreach (PlayerTank playerTank in ClassicGameManager.s_Instance.ActivePlayerTanks)
                            playerTank.FreezeFor(10f);
                    break;
                }

            case PickUpType.Shovel:
                {
                    foreach (var eagle in ClassicGameManager.s_Instance.Eagles)
                        eagle.ActivateShowelEffect(player != null);
                    break;
                }
        }

        player?.Characteristics.Recalculate();
    }
}
