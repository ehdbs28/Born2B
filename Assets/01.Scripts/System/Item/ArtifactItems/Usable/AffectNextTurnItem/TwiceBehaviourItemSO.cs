using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/TwiceBehaviourItem")]
public class TwiceBehaviourItemSO : AffectNextTurnArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;
    private IStatModifierItemHandler _weaponItemHandler;

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler weaponItemHandler))
        {
            return;
        }

        // 여기서 세팅 해주기
        _weaponItemHandler = weaponItemHandler;
    }

    protected override void HandleAddNextTurnAffect(params object[] arr)
    {
        base.HandleAddNextTurnAffect(arr);
        _weaponItemHandler.Stat.AddModifier(StatType.TurnCount, StatModifierType.Addend, 1);
    }

    protected override void HandleRemoveNextTurnAffect(params object[] arr)
    {
        base.HandleRemoveNextTurnAffect(arr);
    }
}
