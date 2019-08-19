using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSystem : PersistentSingleton<MovementSystem>
{
    private List<IMovable> _dummies;

    public enum Direction : sbyte
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft //test
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
        _dummies = new List<IMovable>();
        base.Awake();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (_dummies.Count > 0)
            print("move in ms; dummies = " + _dummies.Count);

        for (int i = 0; i < _dummies.Count; i++)
        {
            Dummy dummy = (_dummies[i] as Dummy);
            dummy.transform.position = (Vector2)dummy.transform.position + dxdy[dummy.direction] * Time.deltaTime;

            if (!_dummies[i].IsConstantMovement)
                _dummies.Remove(_dummies[i]);
            // TODO: inefficiently. Needs to be fixed and optimized
        }
    }

    public void AddDummy(IMovable dummy)
    {
        print("add dummy");
        if (!_dummies.Contains(dummy))
        {
            _dummies.Add(dummy);
            print("add dummy successful");
        }
    }
}
