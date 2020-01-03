using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TestTankController : MonoBehaviour
{
    TankMovement tankMovement;
    void Start()
    {
        tankMovement = GetComponent<TankMovement>();
    }

    void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        tankMovement.stoped = false;
        if (verticalAxis > 0.0f)
            tankMovement.Direction = TankMovement.eDirection.Up;
        else if (verticalAxis < 0.0f)
            tankMovement.Direction = TankMovement.eDirection.Down;
        else if (horizontalAxis < 0.0f)
            tankMovement.Direction = TankMovement.eDirection.Left;
        else if (horizontalAxis > 0.0f)
            tankMovement.Direction = TankMovement.eDirection.Right;
        else
            tankMovement.stoped = true;
    }
}
