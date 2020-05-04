using System.Collections.Generic;
using UnityEngine;
using static PickUp;

public class TankCharacteristicSet
{
    public int BulletStrength { get; set; } 
    public float Velocity { get; set; } 
    public float BulletVelocity { get; set; } 
    public int AmmoLimit { get; set; }
    public float ShootDelay { get; set; }
    public bool HasGun { get; set; }
    public int StarBonusLevel { get; set; }

    public PlayerTankAnimator Animator;

    public void Recalculate()
    {
        BulletStrength = HasGun ? 2 : (StarBonusLevel > 0 ? StarBonusLevel : 1);
        Velocity = 5.4f;
        BulletVelocity = 8f * (StarBonusLevel > 0 || HasGun ? 2 : 1);
        AmmoLimit = 1 + (HasGun ? 2 : (StarBonusLevel > 0 ? 1 : 0));
        ShootDelay = 1f - (HasGun ? 0.6f : (StarBonusLevel > 0 ? StarBonusLevel * 0.3f : 0));

        Debug.Log(
          $"BulletStrength = {BulletStrength} " +
          $"BulletVelocity = {BulletVelocity} " +
          $"AmmoLimit = {AmmoLimit}");

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

