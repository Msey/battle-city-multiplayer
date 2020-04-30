using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blinking : MonoBehaviour
{
    public float time = 0.5f;
    [SerializeField]
    private bool useRealTime;

    private float timeInCurrentState = 0.0f;
    private SpriteRenderer spriteRenderer;
    private Text text;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        if (useRealTime)
            StartCoroutine(ChangeStateCoroutine());
    }

    void Update()
    {
        if (!useRealTime)
        {
            timeInCurrentState += Time.deltaTime;
            if (timeInCurrentState > time)
            {
                timeInCurrentState = 0;
                ChangeState();
            }
        }
    }

    void ChangeState()
    {
        if (text)
            text.enabled = !text.enabled;
        if (spriteRenderer)
            spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    IEnumerator ChangeStateCoroutine()
    {
        while (true)
        {
            ChangeState();
            yield return new WaitForSecondsRealtime(time);
        }
    }
}
