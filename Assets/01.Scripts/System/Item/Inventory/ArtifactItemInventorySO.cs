using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemIventory/Artifact", order = -1)]
public class ArtifactItemInventorySO : ItemInventorySlotSO
{
    [Serializable]
    public class ArtifactItemInventorySlot
    {
        public ArtifactType inventoryType = ArtifactType.None;
        public int inventorySize = 10;
        public List<ArtifactItemSO> inventory = new List<ArtifactItemSO>();
    }

    public override Type ItemType => typeof(ArtifactItemSO);

    [SerializeField] List<ArtifactItemInventorySlot> inventorySlots = new List<ArtifactItemInventorySlot>();

    public override bool AddItem(ItemSO itemData)
    {
        ArtifactItemSO item = itemData as ArtifactItemSO;

        ArtifactItemInventorySlot slot = inventorySlots.Find(i => i.inventoryType.HasFlag(item.ArtifactType));
        if(slot.inventory.Count == slot.inventorySize)
            return false;

        item.Execute(owner);
        slot.inventory.Add(item);

        return true;
    }

    public override bool RemoveItem(ItemSO itemData)
    {
        ArtifactItemSO item = itemData as ArtifactItemSO;

        ArtifactItemInventorySlot slot = inventorySlots.Find(i => i.inventoryType.HasFlag(item.ArtifactType));

        slot.inventory.Remove(item);
        item.Unexecute(owner);

        return true;
    }
}
