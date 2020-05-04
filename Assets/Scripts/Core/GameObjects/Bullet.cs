﻿using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour, IBullet
{
    public Direction Direction { get; set; }
    public ITank Owner { get; set; }

    public float Velocity { get; set; } = 5.4f;
    public GameObject explosionPrefab;

    private CircleCollider2D circleCollider;
    private int obstaclesMask = 0;
    private bool needCreateExplosion = true;

    public float Radius
    {
        get { return circleCollider != null ? circleCollider.radius : 0.0f; }
    }

    public int ObstaclesMask
    {
        get { return obstaclesMask; }
    }

    public GroupType Group { get; set; }

    void Awake()
    {
        Assert.IsNotNull(explosionPrefab);
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete", "LevelBorder", "Tank", "Bullet", "EagleFortress");
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + Velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(Direction) - 90.0f);

        var obstacles = Physics2D.OverlapCircleAll(transform.position, Radius, ObstaclesMask);

        bool destroyCurrent = false;

        foreach (var obstacle in obstacles)
        {
            IBulletTarget bulletTarget = obstacle.gameObject.GetComponent<IBulletTarget>();

            if (bulletTarget?.Group != this.Group)
            {
                if (!bulletTarget.OnHit(this))
                    continue;

                destroyCurrent = true;

                if (bulletTarget is ITank)
                    break;
            }
            else continue;
        }

        if (destroyCurrent)
        {
            Owner?.OnMyBulletHit(this);
            Die();
        }
    }

    public void Die()
    {
        if (needCreateExplosion)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public bool OnHit(IBullet bullet)
    {
        needCreateExplosion = false;
        Die();
        return true;
    }
}

