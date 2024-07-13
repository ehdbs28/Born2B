using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemIventory/Weapon", order = -1)]
public class WeaponItemInventorySO : ItemInventorySlotSO
{
    public override Type ItemType => typeof(WeaponItemSO);

    public override List<ItemSO> AllItems => throw new NotImplementedException();

    private IWeaponItemHandler weaponHandler = null;

    public override void Init(IItemHandler owner)
    {
        base.Init(owner);
        weaponHandler = owner as IWeaponItemHandler;
    }

    public override bool AddItem(ItemSO itemData)
    {
        WeaponItemSO item = itemData as WeaponItemSO;
        if(weaponHandler.CurrentWeaponData == null)
            item.Execute(owner);
        else
        {
            if(weaponHandler.CurrentWeaponData == this)
            {
                WeaponItemSO evolution = item.Evolutions?.PickRandom(weaponHandler.CurrentWeaponData.Rarity) as WeaponItemSO;
                if(evolution == null)
                    return false;

                RemoveItem(weaponHandler.CurrentWeaponData);
                AddItem(evolution);
            }
            else
            {
                RemoveItem(weaponHandler.CurrentWeaponData);
                AddItem(itemData);
            }
        }

        return true;
    }

    public override bool RemoveItem(ItemSO itemData)
    {
        itemData.Unexecute(owner);
        return true;
    }
}
