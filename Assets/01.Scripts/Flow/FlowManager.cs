using Singleton;
using System.Collections.Generic;
using UnityEngine;


public class FlowManager : MonoSingleton<FlowManager>
{

    [SerializeField] private List<FlowSlot> _flowCycle;
    [SerializeField] private int _targetLoopIdx;
    private int _currentCycle;
    public EventType CurrentCycle => _flowCycle[_currentCycle].eventType;

    public void StartFlow()
    {
        ExecuteCycle();
    }

    private void ExecuteCycle()
    {

        EventManager.Instance.PublishEvent(_flowCycle[_currentCycle].eventType);
        _flowCycle[_currentCycle].onFlowEvent?.Invoke();

    }

    public void InitCycle()
    {

        _currentCycle = _targetLoopIdx;

        ExecuteCycle();

    }

    public void NextCycle()
    {

        _currentCycle++;
        if (_currentCycle == _flowCycle.Count)
        {

            _currentCycle = _targetLoopIdx;

        }

        ExecuteCycle();

    }

#if UNITY_EDITOR

    //Debug
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {

            NextCycle();

        }

    }

#endif

}
