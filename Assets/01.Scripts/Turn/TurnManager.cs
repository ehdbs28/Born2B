using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{

    None,
    PreviewCell,
    MovementCell,
    PlayerAttack,
    CheckAllEnemyDie,
    MovementEnemy,
    AttackEnemy,
    SetTurnCount,
    CheckTurnCount

}

public enum TurnDataType
{

    IsPreview,
    IsMovementCell,
    TurnCount,

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
    private TurnType _targetSkipTurn = TurnType.None;

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

    public void Init()
    {
        
        foreach(var item in _turnLogics)
        {

            item.Init();

        }

    }

    public void StartTurn()
    {

        StartCoroutine(TurnLogic());

    }

    public TurnType GetCurrentTurn()
    {

        return _currentTurn;

    }

    public void InitTurn()
    {

        _currentTurn = TurnType.PreviewCell;
        StopAllCoroutines();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {

            _targetSkipTurn = TurnType.MovementEnemy;
            Debug.Log(_targetSkipTurn);

        }

    }

    private IEnumerator TurnLogic()
    {

        yield return null;
        int currentTurnCount = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>().GetComponent<PlayerStatComponent>().GetStat(StatType.TurnCount);
        while (true)
        {

            foreach(var item in _turnOrder)
            {


                if (_targetSkipTurn != TurnType.None && item != _targetSkipTurn)
                    continue;

                if(_targetSkipTurn != TurnType.None && item == _targetSkipTurn)
                {

                    _targetSkipTurn = TurnType.None;
                    Debug.Log("ÅÏÀ» ½ºÅµÇÏ¿´´Ù¶÷Áã½ã´õ¸Å±â");

                }

                EventManager.Instance.PublishEvent(EventType.OnTurnChanged, _currentTurn, item);

                yield return null;

                _currentTurn = item;
                var logic = _turnLogicContainer[item];

                StartCoroutine(logic());

                yield return new WaitUntil(() => _currentTurnEnded || _targetSkipTurn != TurnType.None);

                if(_targetSkipTurn != TurnType.None)
                {

                    break;

                }

                _currentTurnEnded = false;
                var endLogic = _turnEndLogicContainer[item];

                yield return null;

                StartCoroutine(endLogic());

                if (currentTurnCount > 0 && _currentTurn == TurnType.PlayerAttack)
                {

                    currentTurnCount--;
                    break;

                }
                else if(currentTurnCount == 0 && 
                    _currentTurn == TurnType.AttackEnemy)
                {

                    var obj = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();

                    if(obj != null)
                    {

                        currentTurnCount = obj.GetComponent<PlayerStatComponent>().GetStat(StatType.TurnCount);

                    }


                }

                CheckPlayerTurnEnd(item);

            }

            EventManager.Instance.PublishEvent(EventType.OnTurnEnded);

        }

    }

    public void SkipTurn(TurnType targetSkipTurn)
    {

        _targetSkipTurn = targetSkipTurn;

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

        Debug.Log(_currentTurn);
        _currentTurnEnded = true;

    }

}
