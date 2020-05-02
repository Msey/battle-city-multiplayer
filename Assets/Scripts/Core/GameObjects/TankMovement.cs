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
                if (!stopped)
                    animator.SetFloat("Velocity", 0.0f);
                else
                    animator.SetFloat("Velocity", Velocity);
                stopped = value;
            }
        }
    }
    public float Velocity { get; set; } = 0.0f;
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

        if (!makeMovement)
        {
            transform.position = oldCellPosition;
            UpdateColliderPosition();
        }
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
            Utils.RoundByFactor(currentPosition.x, cellSize),
            Utils.RoundByFactor(currentPosition.y, cellSize));
    }

    private Vector2 FrontCellPosition()
    {
        Vector2 currentPosition = transform.position;

        switch (Direction)
        {
            case Direction.Up:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, cellSize),
                    Utils.CeilByFactor(currentPosition.y, cellSize));
            case Direction.Down:
                return new Vector2(
                    Utils.RoundByFactor(currentPosition.x, cellSize),
                    Utils.FloorByFactor(currentPosition.y, cellSize));
            case Direction.Left:
                return new Vector2(
                    Utils.FloorByFactor(currentPosition.x, cellSize),
                    Utils.RoundByFactor(currentPosition.y, cellSize));
            case Direction.Right:
                return new Vector2(
                    Utils.CeilByFactor(currentPosition.x, cellSize),
                    Utils.RoundByFactor(currentPosition.y, cellSize));
        }
        return Vector2.zero;
    }

}
