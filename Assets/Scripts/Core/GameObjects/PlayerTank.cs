using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : TankBase
{
    public override bool isVulnerable { get => true; set { } }
    public override bool isAlive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Die()
    {
    }

    public override void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).gameObject.UseComponent<Bullet>();
        bullet.IsConstantMovement = true;
        bullet.direction = this.direction;
        bullet.Move();
    }

    public override void TakeDamade(int amount)
    {
    }
}
