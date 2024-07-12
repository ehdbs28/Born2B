using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemDatabase", order = -1)]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] List<ItemDatabaseSlot> items = new List<ItemDatabaseSlot>();

    private Dictionary<ItemRarity, ItemDatabaseTable> database = null;
    public ItemDatabaseTable this[ItemRarity rarity] => database[rarity];

    private void OnEnable()
    {
        database = new Dictionary<ItemRarity, ItemDatabaseTable>() {
            [ItemRarity.Common] = new ItemDatabaseTable(),
            [ItemRarity.Rare] = new ItemDatabaseTable(),
            [ItemRarity.Epic] = new ItemDatabaseTable()
        };

        items.ForEach(i => {
            if (i == null || i.itemData == null)
                return;
            database[i.itemData.Rarity].table.Add(i);
        });

        foreach(ItemDatabaseTable table in database.Values)
            table.Init();
    }

    public ItemSO PickRandom()
    {
        ItemRarity rarity = (ItemRarity)Random.Range(((int)ItemRarity.None) + 1, (int)ItemRarity.END);
        return PickRandom(rarity);
    }

    public ItemSO PickRandom(ItemRarity rarity)
    {
        ItemSO item = this[rarity].PickRandom();
        return item;
    }
}
