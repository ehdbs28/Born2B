using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : UIComponent
{
    private Image _playerWeaponProfile;
    private Transform _healthParent;
    private Transform _attackCountParent;

    protected override void Awake()
    {
        base.Awake();

        _playerWeaponProfile = transform.Find("PlayerWeapon/Item").GetComponent<Image>();
        _healthParent = transform.Find("Health");
        _attackCountParent = transform.Find("AttackCount"); 
    }
}