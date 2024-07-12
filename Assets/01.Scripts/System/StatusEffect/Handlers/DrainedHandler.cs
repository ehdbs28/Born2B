using UnityEngine;

namespace StatusEffects
{
    public class DrainedHandler : StatusEffectHandler
    {
        private UnitStatContainer statContainer = null;
        private const float DAMAGE_MULTIPLIER = 0.5f;

        public override void Init(CellObjectInstance owner)
        {
            base.Init(owner);
            statContainer = owner.GetComponent<UnitStatContainer>();
        }

        public override void HandleBegin()
        {
            base.HandleBegin();
            statContainer[StatType.Attack].AddModifier(StatModifierType.MultiplicationMultiplier, DAMAGE_MULTIPLIER);
            statContainer[StatType.MagicPower].AddModifier(StatModifierType.MultiplicationMultiplier, DAMAGE_MULTIPLIER);
        }

        public override void HandleEnd()
        {
            base.HandleEnd();
            statContainer[StatType.Attack].RemoveModifier(StatModifierType.MultiplicationMultiplier, DAMAGE_MULTIPLIER);
            statContainer[StatType.MagicPower].RemoveModifier(StatModifierType.MultiplicationMultiplier, DAMAGE_MULTIPLIER);
        }
    }
}
