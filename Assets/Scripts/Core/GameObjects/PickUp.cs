using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PickUpType Type;

    private Vector2 collisionSize = new Vector2(0.17f, 0.17f);
    int tankMask;

    private void Start()
    {
        tankMask = LayerMask.GetMask("Tank");
    }
    void Update()
    {
        var tank = Physics2D.OverlapBox(transform.position, collisionSize, 0, tankMask)?
            .GetComponent<PlayerTank>();

        if (tank != null)
        {
            switch (Type)
            {
                case PickUpType.Tank:
                    ClassicGameManager.s_Instance.AddLife();
                    print(ClassicGameManager.s_Instance.GetTotalLives());
                    break;
                case PickUpType.Star:
                    if (tank.Characteristics.StarBonusLevel < 2)
                        tank.Characteristics.StarBonusLevel = tank.Characteristics.StarBonusLevel + 1;
                    break;
                case PickUpType.Pistol:
                    tank.Characteristics.HasGun = true;
                    break;
                case PickUpType.Grenade:
                    ClassicGameManager.s_Instance
                        .ActiveEnemyTanks.RemoveWhere(enemy => enemy.Destroy());
                    break;
                case PickUpType.Helmet:
                    tank.HelmetTimer = 10f;
                    break;
                case PickUpType.Clock:
                    ClassicGameManager.s_Instance.EnemyTanksAISystem.SleepFor(10f);
                    break;
            }
            tank.Characteristics.Recalculate();
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
