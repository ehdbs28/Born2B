using System;
using System.Collections.Generic;
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

    [field:SerializeField] public CellObjectType cellObjectType { get; set; }
    [field:SerializeField] public RuntimeAnimatorController animator { get; set; }
    [field:SerializeField] public Sprite sprite { get; set; }

    public Guid key { get; set; }
    public int2 position { get; set; }

    public virtual object Clone()
    {

        var obj = Instantiate(this);
        obj.key = Guid.NewGuid();
        return obj;

    }

}
