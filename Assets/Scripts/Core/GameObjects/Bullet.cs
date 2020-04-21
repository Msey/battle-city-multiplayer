﻿using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public Direction Direction { get; set; } = Direction.Right;

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
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(Direction));
        var obstacles = Physics2D.OverlapCircleAll(transform.position, Radius, ObstaclesMask);
        foreach (var obstacle in obstacles)
        {
            var bulletTarget = (obstacle.gameObject.GetComponent<IBulletTarget>());
            if (bulletTarget != null)
                bulletTarget.OnHit(this);
        }

        if (obstacles.Length > 0)
        {
#if DEBUG
            print("bullet Die();");
#endif
            Die();
 
        }
    }

    public void Die()
    {
        //TODO: in updateammo tell owner that bullet has been destroyed
        Destroy(this.gameObject);
    }
}
