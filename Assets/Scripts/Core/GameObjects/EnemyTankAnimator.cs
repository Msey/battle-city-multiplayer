using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class EnemyTankAnimation
{
    public AnimationClip[] standard;
    public AnimationClip[] blink;

    public AnimationClip[] GetClip(bool blinked)
    {
        if (blinked)
            return blink;
        return standard;
    }
}

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemyTankAnimator : MonoBehaviour
{
    [SerializeField]
    EnemyTankAnimation[] enemyTankAnimations;
    public int tankIndex = 0;

    public bool inBlinkFrameState = false;
    public bool InBlinkFrameState
    {
        get
        {
            return inBlinkFrameState;
        }
        set
        {
            if (inBlinkFrameState == value)
                return;

            inBlinkFrameState = value;
            UpdateArmorColorByBlinkState();
        }
    }

    public void ChangeBlinkFrameState()
    {
        InBlinkFrameState = !InBlinkFrameState;
        ChangeAnimationClips();
    }

    public bool Blinking
    {
        get
        {
            return animator.GetBool("Blinking");
        }
        set
        {
            animator.SetBool("Blinking", value);
        }
    }

    public enum eArmorColor
    {
        Default,
        Green,
        Yellow,
    }
    eArmorColor armorColor = eArmorColor.Default;
    public eArmorColor ArmorColor
    {
        get
        {
            return armorColor;
        }
        set
        {
            if (armorColor == value)
                return;
            armorColor = value;
            UpdateArmorColorByBlinkState();
        }
    }

    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    AnimationClipOverrides clipOverrides;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
        ChangeAnimationClips();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ArmorColor = eArmorColor.Green;
        Blinking = true;
    }

    void CheckAnimations()
    {
        Assert.IsTrue(enemyTankAnimations.Length > 0);
        foreach (var tankAnimation in enemyTankAnimations)
        {
            Assert.IsTrue(tankAnimation.standard.Length == 4);
            Assert.IsTrue(tankAnimation.blink.Length == 4);
        }
    }

    void ChangeAnimationClips()
    {
        var tankStateClips = enemyTankAnimations[tankIndex].GetClip(inBlinkFrameState);
        clipOverrides["PlayerTankUp"] = tankStateClips[0]; //TODO rename
        clipOverrides["PlayerTankDown"] = tankStateClips[1];
        clipOverrides["PlayerTankLeft"] = tankStateClips[2];
        clipOverrides["PlayerTankRight"] = tankStateClips[3];
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }

    Color ArmorColorToUnityColor(eArmorColor color)
    {
        switch (color)
        {
            case eArmorColor.Default:
                return Color.white;
            case eArmorColor.Green:
                return new Color32(159, 231, 156, 255);
            case eArmorColor.Yellow:
                return new Color32(219, 212, 55, 255);
        }
        return Color.white;
    }

    void UpdateArmorColorByBlinkState()
    {
        if (inBlinkFrameState)
            spriteRenderer.color = ArmorColorToUnityColor(eArmorColor.Default);
        else
            spriteRenderer.color = ArmorColorToUnityColor(armorColor);
    }
}
