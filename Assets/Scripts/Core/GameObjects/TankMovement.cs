using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class TankMovement : MonoBehaviour
{
    //256px tank = 0.16 unity
    //1px dendy tank = 0,000625 unity
    
    GameConstants.Direction direction = GameConstants.Direction.Up;
    public GameConstants.Direction Direction
    {
        get
        {
            return direction;
        }
        set
        {
            if (direction == value)
                return;

            if (GameUtils.IsDirectionAxisChanged(direction, value))
            {
                Vector2 cellPosition = CellPosition();
                Vector2 newPosition = transform.position;
                if (GameUtils.IsVerticalAxis(value))
                    newPosition.x = cellPosition.x;
                else //Horizontal Axis
                    newPosition.y = cellPosition.y;
                transform.position = newPosition;
            }

            direction = value;
            if (animator)
                animator.SetInteger("Direction", (int) direction);
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

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Velocity", 0.0f);
        animator.SetInteger("Direction", (int)direction);
        obstaclesMask = LayerMask.GetMask("Water", "Brick", "Concrete");
    }

    void Update()
    {
        if (Stoped)
            return;

        Vector2 oldCellPosition = CellPosition();
        transform.position = (Vector2)transform.position + velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
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

    private static int DirectionAnimationValue(GameConstants.Direction dir)
    {
        switch (dir)
        {
            case GameConstants.Direction.Up:
                return 0;
            case GameConstants.Direction.Down:
                return 1;
            case GameConstants.Direction.Left:
                return 2;
            case GameConstants.Direction.Right:
                return 3;
        }
        return 0;
    }

    private Vector2 CellPosition()
    {
        const float smallOffset = 0.00001f;
        Vector2 currentPosition = transform.position;
        switch (Direction)
        {
            case GameConstants.Direction.Up:
                currentPosition.y += smallOffset;
                break;
            case GameConstants.Direction.Down:
                currentPosition.y -= smallOffset;
                break;
            case GameConstants.Direction.Left:
                currentPosition.x -= smallOffset;
                break;
            case GameConstants.Direction.Right:
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
            case GameConstants.Direction.Up:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.CeilByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.Direction.Down:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.FloorByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.Direction.Left:
                return new Vector2(
                    Utils.FloorByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
            case GameConstants.Direction.Right:
                return new Vector2(
                    Utils.CeilByFactor(currentPosition.x, GameConstants.cellSize),
                    Utils.RoundByFactor(currentPosition.y, GameConstants.cellSize));
        }
        return Vector2.zero;
    }

}
