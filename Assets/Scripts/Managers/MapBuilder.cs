using UnityEngine;
using static GameConstants;

public class MapBuilder : PersistentSingleton<MapBuilder>
{

    public GameObject Concrete;
    public GameObject Forest;
    public GameObject Brick;
    public GameObject Water;
    public GameObject Ice;

    public void WrapEagle(Vector2 position/*, MapElementType element, BuildSide side*/)
    {
        object[] objects = Physics2D.OverlapPointAll(position);

        Vector2[] circleSide = new Vector2[]
        {
            new Vector2(position.x - 1.5f, position.y + 0.5f),
            new Vector2(position.x - 1.5f, position.y - 0.5f),

            new Vector2(position.x + 1.5f, position.y + 0.5f),
            new Vector2(position.x + 1.5f, position.y - 0.5f),

            new Vector2(position.x + 0.5f, position.y + 1.5f),
            new Vector2(position.x - 0.5f, position.y + 1.5f),

            new Vector2(position.x + 0.5f, position.y - 1.5f),
            new Vector2(position.x - 0.5f, position.y - 1.5f),

            new Vector2(position.x - 1.5f, position.y - 1.5f),
            new Vector2(position.x - 1.5f, position.y + 1.5f),
            new Vector2(position.x + 1.5f, position.y - 1.5f),
            new Vector2(position.x + 1.5f, position.y + 1.5f),
        };


        foreach (var point in circleSide)
            Instantiate(Concrete, point, Quaternion.identity);
    }




}

