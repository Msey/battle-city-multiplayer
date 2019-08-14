using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MotionSystem : PersistentSingleton<MotionSystem>
{
    private Dictionary<Transform, Tuple<Transform, Vector2>> _dummies;

    override protected void Awake()
    {
        base.Awake();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var dummy in _dummies)
        //{
        //    dummy.Item1.position = new Vector2(dummy.Item1.position.x + dummy.Item2.normalized.x, 
        //                                       dummy.Item1.position.x + dummy.Item2.normalized.y);
        //}
        
    }
}
