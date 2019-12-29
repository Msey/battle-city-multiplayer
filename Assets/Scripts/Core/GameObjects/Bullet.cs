﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit, IMovable
{
    const float BULLET_TRIGGER_OVERLAPPING = 0.18f;
    public new bool isVulnerable { get; protected set; }
    public new bool isAlive { get; protected set; }
    public bool IsConstantMovement => true;

    public float Speed => 10f;

    bool hitOccured = false;

    public ICombat Owner;
    public override void Die()
    {
        Owner.UpdateAmmo();
        Destroy(gameObject);
    }

    private void Start()
    {
        var collider = this.gameObject.AddComponent<BoxCollider2D>();
        Rigidbody2D rb;
        if (!(rb = gameObject.GetComponent<Rigidbody2D>()))
            rb = this.gameObject.AddComponent<Rigidbody2D>();


        rb.isKinematic = true;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (hitOccured) return;
        //print("triggered: "+ this.GetType().ToString());

        var hitSource = collision.GetComponent<GameUnit>();
        if (hitSource)
            OnHit(hitSource);

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
        MovementSystem.s_Instance.Move(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, BULLET_TRIGGER_OVERLAPPING);
        Gizmos.color = Color.white;
    }

    public override void OnHit(GameUnit hitSource)
    {

        print("OnHit");

        var entryObjects = Physics2D.OverlapCircleAll((Vector2)transform.position, BULLET_TRIGGER_OVERLAPPING);
        
        if (entryObjects.Length > 0)
            print(entryObjects.Length);



        Die();


        //foreach(var obj in entryObjects)
        //{
        //    obj.GetComponent<Bull>
        //}

        Bullet sourceBullet;
        if (sourceBullet = hitSource.GetComponent<Bullet>())
            sourceBullet.Die();// ?. operator probably
        else hitSource.OnHit(this as GameUnit);
    }
}
