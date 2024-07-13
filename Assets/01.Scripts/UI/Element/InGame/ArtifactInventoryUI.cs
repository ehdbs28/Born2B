using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactInventoryUI : UIComponent
{
    [SerializeField] ArtifactItemInventorySO _artifactInventory = null;
    [SerializeField] List<ArtifactInventoryItemHolder> _itemHolders = new List<ArtifactInventoryItemHolder>();

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
        if(type == ArtifactType.Attributed || type == ArtifactType.CallByEvent)
        {
            int i = 0;
            for(i = 0; i < items.Count; ++i)
                _itemHolders[i].SetItem(items[i]);
            for(; i < _itemHolders.Count; ++i)
                _itemHolders[i].SetItem(null);
        }
    }
}