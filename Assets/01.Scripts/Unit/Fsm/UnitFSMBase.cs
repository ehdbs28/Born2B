using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStateType
{

    Move,
    Attack

}

public abstract class UnitFSMStateBase : FSM_State<UnitStateType>
{

    protected Weapon _unitWeapon;
    protected new UnitFSMBase controller;

    protected UnitFSMStateBase(FSM_Controller<UnitStateType> controller) : base(controller)
    {

        _unitWeapon = controller.GetComponent<Weapon>();
        this.controller = controller as UnitFSMBase;

    }

}

public abstract class UnitFSMBase : FSM_Controller<UnitStateType>
{

    public abstract void DoAttack();
    public abstract Vector2 DoMove(List<Vector2> ablePos, Action endCallback);

    protected Dictionary<string, Action> _eventContainer = new();
    protected Dictionary<string, object> _dataContainer = new();

    protected void Register(string key, Action @event)
    {

        _eventContainer.Add(key, @event);

    }

    protected void AddData(string key, object data)
    {

        _dataContainer.Add(key, data);

    }

    public T GetData<T>(string key)
    {

        return (T)_dataContainer[key];

    }

    public void RemoveData(string key)
    {

        _dataContainer.Remove(key);

    }

    public void InvokeEvent(string key)
    {

        if(_eventContainer.TryGetValue(key, out var obj))
        {

            obj?.Invoke();
            _eventContainer.Remove(key);

        }

    }

}