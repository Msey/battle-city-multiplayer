using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Tank,
        Helmet,
        Star,
        Shovel,
        Clock,
        Grenade,
        Pistol
    };

    public PickUpType Type;

    public Sprite Sprite;

    private float radius;

    private void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }
    void Update()
    {
        var tank = Physics2D.OverlapCircle(transform.position, radius)
            .GetComponent<ITank>();

        if (tank != null)
        {
            switch (Type)
            {
                case
                 PickUpType.Tank:
                    {
                        tank.Characteristics.AddLife();
                        print(tank.Characteristics.GetTotalLives());
                    }
                    break;
            }

            Destroy(gameObject);
        }
    }
}
