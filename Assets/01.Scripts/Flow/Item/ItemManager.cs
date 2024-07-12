using UnityEngine;

public class ItemManager : MonoBehaviour
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
        ItemSO[] items = new ItemSO[3];
        for(int i = 0; i < items.Length; ++i)
            items[i] = itemDatabase.PickRandom();

        // 
    }
}
