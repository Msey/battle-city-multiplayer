using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MotionSystem : MonoBehaviour
{
    public static MotionSystem Current { get; private set; }

    private Dictionary<Transform, Tuple<Transform, Vector2>> _dummies;
    
    void Awake()
    {
        if (Current) return;
        Current = this;
        DontDestroyOnLoad(Current.gameObject);
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
