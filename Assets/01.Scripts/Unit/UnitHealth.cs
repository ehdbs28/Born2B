using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IHealth
{
    private int _currentHp;
    private UnitStatContainer _container;
    public int CurrentHp => _currentHp;

    public int MaxHp => _container[StatType.MaxHP];

    private bool canChangedHP = true;

    private int _canChangedHPCount = 0;
    int IHealth.CanChangedHPCount { get => _canChangedHPCount;
        set 
        {
            _canChangedHPCount = Mathf.Clamp(value, 0, int.MaxValue);

            canChangedHP = _canChangedHPCount == 0;
        }
    }

    private void Awake()
    {
        _container = GetComponent<UnitStatContainer>();
    }

    public void AddHp(int healHp)
    {
        if (!canChangedHP) return;

        _currentHp = Mathf.Clamp(_currentHp + healHp, 0, MaxHp);
    }

    public void ReduceHp(int reduceHp)
    {
        if (!canChangedHP) return;

        _currentHp = Mathf.Clamp(_currentHp - reduceHp, 0, MaxHp);
    }

    public void ResetHp()
    {
        if (!canChangedHP) return;

        _currentHp = MaxHp;
    }
}
