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
        if(TryParseHandler(handler, out IEquipmentItemHandler equipmentHandler) == false)
            return;

        AddModifier(equipmentHandler.Stat);
    }

    /// <summary>
    /// Drop Equipment
    /// </summary>
    public override void Unexecute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IEquipmentItemHandler equipmentHandler) == false)
            return;

        RemoveModifier(equipmentHandler.Stat);
    }

    private void AddModifier(StatSO stat)
    {
        modifiers.ForEach(i => {
            stat.AddModifier(i);
        });
    }

    private void RemoveModifier(StatSO stat)
    {
        modifiers.ForEach(i => {
            stat.RemoveModifier(i);
        });
    }
}

public interface IEquipmentItemHandler : IItemHandler
{
    public StatSO Stat { get; }
}