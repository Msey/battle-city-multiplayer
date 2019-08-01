using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            print("w");
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("a");
        }
        if (Input.GetKey(KeyCode.S))
        {
            print("s");
        }
        if (Input.GetKey(KeyCode.D))
        {
            print("d");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            print("fire");
        }
    }
}
