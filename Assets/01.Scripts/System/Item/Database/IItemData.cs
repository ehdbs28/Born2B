
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemData : ScriptableObject
{
    public abstract void RegisterItemData(Dictionary<ItemRarity, ItemDatabaseTable> database, float weight);
    public abstract ItemSO PickRandom();
}
