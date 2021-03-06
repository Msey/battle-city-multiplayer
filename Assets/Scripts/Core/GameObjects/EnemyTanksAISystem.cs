﻿using System.Collections;
using UnityEngine;
using static GameConstants;

public class EnemyTanksAISystem
{
    private ClassicGameManager gameManager;
    private float freezeTime;

    float FrameScale
    {
        get => Time.deltaTime / (1.0f / 60.0f);
    }

    float SystemPeriod
    {
        get => Time.timeSinceLevelLoad % 256;
    }

    public EnemyTanksAISystem(ClassicGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Start()
    {
        if (gameManager == null)
            return;
        gameManager.StartCoroutine(UpdateSystem());
    }

    public void FreezeFor(float freezeTime) => this.freezeTime = freezeTime;

    IEnumerator UpdateSystem()
    {
        while (true)
        {
            if (freezeTime > 0)
                freezeTime -= Time.deltaTime;

            if (freezeTime < 0)
                freezeTime = 0;

            foreach (EnemyTank tank in gameManager.ActiveEnemyTanks)
                HandleTank(tank);
            yield return new WaitForSeconds(1.0f / 60.0f);
        }
    }

    void HandleTank(EnemyTank tank)
    {
        if (freezeTime > 0)
        {
            tank.Stopped = true;
            return;
        }
        tank.Stopped = false;
        if (TankIsOnCellPosition(tank) && BooleanRand(0.0625f))
        {
            ChangeTankDirection(tank);
        }
        else if (tank.TankMovement.HasBarrier && BooleanRand(0.25f))
        {
            if (BooleanRand(0.7f))
                tank.Direction = GameUtils.InvertDirection(tank.Direction);
            else
                ChangeTankDirection(tank);
        }

        if (BooleanRand(0.03125f))
            tank.Shoot();
    }

    void ChangeTankDirection(EnemyTank tank)
    {
        if (BooleanRand(tank.TankMovement.HasBarrier ? 0.0625f : 0.5f))
            ChangeTankDirection2(tank);
        else if (BooleanRand(0.5f))
            tank.TankMovement.Direction = GameUtils.ClockwiseDirection(tank.TankMovement.Direction);
        else
            tank.TankMovement.Direction = GameUtils.CounterClockwiseDirection(tank.TankMovement.Direction);
    }

    void ChangeTankDirection2(EnemyTank tank)
    {
        float periodDuration = gameManager.RespawnTime * 60 / 8;
        if (SystemPeriod < periodDuration)
        {
            tank.Direction = ComputeRandomDirection(tank.TankMovement.Direction);
        }
        else if (SystemPeriod < 2 * periodDuration)
        {
            PlayerTank targetTank = ComputeTargetTank(tank.TankIndex);
            if (targetTank != null)
                tank.TankMovement.Direction = GameUtils.DirectionToTarget(tank.transform, targetTank.transform);
            else
                tank.Direction = ComputeRandomDirection(tank.TankMovement.Direction);
        }
        else
        {
            Eagle eagle = ComputeTargetEagle(tank.TankIndex);
            if (eagle != null)
                tank.TankMovement.Direction = GameUtils.DirectionToTarget(tank.transform, eagle.transform);
            else
                tank.Direction = ComputeRandomDirection(tank.TankMovement.Direction);
        }
    }

    bool BooleanRand(float probability)
    {
        return Random.Range(0.0f, 1.0f) < probability * 1.0f; //FrameScale;
    }

    bool TankIsOnCellPosition(EnemyTank tank)
    {
        Vector2 tankCellCoordinate = new Vector2(
            Utils.RoundByFactor(tank.transform.position.x, CELL_SIZE / 4),
            Utils.RoundByFactor(tank.transform.position.y, CELL_SIZE / 4));

        return (tankCellCoordinate.x % CELL_SIZE == 0
            && tankCellCoordinate.y % CELL_SIZE == 0);
    }

    Direction ComputeRandomDirection(Direction oldDirection)
    {
        float randValue = Random.Range(0.0f, 1.0f);
        Direction newDirection = oldDirection;
        if (randValue < 0.25f)
            newDirection = Direction.Down;
        else if (randValue < 0.5f)
            newDirection = Direction.Up;
        else if (randValue < 0.75f)
            newDirection = Direction.Right;
        else
            newDirection = Direction.Left;

        if (newDirection == oldDirection)
            return ComputeRandomDirection(oldDirection);
        return newDirection;
    }

    Eagle ComputeTargetEagle(int tankIndex)
    {
        var eagles = gameManager.Eagles;
        if (eagles == null)
            return null;

        if (eagles.Count == 0)
            return null;
        return eagles[tankIndex % eagles.Count];
    }

    PlayerTank ComputeTargetTank(int tankIndex)
    {
        var playerTanks = gameManager.ActivePlayerTanks;
        if (playerTanks == null)
            return null;

        if (playerTanks.Count == 0)
            return null;
        return playerTanks.GetEnumerator().Current; //TODO what?
    }
}
