using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ControlPlayerTurnItem")]
public class ControlPlayerTurnItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.UseImmediately;

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler weaponItemHandler))
        {
            return;
        }

        weaponItemHandler.Stat.AddModifier(StatType.Attack, StatModifierType.MultiplicationMultiplier, 2);
        weaponItemHandler.Stat.AddModifier(StatType.MaxHP, StatModifierType.MultiplicationMultiplier, 0.5f);
    }
}
