namespace StatusEffects
{
    public class SilenceHandler : StatusEffectHandler
    {
        private UnitStatContainer statContainer = null;

        public override void Init(CellObjectInstance owner)
        {
            base.Init(owner);
            statContainer = owner.GetComponent<UnitStatContainer>();
        }

        public override void HandleBegin()
        {
            base.HandleBegin();
            statContainer[StatType.Attack].AddModifier(StatModifierType.MultiplicationMultiplier, 0);
            statContainer[StatType.MagicPower].AddModifier(StatModifierType.MultiplicationMultiplier, 0);
        }

        public override void HandleEnd()
        {
            base.HandleEnd();
            statContainer[StatType.Attack].RemoveModifier(StatModifierType.MultiplicationMultiplier, 0);
            statContainer[StatType.MagicPower].RemoveModifier(StatModifierType.MultiplicationMultiplier, 0);
        }
    }
}
