using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : IItemData
{
    public ItemRarity Rarity;
    public Sprite ItemIcon;
    public string ItemName;
    [TextArea] public string ItemDescription;

	public abstract bool Execute(IItemHandler handler);

    public abstract bool Unexecute(IItemHandler handler);
    protected bool TryParseHandler<T>(IItemHandler inHandler, out T outHandler) where T : class, IItemHandler
    {
        outHandler = inHandler as T;
        if (outHandler == null)
            return false;
        return true;
    }

    public override ItemSO PickRandom() => this;

    public override void RegisterItemData(Dictionary<ItemRarity, ItemDatabaseTable> database, int weight)
    {
        ItemDatabaseSlot slot = new ItemDatabaseSlot() { itemData = this, weight = weight };
        if(database[Rarity].table.Contains(slot))
            return;

        database[Rarity].table.Add(slot);
    }
}
