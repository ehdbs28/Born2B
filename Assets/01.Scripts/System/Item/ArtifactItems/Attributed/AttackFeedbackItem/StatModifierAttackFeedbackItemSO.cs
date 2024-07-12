using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/StatModifierAttackFeedbackItem")]
public class StatModifierAttackFeedbackItemSO : AttackFeedbackItemSO
{
    [SerializeField] int turnCount = 4;
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();

    private int remainTurnCount = 0;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, HandleTurnEnded);
    }

    public override void Unexecute(IItemHandler handler)
    {
        base.Execute(handler);
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, HandleTurnEnded);

        if(remainTurnCount > 0)
        {
            remainTurnCount = 0;
            RemoveModifier();
        }
    }

    public override void OnUnitDamaged(UnitInstance unit, bool isDead)
    {
        if(isDead == false)
            return;

        if(remainTurnCount <= 0)
            AddModifier();

        remainTurnCount = turnCount;
    }

    private void HandleTurnEnded(object[] args)
    {
        if(remainTurnCount <= 0)
            return;

        remainTurnCount--;
        if(remainTurnCount <= 0)
            RemoveModifier();
    }

    private void AddModifier()
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        modifiers.ForEach(statModifierHandler.Stat.AddModifier);
    }

    private void RemoveModifier()
    {
        if(TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        modifiers.ForEach(statModifierHandler.Stat.RemoveModifier);
    }
}
