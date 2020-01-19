using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawnPointAnimatorController : MonoBehaviour
{
    public delegate void AnimationFinishedHandler();
    public event AnimationFinishedHandler AnimationFinished;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("StartSpawning");
    }

    void OnAnimationFinished()
    {
        AnimationFinished?.Invoke();
    }
}
