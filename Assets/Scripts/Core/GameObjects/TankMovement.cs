﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class TankMovement : MonoBehaviour
{
    //256px tank = 0.16 unity
    //1px dendy tank = 0,000625 unity

    [SerializeField]
    Direction direction;
    public Direction Direction
    {
        get
        {
            return direction;
        }
        set
        {
            if (direction != value)
            {

                if (GameUtils.IsDirectionAxisChanged(direction, value))
                {
                    Vector2 cellPosition = CellPosition();
                    Vector2 newPosition = transform.position;

                    if (GameUtils.IsVerticalAxis(value))
                        newPosition.x = cellPosition.x;
                    else
                        newPosition.y = cellPosition.y;

                    transform.position = newPosition;
                }

                direction = value;
                if (animator)
                    animator.SetInteger("Direction", (int)direction);
                UpdateAnimator();
            }
        }
    }

    bool stopped = true;
    public bool Stopped
    {
        get
        {
            return stopped;
        }
        set
        {
            if (stopped != value)
            {
                stopped = value;
                if (stopped)
                    animator.SetFloat("Velocity", 0.0f);
                else
                    animator.SetFloat("Velocity", Velocity);
                UpdateAnimator();
            }
        }
    }

    private bool hasBarrier;
    public bool HasBarrier
    {
        get => hasBarrier;
    }
    public float Velocity { get; set; } = 0.0f;

    public bool BlockMovement { get; set; } = false;

    [SerializeField]
    private bool TransparentForTanks = true;

    CircleCollider2D circleCollider;
    Animator animator;
    int obstaclesMask = 0;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Velocity", 0.0f);
        animator.SetInteger("Direction", DirectionAnimationValue(direction));
        obstaclesMask = LayerMask.GetMask("Water", "Brick", "Concrete", "LevelBorder", "Tank", "EagleFortress");
    }

    void Update()
    {
        hasBarrier = false;
        if (Stopped)
            return;

        Vector2 oldCellPosition = CellPosition();
        transform.position = (Vector2)transform.position + Velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        UpdateColliderPosition();

        Collider2D[] obstacles = Physics2D.OverlapCircleAll(FrontCellPosition(), ColliderScaledRadius(), obstaclesMask);

        bool makeMovement = true;
        foreach (var obstacle in obstacles)
        {
            if (obstacle.gameObject == this.gameObject)
                continue;

            if (TransparentForTanks)
            {
                TankMovement obstacleTankMovement = obstacle.GetComponent<TankMovement>();
                if (obstacleTankMovement != null)
                {
                    obstacleTankMovement.TransparentForTanks = true;
                    obstacleTankMovement.hasBarrier = true;
                }
                else
                {
                    makeMovement = false;
                    TransparentForTanks = false;
                    break;
                }
            }
            else
            {
                makeMovement = false;
                break;
            }
        }

        if (obstacles.Length <= 1 && TransparentForTanks)
            TransparentForTanks = false;

        if (!makeMovement || BlockMovement)
        {
            transform.position = oldCellPosition;
            UpdateColliderPosition();
            hasBarrier = true;
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        animator.SetFloat("Velocity", 1.0f);
        animator.Update(1.0f);
        if (stopped)
            animator.SetFloat("Velocity", 0.0f);
        else
            animator.SetFloat("Velocity", Velocity);
    }

    float ColliderScaledRadius()
    {
        return transform.lossyScale.x * circleCollider.radius * 0.75f;
    }

    void UpdateColliderPosition()
    {
        circleCollider.offset = ColliderPosition();
    }

    private Vector2 ColliderPosition()
    {
        return transform.InverseTransformPoint(CellPosition());
    }

    private static int DirectionAnimationValue(Direction dir) => (int)dir;

    private Vector2 CellPosition()
    {
        const float smallOffset = 0.00001f;
        Vector2 currentPosition = transform.position;
        switch (Direction)
        {
            case Direction.Up:
                currentPosition.y += smallOffset;
                break;
            case Direction.Down:
                currentPosition.y -= smallOffset;
                break;
            case Direction.Left:
                currentPosition.x -= smallOffset;
                break;
            case Direction.Right:
                currentPosition.x += smallOffset;
                break;
        }

        return new Vector2(
            Utils.RoundByFactor(currentPosition.x, CELL_SIZE),
            Utils.RoundByFactor(currentPosition.y, CELL_SIZE));
    }

    private Vector2 FrontCellPosition()
    {
        Vector2 currentPosition = transform.position;

        switch (Direction)
        {
            case Direction.Up:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, CELL_SIZE),
                    Utils.CeilByFactor(currentPosition.y, CELL_SIZE));
            case Direction.Down:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, CELL_SIZE),
                    Utils.FloorByFactor(currentPosition.y, CELL_SIZE));
            case Direction.Left:
                return new Vector2(
                    Utils.FloorByFactor(currentPosition.x, CELL_SIZE),
                    Utils.RoundByFactor(currentPosition.y, CELL_SIZE));
            case Direction.Right:
                return new Vector2(
                    Utils.CeilByFactor(currentPosition.x, CELL_SIZE),
                    Utils.RoundByFactor(currentPosition.y, CELL_SIZE));
        }
        return Vector2.zero;
    }

}
