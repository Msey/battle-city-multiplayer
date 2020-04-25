using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class PlayerTankAnimation
{
    public AnimationClip[] tankette;
    public AnimationClip[] light;
    public AnimationClip[] medium;
    public AnimationClip[] heavy;

    public AnimationClip[] GetClips(PlayerTankAnimator.TankLevelType state)
    {
        switch (state)
        {
            case PlayerTankAnimator.TankLevelType.Tankette:
                return tankette;
            case PlayerTankAnimator.TankLevelType.Light:
                return light;
            case PlayerTankAnimator.TankLevelType.Medium:
                return medium;
            case PlayerTankAnimator.TankLevelType.Heavy:
                return heavy;
        }
        Assert.IsTrue(false);
        return tankette;
    }
}

[RequireComponent(typeof(Animator))]
public class PlayerTankAnimator : MonoBehaviour
{
    [SerializeField]
    PlayerTankAnimation[] playerTankAnimations;
    public int AnimationColorIndex { get; set; }

    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    AnimationClipOverrides clipOverrides;

    public int AnimationColorCount
    {
        get
        {
            return playerTankAnimations.Length;
        }
    }

    public enum TankLevelType
    {
        Tankette,
        Light,
        Medium,
        Heavy,
    }

    TankLevelType tankLevelType = TankLevelType.Tankette;
    public TankLevelType LevelType
    {
        get
        {
            return tankLevelType;
        }
        set
        {
            if (tankLevelType == value)
                return;

            tankLevelType = value;
            ChangeAnimationClips();
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
        ChangeAnimationClips();
    }

    void CheckAnimations()
    {
        Assert.IsTrue(playerTankAnimations.Length > 0);
        foreach (var tankAnimation in playerTankAnimations)
        {
            Assert.IsTrue(tankAnimation.tankette.Length == 4);
            Assert.IsTrue(tankAnimation.light.Length == 4);
            Assert.IsTrue(tankAnimation.medium.Length == 4);
            Assert.IsTrue(tankAnimation.heavy.Length == 4);
        }
    }

    void ChangeAnimationClips()
    {
        var tankStateClips = playerTankAnimations[AnimationColorIndex].GetClips(LevelType);
        clipOverrides["PlayerTankUp"] = tankStateClips[0]; //TODO rename
        clipOverrides["PlayerTankDown"] = tankStateClips[1];
        clipOverrides["PlayerTankLeft"] = tankStateClips[2];
        clipOverrides["PlayerTankRight"] = tankStateClips[3];
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }
}
