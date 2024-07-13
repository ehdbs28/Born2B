using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/HealthConditionalStatModifierItem")]
public class HealthConditionalStatModifierItemSO : AttributedCallByEventItemSO
{
    private enum Condition
    {
        Less = -1,
        Equal = 0,
        Greater = 1
    }

    [SerializeField] Condition condition;
    [SerializeField, Range(0f, 1f)] float compareValue;
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();

    protected override EventType CallingEventType => EventType.OnTurnEnded;

    private bool actived = false;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);
        TryApply();
    }

    public override void UseArtifact(params object[] args)
    {
        TryApply();
    }

    public override void Unexecute(IItemHandler handler)
    {
        if(actived)
        {
            if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
                return;

            if (TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthItemHandler) == false)
                return;

            StatSO stat = statModifierHandler.Stat;
            modifiers.ForEach(stat.RemoveModifier);
        }

        base.Unexecute(handler);
    }

    private void TryApply()
    {
        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        if (TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthItemHandler) == false)
            return;

        StatSO stat = statModifierHandler.Stat;
        IHealth health = healthItemHandler.Health;

        float hpRatio = health.CurrentHp / (float)health.MaxHp;
        int compare = hpRatio.CompareTo(compareValue);
        bool condition = compare == (int)this.condition;

        if(actived)
        {
            if(condition == false)
            {
                modifiers.ForEach(stat.RemoveModifier);
                actived = false;
            }
        }
        else
        {
            if(condition)
            {
                modifiers.ForEach(stat.AddModifier);
                actived = true;
            }
        }
    }
}
