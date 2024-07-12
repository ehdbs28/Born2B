using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/EquipmentItem")]
public class EquipmentItemSO : ItemSO
{
    [Space(15f)]
    [SerializeField] List<StatModifierSlot> modifiers;

    /// <summary>
    /// Get Equipment
    /// </summary>
    public override bool Execute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IEquipmentItemHandler equipmentHandler) == false)
            return false;

        AddModifier(equipmentHandler.Stat);
        return true;
    }

    /// <summary>
    /// Drop Equipment
    /// </summary>
    public override bool Unexecute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IEquipmentItemHandler equipmentHandler) == false)
            return false;

        RemoveModifier(equipmentHandler.Stat);
        return true;
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