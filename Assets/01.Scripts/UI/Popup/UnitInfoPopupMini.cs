using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoPopupMini : UIComponent
{
    private Transform _statusParent;

    private Material _hpSliderMat;
    private List<UIComponent> _statusIcons;

    private UnitDataSO _unitData;

    protected override void Awake()
    {
        base.Awake();
        _statusParent = transform.Find("StatusEffect");
    }

    public void Init(UnitDataSO newData)
    {
        newData.statusController.StatusEffect.OnStatusChanged += StatusHandle;
        StatusHandle();
        _unitData = newData;
    }

    public override void Disappear(bool poolIn = true)
    {
        foreach (var statusIcon in _statusIcons)
        {
            statusIcon.Disappear();
        }
        _statusIcons.Clear();
        
        _unitData.statusController.StatusEffect.OnStatusChanged += StatusHandle;
        base.Disappear(poolIn);
    }
    
    private void StatusHandle()
    {
        foreach (var statusIcon in _statusIcons)
        {
            statusIcon.Disappear();
        }
        _statusIcons.Clear();
        
        var effectedStatus = _unitData.statusController.StatusEffect.EffectedStatus;
        foreach (var effected in effectedStatus)
        {
            var statusIcon = UIManager.Instance.AppearUI(Enum.Parse<PoolingItemType>($"StatusIconMiniUnit-{effected}"), _statusParent);
            _statusIcons.Add(statusIcon);
        }
    }
}