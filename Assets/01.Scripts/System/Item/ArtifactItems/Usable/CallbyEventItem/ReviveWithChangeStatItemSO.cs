using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ReviveWithChangeStatItem")]
public class ReviveWithChangeStatItemSO : ReviveItemSO
{
    [SerializeField] private List<StatModifierSlot> _toChangeStatGroup = new();

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);

        onReviveAfterCallback += HandleChangeStat;
    }

    private void HandleChangeStat()
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler statHandler))
        {
            return;
        }

        foreach (var slot in _toChangeStatGroup)
        {
            statHandler.Stat.AddModifier(slot);
        }
    }
}
