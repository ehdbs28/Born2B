namespace StatusEffects
{
    public class BurnHandler : StatusEffectHandler
    {
        public const float BURN_DAMAGE_RATIO = 1f / 8f;

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            
            if(owner.TryGetComponent<IHealth>(out IHealth ih))
            {
                float damage = ih.MaxHp * BURN_DAMAGE_RATIO;
                ih.ReduceHp((int)damage);
            }
        }
    }
}
