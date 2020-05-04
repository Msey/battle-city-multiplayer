using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class Eagle : MonoBehaviour, IBulletTarget
{
    public Sprite destroyedTexture;
    public GameObject explosionPrefab;
    public GroupType Group { get; set; }
    static public event EventHandler EagleDestroyed;
    private SpriteRenderer spriteRenderer;
    private bool isDestroyed = false;
    void Awake()
    {
        Assert.IsNotNull(explosionPrefab);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool OnHit(IBullet bullet)
    {
        if (isDestroyed)
            return false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        spriteRenderer.sprite = destroyedTexture;
        isDestroyed = true;
        EagleDestroyed?.Invoke(this, EventArgs.Empty);
        return true;
    }
}
