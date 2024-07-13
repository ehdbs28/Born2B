using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSoundPlayHandler : AudioPlayer
{

    private void Awake()
    {

        EventManager.Instance.RegisterEvent(EventType.OnGridMoveStart, HandleGridMoveStart);
        EventManager.Instance.RegisterEvent(EventType.OnGridMoveFinish, HandleGridMoveFinish);

    }

    private void HandleGridMoveFinish(object[] args)
    {
    }

    private void HandleGridMoveStart(object[] args)
    {
    }

}
