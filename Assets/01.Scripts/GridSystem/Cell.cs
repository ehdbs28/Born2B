using System;
using Unity.Mathematics;
using UnityEngine;

public struct Cell : IEquatable<Cell>, ICloneable
{

    public Cell(int2 position)
    {

        this.position = position;
        guid = Guid.NewGuid();
        unitKey = Guid.Empty;

    }

    public int2 position;
    public Guid guid;
    public Guid unitKey;

    public bool Equals(Cell other)
    {

        return other.guid == guid;

    }

    public object Clone()
    {

        var obj = new Cell();
        obj.guid = Guid.NewGuid();

        obj.unitKey = unitKey;

        return obj;

    }

    //Debug
    public void FindAndGrow()
    {

        var ins = StageManager.Instance.Grid.GetCellInstance(guid);
        ins.Grow();

    }

}
