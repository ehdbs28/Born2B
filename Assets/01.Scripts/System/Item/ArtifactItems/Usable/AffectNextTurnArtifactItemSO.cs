using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AffectNextTurnArtifactItemSO : ArtifactItemSO
{
    [SerializeField] private EventType _remveCondition;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);

        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, HandleAddNextTurnAffect);
        EventManager.Instance.RegisterEvent(_remveCondition, HandleRemoveNextTurnAffect);
    }

    protected virtual void HandleAddNextTurnAffect(params object[] arr)
    {
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, HandleAddNextTurnAffect);
    }
    protected virtual void HandleRemoveNextTurnAffect(params object[] arr)
    {
        EventManager.Instance.UnRegisterEvent(_remveCondition, HandleRemoveNextTurnAffect);
    }
}
