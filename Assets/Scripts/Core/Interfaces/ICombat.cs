using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat 
{
    void Shoot();

    int AmmoLimit { get; }

    float ShootDelay{ get; }

    void UpdateAmmo();
}
