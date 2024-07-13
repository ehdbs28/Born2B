using System;
using UnityEngine;

public abstract class ReviveItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.CallByEvent;
    protected override EventType CallingEventType => EventType.OnPlayerDead;
    protected Action onReviveAfterCallback;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);

        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler statHandler))
        {
            return;
        }

        statHandler.Stat.AddModifier(StatType.ReviveCount, StatModifierType.Addend, 1);
    }

    public override void Unexecute(IItemHandler handler)
    {
        base.Unexecute(handler);

        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler statHandler))
        {
            return;
        }

        statHandler.Stat.AddModifier(StatType.ReviveCount, StatModifierType.Addend, -1);
    }

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler statHandler))
        {
            return;
        }

        statHandler.Stat.RemoveModifier(StatType.ReviveCount, StatModifierType.Addend, -1);
        onReviveAfterCallback?.Invoke();
    }
}