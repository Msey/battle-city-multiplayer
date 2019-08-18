using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Dummy, IMovable
{
    public override bool isVulnerable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override bool isAlive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Die()
    {
    }

    public void MoveDown()
    {
    }

    public void MoveLeft()
    {
    }

    public void MoveRight()
    {
    }

    public void MoveUp()
    {
    }

    public override void TakeDamade(int amount)
    {
    }
}
