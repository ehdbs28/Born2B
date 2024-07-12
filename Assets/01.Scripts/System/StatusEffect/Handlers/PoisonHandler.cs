using UnityEngine;

namespace StatusEffects
{
    public class PoisonHandler : StatusEffectHandler
    {
        private const int MAX_STACK_COUNT = 16;
        private int stack = 0;

        public override void HandleBegin()
        {
            base.HandleBegin();
            stack = 1;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();

            stack = Mathf.Min(stack + 1, MAX_STACK_COUNT);
            if(owner.TryGetComponent<IHealth>(out IHealth ih))
            {
                float delta = stack / (float)MAX_STACK_COUNT;
                float damage = ih.MaxHp * delta;
                ih.ReduceHp((int)damage);
            }
        }
    }
}
