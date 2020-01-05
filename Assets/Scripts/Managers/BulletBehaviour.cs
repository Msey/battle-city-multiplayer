using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    static List<Bullet> bullets;
    void Start()
    {
        bullets = new List<Bullet>();
    }

    void Update()
    {
        for ( int i = 0; i < bullets.Count; i++)
        {
            bullets[i].transform.position = (Vector2)bullets[i].transform.position + bullets[i].velocity * GameConstants.DirectionVector(bullets[i].Direction) * Time.deltaTime;
            bullets[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameConstants.DirectionAngle(bullets[i].Direction));
            var obstacles = Physics2D.OverlapCircleAll(bullets[i].transform.position, bullets[i].Radius, bullets[i].ObstaclesMask);
            foreach (var obstacle in obstacles)
            {
                var bulletTarget = (obstacle.gameObject.GetComponent<IBulletTarget>());
                if (bulletTarget != null)
                    bulletTarget.OnHit(bullets[i]);
            }

            if (obstacles.Length > 0)
            {
                bullets[i].Die();
                bullets.Remove(bullets[i]);
            }
        }
#if DEBUG
        print(bullets.Count);
#endif
    }

    public static void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }



}
