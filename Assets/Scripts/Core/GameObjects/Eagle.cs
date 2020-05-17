using System;
using UnityEngine;
using UnityEngine.Assertions;
using static GameConstants;


[RequireComponent(typeof(SpriteRenderer))]
public class Eagle : MonoBehaviour, IBulletTarget
{
    public Sprite destroyedTexture;
    public GroupType Group { get; set; }
    public static event EventHandler EagleDestroyed;
    private SpriteRenderer spriteRenderer;
    private bool isDestroyed;
    private  float wallTimer;
    private float wallToggleTimer;

    private bool switchTexture;
    private MapElementType currentElementType = MapElementType.Concrete;

    void Awake()
    {
        Assert.IsNotNull(ResourceManager.s_Instance.BigExplosionPrefab);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool OnHit(IBullet bullet)
    {
        if (isDestroyed)
            return false;

        AudioManager.s_Instance.PlayFxClip(AudioManager.AudioClipType.PlayerExplosion);
        Instantiate(ResourceManager.s_Instance.BigExplosionPrefab, transform.position, Quaternion.identity);
        spriteRenderer.sprite = destroyedTexture;
        isDestroyed = true;
        EagleDestroyed?.Invoke(this, EventArgs.Empty);
        return true;
    }


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
            else
                MapBuilder.s_Instance.WrapEagle(transform, MapElementType.Nothing);
        }
    }

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
                wallToggleTimer -= Time.deltaTime;
            else switchTexture = false;
        }
    }


    private MapElementType ToggleElement()
        => currentElementType == MapElementType.Brick
        ? currentElementType = MapElementType.Concrete
        : currentElementType = MapElementType.Brick;
}
