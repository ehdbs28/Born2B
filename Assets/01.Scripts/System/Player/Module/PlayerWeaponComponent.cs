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

    #if UNITY_EDITOR
    [Tooltip("EDITOR CODE")]
    [SerializeField] WeaponItemSO weaponData = null;
    [SerializeField] SpriteRenderer tilePrefab = null;
    private List<GameObject> tiles = new List<GameObject>();
    
    public void DrawRange()
    {
        foreach (WeaponRangeSlot range in currentWeapon.Ranges)
        {
            Vector2Int positionInt = range.Position + lastAttackPoint;
            Vector3 position = new Vector3(positionInt.x, positionInt.y);
            SpriteRenderer tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.color = new Color(tile.color.r, tile.color.g, tile.color.b, 0.5f * range.Theta * 0.01f);
            tiles.Add(tile.gameObject);
        }
    }

    public void ClearDraw()
    {
        tiles.ForEach(i => Destroy(i));
        tiles.Clear();
    }

    [ContextMenu("Equip Weapon")]
    public void Equip()
    {
        EquipWeapon(weaponData);
    }

#endif

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
