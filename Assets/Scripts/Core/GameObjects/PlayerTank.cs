using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : TankBase
{
    public new bool isVulnerable { get => true; set { } }
    public new bool isAlive { get => true; }


    const float SHOOT_DELAY_CONSTANT = 0.6f; // TODO: need to be replaced with Level_Upgrade_Constants (later probably)

    public override void Die()
    {
    }

    public override void Shoot()
    {
        if (ammoLimit > 0 && ShootDelay <= 0)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).gameObject.UseComponent<Bullet>();
            //bullet.Owner = this;
            //bullet.direction = this.direction;
            //bullet.Move();
            ammoLimit--;
            shootDelay = SHOOT_DELAY_CONSTANT;
            print(ammoLimit);
        }
    }

    void Awake()
    {
        ammoLimit = 2;
        shootDelay = 0;
    }

    void Update()
    {
        if (shootDelay > 0)
            shootDelay -= Time.deltaTime;
    }

    public override void OnHit(GameUnit hitSource)
    {
    }
}
