using UnityEngine;

public class LevelBorder : MonoBehaviour, IBulletTarget
{
    public GroupType Group { get; set; }

    void Start()
    {
    }

    public bool OnHit(IBullet bullet)
    {
        return true;
    }
}
