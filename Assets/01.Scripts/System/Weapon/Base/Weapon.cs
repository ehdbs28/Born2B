using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField] WeaponItemSO weaponData = null;
    public WeaponItemSO WeaponData => weaponData;

    protected List<WeaponRangeSlot> ranges = null;
    public List<WeaponRangeSlot> Ranges => ranges;

    protected CellObjectInstance owner = null;
    private int directionIndex = 0;

    private Queue<StatusEffectSlot> disposableStatusEffects = new Queue<StatusEffectSlot>();

    public event Action<bool> OnCaculateCriticalEvent = null;

    protected event Action<CellObjectInstance> OnUnitHitEvent = null;
    protected event Action OnAttackEndEvent = null;

    protected float finalSlotDamage;

    public virtual void Init(CellObjectInstance owner)
    {
        this.owner = owner;
        RotateAttackRange(0f);
    }

    public virtual void Release()
    {
        ranges.Clear();
    }

    public void Attack(AttackParams attackParams, Vector2Int position, Vector2Int point)
    {
        float finalDamage = WeaponData.Damage * ((100 + attackParams.attack) * 0.01f);
        List<WeaponRangeSlot> attackRange = GetAttackRange(position, point);
        foreach(WeaponRangeSlot rangeSlot in attackRange)
        {            
            #if UNITY_EDITOR
            Debug.DrawLine(rangeSlot.Position - new Vector2(-0.5f, 0.5f), rangeSlot.Position - new Vector2(0.5f, 0.5f), Color.red, 1f);
            Debug.DrawLine(rangeSlot.Position - new Vector2(0.5f, 0.5f), rangeSlot.Position - new Vector2(0.5f, -0.5f), Color.red, 1f);
            Debug.DrawLine(rangeSlot.Position - new Vector2(0.5f, -0.5f), rangeSlot.Position - new Vector2(-0.5f, -0.5f), Color.red, 1f);
            Debug.DrawLine(rangeSlot.Position - new Vector2(-0.5f, -0.5f), rangeSlot.Position - new Vector2(-0.5f, 0.5f), Color.red, 1f);
            #endif

            float slotDamage = finalDamage * rangeSlot.Theta; // 최종적으로 theta는 데미지의 계수를 의미함
            bool critical = attackParams.ProcessCritical();
            if(critical)
                slotDamage += slotDamage * attackParams.criticalDamage;
            OnCaculateCriticalEvent?.Invoke(critical);

            Cell? cell = StageManager.Instance.Grid.FindCellByPosition(rangeSlot.Position);
            cell?.FindAndGrow();
            if(cell == null)
                continue;

            CellObjectInstance cellObject = CellObjectManager.Instance.GetCellObjectInstance(cell.Value.unitKey);
            if(cellObject == null)
                continue;

            int enemyLayer = 1 << cellObject.gameObject.layer;
            int compare = attackParams.targetLayer & enemyLayer;
            if(compare < 1)
                continue;
            
            // 마크
            if(cellObject.TryGetComponent<StatusController>(out StatusController sc))
            {
                WeaponData.StatusEffects.ForEach(i => sc.AddStatus(i.statusType, i.effectedTurn));
                while(disposableStatusEffects.Count > 0)
                {
                    StatusEffectSlot data = disposableStatusEffects.Dequeue();
                    sc.AddStatus(data.statusType, data.effectedTurn);
                }
            }

            if(cellObject.TryGetComponent<IHitable>(out IHitable ih))
            {
                ih.Hit(owner, slotDamage, critical, OnUnitHitEvent);
                finalDamage = slotDamage;
            }
        }

        OnAttackEndEvent?.Invoke();
    }

    public void RotateAttackRange(float angle)
    {
        int index = GetAttackDirectionIndex(angle);
        if(directionIndex == index)
            return;

        directionIndex = index;
        ranges = WeaponData.Range.GetRanges(directionIndex);
    }

    private int GetAttackDirectionIndex(float angle)
    {
        int directionCount = (int)WeaponDirection.END;
        float threshold = 360f / directionCount;

        angle %= 360;
        if (angle < 0)
            angle += 360;
        angle -= threshold * 0.5f;

        int index = Mathf.FloorToInt(angle / threshold) % directionCount;
        index = (directionCount + index - 3) % directionCount;
        index = directionCount - 1 - index;

        return index;
    }

    public void AddDisposableStatusEffect(StatusEffectSlot data) => disposableStatusEffects.Enqueue(data);
    
    protected abstract List<WeaponRangeSlot> GetAttackRange(Vector2Int position, Vector2Int point);
    public abstract Vector2Int GetAttackPoint(Vector2Int position, Vector2Int point);
}
