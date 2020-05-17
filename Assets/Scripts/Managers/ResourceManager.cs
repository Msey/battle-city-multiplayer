using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : PersistentSingleton<ResourceManager>
{
    [HideInInspector] public GameObject ImmortalityEffectPrefab;
    [HideInInspector] public GameObject SpawnPointPrefab;

    [HideInInspector] public GameObject TankBonusPrefab;
    [HideInInspector] public GameObject StarBonusPrefab;
    [HideInInspector] public GameObject ShovelBonusPrefab;
    [HideInInspector] public GameObject HelmetBonusPrefab;
    [HideInInspector] public GameObject PistolBonusPrefab;
    [HideInInspector] public GameObject GrenadeBonusPrefab;
    [HideInInspector] public GameObject ClockBonusPrefab;

    [HideInInspector] public GameObject BulletPrefab;
    [HideInInspector] public GameObject EnemyTankPrefab;
    [HideInInspector] public GameObject PlayerTankPrefab;

    [HideInInspector] public GameObject SmallExplosionPrefab;
    [HideInInspector] public GameObject BigExplosionPrefab;

    [HideInInspector] public GameObject BrickPrefab;   
    [HideInInspector] public GameObject ConcretePrefab;
    [HideInInspector] public GameObject ForestPrefab;
    [HideInInspector] public GameObject WaterPrefab;
    [HideInInspector] public GameObject IcePrefab;


    private void Start()
    {
        ImmortalityEffectPrefab = Resources.Load<GameObject>("Effects/ImmortalityEffect");
        SpawnPointPrefab = Resources.Load<GameObject>("Effects/SpawnPoint");

        TankBonusPrefab = Resources.Load<GameObject>("PickUps/Tank");
        StarBonusPrefab = Resources.Load<GameObject>("PickUps/Star");
        ShovelBonusPrefab = Resources.Load<GameObject>("PickUps/Shovel");
        HelmetBonusPrefab = Resources.Load<GameObject>("PickUps/Helmet");
        PistolBonusPrefab = Resources.Load<GameObject>("PickUps/Pistol");
        GrenadeBonusPrefab = Resources.Load<GameObject>("PickUps/Grenade");
        ClockBonusPrefab = Resources.Load<GameObject>("PickUps/Clock");

        BulletPrefab = Resources.Load<GameObject>("Tanks/Bullet");
        EnemyTankPrefab = Resources.Load<GameObject>("Tanks/EnemyTank");
        PlayerTankPrefab = Resources.Load<GameObject>("Tanks/PlayerTank");

        SmallExplosionPrefab = Resources.Load<GameObject>("Explosions/SmallExplosion");
        BigExplosionPrefab = Resources.Load<GameObject>("Explosions/BigExplosion");

        BrickPrefab = Resources.Load<GameObject>("Environment/Brick");
        ConcretePrefab = Resources.Load<GameObject>("Environment/Concrete");
        ForestPrefab = Resources.Load<GameObject>("Environment/Forest");
        WaterPrefab = Resources.Load<GameObject>("Environment/Water");
        IcePrefab = Resources.Load<GameObject>("Environment/Ice");
    }
}
