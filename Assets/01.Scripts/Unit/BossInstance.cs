using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInstance : CellObjectInstance
{

    public override void Init(CellObjectSO so)
    {

        base.Init(so);
        var casted = so as BossSO;
        var parent = transform.Find("WeaponContainer");

        foreach(var item in casted.weapons)
        {

            var obj = new GameObject(item.objectName);
            var wp = obj.AddComponent<UnitWeaponController>();
            obj.transform.SetParent(parent);

            wp.Init(item.item, this);

        }

    }

}
