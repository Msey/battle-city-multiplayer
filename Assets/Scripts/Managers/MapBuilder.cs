using UnityEngine;
using static GameConstants;

public class MapBuilder : PersistentSingleton<MapBuilder>
{
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
            case MapElementType.Brick: return ResourceManager.s_Instance.BrickPrefab;
            case MapElementType.Concrete: return ResourceManager.s_Instance.ConcretePrefab;
            case MapElementType.Forest: return ResourceManager.s_Instance.ForestPrefab;
            case MapElementType.Ice: return ResourceManager.s_Instance.IcePrefab;
            case MapElementType.Water: return ResourceManager.s_Instance.WaterPrefab;

            case MapElementType.Nothing:
            default: return null;
        }
    }
}

