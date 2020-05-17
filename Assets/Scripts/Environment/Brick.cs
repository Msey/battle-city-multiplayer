using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(Animator))]
public class Brick : Environment
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

    public override bool OnHit(IBullet bullet)
    {
        if (brickState != BrickState.Full || bullet.CanDestroyConcrete)
            DieBy(bullet);
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

            if (bullet.Group == GroupType.Players)
                AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.TargetHit);
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
