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

    public void WrapEagle(Transform eagle, MapElementType element/*, BuildSide side*/)
    {
        Vector2 eagleLocalPoint = eagle.localPosition;

        var Tilemap = ClassicGameManager.s_Instance.Tilemap;
        var Level = Tilemap.parent;

        var material = GetMaterial(element);

        circleSide = new Vector2[]
        {
            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MAX, eagleLocalPoint.y + BUILD_THRESHOLD_MIN),
            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MAX, eagleLocalPoint.y - BUILD_THRESHOLD_MIN),

            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MAX, eagleLocalPoint.y + BUILD_THRESHOLD_MIN),
            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MAX, eagleLocalPoint.y - BUILD_THRESHOLD_MIN),

            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MIN, eagleLocalPoint.y + BUILD_THRESHOLD_MAX),
            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MIN, eagleLocalPoint.y + BUILD_THRESHOLD_MAX),

            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MIN, eagleLocalPoint.y - BUILD_THRESHOLD_MAX),
            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MIN, eagleLocalPoint.y - BUILD_THRESHOLD_MAX),

            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MAX, eagleLocalPoint.y - BUILD_THRESHOLD_MAX),
            new Vector2(eagleLocalPoint.x - BUILD_THRESHOLD_MAX, eagleLocalPoint.y + BUILD_THRESHOLD_MAX),
            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MAX, eagleLocalPoint.y - BUILD_THRESHOLD_MAX),
            new Vector2(eagleLocalPoint.x + BUILD_THRESHOLD_MAX, eagleLocalPoint.y + BUILD_THRESHOLD_MAX),
        };


        foreach (var point in circleSide)
        {
            var eagleGlobalPoint = Level.TransformVector(point);
            Collider2D[] objects = Physics2D.OverlapPointAll(eagleGlobalPoint);

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

            if (canbuild && material != null)
            {
                Instantiate(material, Vector2.zero, Quaternion.identity, Tilemap)
                    .transform.localPosition = point;
            }
        }
    }

    private GameObject GetMaterial(MapElementType element)
    {
        switch (element)
        {
            case MapElementType.Brick: return Brick;
            case MapElementType.Concrete: return Concrete;
            case MapElementType.Forest: return Forest;
            case MapElementType.Ice: return Ice;
            case MapElementType.Water: return Water;

            case MapElementType.Nothing:
            default: return null;
        }
    }
}

