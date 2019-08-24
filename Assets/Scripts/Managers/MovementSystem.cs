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

    private IList<IMovable> _movingUnits;
         
    override protected void Awake()
    {
        _movingUnits = new List<IMovable>();
        base.Awake();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        //if (_movingUnits.Count > 0)
        //    print("move in ms; dummies = " + _movingUnits.Count);

        for (int i = 0; i < _movingUnits.Count; i++)
        {
            float speed = _movingUnits[i].Speed;
            GameUnit dummy = (_movingUnits[i] as GameUnit); // explicit cast is bad as DI point of view
            dummy.transform.position = (Vector2)dummy.transform.position + dxdy[dummy.direction] * speed * Time.deltaTime;

            if (!_movingUnits[i].IsConstantMovement)
                RemoveUnit(_movingUnits[i]);
            // TODO: inefficiently. Needs to be fixed and optimized
        }
    }

    public void AddUnit(IMovable movingUnit)
    {
        if (!_movingUnits.Contains(movingUnit))
            _movingUnits.Add(movingUnit);
    }

    public void RemoveUnit(IMovable movingUnit)
    {
        //print("add movingUnit");
        if (_movingUnits.Contains(movingUnit))
        {
            _movingUnits.Remove(movingUnit);
            //print("remove movingUnit successful");
        }
    }
}
