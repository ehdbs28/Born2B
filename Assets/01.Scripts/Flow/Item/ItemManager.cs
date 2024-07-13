using System;
using Singleton;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] ItemDatabaseSO equipmentItemDatabase = null;
    [SerializeField] ItemDatabaseSO artifactItemDatabase = null;
    private ItemDatabaseSO itemDatabase = null;

    public void Init(ItemDatabaseSO weaponDatabase)
    {
        itemDatabase = Instantiate(weaponDatabase);
        itemDatabase.Init(equipmentItemDatabase, artifactItemDatabase);
    }

    public void PickItems()
    {
        var itemSelectPanel = UIManager.Instance.AppearUI(PoolingItemType.ItemSelectPanel) as ItemSelectPanel;
        
        ItemSO[] items = new ItemSO[3];
        for(int i = 0; i < items.Length; ++i)
        {
            do
                items[i] = itemDatabase.PickRandom();
            while (items[i] == null);
        }
        
        itemSelectPanel.ItemSet(items);
    }
}
