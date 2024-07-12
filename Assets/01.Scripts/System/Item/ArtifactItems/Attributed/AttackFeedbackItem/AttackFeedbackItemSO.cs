using UnityEngine;

public abstract class AttackFeedbackItemSO : AttributedCallByEventItemSO
{
    protected override EventType CallingEventType => EventType.OnUnitDamaged;
    
    [SerializeField, Range(0f, 1f)] float activeChance = 0.5f;

    public override void UseArtifact(params object[] args)
    {
        if(Random.value > activeChance)
            return;

        UnitInstance unit = args[0] as UnitInstance;
        bool isDead = (bool)args[1];

        OnUnitDamaged(unit, isDead);
    }

    public abstract void OnUnitDamaged(UnitInstance unit, bool isDead);
}
