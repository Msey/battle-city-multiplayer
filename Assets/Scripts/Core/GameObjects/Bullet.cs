using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit, IMovable
{
    public override bool isVulnerable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override bool isAlive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool IsConstantMovement { get; set; }

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
