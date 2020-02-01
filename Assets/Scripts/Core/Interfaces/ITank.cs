using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITank
{
    GameConstants.Direction Direction { get; set; }

    bool Stopped { get; set; }

    void Shoot();
}