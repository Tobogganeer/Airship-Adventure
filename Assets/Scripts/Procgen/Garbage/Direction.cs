using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public static readonly Direction Up = new Direction(Vector3.up);
    public static readonly Direction Down = new Direction(Vector3.down);
    public static readonly Direction Left = new Direction(Vector3.left);
    public static readonly Direction Right = new Direction(Vector3.right);
    public static readonly Direction Forward = new Direction(Vector3.forward);
    public static readonly Direction Back = new Direction(Vector3.back);


    public readonly Vector3 Normal;

    private Direction(Vector3 normal)
    {
        Normal = normal;
    }
}
