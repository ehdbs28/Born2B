using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/StatConditionalStatModifierItem")]
public class StatConditionalStatModifierItemSO : AttributedCallByEventItemSO
{
    private enum Condition
    {
        Less = -1,
        Equal = 0,
        Greater = 1
    }

    private struct ConditionSlot
    {
        public StatType statType;
        public Condition condition;
        public float compareValue;

        public bool IsCondition(float value) 
        {
            int compare = value.CompareTo(compareValue);
            return compare == (int)condition;
        }
    }

    protected override EventType CallingEventType => EventType.OnTurnEnded;

    [SerializeField] List<ConditionSlot> conditions = new List<ConditionSlot>();
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();

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

            StatSO stat = statModifierHandler.Stat;
            modifiers.ForEach(stat.RemoveModifier);
        }

        base.Unexecute(handler);
    }

    private void TryApply()
    {
        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        StatSO stat = statModifierHandler.Stat;

        bool condition = true;
        for(int i = 0; i < conditions.Count; ++i)
        {
            ConditionSlot conditionSlot = conditions[i];
            condition &= conditionSlot.IsCondition(stat[conditionSlot.statType]);
            if(condition == false)
                break;
        }

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