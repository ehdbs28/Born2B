using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInstance : UnitInstance
{

    public override void Init(CellObjectSO so)
    {

        base.Init(so);
        var casted = so as BossSO;
        var parent = transform.Find("WeaponContainer");

        foreach(var item in casted.weapons)
        {

            var obj = new GameObject(item.objectName);
            obj.transform.SetParent(parent);
            var wp = obj.AddComponent<UnitWeaponController>();

            wp.Init(item.item, this);

        }

    }

}
