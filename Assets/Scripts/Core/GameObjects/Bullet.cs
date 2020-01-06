using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public GameConstants.Direction Direction { get; set; } = GameConstants.Direction.Right;
    CircleCollider2D circleCollider;
    public float velocity = 5.4f;
    int obstaclesMask = 0;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete");
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(Direction));

        var obstacles = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius, obstaclesMask);
        foreach(var obstacle in obstacles)
        {
            var bulletTarget = (obstacle.gameObject.GetComponent<IBulletTarget>());
            if (bulletTarget != null)
                bulletTarget.OnHit(this);
        }

        if (obstacles.Length != 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
