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
    public GameConstants.Direction Direction { get; set; } = GameConstants.Direction.Right;

    public BulletStrength Power = BulletStrength.Standart;

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

    ICombat owner;
    public ICombat Owner
    {
        get { return owner; }
        set { print("set owner"); owner = value; }
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete");

        BulletBehaviour.s_Instance.AddBullet(this);
    }


    public void Die()
    {
        print("destroyed");
        //Owner.UpdateAmmo();
        Destroy(this.gameObject);
    }
}
