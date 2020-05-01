using UnityEngine;

public class PickUp : MonoBehaviour
{
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
                case PickUpType.Tank:
                    ClassicGameManager.s_Instance.AddLife();
                    print(ClassicGameManager.s_Instance.GetTotalLives());
                    break;
                case PickUpType.Star:
                case PickUpType.Pistol:
                    tank.Characteristics.AddUpgrade(new Upgrade(Type));
                    break;
            }

            Destroy(gameObject);
        }
    }

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
}
