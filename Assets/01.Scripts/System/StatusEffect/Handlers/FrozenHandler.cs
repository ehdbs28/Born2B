namespace StatusEffects
{
    public class FrozenHandler : StatusEffectHandler
    {
        public override void HandleUpdate()
        {
            base.HandleUpdate();

            owner.isSkip = true;
        }
    }
}
