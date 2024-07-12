using System;
using UnityEngine;

[Serializable]
public class ItemDatabaseSlot : IEquatable<ItemDatabaseSlot>
{
    public IItemData itemData;
    [Range(0, 10)] public float weight;

    public bool Equals(ItemDatabaseSlot other)
    {
        return other.itemData == itemData;
    }
}