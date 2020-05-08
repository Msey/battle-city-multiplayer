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

    private void Start()
    {
        wallTimer = CONCRETE_WALL_TIMER; 
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

    const int CONCRETE_WALL_TIMER = 10;
    const int CONCRETE_WALL_BLINK_TIMER = 4;
    const float CONCRETE_WALL_CHANGE_TEXTURE_TIMER = 0.5f;

    float wallTimer;
    float wallChangeTextureTimer;

    bool startedChangeTexture;

    public void ActivateShowelEffect(bool positive = true)
    {
        if (!isDestroyed)
        {
            if (positive)
            {
                wallTimer = CONCRETE_WALL_TIMER;
            }

        }
    }
    int test = 1;

    private void Update()
    {
        wallTimer -= Time.deltaTime;

        if (wallTimer <= CONCRETE_WALL_BLINK_TIMER)
        {
            if (!startedChangeTexture)
            {
                startedChangeTexture = true;
                wallChangeTextureTimer = CONCRETE_WALL_CHANGE_TEXTURE_TIMER;
                print(test);
                test *= -1;
            }
            else if (wallChangeTextureTimer > 0)
            {
                wallChangeTextureTimer -= Time.deltaTime;
            }
            else startedChangeTexture = true;
        }
    }
}
