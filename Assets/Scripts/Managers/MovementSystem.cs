using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSystem : PersistentSingleton<MovementSystem>
{
    public enum Direction : sbyte
    {
        Up,
        Down,
        Left,
        Right
    }

    private static Dictionary<Direction, Vector2> dxdy = new Dictionary<Direction, Vector2>
    {
        {Direction.Left, Vector2.left },
        {Direction.Right, Vector2.right },
        {Direction.Up, Vector2.up },
        {Direction.Down, Vector2.down },
        //{Direction.UpLeft, dxdy[Direction.Up] + dxdy[Direction.Left] } //test 
    };


    override protected void Awake()
    {
        base.Awake();
    }



    public void Move(IMovable movingUnit)
    {
        float speed = movingUnit.Speed;
        GameUnit unit = (movingUnit as GameUnit); // explicit cast is bad as DI point of view
        if (AreBoundsCorrect(unit, speed))
            unit.transform.position = (Vector2)unit.transform.position + dxdy[unit.direction] * speed * Time.deltaTime;
    }


    private bool AreBoundsCorrect(GameUnit u, float speed)
    {
        var collider = u.GetComponent<BoxCollider2D>();

        var t = (Vector2)u.transform.position + dxdy[u.direction] * speed * Time.deltaTime;

        //Debug.DrawLine(new Vector2(t.x + collider.size.x, t.y + collider.size.y),
        //               new Vector2(t.x - collider.size.x, t.y - collider.size.y));


        if (collider != null)
        {
            var objectsInBox = Physics2D.OverlapBoxAll((Vector2)u.transform.position + dxdy[u.direction] * speed * Time.deltaTime,
                 collider.size * 2, 0);


            for (int i = 0; i < objectsInBox.Length; i++)
            {
                if (objectsInBox[i].gameObject != u.gameObject && objectsInBox[i].GetComponent<BoxCollider2D>())
                    return false;
            }
        }


        return true;
    }
}

