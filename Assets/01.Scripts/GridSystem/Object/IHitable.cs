using UnityEngine;

public interface IHitable
{
    public Vector2Int Position { get; }
    public bool Hit(CellObjectInstance attackObject, float damage, bool critical);
}
