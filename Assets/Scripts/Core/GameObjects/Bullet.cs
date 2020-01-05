using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletStrength
{
    Standart,
    ConcreteOne,
    ConcreteTwo
}

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public eDirection Direction { get; set; } = eDirection.Right;
    CircleCollider2D circleCollider;
    public float velocity = 5.4f;
    private int obstaclesMask = 0;

    public float Radius
    {
        get { return circleCollider != null ? circleCollider.radius : 0.0f; }
    }

    public int ObstaclesMask
    {
        get { return obstaclesMask; }
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete");

        BulletBehaviour.AddBullet(this);
    }


    public void Die()
    {
        Destroy(gameObject);
    }
}
