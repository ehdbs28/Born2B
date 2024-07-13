using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/WithstandDamageItem")]
public class WithstandDamageItemSO : ReviveItemSO
{
    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IHealthItemHandler healthHandler))
        {
            return;
        }

        healthHandler.Health.CanChangedHP = false;
    }

    public override void Unexecute(IItemHandler handler)
    {
        base.Unexecute(handler);

        if (!TryParseHandler(OwnerHandler, out IHealthItemHandler healthHandler))
        {
            return;
        }

        healthHandler.Health.CanChangedHP = true;
    }
}
