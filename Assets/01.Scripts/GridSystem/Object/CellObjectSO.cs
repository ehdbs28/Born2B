using System;
using Unity.Mathematics;
using UnityEngine;

public enum CellObjectType
{

    MoveAble,
    NotMoveAble

}

[CreateAssetMenu(menuName = "SO/Cell/CellObject")]
public class CellObjectSO : StageObjectSO, ICloneable
{

    [field:SerializeField] public virtual GameObject cellObjectInstancePrefab { get; protected set; }
    [field:SerializeField] public CellObjectType cellObjectType { get; set; }
    public Guid key { get; set; }
    public int2 position { get; set; }

    public virtual object Clone()
    {

        var obj = Instantiate(this);
        obj.key = Guid.NewGuid();
        return obj;

    }

}
