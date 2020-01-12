using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : Singleton<BulletBehaviour>
{
    static HashSet<Bullet> bullets;
    void Start()
    {
        bullets = new HashSet<Bullet>();
    }

    void Update()
    {
        var removedBullets = new List<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            bullet.transform.position = (Vector2)bullet.transform.position + bullet.velocity * GameUtils.DirectionVector(bullet.Direction) * Time.deltaTime;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(bullet.Direction));
            var obstacles = Physics2D.OverlapCircleAll(bullet.transform.position, bullet.Radius, bullet.ObstaclesMask);
            foreach (var obstacle in obstacles)
            {
                var bulletTarget = (obstacle.gameObject.GetComponent<IBulletTarget>());
                if (bulletTarget != null)
                    bulletTarget.OnHit(bullet);
            }

            if (obstacles.Length > 0)
            {
#if DEBUG
                print("bullet Die();");
#endif
                bullet.Die();
                removedBullets.Add(bullet);
            }
        }

        foreach (Bullet removedBullet in removedBullets)
            bullets.Remove(removedBullet);
#if DEBUG
            print("Total bullets: " + bullets.Count);
#endif
    }

    public void AddBullet(Bullet bullet)
    {
        if (bullet != null && bullets != null)
            bullets.Add(bullet);
    }
}
