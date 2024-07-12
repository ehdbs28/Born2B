using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/EquipmentItem")]
public class EquipmentItemSO : ItemSO
{
    [Space(15f)]
    [SerializeField] List<StatModifierSlot> modifiers;
    public override Type ItemType => typeof(EquipmentItemSO);

    /// <summary>
    /// Get Equipment
    /// </summary>
    public override void Execute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        AddModifier(statModifierHandler.Stat);
    }

    /// <summary>
    /// Drop Equipment
    /// </summary>
    public override void Unexecute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        RemoveModifier(statModifierHandler.Stat);
    }

    private void AddModifier(StatSO stat)
    {
        modifiers.ForEach(stat.AddModifier);
    }

    private void RemoveModifier(StatSO stat)
    {
        modifiers.ForEach(stat.RemoveModifier);
    }
}