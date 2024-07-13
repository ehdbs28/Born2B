using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/AttackWaitingStatModifierItem")]
public class AttackWaitingStatModifierItemSO : AttributedCallByEventItemSO
{
    [SerializeField] List<StatModifierSlot> modifiers = new List<StatModifierSlot>();

    protected override EventType CallingEventType => EventType.OnTurnEnded;
    private bool attacked = false;
    private bool actived = false;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);
        EventManager.Instance.RegisterEvent(EventType.OnPlayerAttacked, HandlePlayerAttacked);
    }

    public override void UseArtifact(params object[] args)
    {
        RemoveModifier();

        if(attacked == false)
            AddModifier();

        attacked = false;
    }

    public override void Unexecute(IItemHandler handler)
    {
        base.Unexecute(handler);

        if(actived)
            RemoveModifier();
        attacked = false;
    }

    private void HandlePlayerAttacked(object[] args)
    {
        attacked = true;
    }

    private void AddModifier()
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        modifiers.ForEach(statModifierHandler.Stat.AddModifier);
        actived = true;
    }

    private void RemoveModifier()
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;

        modifiers.ForEach(statModifierHandler.Stat.RemoveModifier);
        actived = false;
    }
}