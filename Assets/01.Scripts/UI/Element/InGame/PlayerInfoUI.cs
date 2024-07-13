using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : UIComponent
{
    private Image _playerWeaponProfile;
    // private Transform _healthParent;
    // private Transform _attackCountParent;

    [SerializeField] List<Image> healhSlots = new List<Image>();
    [SerializeField] List<Image> attackCountSlots = new List<Image>();

    protected override void Awake()
    {
        base.Awake();

        _playerWeaponProfile = transform.Find("PlayerWeapon/Item").GetComponent<Image>();
        // _healthParent = transform.Find("Health");
        // _attackCountParent = transform.Find("AttackCount"); 
    }

    public void DisplayWeapon(Sprite sprite)
    {
        _playerWeaponProfile.sprite = sprite;
    }

    public void DisplayHealthInfo(int current, int max) => DislaySlot(healhSlots, current, max);
    public void DisplayAttackCount(int current, int max) => DislaySlot(attackCountSlots, current, max);

    private void DislaySlot(List<Image> list, int current, int max)
    {
        int i = 0;
        for(; i < current; ++i)
        {
            list[i].gameObject.SetActive(true);
            list[i].color = Color.white;
        }

        for(; i < max; ++i)
        {
            list[i].gameObject.SetActive(true);
            list[i].color = new Color(1, 1, 1, 0.25f);
        }

        for(; i < list.Count; ++i)
            list[i].gameObject.SetActive(false);
    }
}