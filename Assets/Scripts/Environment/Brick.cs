using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class Brick : GameUnit
{
    enum BrickState
    {
        Full,
        Left,
        Right,
        Top,
        Bottom,
    }

    private BrickState brickState = BrickState.Full;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
    public override void OnHit(GameUnit hitSource)
    {
        if (!hitSource)
            return;

        if (brickState != BrickState.Full)
            Die();
        else
        {
            switch (hitSource.direction)
            {
                case MovementSystem.Direction.Down:
                    SetBrickState(BrickState.Bottom);
                    break;
                case MovementSystem.Direction.Up:
                    SetBrickState(BrickState.Top);
                    break;
                case MovementSystem.Direction.Left:
                    SetBrickState(BrickState.Left);
                    break;
                case MovementSystem.Direction.Right:
                    SetBrickState(BrickState.Right);
                    break;
            }
        }
    }

    void SetBrickState(BrickState brickState)
    {
        this.brickState = brickState;
        switch (brickState)
        {
            case BrickState.Left:
                animator.SetTrigger("OnRightHit");
                break;
            case BrickState.Right:
                animator.SetTrigger("OnLeftHit");
                break;
            case BrickState.Top:
                animator.SetTrigger("OnBottomHit");
                break;
            case BrickState.Bottom:
                animator.SetTrigger("OnTopHit");
                break;
        }
    }
}
