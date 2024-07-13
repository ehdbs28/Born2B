using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviveItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.CallByEvent;

    public override void UseArtifact(params object[] args)
    {
        //EventManager.Instance.RegisterEvent()
    }
}
