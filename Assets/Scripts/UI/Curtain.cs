using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Curtain : MonoBehaviour
{
    Animator animator;

    public void Open()
    {
        animator.SetTrigger("Open");
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
