using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthComponent : PlayerComponent, IHealth
{
    private PlayerStatComponent stat = null;
    
    public Action<int, int> OnChangedHpEvent { get; set;  }

    [SerializeField] private int currentHp = 0;
    public int CurrentHp => currentHp;

    public int MaxHp => stat.GetStat(StatType.MaxHP);

    private bool canChangedHP = true;

    private int _canChangedHPCount = 0;
    int IHealth.CanChangedHPCount { get => _canChangedHPCount; 
        set
        {
            _canChangedHPCount = Mathf.Clamp(value, 0, int.MaxValue);

            canChangedHP = _canChangedHPCount == 0;
        }
    }

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        stat = player.GetPlayerComponent<PlayerStatComponent>();
        EventManager.Instance.RegisterEvent(EventType.OnStageLoaded, HandleStageLoad);

        
    }

    private void HandleStageLoad(object[] args)
    {

        ResetHp();

    }

    public void ResetHp()
    {
        if (!canChangedHP) return;

        currentHp = MaxHp;
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }

    public void AddHp(int healHp)
    {
        if (!canChangedHP) return;

        currentHp = Mathf.Clamp(currentHp + healHp, 0, MaxHp);
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }

    public void ReduceHp(int reduceHp)
    {
        if (!canChangedHP) return;

        currentHp = Mathf.Clamp(currentHp - reduceHp, 0, MaxHp);
        OnChangedHpEvent?.Invoke(currentHp, MaxHp);
    }
}
