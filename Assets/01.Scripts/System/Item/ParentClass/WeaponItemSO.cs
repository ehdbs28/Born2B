using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Weapon/WeaponItem")]
public class WeaponItemSO : ItemSO
{
    [Space(15f)]
    public Weapon WeaponPrefab;
    public ItemDatabaseSO Evolutions = null;

    [Space(15f)]
    public List<StatusEffectSlot> StatusEffects = new List<StatusEffectSlot>();

    [Space(15f)]
    public int KnockBackPower;
    public float Damage;

    public WeaponRange Range;

    public override Type ItemType => typeof(WeaponItemSO);

    /// <summary>
    /// Equip Weapon
    /// </summary>
    public override void Execute(IItemHandler handler)
    {
        if(TryParseHandler(handler, out IWeaponItemHandler weaponHandler) == false)
            return;

        weaponHandler.EquipWeapon(this);
    }

    /// <summary>
    /// Unequip Weapon
    /// </summary>
    public override void Unexecute(IItemHandler handler)
    {
        if (TryParseHandler(handler, out IWeaponItemHandler weaponHandler) == false)
            return;

        weaponHandler.UnequipWeapon();
    }
}

public interface IWeaponItemHandler : IItemHandler
{
    public WeaponItemSO CurrentWeaponData { get; }
    public bool EquipWeapon(WeaponItemSO weaponData);
    public bool UnequipWeapon();
}