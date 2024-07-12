using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IMovementable
{

    public List<int2> moveRole { get; set; }
    public Vector2 Move(List<Vector2> targetPositions, Action endCallback);

}
