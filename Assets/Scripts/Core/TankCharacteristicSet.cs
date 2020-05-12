using System;
using System.Collections.Generic;
using UnityEngine;
using static PickUp;

public class TankCharacteristicSet
{
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public float ShootDelay { get; set; }
    public bool CanDestroyConcrete => (StarBonusLevel == 2 || HasGun);
    public bool CanDestroyForest => HasGun;
    private bool hasGun;
    public bool HasGun
    {
        get => hasGun;
        set
        {
            if (hasGun == value) return;

            int appendix = value ? 1 : -1;
            hasGun = value;

            if (!(starBonusLevel == 2))
                UpdateAmmo(appendix);

            starBonusLevel = 0;
        }
    }

    private int starBonusLevel;
    public int StarBonusLevel
    {
        get => starBonusLevel;
        set
        {
            if (starBonusLevel == value || hasGun) return;

            int appendix = value < starBonusLevel ? -1 : 1;
            int prevStarLevel = starBonusLevel;
            starBonusLevel = value;

            if (!(prevStarLevel + value == 1))
                UpdateAmmo(appendix);
        }
    }

    public PlayerTankAnimator Animator;

    public Action<int> UpdateAmmo;

    public void Recalculate()
    {
        Velocity = 5.4f;
        BulletVelocity = 9f * (StarBonusLevel > 0 || HasGun ? 2 : 1);
        ShootDelay = 0.2f;

        UpdateTankAppearance();
    }

    private void UpdateTankAppearance()
    {
        if (Animator is null) return;

        PlayerTankAnimator.TankLevelType
            currentAnimationLevelType = default;

        if (HasGun)
            currentAnimationLevelType = PlayerTankAnimator.TankLevelType.Heavy;
        else
            if (StarBonusLevel == 1)
            currentAnimationLevelType = PlayerTankAnimator.TankLevelType.Light;
        else
            if (StarBonusLevel == 2)
            currentAnimationLevelType = PlayerTankAnimator.TankLevelType.Medium;

        //Animator.LevelType = (PlayerTankAnimator.TankLevelType)(int)(Animator.LevelType) + 1;
        Animator.LevelType = currentAnimationLevelType;
    }

    public TankCharacteristicSet(PlayerTankAnimator animator)
    {
        Animator = animator;
        Recalculate();
    }
}

