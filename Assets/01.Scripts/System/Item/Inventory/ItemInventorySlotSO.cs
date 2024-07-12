using System;
using UnityEngine;

public abstract class ItemInventorySlotSO : ScriptableObject
{
    public abstract Type ItemType { get; }

    protected IItemHandler owner = null;
    public virtual void Init(IItemHandler owner)
    {
        this.owner = owner;
    }

    public abstract bool AddItem(ItemSO itemData);
    public abstract bool RemoveItem(ItemSO itemData);
}
