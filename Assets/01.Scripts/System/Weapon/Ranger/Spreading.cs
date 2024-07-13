using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spreading : WideAreaWeapon
{
    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);

        OnUnitHitEvent += HandleUnitHit;
    }

    private void HandleUnitHit(CellObjectInstance obj)
    {
        UnitInstance unit = obj as UnitInstance;

        if(unit.TryGetComponent<StatusController>(out var controller))
        {
            StatusType type = Convert.ToBoolean(Random.Range(0, 1)) ? StatusType.Burn : StatusType.Frozen;

            controller.AddStatus(type, 2);
        }
    }
}
