using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ContinuousAttackFeedbackItem")]
public class ContinuousAttackFeedbackItemSO : AttackFeedbackItemSO
{
    [SerializeField] int stackLimit = 8;
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();

    private int stack = 0;
    private UnitInstance lastAttackedUnit = null;

    public override void OnUnitDamaged(UnitInstance unit, bool isDead)
    {
        if(lastAttackedUnit == unit)
        {
            stack = (stack + 1) % stackLimit;
            AddModifier();
        }
        else
        {
            lastAttackedUnit = unit;
            RemoveModifier();
            stack = 0;
        }
    }

    public override void Unexecute(IItemHandler handler)
    {
        base.Unexecute(handler);

        RemoveModifier();
        stack = 0;
    }

    private void AddModifier()
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        StatSO stat = statModifierHandler.Stat;
        modifiers.ForEach(i => {
            stat.AddModifier(i.StatType, i.ModifierType, i.Value * stack);
        });
    }
    
    private void RemoveModifier()
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        StatSO stat = statModifierHandler.Stat;
        modifiers.ForEach(i => {
            stat.RemoveModifier(i.StatType, i.ModifierType, i.Value * stack);
        });
    }
}
