using System;
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

    public void Attack(Vector2Int position)
    {

        var param = new AttackParams(0, 0, 0, mask);
        Vector2 cloestTarget = GetCloestTarget(position);
        Rotate(position, cloestTarget);
        _controlWeapon.Attack(param, position, position);

    }

    private Vector2 GetCloestTarget(Vector2Int position)
    {

        var objs = Physics2D.OverlapCircleAll(position, 100, mask);
        Vector2 target = position;
        var min = float.MaxValue;

        foreach(var obj in objs)
        {
            var dist = Vector2.Distance(position, obj.transform.position);
            if (dist < min)
            {

                target = obj.transform.position;
                min = dist;

            }

        }

        return target;

    }

    public void Rotate(Vector2 origin, Vector2 target)
    {

        var dir = target - origin;
        var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _controlWeapon.RotateAttackRange(ang);

    }

}
