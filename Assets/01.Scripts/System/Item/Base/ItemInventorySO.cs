using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemInventory", order = -1)]
public class ItemInventorySO : ScriptableObject
{
    public event Action<ItemSO> OnItemAddedEvent = null;
    public event Action<ItemSO> OnItemRemovedEvent = null;

	private Dictionary<ItemSO, int> items = null;
    private IItemHandler owner = null;

    public void Init(IItemHandler owner)
    {
        this.owner = owner;
        items = new Dictionary<ItemSO, int>();
    }

    public int GetItemCount(ItemSO item)
    {
        if(items.ContainsKey(item) == false)
            return 0;
        return items[item];
    }

    public void AddItem(ItemSO item)
    {
        bool result = item.Execute(owner);
        if(result == false)
            return;

        if(items.ContainsKey(item) == false)
            items.Add(item, 0);
        items[item]++;
        OnItemAddedEvent.Invoke(item);
    }

    public void RemoveItem(ItemSO item)
    {
        if(items.ContainsKey(item) == false || items[item] <= 0)
        {
            Debug.LogError("Trying to Remove Unexisted Item");
            return;
        }

        bool result = item.Unexecute(owner);
        if(result == false)
            return;

        items[item]--;
        OnItemRemovedEvent?.Invoke(item);
    }
}
