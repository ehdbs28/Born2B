using System;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private StatusEffectSO _statusEffect;

    public void Init(CellObjectInstance objectInstance)
    {
        _statusEffect = Instantiate(_statusEffect);
        _statusEffect.Init(objectInstance);
    }

    public void Release()
    {
        _statusEffect.Release();
    }

    public void AddStatus(StatusType type, int remainTurn)
    {
        if(type == StatusType.None)
            return;

        foreach(StatusType status in Enum.GetValues(typeof(StatusType)))
        {
            if(type.HasFlag(status))
                _statusEffect.AddStatus(status, remainTurn);
        }
    }
}