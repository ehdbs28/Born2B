using UnityEngine;

public class PlayerUIComponent : PlayerComponent
{
    private PlayerInfoUI infoPanel = null;

    private PlayerHealthComponent healthComponent = null;
    private PlayerAttackComponent attackComponent = null;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        infoPanel = UIManager.Instance.MainCanvas.transform.Find("InGamePanel/PlayerInfo")?.GetComponent<PlayerInfoUI>();
        if(infoPanel == null)
            Debug.LogWarning("Info Panel Missing");

        healthComponent = player.GetPlayerComponent<PlayerHealthComponent>();
        healthComponent.OnChangedHpEvent += HandleHPChanged;

        attackComponent = player.GetPlayerComponent<PlayerAttackComponent>();
        attackComponent.OnAttackEvent += HandleAttack;

        player.GetPlayerComponent<PlayerStatComponent>().Stat.OnStatChangedEvent += HandleStatChanged;
        player.GetPlayerComponent<PlayerWeaponComponent>().OnWeaponEquipEvent += HandleWeaponEquip;
    }

    private void HandleHPChanged(int current, int max)
    {
        infoPanel.DisplayHealthInfo(healthComponent.CurrentHp, healthComponent.MaxHp);
    }

    private void HandleStatChanged()
    {
        infoPanel.DisplayHealthInfo(healthComponent.CurrentHp, healthComponent.MaxHp);
    }

    private void HandleAttack()
    {
        infoPanel.DisplayAttackCount(attackComponent.CurrentAmmoCount, attackComponent.MaxAmmoCount);
    }

    private void HandleWeaponEquip(WeaponItemSO weaponData)
    {
        infoPanel.DisplayWeapon(weaponData.ItemIcon);
    }
}
