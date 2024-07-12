using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{

    PreviewCell,
    MovementCell,
    PlayerAttack,
    CheckAllEnemyDie,
    MovementEnemy,
    AttackEnemy,

}

public enum TurnDataType
{

    IsPreview,
    IsMovementCell

}

public class TurnManager : MonoSingleton<TurnManager>
{

    [SerializeField] private List<TurnType> _turnOrder;
    [SerializeField] private List<TurnObjectSO> _turnLogics;

    private Dictionary<TurnType, Func<IEnumerator>> _turnLogicContainer = new();
    private Dictionary<TurnType, Func<IEnumerator>> _turnEndLogicContainer = new();
    private Dictionary<TurnDataType, object> _turnDataContainer = new();
    private bool _currentTurnEnded;
    private TurnType _currentTurn;

    public void CommitTurnLogic(TurnType type, Func<IEnumerator> enumerator, Func<IEnumerator> turnEndLogic)
    {

        _turnLogicContainer.Add(type, enumerator);
        _turnEndLogicContainer.Add(type, turnEndLogic);

    }

    public void AddTurnData(TurnDataType type, object data)
    {

        if(!_turnDataContainer.TryAdd(type, data))
        {

            _turnDataContainer[type] = data;

        }

    }

    public T GetTurnData<T>(TurnDataType type)
    {

        if(_turnDataContainer.TryGetValue(type, out var obj))
        {

            return (T)obj;

        }

        return default(T);

    }

    public void SetTurnData(TurnDataType type, object value)
    {

        _turnDataContainer[type] = value;

    }

    private void Awake()
    {
        
        foreach(var item in _turnLogics)
        {

            item.Init();

        }

    }

    public void StartTurn()
    {

        StartSafeCoroutine("Turn", TurnLogic());

    }

    public TurnType GetCurrentTurn()
    {

        return _currentTurn;

    }

    public void InitTurn()
    {

        _currentTurn = TurnType.PreviewCell;
        StopSafeCoroutine("Turn");

    }

    private IEnumerator TurnLogic()
    {

        while (true)
        {

            foreach(var item in _turnOrder)
            {

                EventManager.Instance.PublishEvent(EventType.OnTurnChanged, _currentTurn, item);

                yield return null;

                _currentTurn = item;
                var logic = _turnLogicContainer[item];

                StartCoroutine(logic());

                yield return new WaitUntil(() => _currentTurnEnded);

                _currentTurnEnded = false;
                var endLogic = _turnEndLogicContainer[item];

                yield return null;
                StartCoroutine(endLogic());
                CheckPlayerTurnEnd(item);

            }

            EventManager.Instance.PublishEvent(EventType.OnTurnEnded);

        }

    }

    private void CheckPlayerTurnEnd(TurnType item)
    {

        if(item == TurnType.PlayerAttack)
        {

            EventManager.Instance.PublishEvent(EventType.OnPlayerTurnOver);

        }

    }

    public void EndCurrentTurn()
    {

        _currentTurnEnded = true;

    }

}
