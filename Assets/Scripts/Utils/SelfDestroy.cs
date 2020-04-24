using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float time = 1.0f;
    void Start()
    {
        Destroy(gameObject, time);
    }
}
