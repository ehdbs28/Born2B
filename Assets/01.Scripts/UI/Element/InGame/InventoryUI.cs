using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UIComponent
{
    private List<InventoryUnitHolder> _unitHolders;

    protected override void Awake()
    {
        base.Awake();

        var unitParent = transform.Find("UnitParent");
        _unitHolders = new List<InventoryUnitHolder>();
        unitParent.GetComponentsInChildren(_unitHolders);
    }
}