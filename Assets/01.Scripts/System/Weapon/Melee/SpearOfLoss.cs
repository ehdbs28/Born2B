using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOfLoss : MeleeWeapon
{
    private int _atkToken;
    private bool _canCounting = true;

    public override void Init(CellObjectInstance owner)
    {
        base.Init(owner);

        OnUnitHitEvent += HandleHitUnit;
        EventManager.Instance.RegisterEvent(EventType.OnPlayerTurnOver, HandleResetToken);
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnRegisterEvent(EventType.OnPlayerTurnOver, HandleResetToken);
    }

    private void HandleResetToken(object[] args)
    {
        _canCounting = true;
        _atkToken = 0;
    }

    private void HandleHitUnit(CellObjectInstance obj)
    {
        if (!_canCounting) return;

        _atkToken++;

        if(_atkToken == 2)
        {
            _canCounting = false;

            var Units = CellObjectManager.Instance.GetCellObjectInstances<UnitInstance>();

            foreach (var u in Units) 
            {
                if(u.TryGetComponent<StatusController>(out StatusController controller))
                {
                    controller.AddStatus(StatusType.Fixed, 1);
                }
            }
        }
    }
}
