using UnityEngine;
using UnityEngine.Assertions;

public class Curtain : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void Open()
    {
        animator.SetTrigger("Open");
    }

    protected void Awake()
    {
        Assert.IsNotNull(animator);
    }
}
