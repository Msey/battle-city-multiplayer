using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class Eagle : MonoBehaviour, IBulletTarget
{
    public Sprite destroyedTexture;
    private SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;
    public EntityRelationGroup Group { get; set; }
    void Awake()
    {
        Assert.IsNotNull(explosionPrefab);
        Group = new EntityRelationGroup(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnHit(IBullet bullet)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        spriteRenderer.sprite = destroyedTexture;
    }
}
