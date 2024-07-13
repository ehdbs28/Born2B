using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ReviveWithChangeHealthItem")]
public class ReviveWithChangeHealthItemSO : ReviveItemSO
{
    [SerializeField] private int _toRevivehealth;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);

        onReviveAfterCallback += HandleChangeStat;
    }

    private void HandleChangeStat()
    {
        if (!TryParseHandler(OwnerHandler, out IHealthItemHandler healthHandler))
        {
            return;
        }
         
        healthHandler.Health.ReduceHp(int.MaxValue);
        healthHandler.Health.AddHp(1);
    }
}
