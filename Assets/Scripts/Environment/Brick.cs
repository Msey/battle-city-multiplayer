using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(Animator))]
public class Brick : MonoBehaviour, IBulletTarget
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

    public GroupType Group { get; set; }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    public bool OnHit(IBullet bullet)
    {
        if (bullet.CanDestroyConcrete)
        {
            Die();
            return true;
        }

        if (brickState != BrickState.Full)
            Die();
        else
        {
            switch (bullet.Direction)
            {
                case Direction.Down:
                    SetBrickState(BrickState.Bottom);
                    break;
                case Direction.Up:
                    SetBrickState(BrickState.Top);
                    break;
                case Direction.Left:
                    SetBrickState(BrickState.Left);
                    break;
                case Direction.Right:
                    SetBrickState(BrickState.Right);
                    break;
            }
        }

        return true;
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
