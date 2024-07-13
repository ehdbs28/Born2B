using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemIventory/Equipment", order = -1)]
public class EquipmentItemInventorySO : ItemInventorySlotSO
{
    public override Type ItemType => typeof(EquipmentItemSO);

    public override List<ItemSO> AllItems
    {
        get
        {
            var list = new List<ItemSO>();
            foreach (var pair in inventory)
            {
                list.Add(pair.Key);
            }
            return list;
        }
    }

    private Dictionary<EquipmentItemSO, int> inventory = new Dictionary<EquipmentItemSO, int>();

    public override bool AddItem(ItemSO itemData)
    {
        EquipmentItemSO item = itemData as EquipmentItemSO;
        if(inventory.ContainsKey(item) == false)
            inventory.Add(item, 0);

        item.Execute(owner);
        inventory[item]++;
        return true;
    }

    public override bool RemoveItem(ItemSO itemData)
    {
        EquipmentItemSO item = itemData as EquipmentItemSO;
        if(inventory.ContainsKey(item) == false)
            return false;

        if(inventory[item] <= 0)
            return false;

        inventory[item]--;
        item.Unexecute(owner);
        return true;
    }
}
