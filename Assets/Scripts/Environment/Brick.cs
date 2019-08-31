using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    public override void Die()
    {
        Destroy(gameObject);
    }
    public override void OnHit(GameObject hitSource)
    {
        Die();
    }
}
