using System;
using UnityEngine;

[Serializable]
public class ItemDatabaseSlot : IEquatable<ItemDatabaseSlot>
{
    public IItemData itemData;
    [Range(1, 10)] public int weight;

    public bool Equals(ItemDatabaseSlot other)
    {
        return other.itemData == itemData;
    }
}