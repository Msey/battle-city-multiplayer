using System;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;


[RequireComponent(typeof(SpriteRenderer))]
public class Eagle : MonoBehaviour, IBulletTarget
{
    public Sprite destroyedTexture;
    public GameObject explosionPrefab;
    public GroupType Group { get; set; }
    static public event EventHandler EagleDestroyed;
    private SpriteRenderer spriteRenderer;
    private bool isDestroyed;
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

    float wallTimer;
    float wallToggleTimer;

    bool switchTexture;
    MapElementType currentElementType = MapElementType.Concrete;
    public void ActivateShowelEffect(bool positive = true)
    {
        if (!isDestroyed)
        {
            if (positive)
            {
                wallTimer = CONCRETE_WALL_TIMER;
                wallToggleTimer = 0;
                MapBuilder.s_Instance.WrapEagle(transform, currentElementType);
            }
        }
    }
    int test = 1;

    private void Update()
    {

        wallTimer -= Time.deltaTime;

        if (Utils.InRange(0, wallTimer, CONCRETE_WALL_BLINK_TIMER))
        {
            if (!switchTexture)
            {
                switchTexture = true;
                wallToggleTimer = CONCRETE_WALL_CHANGE_TEXTURE_TIMER;
                ToggleElement();
                MapBuilder.s_Instance.WrapEagle(transform, currentElementType);
            }
            else if (wallToggleTimer > 0)
            {
                wallToggleTimer -= Time.deltaTime;
            }
            else switchTexture = false;
        }
    }


    private MapElementType ToggleElement()
        => currentElementType == MapElementType.Brick
        ? currentElementType = MapElementType.Concrete
        : currentElementType = MapElementType.Brick;
}
