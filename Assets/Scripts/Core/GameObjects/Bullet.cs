using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour, IBullet
{
    public Direction Direction { get; set; }
    public ITank Owner { get; set; }

    public float Velocity { get; set; } = 5.4f;

    public bool CanDestroyConcrete { get; set; }
    public bool CanDestroyForest { get; set; }

    private CircleCollider2D circleCollider;
    private int obstaclesMask;
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
        Assert.IsNotNull(ResourceManager.s_Instance.SmallExplosionPrefab);
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete", "LevelBorder", "Tank", "Bullet", "EagleFortress", "Forest");
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + Velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(Direction) - 90.0f);

        var obstacles = Physics2D.OverlapCircleAll(transform.position, Radius, ObstaclesMask);

        bool destroyCurrent = false;
        List<IBulletTarget> targets = null;

        foreach (var obstacle in obstacles)
        {
            if (obstacle.gameObject == this.gameObject)
                continue;

            IBulletTarget bulletTarget = obstacle.gameObject.GetComponent<IBulletTarget>();

            if (bulletTarget?.Group != this.Group || bulletTarget is Bullet)
            {
                if (!bulletTarget.OnHit(this))
                    continue;

                destroyCurrent = true;

                if (targets == null)
                    targets = new List<IBulletTarget>();
                targets.Add(bulletTarget);

                if (bulletTarget is ITank)
                    break;
            }
        }

        if (destroyCurrent)
            Die(targets);
    }

    private void LateUpdate()
    {
        if (Owner == null)
            return;

        if (Owner.IsDestroyed)
            Owner = null;
    }

    public void Die(List<IBulletTarget> targets)
    {
        if (needCreateExplosion)
            Instantiate(ResourceManager.s_Instance.SmallExplosionPrefab, transform.position, Quaternion.identity);
        Owner?.OnMyBulletHit(this, targets);
        Destroy(gameObject);
    }

    public bool OnHit(IBullet bullet)
    {
        needCreateExplosion = false;
        Die(new List<IBulletTarget>());
        return true;
    }
}

