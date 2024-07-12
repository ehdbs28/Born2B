using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemDatabase", order = -1)]
public class ItemDatabaseSO : IItemData
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


        RegisterItemData(database, 0);

        foreach(ItemDatabaseTable table in database.Values)
            table.Init();
    }

    public override ItemSO PickRandom()
    {
        ItemRarity rarity = (ItemRarity)Random.Range(((int)ItemRarity.None) + 1, (int)ItemRarity.END);
        return PickRandom(rarity);
    }

    public ItemSO PickRandom(ItemRarity rarity)
    {
        ItemSO item = this[rarity].PickRandom();
        return item;
    }

    public override void RegisterItemData(Dictionary<ItemRarity, ItemDatabaseTable> database, float weight)
    {
        for(int i = 0; i < items.Count; ++i)
        {
            
        }
        items.ForEach(i => {            
            if (i == null || i.itemData == null)
                return;

            if(i.itemData == this)
            {
                Debug.LogError("Attempting to refer to its self. This may cause a stack overflow.");
                return;
            }
            
            i.itemData.RegisterItemData(database, i.weight);
        });
    }
}
