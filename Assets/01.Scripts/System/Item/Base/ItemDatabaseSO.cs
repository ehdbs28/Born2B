using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemDatabase", order = -1)]
public class ItemDatabaseSO : ScriptableObject
{
    //[SerializeField] List<ItemSO> commonItems = new List<ItemSO>();
    //[SerializeField] List<ItemSO> rareItems = new List<ItemSO>();
    [SerializeField] List<ItemSO> items= new List<ItemSO>();

    private Dictionary<ItemRarity, List<ItemSO>> database = null;
    public List<ItemSO> this[ItemRarity rarity] => database[rarity];

    private void OnEnable()
    {
        database = new Dictionary<ItemRarity, List<ItemSO>>() {
            [ItemRarity.Common] = new List<ItemSO>(),
            [ItemRarity.Rare] = new List<ItemSO>(),
            [ItemRarity.Epic] = new List<ItemSO>()
        };

        items.ForEach(i => {
            if (i == null)
                return;
            database[i.Rarity].Add(i);
        });
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
