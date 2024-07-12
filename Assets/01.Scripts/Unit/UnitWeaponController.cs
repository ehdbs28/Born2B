using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponController : MonoBehaviour
{
    
    protected virtual Weapon _controlWeapon { get; set; }
    [field:SerializeField] public LayerMask mask { get; set; }

    public void Init(WeaponItemSO so, CellObjectInstance owner)
    {

        _controlWeapon = Instantiate(so.WeaponPrefab, transform);
        _controlWeapon.Init(owner);

    }

    public void Attack(Vector2Int position, Vector2Int point)
    {

        var param = new AttackParams(0, 0, 0, mask);
        _controlWeapon.Attack(param, position, point);

    }

    public void Rotate(Vector2 origin, Vector2 target)
    {

        //

    }

}
