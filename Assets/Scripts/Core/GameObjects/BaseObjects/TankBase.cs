using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MovementSystem))]
public abstract class TankBase : Dummy, IMovable, ICombat
{
    public virtual bool IsConstantMovement { get => false; }

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

    private void Move()
    {
        MovementSystem.s_Instance.AddDummy(this);
    }

    public abstract void Shoot();
}
