using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : PersistentSingleton<ResourceManager>
{
    [HideInInspector] public GameObject TankBonusPrefab;
    [HideInInspector] public GameObject StarBonusPrefab;
    [HideInInspector] public GameObject ShovelBonusPrefab;
    [HideInInspector] public GameObject HelmetBonusPrefab;
    [HideInInspector] public GameObject PistolBonusPrefab;
    [HideInInspector] public GameObject GrenadeBonusPrefab;
    [HideInInspector] public GameObject ClockBonusPrefab;


    private void Start()
    {
        TankBonusPrefab = Resources.Load<GameObject>("PickUps/Tank");
        StarBonusPrefab = Resources.Load<GameObject>("PickUps/Star");
        ShovelBonusPrefab = Resources.Load<GameObject>("PickUps/Shovel");
        HelmetBonusPrefab = Resources.Load<GameObject>("PickUps/Helmet");
        PistolBonusPrefab = Resources.Load<GameObject>("PickUps/Pistol");
        GrenadeBonusPrefab = Resources.Load<GameObject>("PickUps/Grenade");
        ClockBonusPrefab = Resources.Load<GameObject>("PickUps/Clock");
    }
}
