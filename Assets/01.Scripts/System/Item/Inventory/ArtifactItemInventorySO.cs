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

    public event Action<ArtifactType, List<ArtifactItemSO>> OnInventoryChangedEvent = null;
    public override Type ItemType => typeof(ArtifactItemSO);

    public override List<ItemSO> AllItems => throw new NotImplementedException();

    [SerializeField] List<ArtifactItemInventorySlot> inventorySlots = new List<ArtifactItemInventorySlot>();

    private void OnEnable()
    {
        foreach (var slot in inventorySlots)
        {
            slot.inventory.Clear();
        }
    }

    public override bool AddItem(ItemSO itemData)
    {
        ArtifactItemSO item = itemData as ArtifactItemSO;

        ArtifactItemInventorySlot slot = inventorySlots.Find(i => i.inventoryType.HasFlag(item.ArtifactType));
        if (slot == null)
        {
            item.Execute(owner);
            return true;
        }

        if(slot.inventory.Count == slot.inventorySize)
            return false;

        item.Execute(owner);
        slot.inventory.Add(item);
        OnInventoryChangedEvent?.Invoke(item.ArtifactType, slot.inventory);

        return true;
    }

    public override bool RemoveItem(ItemSO itemData)
    {
        ArtifactItemSO item = itemData as ArtifactItemSO;

        ArtifactItemInventorySlot slot = inventorySlots.Find(i => i.inventoryType.HasFlag(item.ArtifactType));

        slot?.inventory.Remove(item);
        item.Unexecute(owner);
        OnInventoryChangedEvent?.Invoke(item.ArtifactType, slot?.inventory);

        return true;
    }
}
