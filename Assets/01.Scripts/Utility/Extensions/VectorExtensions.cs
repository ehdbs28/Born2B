using Unity.Mathematics;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 GetVector(this Vector2Int left)
    {
        return new Vector2(left.x, left.y);
    }

    public static int2 GetInt2(this Vector2Int left)
    {
        return new int2(left.x, left.y);
    }

    public static Vector2Int GetVectorInt(this Vector2 left)
    {
        return new Vector2Int(Mathf.RoundToInt(left.x), Mathf.RoundToInt(left.y));
    }

    public static Vector2Int GetVectorInt(this Vector3 left)
    {
        return ((Vector2)left).GetVectorInt();
    }

    public static Vector2Int GetVectorInt(this int2 left)
    {
        return new Vector2Int(left.x, left.y);
    }
}
