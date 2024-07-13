using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSoundPlayHandler : AudioPlayer
{

    [SerializeField] private AudioData _moveStartData;
    [SerializeField] private AudioData _movieFinish;

    private void OnEnable()
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

    private void OnDisable()
    {

        EventManager.Instance.UnRegisterEvent(EventType.OnGridMoveStart, HandleGridMoveStart);
        EventManager.Instance.UnRegisterEvent(EventType.OnGridMoveFinish, HandleGridMoveFinish);

    }


}
