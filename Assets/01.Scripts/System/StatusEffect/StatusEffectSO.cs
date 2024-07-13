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
    private Dictionary<StatusType, StatusEffectHandler> _effecteHandlers;

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
        _effecteHandlers = new Dictionary<StatusType, StatusEffectHandler>();

        Assembly assembly = Assembly.GetExecutingAssembly();
        foreach (StatusType type in Enum.GetValues(typeof(StatusType)))
        {
            Type instanceType = assembly.GetType($"StatusEffects.{type}Handler");
            if(instanceType == null)
                continue;
            
            StatusEffectHandler handler = Activator.CreateInstance(instanceType) as StatusEffectHandler;
            handler.Init(this.owner);

            _effecteHandlers.Add(type, handler);
        }
        
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, TurnEndedHandle);
    }

    public void Release()
    {
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, TurnEndedHandle);
    }

    public void AddStatus(StatusType type, int turn)
    {
        if(_remainTurnDictionary[type] == 0)
            _effecteHandlers[type].HandleBegin();

        _remainTurnDictionary[type] = turn;
    }

    private void RemoveStatus(StatusType type)
    {
        _remainTurnDictionary[type] = 0;
        _effecteHandlers[type].HandleEnd();
    }

    private void TurnEndedHandle(params object[] args)
    {
        foreach (StatusType statusType in Enum.GetValues(typeof(StatusType)))
        {
            if (!_remainTurnDictionary.ContainsKey(statusType) || _remainTurnDictionary[statusType] <= 0)
            {
                RemoveStatus(statusType);
                continue;
            }

            _effecteHandlers[statusType].HandleUpdate();
            Debug.Log($"{statusType} Status Effect Handled");
            --_remainTurnDictionary[statusType];
        }
    }
}