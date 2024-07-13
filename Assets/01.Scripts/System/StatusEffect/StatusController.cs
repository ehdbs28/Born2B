using System;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    private StatusEffectSO _statusEffect;
    public StatusEffectSO StatusEffect => _statusEffect;

    public void Init(CellObjectInstance objectInstance)
    {
        _statusEffect = ScriptableObject.CreateInstance<StatusEffectSO>();
        _statusEffect.Init(objectInstance);
    }

    public void Release()
    {
        if (_statusEffect)
        {
            _statusEffect.Release();
        }
    }

    public void AddStatus(StatusType type, int remainTurn)
    {
        if(type == StatusType.None)
            return;

        foreach(StatusType status in Enum.GetValues(typeof(StatusType)))
        {
            if(status == StatusType.None)
                continue;

            if(type.HasFlag(status))
                _statusEffect.AddStatus(new StatusEffectParams(status, remainTurn));
        }
    }
}