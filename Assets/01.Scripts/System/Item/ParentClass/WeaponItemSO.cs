using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Weapon/WeaponItem")]
public class WeaponItemSO : ItemSO
{
    [Space(15f)]
    public Weapon WeaponPrefab;

    [Space(15f)]
    public StatusType EffectedStatusType;
    public int EffectedTurnCount;

    [Space(15f)]
    public int KnockBackPower;
    public float Damage;

    public WeaponRange Range;

    /// <summary>
    /// Equip Weapon
    /// </summary>
    public override bool Execute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IWeaponItemHandler weaponHandler) == false)
            return false;

        bool result = weaponHandler.EquipWeapon(this);
        return result;
    }

    /// <summary>
    /// Unequip Weapon
    /// </summary>
    public override bool Unexecute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IWeaponItemHandler weaponHandler) == false)
            return false;

        bool result = weaponHandler.UnequipWeapon();
        return result;
    }
}

public interface IWeaponItemHandler : IItemHandler
{
    public WeaponItemSO CurrentWeaponData { get; }
    public bool EquipWeapon(WeaponItemSO weaponData);
    public bool UnequipWeapon();
}