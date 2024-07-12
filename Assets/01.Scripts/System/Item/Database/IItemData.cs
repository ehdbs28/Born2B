
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemData : ScriptableObject
{
    public abstract void RegisterItemData(Dictionary<ItemRarity, ItemDatabaseTable> database, int weight);
    public abstract ItemSO PickRandom();
}
