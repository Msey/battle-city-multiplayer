using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Blinking : MonoBehaviour
{
    public float time = 0.5f;

    private float timeInCurrentState = 0.0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timeInCurrentState += Time.deltaTime;
        if (timeInCurrentState > time)
        {
            timeInCurrentState = 0;
            ChangeState();
        }
    }

    void ChangeState()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
