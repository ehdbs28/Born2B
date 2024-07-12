using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region TEST
    [SerializeField] ItemInventorySO inventory;
    [SerializeField] ItemDatabaseSO weaponDatabase;
    #endregion

    [SerializeField] ItemDatabaseSO equipmentItemDatabase = null;
    [SerializeField] ItemDatabaseSO artifactItemDatabase = null;
    private ItemDatabaseSO itemDatabase = null;

    private void Awake()
    {
        Init(weaponDatabase);
    }

    public void Init(ItemDatabaseSO weaponDatabase)
    {
        itemDatabase = Instantiate(weaponDatabase);
        itemDatabase.Init(equipmentItemDatabase, artifactItemDatabase);
    }

    public void PickItems()
    {
        ItemSO[] items = new ItemSO[3];
        for(int i = 0; i < items.Length; ++i)
        {
            do
                items[i] = itemDatabase.PickRandom();
            while (items[i] == null);
        }

        inventory.AddItem(items[0]);
    }
}
