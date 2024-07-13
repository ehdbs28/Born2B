using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackShot : HitscanWeapon
{
    private Transform _playerTrm;

    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);

        _playerTrm = owner.transform;
        OnUnitHitEvent += HandleCheckEnemyBack;
    }

    private void HandleCheckEnemyBack(CellObjectInstance obj)
    {
        if(_playerTrm.position.y > obj.transform.position.y)
        {
            if(obj.TryGetComponent<StatusController>(out var control))
            {
                control.AddStatus(StatusType.Sturned, 1);
                control.AddStatus(StatusType.Burn, 2);
                control.AddStatus(StatusType.Drained, 1);
                control.AddStatus(StatusType.Silence, 2);
            }
        }
    }
}
