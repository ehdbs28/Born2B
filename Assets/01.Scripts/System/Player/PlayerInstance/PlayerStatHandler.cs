public partial class PlayerInstance : IStatModifierItemHandler
{
    public StatSO Stat => GetPlayerComponent<PlayerStatComponent>().Stat;
}
