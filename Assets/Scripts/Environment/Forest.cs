﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : GameUnit
{
    public override void Die()
    {
        Destroy(gameObject);
    }
    public override void OnHit(GameUnit hitSource)
    {
        Die();
    }
}
