using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 위치에서 해당 위치까지의 
/// </summary>
public abstract class RangeWeapon : Weapon
{
    [SerializeField] protected LayerMask obstacleLayer = 1 << 6;

    protected override List<WeaponRangeSlot> GetAttackRange(Vector2Int position, Vector2Int point)
    {
        List<WeaponRangeSlot> attackRanges = new List<WeaponRangeSlot>(ranges.Count);
        Dictionary<Vector2Int, int> areas = new Dictionary<Vector2Int, int>();

        for(int i = 0; i < ranges.Count; ++i)
        {
            WeaponRangeSlot rangeSlot = ranges[i];
            rangeSlot.Position = ProcessTarget(position, rangeSlot.Position + point);
            rangeSlot.Theta = ProcessTheta(rangeSlot.Theta);
            if(rangeSlot.Theta >= 0f)
            {
                if(areas.ContainsKey(rangeSlot.Position))
                {
                    int index = areas[rangeSlot.Position];
                    if(rangeSlot.Theta > attackRanges[index].Theta)
                        attackRanges[index] = rangeSlot;
                }
                else
                {
                    attackRanges.Add(rangeSlot);
                    areas.Add(rangeSlot.Position, attackRanges.Count - 1);
                }
            }
        }
        
        return attackRanges;
    }

    public override Vector2Int GetAttackPoint(Vector2Int position, Vector2Int point) => point;

    protected abstract float ProcessTheta(float theta);

	public virtual Vector2Int ProcessTarget(Vector2Int from, Vector2Int target)
    {
        #if UNITY_EDITOR
        Debug.DrawLine(from.GetVector(), target.GetVector(), Color.red, 1f);
        #endif

        Vector2 direction = target - from;
        RaycastHit2D hit = Physics2D.Raycast(from, direction.normalized, float.MaxValue, obstacleLayer);
        if (hit && hit.transform.TryGetComponent<IHitable>(out IHitable id))   
            target = id.Position;

        #if UNITY_EDITOR
                Debug.DrawLine(from.GetVector(), target.GetVector(), Color.green, 1f);
        #endif

        return target;
    }
}
