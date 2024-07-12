using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemIventory/Inventory", order = -2)]
public class ItemInventorySO : ScriptableObject
{
    public event Action<ItemSO> OnItemAddedEvent = null;
    public event Action<ItemSO> OnItemRemovedEvent = null;

    [SerializeField] List<ItemInventorySlotSO> inventorySlots = new List<ItemInventorySlotSO>();
    private Dictionary<Type, ItemInventorySlotSO> inventory = new Dictionary<Type, ItemInventorySlotSO>();

    private IItemHandler owner = null;

    public void Init(IItemHandler owner)
    {
        this.owner = owner;

        inventory = new Dictionary<Type, ItemInventorySlotSO>();
        inventorySlots.ForEach(i => {
            if(inventory.ContainsKey(i.ItemType))
                return;
            inventory.Add(i.ItemType, i);
            i.Init(owner);
        });
    }

    public bool AddItem(ItemSO item)
    {
        if(inventory.ContainsKey(item.ItemType) == false)
            return false;

        bool result = inventory[item.ItemType].AddItem(item);
        if(result == false)
            return result;

        OnItemAddedEvent.Invoke(item);

        return true;
    }

    public bool RemoveItem(ItemSO item)
    {
        if(inventory.ContainsKey(item.ItemType) == false)
            return false;

        bool result = inventory[item.ItemType].RemoveItem(item);
        if(result == false)
            return result;

        OnItemRemovedEvent.Invoke(item);

        return true;
    }
}
