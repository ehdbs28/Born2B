using UnityEngine;

public class PlayerHealthComponent : PlayerComponent, IHealth
{
    private PlayerStatComponent stat = null;

    private int currentHp = 0;
    public int CurrentHp => currentHp;

    public int MaxHp => stat.GetStat(StatType.MaxHP);

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        stat = player.GetPlayerComponent<PlayerStatComponent>();
    }

    public void ResetHp()
    {
        currentHp = (int)stat.GetStat(StatType.MaxHP).CurrentValue;
    }

    public void AddHp(int healHp)
    {
        currentHp += healHp;
    }

    public void ReduceHp(int reduceHp)
    {
        currentHp -= reduceHp;
    }
}
