using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PickUpType Type;

    private void Start()
    {
    }
    void Update()
    {
        var tank = Physics2D.OverlapCircle(transform.position, 0.16f)
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
