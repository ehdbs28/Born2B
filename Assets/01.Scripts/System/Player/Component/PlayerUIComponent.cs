using UnityEngine;

public class PlayerUIComponent : PlayerComponent
{
    private PlayerInfoUI infoPanel = null;

    private PlayerHealthComponent healthComponent = null;
    private PlayerAttackComponent attackComponent = null;
    private PlayerWeaponComponent weaponComponent = null;

    private StatSO stat = null;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        infoPanel = UIManager.Instance.MainCanvas.transform.Find("InGamePanel/PlayerInfo")?.GetComponent<PlayerInfoUI>();
        if(infoPanel == null)
            Debug.LogWarning("Info Panel Missing");

        healthComponent = player.GetPlayerComponent<PlayerHealthComponent>();
        healthComponent.OnChangedHpEvent += HandleHPChanged;

        attackComponent = player.GetPlayerComponent<PlayerAttackComponent>();
        attackComponent.OnAmmoChangedEvent += HandleAmmoChanged;

        weaponComponent = player.GetPlayerComponent<PlayerWeaponComponent>();
        weaponComponent.OnWeaponEquipEvent += HandleWeaponEquip;

        stat = player.GetPlayerComponent<PlayerStatComponent>().Stat;
        stat.OnStatChangedEvent += HandleStatChanged;

        if(player.isClone)
            return;

        infoPanel.DisplayHealthInfo(healthComponent.CurrentHp, healthComponent.MaxHp);
        infoPanel.DisplayAttackCount(attackComponent.CurrentAmmoCount, attackComponent.MaxAmmoCount);
        infoPanel.DisplayWeapon(weaponComponent.CurrentWeapon.WeaponData.ItemIcon);
    }

    public override void Release()
    {
        base.Release();

        if (stat == null)
            Debug.Log("ASD");
        else
            stat.OnStatChangedEvent -= HandleStatChanged;
    }

    private void HandleHPChanged(int current, int max)
    {
        infoPanel.DisplayHealthInfo(healthComponent.CurrentHp, healthComponent.MaxHp);
    }

    private void HandleStatChanged()
    {
        Debug.Log($"{healthComponent.CurrentHp},{healthComponent.MaxHp}");
        infoPanel.DisplayHealthInfo(healthComponent.CurrentHp, healthComponent.MaxHp);
    }

    private void HandleAmmoChanged()
    {
        Debug.Log($"Current : {attackComponent.CurrentAmmoCount} / MAX : {attackComponent.MaxAmmoCount}");
        infoPanel.DisplayAttackCount(attackComponent.CurrentAmmoCount, attackComponent.MaxAmmoCount);
    }

    private void HandleWeaponEquip(WeaponItemSO weaponData)
    {
        infoPanel.DisplayWeapon(weaponData.ItemIcon);
    }
}
