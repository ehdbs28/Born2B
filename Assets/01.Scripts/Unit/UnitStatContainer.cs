using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatContainer : MonoBehaviour
{

    [SerializeField] private StatSO _stat;

    private void Awake()
    {
        
        _stat = Instantiate(_stat);

    }

    public Stat this[StatType type]
    {

        get
        {

            return _stat[type];

        }

    }

}
