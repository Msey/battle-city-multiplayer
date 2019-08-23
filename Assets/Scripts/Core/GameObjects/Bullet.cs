using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit, IMovable
{
    public new bool isVulnerable { get; protected set; }
    public new bool isAlive { get; protected set; }

    public bool IsConstantMovement { get; set; }

    public float Speed => 10f;

    public override void Die()
    {
        Destroy(this, 1f);
    }
    

    public override void TakeDamade(int amount)
    {
    }


    public void MoveDown()
    {
        direction = MovementSystem.Direction.Down;
        Move();
    }

    public void MoveLeft()
    {
        direction = MovementSystem.Direction.Left;
        Move();
    }

    public void MoveRight()
    {
        direction = MovementSystem.Direction.Right;
        Move();
    }

    public void MoveUp()
    {
        direction = MovementSystem.Direction.Up;
        Move();
    }

    public void Move()
    {
        MovementSystem.s_Instance.AddUnit(this);
    }
}
