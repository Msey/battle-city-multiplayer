using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void MoveUp();
    void MoveDown();
    void MoveLeft();
    void MoveRight();
    bool IsConstantMovement { get; }
    float Speed { get; }
}
