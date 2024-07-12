using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Weapon/PlayerWeaponItem")]
public class PlayerWeaponItemSO : WeaponItemSO
{
    [Header("Player Weapon Property")]
    [SerializeField] ItemInventorySO inventory = null;
    [SerializeField] ItemDatabaseSO evolutions = null;

    public override bool Execute(IItemHandler handler)
    {
        bool result = base.Execute(handler);
        if(result)
            return result;

        if(TryParseHandler(handler, out IWeaponItemHandler weaponHandler) == false)
            return false;

        if(weaponHandler.CurrentWeaponData == this)
        {
            PlayerWeaponItemSO evolution = evolutions.PickRandom(weaponHandler.CurrentWeaponData.Rarity) as PlayerWeaponItemSO;
            if(evolution == null)
                return false;

            inventory.RemoveItem(this);
            inventory.AddItem(evolution);
            return false;
        }

        return true;
    }
}
