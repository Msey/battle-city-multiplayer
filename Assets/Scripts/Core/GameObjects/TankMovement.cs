using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class TankMovement : MonoBehaviour
{
    //256px tank = 0.16 unity
    //1px dendy tank = 0,000625 unity
    
    eDirection direction = eDirection.Up;
    public eDirection Direction
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
        transform.position = (Vector2)transform.position + velocity * GameConstants.DirectionVector(Direction) * Time.deltaTime;
        UpdateColliderPoition();

        var obstacle = Physics2D.OverlapCircle(FrontCellPosition(), ColliderScaledRadius(), obstaclesMask);
        if (obstacle != null)
        {
            transform.position = oldCellPosition;
            UpdateColliderPoition();
        }
    }

    float ColliderScaledRadius()
    {
        return transform.lossyScale.x * circleCollider.radius * 0.75f;
    }

    void UpdateColliderPoition()
    {
        circleCollider.offset = ColliderPosition();
    }

    private Vector2 ColliderPosition()
    {
        return transform.InverseTransformPoint(CellPosition());
    }

    private static string DirectionAnimationTrigger(eDirection dir)
    {
        switch (dir)
        {
            case eDirection.Up:
                return "Up";
            case eDirection.Down:
                return "Down";
            case eDirection.Left:
                return "Left";
            case eDirection.Right:
                return "Right";
        }
        return "";
    }

    private Vector2 CellPosition()
    {
        const float smallOffset = 0.00001f;
        Vector2 currentPosition = transform.position;
        switch (Direction)
        {
            case eDirection.Up:
                currentPosition.y += smallOffset;
                break;
            case eDirection.Down:
                currentPosition.y -= smallOffset;
                break;
            case eDirection.Left:
                currentPosition.x -= smallOffset;
                break;
            case eDirection.Right:
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
            case eDirection.Up:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.CeilByFactor(currentPosition.y, GameConstants.cellSize));
            case eDirection.Down:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.FloorByFactor(currentPosition.y, GameConstants.cellSize));
            case eDirection.Left:
                return new Vector2(
                    Utils.FloorByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
            case eDirection.Right:
                return new Vector2(
                    Utils.CeilByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
        }
        return Vector2.zero;
    }

    static bool IsDirectionAxisChanged(eDirection oldDir, eDirection newDir)
    {
        return IsVerticalAxis(oldDir) != IsVerticalAxis(newDir);
    }

    static bool IsVerticalAxis(eDirection dir)
    {
        return dir == eDirection.Up || dir == eDirection.Down;
    }

    static bool IsHorizontalAxis(eDirection dir)
    {
        return !IsVerticalAxis(dir);
    }
}
