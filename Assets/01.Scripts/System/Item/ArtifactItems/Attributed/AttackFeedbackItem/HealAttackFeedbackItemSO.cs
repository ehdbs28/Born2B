using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/HealAttackFeedbackItem")]
public class HealAttackFeedbackItemSO : AttackFeedbackItemSO
{
    [SerializeField] int amount = 1;

    public override void OnUnitDamaged(UnitInstance unit, bool isDead)
    {
        if(TryParseHandler<IHealthItemHandler>(OwnerHandler, out IHealthItemHandler healthHandler) == false)
            return;

        healthHandler.Health.AddHp(amount);
    }
}
