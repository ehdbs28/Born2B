using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모듈 컴포넌트
/// </summary>
public class PlayerWeaponComponent : PlayerComponent
{
    [SerializeField] Transform weaponContainer = null;
    private Weapon currentWeapon = null;
    public Weapon CurrentWeapon => currentWeapon;

    private Vector2Int lastAttackPoint = Vector2Int.zero;
    private Queue<WeaponRangeTile> tiles = new Queue<WeaponRangeTile>();
    
    public void DrawRange()
    {
        foreach (WeaponRangeSlot range in currentWeapon.Ranges)
        {

            Vector2Int positionInt = range.Position + lastAttackPoint;
            Cell? cell = StageManager.Instance.Grid.FindCellByPosition(positionInt);
            if(cell == null)
                return;
            
            CellInstance cellInstance = StageManager.Instance.Grid.GetCellInstance(cell.Value.guid);
            WeaponRangeTile tile = cellInstance.GetComponent<WeaponRangeTile>();
            tile.SetTileColor(range.Theta * 0.25f * 0.01f);
            tiles.Enqueue(tile);
        }
    }

    public void ClearDraw()
    {
        while(tiles.Count > 0)
            tiles.Dequeue().SetTileColor(0f);
    }

    public override void Init(PlayerInstance player)
    {
        base.Init(player);
        EquipWeapon((player.GetData() as UnitDataSO).weaponItem);

    }

    public override void OnClone(PlayerInstance clone)
    {
        base.OnClone(clone);

        PlayerWeaponComponent component = clone.GetPlayerComponent<PlayerWeaponComponent>();
        component.currentWeapon = component.weaponContainer.GetComponentInChildren<Weapon>();
        component.currentWeapon.Init(clone);
    }

    public Vector2Int RotateWeapon(Vector2Int position, Vector2Int point)
    {
        if(currentWeapon == null)
            return Vector2Int.zero;

        Vector2Int attackPoint = currentWeapon.GetAttackPoint(position, point);
        if(lastAttackPoint == attackPoint)
            return Vector2Int.zero;


        lastAttackPoint = attackPoint;

        Vector2Int direction = attackPoint - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentWeapon.RotateAttackRange(angle);

        return lastAttackPoint;
    }

    public void Attack(Vector2Int inputPosition, AttackParams attackParams)
    {
        Vector2Int position = player.transform.position.GetVectorInt();
        Vector2Int attackPoint = currentWeapon.GetAttackPoint(position, inputPosition);

        currentWeapon.Attack(attackParams, position, attackPoint);
    }

	public bool EquipWeapon(WeaponItemSO weaponData)
    {
        if(currentWeapon != null)
            return false;

        currentWeapon = Instantiate(weaponData.WeaponPrefab, weaponContainer);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.Init(player);

        EventManager.Instance.PublishEvent(EventType.OnPlayerWeaponChanged);

        return true;
    }

    public bool UnequipWeapon()
    {
        currentWeapon.Release();
        currentWeapon = null;

        EventManager.Instance.PublishEvent(EventType.OnPlayerWeaponChanged);

        return true;
    }
}
