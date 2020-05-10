using UnityEngine;
using static GameConstants;

public class MapBuilder : PersistentSingleton<MapBuilder>
{

    public GameObject Concrete;
    public GameObject Forest;
    public GameObject Brick;
    public GameObject Water;
    public GameObject Ice;

    Vector2[] circleSide;



    public void WrapEagle(Transform eagle/*, MapElementType element, BuildSide side*/)
    {
        int mask = LayerMask.GetMask("Brick", "Concrete", "Forest", "Ice", "Water", "LevelBorder", "Tank", "EagleFortress");

        Vector2 position = eagle.localPosition;

        var Tilemap = GameObject.Find("Tilemap").transform;
        var Level = Tilemap.parent;

        circleSide = new Vector2[]
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
        {
            var v = Level.TransformVector(point);
            Collider2D[] objects = Physics2D.OverlapPointAll(v, mask);

            bool canbuild = objects.Length == 0;

            foreach (var obj in objects)
            {
                if (!obj.GetComponent<PlayerTank>()
                    && !obj.GetComponent<EnemyTank>()
                    && !obj.GetComponent<LevelBorder>()
                    && !obj.GetComponent<Eagle>())
                {
                    Destroy(obj.gameObject);
                    canbuild = true;
                }
                else canbuild = false;
            }

            if (canbuild)
            {
                var a = Instantiate(Concrete, Vector2.zero, Quaternion.identity, Tilemap);
                a.transform.localPosition = point;
            }
        }
    }
}

