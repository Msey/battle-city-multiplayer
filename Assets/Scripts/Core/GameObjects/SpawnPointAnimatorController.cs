﻿using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawnPointAnimatorController : MonoBehaviour
{
    public Action OnAnimationFinishedCallback = ()=> {};

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        animator?.SetTrigger("StartSpawning");
    }

    public void OnAnimationFinished()
    {
        OnAnimationFinishedCallback.Invoke();
    }
}
