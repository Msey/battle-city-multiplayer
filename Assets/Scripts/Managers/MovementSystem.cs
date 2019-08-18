using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSystem : PersistentSingleton<MovementSystem>
{
    private List<Dummy> _dummies;

    const float V = 100; // скорость надо брать у Dummy

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
        _dummies = new List<Dummy>();
        base.Awake();
    }

    public void Move(Direction direction)
    {
        foreach (var dummy in _dummies)
            dummy.current.position = (Vector2)dummy.current.position + dxdy[direction] * V;
    }

    public void AddDummy(Dummy dummy, Direction direction)
    {

    }
}
