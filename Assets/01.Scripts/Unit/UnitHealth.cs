using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IHealth
{
    private int _currentHp;
    private UnitStatContainer _container;
    public int CurrentHp => _currentHp;
    
    public  Action<int, int> OnChangedHpEvent { get; set;  }

    public int MaxHp => _container[StatType.MaxHP];

    public bool CanChangedHP { get; set; }

    private void Awake()
    {
        _container = GetComponent<UnitStatContainer>();
    }

    public void AddHp(int healHp)
    {
        
    }

    public void ReduceHp(int reduceHp)
    {
        _currentHp = Mathf.Clamp(_currentHp - reduceHp, 0, MaxHp);
        OnChangedHpEvent?.Invoke(_currentHp, MaxHp);
    }
    
    public void ResetHp()
    {
        _currentHp = MaxHp;
        OnChangedHpEvent?.Invoke(_currentHp, MaxHp);
    }
}
