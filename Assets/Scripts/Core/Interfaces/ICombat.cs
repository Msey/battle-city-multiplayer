using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat 
{
    void Shoot();

    float AmmoLimit { get; }

    float ShootDelay{ get; }
}
