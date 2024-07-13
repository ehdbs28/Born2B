using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
    private CellObjectInstance owner = null;

    private Dictionary<StatusType, int> _remainTurnDictionary;
    private Dictionary<StatusType, StatusEffectHandler> _effectHandlers;
    private Queue<StatusEffectParams> _statusInitQueue;

    public List<StatusType> EffectedStatus {
        get
        {
            var list = new List<StatusType>();
            foreach (var pair in _remainTurnDictionary)
            {
                if (pair.Value > 0)
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }
    }

    public event Action OnStatusChanged = null;

    public void Init(CellObjectInstance owner)
    {
        this.owner = owner;

        _remainTurnDictionary = new Dictionary<StatusType, int>();
        _effectHandlers = new Dictionary<StatusType, StatusEffectHandler>();
        _statusInitQueue = new Queue<StatusEffectParams>();

        Assembly assembly = Assembly.GetExecutingAssembly();
        foreach (StatusType type in Enum.GetValues(typeof(StatusType)))
        {
            Type instanceType = assembly.GetType($"StatusEffects.{type}Handler");
            if(instanceType == null)
                continue;
            
            StatusEffectHandler handler = Activator.CreateInstance(instanceType) as StatusEffectHandler;
            handler.Init(this.owner);

            _effectHandlers.Add(type, handler);
        }
        
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, TurnEndedHandle);
    }

    public void Release()
    {
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, TurnEndedHandle);
    }

    public void AddStatus(StatusEffectParams effectParams)
    {
        _statusInitQueue.Enqueue(effectParams);
    }

    private void RemoveStatus(StatusType type)
    {
        _remainTurnDictionary[type] = 0;
        _effectHandlers[type].HandleEnd();
    }

    private void TurnEndedHandle(params object[] args)
    {
        foreach(StatusType statusType in _remainTurnDictionary.Keys)
        {
            if (_remainTurnDictionary[statusType] <= 0)
            {
                RemoveStatus(statusType);
                continue;
            }

            _effectHandlers[statusType].HandleUpdate();
            --_remainTurnDictionary[statusType];
        }

        while(_statusInitQueue.Count > 0)
        {
            StatusEffectParams effectParams = _statusInitQueue.Dequeue();
            if(_remainTurnDictionary.ContainsKey(effectParams.statusType))
                _remainTurnDictionary[effectParams.statusType] = effectParams.turnCount;
            else
            {
                _remainTurnDictionary.Add(effectParams.statusType, effectParams.turnCount);
                _effectHandlers[effectParams.statusType].HandleBegin();
            }
        }
    }
}