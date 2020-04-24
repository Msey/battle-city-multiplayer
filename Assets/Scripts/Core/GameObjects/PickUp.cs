using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Tank,
        Helmet,
        Star,
        Shovel,
        Clock,
        Grenade,
        Pistol
    };

    public PickUpType Type;
}
