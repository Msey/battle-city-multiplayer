using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void MoveUp();
    void MoveDown();
    void MoveLeft();
    void MoveRight();
    // gaz, tormoz etc.
    bool IsConstantMovement { get; }
}
