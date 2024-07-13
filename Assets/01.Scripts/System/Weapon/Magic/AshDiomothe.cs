using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshDiomothe : HitscanWeapon
{
    private IStatModifierItemHandler _statHandler;

    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);
        _statHandler = owner as PlayerInstance;
        OnAttackEndEvent += HandleSetBurnAllUnit;
    }

    private void HandleSetBurnAllUnit()
    {
        if (_statHandler.Stat[StatType.MagicPower] < 10) return;

        _statHandler.Stat.RemoveModifier(StatType.MagicPower, StatModifierType.Addend, 10);

        var Units = CellObjectManager.Instance.GetCellObjectInstances<UnitInstance>();

        foreach (var u in Units)
        {
            if (u.TryGetComponent<StatusController>(out StatusController controller))
            {
                controller.AddStatus(StatusType.Burn, 1);
            }
        }
    }
}
