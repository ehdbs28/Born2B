using UnityEngine;

public partial class PlayerInstance : IWeaponItemHandler
{
    public WeaponItemSO CurrentWeaponData => GetPlayerComponent<PlayerWeaponComponent>().CurrentWeapon?.WeaponData;

    public bool EquipWeapon(WeaponItemSO weaponData)
    {
        bool result = GetPlayerComponent<PlayerWeaponComponent>().EquipWeapon(weaponData);
        return result;
    }

    public bool UnequipWeapon()
    {
        bool result = GetPlayerComponent<PlayerWeaponComponent>().UnequipWeapon();
        return result;
    }
}
