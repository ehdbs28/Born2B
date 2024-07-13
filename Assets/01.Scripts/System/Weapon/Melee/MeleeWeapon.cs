using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    /// <summary>
    /// 검 무기의 경우 별 다른 프로세싱 없이 공격 범위 그대로를 Offset만 적용시켜 넘김.
    /// </summary>
    protected override List<WeaponRangeSlot> GetAttackRange(Vector2Int position, Vector2Int point)
    {
        List<WeaponRangeSlot> attackRanges = new List<WeaponRangeSlot>(ranges.Count);

        for(int i = 0; i < ranges.Count; ++i)
        {
            WeaponRangeSlot rangeSlot = ranges[i];
            rangeSlot.Position += point;
            rangeSlot.Theta = 1f;
            attackRanges.Add(rangeSlot);
        }
        
        return attackRanges;
    }

    public override Vector2Int GetAttackPoint(Vector2Int position, Vector2Int point)
    {
        if(position == point)
            return position + Vector2Int.up;

        Vector2 direction = point - position;
        return (position + direction.normalized).GetVectorInt();
    }
}
