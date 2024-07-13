using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// theta를 백분율로 바꿔 각 위치의 데미지 계수를 반환
/// </summary>
public class WideAreaWeapon : RangeWeapon
{
    protected override List<WeaponRangeSlot> GetAttackRange(Vector2Int position, Vector2Int point)
    {
        #if UNITY_EDITOR
        Debug.DrawLine(position.GetVector(), point.GetVector(), Color.red, 1f);
        #endif

        Vector2 direction = point - position;
        RaycastHit2D hit = Physics2D.Raycast(position, direction.normalized, float.MaxValue, obstacleLayer);
        if (hit && hit.transform.TryGetComponent<IHitable>(out IHitable id))   
            point = id.Position;

        #if UNITY_EDITOR
                Debug.DrawLine(position.GetVector(), point.GetVector(), Color.green, 1f);
        #endif

        return base.GetAttackRange(position, point);
    }

    public override Vector2Int ProcessTarget(Vector2Int from, Vector2Int target)
    {
        return target;
    }

    protected override float ProcessTheta(float theta)
    {
        return theta * 0.01f;
    }
}
