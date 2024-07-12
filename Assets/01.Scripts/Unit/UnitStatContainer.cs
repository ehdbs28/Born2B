using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatContainer : MonoBehaviour
{

    private StatSO _stat;

    public void Init(StatSO so)
    {
        
        _stat = so;
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
