using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UIComponent
{
    [SerializeField] ArtifactItemInventorySO _artifactInventory = null;
    [SerializeField] List<InventoryUnitHolder> _unitHolders;

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        _artifactInventory.OnInventoryChangedEvent += HandleInventoryChanged;
    }

    public override void Disappear(bool poolIn = true)
    {
        base.Disappear(poolIn);
    }

    private void HandleInventoryChanged(ArtifactType type, List<ArtifactItemSO> items)
    {
        if(type == ArtifactType.Usable)
        {
            int i = 0;
            for(i = 0; i < items.Count; ++i)
                _unitHolders[i].SetItem(items[i]);
            for(; i < _unitHolders.Count; ++i)
                _unitHolders[i].SetItem(null);
        }
    }
}