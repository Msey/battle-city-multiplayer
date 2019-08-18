using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSystem : PersistentSingleton<MovementSystem>
{
    private List<Transform> _dummies;

    const float V = 100; // test speed

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
        {Direction.UpLeft, dxdy[Direction.Up] + dxdy[Direction.Left] } //test 
    };


    override protected void Awake()
    {
        base.Awake();
    }

    public void Move(Direction direction)
    {
        foreach (var dummy in _dummies)
            dummy.position = (Vector2)dummy.position + dxdy[direction] * V;
    }

    public void AddDummy(Transform dummy, Direction direction)
    {

    }
}
