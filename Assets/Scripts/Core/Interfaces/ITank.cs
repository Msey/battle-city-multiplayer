﻿using static GameConstants;

public interface ITank : IBulletTarget
{
    Direction Direction { get; set; }

    bool Stopped { get; set; }

    void Shoot();
}