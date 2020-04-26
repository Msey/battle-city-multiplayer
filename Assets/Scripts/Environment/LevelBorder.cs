﻿using UnityEngine;

public class LevelBorder : MonoBehaviour, IBulletTarget
{
    public EntityRelationGroup Group { get; set; } 

    void Start()
    {
        Group = new EntityRelationGroup(this);
    }

    public bool OnHit(IBullet bullet)
    {
        return true;
    }
}