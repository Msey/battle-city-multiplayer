﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class TankMovement : MonoBehaviour
{
    //256px tank = 0.16 unity
    //1px dendy tank = 0,000625 unity
    
    GameConstants.eDirection direction = GameConstants.eDirection.Up;
    public GameConstants.eDirection Direction
    {
        get
        {
            return direction;
        }
        set
        {
            if (direction == value)
                return;

            if (IsDirectionAxisChanged(direction, value))
            {
                Vector2 cellPosition = CellPosition();
                Vector2 newPosition = transform.position;
                if (IsVerticalAxis(value))
                    newPosition.x = cellPosition.x;
                else //Horizontal Axis
                    newPosition.y = cellPosition.y;
                transform.position = newPosition;
            }

            direction = value;
            animator.SetTrigger(DirectionAnimationTrigger(direction));
        }
    }

    bool stoped = true;
    public bool Stoped
    {
        get
        {
            return stoped;
        }
        set
        {
            if (stoped == value)
                return;

            if (!stoped)
                animator.SetFloat("Velocity", 0.0f);
            else
                animator.SetFloat("Velocity", velocity);
            stoped = value;
        }
    }
    public float velocity = 0.0f;

    CircleCollider2D circleCollider;
    Animator animator;
    int obstaclesMask = 0;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Velocity", 0.0f);
        obstaclesMask = LayerMask.GetMask("Water", "Brick", "Concrete");
    }

    void Update()
    {
        if (Stoped)
            return;

        Vector2 oldCellPosition = CellPosition();
        transform.position = (Vector2)transform.position + velocity * DirectionVector() * Time.deltaTime;
        UpdateColliderPoition();

        var obstacle = Physics2D.OverlapCircle(FrontCellPosition(), circleCollider.radius, obstaclesMask);
        if (obstacle != null)
        {
            transform.position = oldCellPosition;
            UpdateColliderPoition();
        }
    }


    void UpdateColliderPoition()
    {
        var colliderPosition = transform.InverseTransformPoint(CellPosition());
        circleCollider.offset = ColliderPosition();
    }

    private Vector2 ColliderPosition()
    {
        return transform.InverseTransformPoint(CellPosition());
    }

    private Vector2 DirectionVector()
    {
        switch (Direction)
        {
            case GameConstants.eDirection.Up:
                return Vector2.up;
            case GameConstants.eDirection.Down:
                return Vector2.down;
            case GameConstants.eDirection.Left:
                return Vector2.left;
            case GameConstants.eDirection.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    private static string DirectionAnimationTrigger(GameConstants.eDirection dir)
    {
        switch (dir)
        {
            case GameConstants.eDirection.Up:
                return "Up";
            case GameConstants.eDirection.Down:
                return "Down";
            case GameConstants.eDirection.Left:
                return "Left";
            case GameConstants.eDirection.Right:
                return "Right";
        }
        return "";
    }

    private float DirectionAngle()
    {
        return Vector2.Angle(Vector2.up, DirectionVector());
    }

    private Vector2 CellPosition()
    {
        const float smallOffset = 0.00001f;
        Vector2 currentPosition = transform.position;
        switch (Direction)
        {
            case GameConstants.eDirection.Up:
                currentPosition.y += smallOffset;
                break;
            case GameConstants.eDirection.Down:
                currentPosition.y -= smallOffset;
                break;
            case GameConstants.eDirection.Left:
                currentPosition.x -= smallOffset;
                break;
            case GameConstants.eDirection.Right:
                currentPosition.x += smallOffset;
                break;
        }

        return new Vector2(
            Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
            Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
    }

    private Vector2 FrontCellPosition()
    {
        Vector2 currentPosition = transform.position;

        switch (Direction)
        {
            case GameConstants.eDirection.Up:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.CeilByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.eDirection.Down:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.FloorByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.eDirection.Left:
                return new Vector2(
                    Utils.FloorByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.eDirection.Right:
                return new Vector2(
                    Utils.CeilByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
        }
        return Vector2.zero;
    }

    static bool IsDirectionAxisChanged(GameConstants.eDirection oldDir, GameConstants.eDirection newDir)
    {
        return IsVerticalAxis(oldDir) != IsVerticalAxis(newDir);
    }

    static bool IsVerticalAxis(GameConstants.eDirection dir)
    {
        return dir == GameConstants.eDirection.Up || dir == GameConstants.eDirection.Down;
    }

    static bool IsHorizontalAxis(GameConstants.eDirection dir)
    {
        return !IsVerticalAxis(dir);
    }
}
