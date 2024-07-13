using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundPlayer : AudioPlayer
{

    [SerializeField] private List<AudioData> _melee;
    [SerializeField] private List<AudioData> _range;
    [SerializeField] private AudioData _getItem;

    private void Awake()
    {

        EventManager.Instance.RegisterEvent(EventType.OnPlayerAttackAndGetWeapon, HandlePlaySound);

    }

    public void GetItem()
    {

        PlayAudio(_getItem);

    }

    private void HandlePlaySound(object[] args)
    {

        var weapon = args[0] as Weapon;

        if (weapon is MeleeWeapon)
        {

            PlayAudio(_melee.PickRandom());

        }
        else
        {

            PlayAudio(_range.PickRandom());

        }

    }

    private void OnDestroy()
    {

        EventManager.Instance.UnRegisterEvent(EventType.OnPlayerAttackAndGetWeapon, HandlePlaySound);

    }

}
