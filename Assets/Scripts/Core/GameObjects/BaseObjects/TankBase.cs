using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankBase : Dummy, IMovable, ICombat
{
    public abstract void MoveDown();

    public abstract void MoveLeft();

    public abstract void MoveRight();

    public abstract void MoveUp();

    public abstract void Shoot();
}
