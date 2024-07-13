public partial class PlayerInstance : IHealthItemHandler
{
    public IHealth Health => GetPlayerComponent<PlayerHealthComponent>();
}
