using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jinkalkum : MeleeWeapon
{
    private IStatModifierItemHandler _statHandler;

    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);

        _statHandler = owner as PlayerInstance;
        OnUnitHitEvent += HandleChangeStat;
    }

    private void HandleChangeStat(CellObjectInstance obj)
    {
        _statHandler.Stat.AddModifier(StatType.Attack, StatModifierType.Addend, 5);
    }
}
