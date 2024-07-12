public partial class PlayerInstance : IHealingItemHandler
{
    public IHealth Health => GetPlayerComponent<PlayerHealthComponent>();
}
