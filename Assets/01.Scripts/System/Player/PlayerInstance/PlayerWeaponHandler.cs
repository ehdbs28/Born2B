using UnityEngine;

public partial class PlayerInstance : IWeaponItemHandler, IWeaponArtifactItemHandler
{
    public WeaponItemSO CurrentWeaponData => GetPlayerComponent<PlayerWeaponComponent>().CurrentWeapon?.WeaponData;

    public Weapon CurrentWeapon => GetPlayerComponent<PlayerWeaponComponent>().CurrentWeapon;

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
