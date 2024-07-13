using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerHealthComponent : PlayerComponent, IHealth
{
    private PlayerStatComponent stat = null;
    
    public Action<int, int> OnChangedHpEvent { get; set;  }

    
    private int currentHp = 0;
    public int CurrentHp => currentHp;

    public int MaxHp => stat.GetStat(StatType.MaxHP);

    public bool CanChangedHP { get; set; } = true;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        stat = player.GetPlayerComponent<PlayerStatComponent>();
        ResetHp();
    }
    
    public void ResetHp()
    {
        if (!CanChangedHP) return;

        currentHp = MaxHp;
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }

    public void AddHp(int healHp)
    {
        if (!CanChangedHP) return;

        currentHp = Mathf.Clamp(currentHp + healHp, 0, MaxHp);
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }

    public void ReduceHp(int reduceHp)
    {
        if (!CanChangedHP) return;

        currentHp = Mathf.Clamp(currentHp - reduceHp, 0, MaxHp);
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }
}
