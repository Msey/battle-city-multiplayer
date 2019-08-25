using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit, IMovable
{
    public new bool isVulnerable { get; protected set; }
    public new bool isAlive { get; protected set; }
    public bool IsConstantMovement => true;

    public float Speed => 10f;

    public ICombat Owner;
    public override void Die()
    {
        Owner.UpdateAmmo();
        MovementSystem.s_Instance.RemoveUnit(this);
        Destroy(gameObject);
    }
    
    private void Start()
    {
       var collider = this.gameObject.AddComponent<BoxCollider2D>();
        this.gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("triggered: "+ this.GetType().ToString());
        this.Die();
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
