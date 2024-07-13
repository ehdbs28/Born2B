using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclaresTears : HitscanWeapon
{
    private IStatModifierItemHandler _statHandler;

    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);
        _statHandler = owner as PlayerInstance;
        OnAttackEndEvent += HandleChangeStat;
    }

    private void HandleChangeStat()
    {
        _statHandler.Stat.AddModifier(StatType.MagicPower, StatModifierType.Addend, 2);
    }
}
