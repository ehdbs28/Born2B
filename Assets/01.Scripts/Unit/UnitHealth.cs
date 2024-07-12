using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IHealth
{

    private int _currentHp;
    private UnitStatContainer _container;
    public int CurrentHp => _currentHp;

    public int MaxHp => _container[StatType.MaxHP];

    private void Awake()
    {
        
        _container = GetComponent<UnitStatContainer>();

    }

    public void AddHp(int healHp)
    {

        _currentHp += healHp;

    }

    public void ReduceHp(int reduceHp)
    {

        _currentHp -= reduceHp;

    }

    public void ResetHp()
    {

        _currentHp = MaxHp;

    }


}
