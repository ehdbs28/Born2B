using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSoundPlayHandler : AudioPlayer
{

    [SerializeField] private AudioData _moveStartData;
    [SerializeField] private AudioData _movieFinish;

    private void Awake()
    {

        EventManager.Instance.RegisterEvent(EventType.OnGridMoveStart, HandleGridMoveStart);
        EventManager.Instance.RegisterEvent(EventType.OnGridMoveFinish, HandleGridMoveFinish);

    }

    private void HandleGridMoveFinish(object[] args)
    {

        PlayAudio(_movieFinish);

    }

    private void HandleGridMoveStart(object[] args)
    {

        PlayAudio(_moveStartData);

    }


}
