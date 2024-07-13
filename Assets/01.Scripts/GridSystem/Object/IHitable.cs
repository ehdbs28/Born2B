using System;
using UnityEngine;

public interface IHitable
{
    public Vector2Int Position { get; }
    public void Hit(CellObjectInstance attackObject, float damage, bool critical, Action<CellObjectInstance> callBack);
}
