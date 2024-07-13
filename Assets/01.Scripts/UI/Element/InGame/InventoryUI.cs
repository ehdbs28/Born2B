using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UIComponent
{
    [SerializeField] ArtifactItemInventorySO _artifactInventory = null;
    [SerializeField] List<InventoryUnitHolder> _unitHolders;

    public void Init()
    {
        _artifactInventory.OnInventoryChangedEvent += HandleInventoryChanged;
    }

    public void Release()
    {
        _artifactInventory.OnInventoryChangedEvent -= HandleInventoryChanged;
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