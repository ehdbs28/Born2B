using UnityEngine;

namespace StatusEffects
{
    public class ShockHandler : StatusEffectHandler
    {
        private const float SHOCK_CHANCE = 1f / 4f;

        public override void HandleUpdate()
        {
            base.HandleUpdate();

            float delta = Random.Range(0f, 100f);
            if (delta < SHOCK_CHANCE)
                owner.isSkip = true;
        }
    }
}
