using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponController : MonoBehaviour
{
    
    protected virtual Weapon _controlWeapon { get; set; }

    public void Init(WeaponItemSO so, CellObjectInstance owner)
    {

        _controlWeapon = Instantiate(so.WeaponPrefab, transform);
        _controlWeapon.Init(owner);

    }

    public void Attack(Vector2Int position, Vector2Int point)
    {

        var param = default(AttackParams);
        _controlWeapon.Attack(param, position, point);

    }

    public void Rotate(Vector2 origin, Vector2 target)
    {

        //

    }

}
