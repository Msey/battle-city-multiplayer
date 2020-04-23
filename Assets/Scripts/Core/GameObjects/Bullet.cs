using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour, IBullet
{
    public Direction Direction { get; set; }
    public ITank Owner { get; set; }

    public float velocity = 5.4f;

    private CircleCollider2D circleCollider;
    private int obstaclesMask = 0;

    public float Radius
    {
        get { return circleCollider != null ? circleCollider.radius : 0.0f; }
    }

    public int ObstaclesMask
    {
        get { return obstaclesMask; }
    }

    public EntityRelationGroup Group { get; set; }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        obstaclesMask = LayerMask.GetMask("Brick", "Concrete", "LevelBorder", "Tank", "Bullet");
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + velocity * GameUtils.DirectionVector(Direction) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, GameUtils.DirectionAngle(Direction));

        var obstacles = Physics2D.OverlapCircleAll(transform.position, Radius, ObstaclesMask);

        bool destroyCurrent = false;

        foreach (var obstacle in obstacles)
        {
            IBulletTarget other = obstacle.gameObject.GetComponent<IBulletTarget>();

            if (other != null)
            {
                if (other.Group != this.Group)
                {
                    other.OnHit(this);
                    destroyCurrent = true;
                }
                else continue;
            }
        }

        if (destroyCurrent) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnHit(IBullet bullet)
    {
    }
}

