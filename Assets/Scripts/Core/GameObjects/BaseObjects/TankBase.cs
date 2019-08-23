using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MovementSystem))]
public abstract class TankBase : GameUnit, IMovable, ICombat
{
    public Transform bulletPrefab;

    public virtual bool IsConstantMovement { get => false; }

    public float Speed => 5f;

    protected float ammoLimit;
    public float AmmoLimit => ammoLimit;

    protected float shootDelay;
    public float ShootDelay => shootDelay;

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
        MovementSystem.s_Instance.AddUnit(this);
    }

    public abstract void Shoot();
}
