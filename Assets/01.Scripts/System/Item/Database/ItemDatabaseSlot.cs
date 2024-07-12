using System;
using UnityEngine;

[Serializable]
public class ItemDatabaseSlot
{
    public ItemSO itemData;
    [Range(1, 10)] public int weight;
}