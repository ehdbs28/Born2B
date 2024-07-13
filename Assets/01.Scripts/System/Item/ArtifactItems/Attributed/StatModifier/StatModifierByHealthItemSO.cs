using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/StatModifierByHealthItem")]
public class StatModifierByHealthItemSO : AttributedCallByEventItemSO
{
    [SerializeField] int multiplierLimit = 5;
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();
    protected override EventType CallingEventType => EventType.OnTurnEnded;

    private int lastModifiedHP = 0;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);

        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        if (TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthItemHandler) == false)
            return;

        lastModifiedHP = healthItemHandler.Health.MaxHp;
        AddModifier(statModifierHandler.Stat, healthItemHandler.Health);
    }

    public override void UseArtifact(params object[] args)
    {
        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        if (TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthItemHandler) == false)
            return;

        AddModifier(statModifierHandler.Stat, healthItemHandler.Health);
    }

    public override void Unexecute(IItemHandler handler)
    {
        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        if (TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthItemHandler) == false)
            return;

        int multiplier = healthItemHandler.Health.MaxHp - lastModifiedHP;
        RemoveModifier(statModifierHandler.Stat, multiplier);

        base.Unexecute(handler);
    }

    private void AddModifier(StatSO stat, IHealth health)
    {
        RemoveModifier(stat, health.MaxHp - lastModifiedHP);
        lastModifiedHP = health.CurrentHp;

        int multiplier = health.MaxHp - lastModifiedHP;
        multiplier = Mathf.Min(multiplier, multiplierLimit);
        if(multiplier == 0)
            return;
        
        modifiers.ForEach(i => {
           stat.AddModifier(i.StatType, i.ModifierType, i.Value * multiplier);
        });
    }

    private void RemoveModifier(StatSO stat, int multiplier)
    {
        multiplier = Mathf.Min(multiplier, multiplierLimit);
        if(multiplier == 0)
            return;

        modifiers.ForEach(i => {
           stat.RemoveModifier(i.StatType, i.ModifierType, i.Value * multiplier);
        });
    }
}
